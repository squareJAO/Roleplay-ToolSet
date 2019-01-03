using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace RoleplayToolSet
{
    [JsonObject]
    public class Expression
    {
        public string Formula { get; private set; }

        [JsonIgnore]
        public List<Entity.Attribute> References { get; private set; } = new List<Entity.Attribute>(); // The attributes that this formula references
        [JsonIgnore]
        public List<string> MissingReferences { get; private set; } = new List<string>(); // The attributes that this formula references

        [JsonConstructor]
        public Expression(string formula)
        {
            Formula = formula ?? throw new ArgumentNullException(nameof(formula));
        }
        
        public struct StringOrError
        {
            public bool IsError;
            public string Value;

            public StringOrError(bool isError, string value)
            {
                IsError = isError;
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public string RunOnEntity(Entity entity, Entity.Attribute attribute = null)
        {
            References = new List<Entity.Attribute>();
            MissingReferences = new List<string>();

            StringOrError result = ParseAndExecute(entity, attribute);

            if (MissingReferences.Count != 0)
            {
                return $"Could not find groups {string.Join(", ", MissingReferences)}";
            }

            return result.Value;
        }

        private StringOrError ParseAndExecute(Entity entity, Entity.Attribute attribute)
        {
            string formula = Formula.Trim();

            // Check if the formula is empty
            if (string.IsNullOrEmpty(formula))
            {
                return new StringOrError(false, "");
            }

            // Check if the formula is a literal
            var matchString = Regex.Match(formula, "(\"([^\"]*)\")|(\'([^\']*)\')");
            if (matchString.Success && matchString.Value == formula)
            {
                return new StringOrError(false, matchString.Groups[2].Value);
            }
            var matchDecimal = Regex.Match(formula, "-?[0-9]+(\\.[0-9]+)?");
            if (matchDecimal.Success && matchDecimal.Value == formula)
            {
                return new StringOrError(false, matchDecimal.Value);
            }

            // Check if it is referencing another group
            var matchReference = Regex.Match(formula, @"&([^^&""\(\)\+\-\\,]+)"); // Any name ommiting special characters
            if (matchReference.Success && matchReference.Value == formula)
            {
                string groupName = matchReference.Groups[1].Value;
                Entity.Attribute referencedAttribute = entity.Attributes.FirstOrDefault(x => x.GroupName == groupName);
                if (referencedAttribute == null) // If attribute by the name groupName doesn't exist
                {
                    MissingReferences.Add(groupName);
                    return new StringOrError(true, $"Could not find attribute '{groupName}'");
                }
                else // If reference is to an existing attribute
                {
                    References.Add(referencedAttribute);
                    switch (referencedAttribute.GetAttributeType())
                    {
                        case Entity.AttributeType.Image:
                            return new StringOrError(true, $"Cannot convert image attribute '{groupName}' to string");
                        case Entity.AttributeType.Formula:
                            if (attribute != null)
                            {
                                List<Entity.Attribute> referenceReferences = ((Entity.FormulaAttribute)referencedAttribute).Formula.References;
                                if (referenceReferences.Contains(attribute)) // Can't have looping dependencies
                                {
                                    return new StringOrError(true, $"Circular reference with '{groupName}'");
                                }
                                References.AddRange(referenceReferences);
                            }
                            break;
                    }
                    return new StringOrError(false, (string)referencedAttribute.GetListViewValue());
                }
            }

            // Check for monads
            var monadReference = Regex.Match(formula, @"([a-zA-Z]+)\(([^,]+)\)");
            if (monadReference.Success && monadReference.Value == formula)
            {
                return new StringOrError(true, $"Could not parse monad '{formula}'");
            }

            // Check for diads
            var diadReference = Regex.Match(formula, @"([a-zA-Z]+)\(([^,]+),([^,]+)\)");
            if (diadReference.Success && diadReference.Value == formula)
            {
                string functionName = diadReference.Groups[1].Value.ToLower();

                Expression leftExp = new Expression(diadReference.Groups[2].Value);
                StringOrError left = leftExp.ParseAndExecute(entity, attribute);
                References.AddRange(leftExp.References);
                MissingReferences.AddRange(leftExp.MissingReferences);
                if (left.IsError)
                    return left;
                Expression rightExp = new Expression(diadReference.Groups[3].Value);
                StringOrError right = rightExp.ParseAndExecute(entity, attribute);
                References.AddRange(rightExp.References);
                MissingReferences.AddRange(rightExp.MissingReferences);
                if (right.IsError)
                    return right;

                if (functionName == "add" || functionName == "sub" || functionName == "mult" || functionName == "div" || functionName == "gt" || functionName == "lt")
                {
                    bool isLeftInt = decimal.TryParse(left.Value, out decimal leftDec);
                    bool isRightInt = decimal.TryParse(right.Value, out decimal rightDec);
                    if (!isLeftInt || !isRightInt)
                        return new StringOrError(true, $"Could not parse '{(isLeftInt ? left.Value : right.Value)}' to an decimal in formula '{formula}'");

                    if (functionName == "add")
                    {
                        return new StringOrError(false, (leftDec + rightDec).ToString());
                    }
                    if (functionName == "sub")
                    {
                        return new StringOrError(false, (leftDec - rightDec).ToString());
                    }
                    if (functionName == "mult")
                    {
                        return new StringOrError(false, (leftDec * rightDec).ToString());
                    }
                    if (functionName == "div")
                    {
                        if (rightDec == 0M)
                        {
                            return new StringOrError(true, $"Could not divide by 0 in '{formula}'");
                        }
                        return new StringOrError(false, (leftDec / rightDec).ToString());
                    }
                    if (functionName == "gt")
                    {
                        return new StringOrError(false, (leftDec > rightDec).ToString());
                    }
                    if (functionName == "lt")
                    {
                        return new StringOrError(false, (leftDec < rightDec).ToString());
                    }
                }
                if (functionName == "eq")
                {
                    return new StringOrError(false, (left.Value == right.Value).ToString());
                }
            }

            return new StringOrError(true, $"Could not parse expression '{formula}'");
        }
    }
}
