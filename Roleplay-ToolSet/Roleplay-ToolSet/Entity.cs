using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RoleplayToolSet
{
    // Used in events regarding changed entities
    public delegate void EntityEventHandler(Entity entity, EventArgs eventArgs);

    // Used in events regarding changed attributes
    public delegate void AttributeEventHandler(Entity.Attribute attr, EventArgs eventArgs);

    // Used in events regarding changed attribute collections
    public class AttributeNameChangeEventArgs : EventArgs
    {
        public string OldAttributeName;
        public string NewAttributeName;

        public AttributeNameChangeEventArgs(string oldAttributeName, string newAttributeName)
        {
            OldAttributeName = oldAttributeName;
            NewAttributeName = newAttributeName;
        }
    }

    public delegate void AttributeNameChangeEventHandler(Entity.Attribute attr, AttributeNameChangeEventArgs eventArgs);

    // Used in events regarding entities changing attributes
    public class EntityAttributeEventArgs : EventArgs
    {
        public Entity.Attribute ChangedAttribute;
        public Entity ChangedEntity;

        public EntityAttributeEventArgs(Entity entity, Entity.Attribute attribute)
        {
            ChangedAttribute = attribute;
            ChangedEntity = entity;
        }
    }

    public delegate void EntityAttributeEventHandler(Entity.Attribute attribute, EventArgs eventArgs);

    [JsonObject(IsReference = true)]
    public class Entity
    {
        // ---------------------------------------------------------
        // -              Attributes for entities                  -
        // ---------------------------------------------------------
        public enum AttributeType
        {
            Numeric,
            String,
            Image,
            Bool
        }
        /// <summary>
        /// The format that the attribute takes
        /// </summary>
        [Serializable]
        public class AttributeFormat
        {
            // This means something different to each attribute
            public int Style { get; private set; }

            // The attribute group will be deleted if no entities use it
            public bool DeleteIfEmpty { get; private set; }

            // The attribute group can't be deleted by the user - eg name
            public bool DeleteLocked { get; private set; }

            // The attribute group can't be renamed by the user - eg name
            public bool NameLocked { get; private set; }

            [JsonConstructor]
            public AttributeFormat(int style = 0, bool deleteIfEmpty=false, bool deleteLocked=false, bool nameLocked = false)
            {
                Style = style;
                DeleteIfEmpty = deleteIfEmpty;
                DeleteLocked = deleteLocked;
                NameLocked = nameLocked;
            }

            public AttributeFormat(AttributeFormat format, string columnName = null, int? style = null, bool? deleteIfEmpty = null)
            {
                Style = style ?? format.Style;
                DeleteIfEmpty = deleteIfEmpty ?? format.DeleteIfEmpty;
                DeleteLocked = format.DeleteLocked;
                NameLocked = format.NameLocked;
            }
        }

        /// <summary>
        /// An item in the entity such as health, image, etc
        /// </summary>
        [Serializable]
        public abstract class Attribute
        {
            public AttributeFormat Format { get; private set; }
            public string GroupName { get; private set; }

            [JsonIgnore]
            public Entity ParentEntity { get; internal set; }

            public event AttributeEventHandler ValueChanged;

            public Attribute(string groupName, Entity parentEntity)
                : this(groupName, parentEntity, new AttributeFormat())
            {

            }

            public Attribute(string groupName, Entity parentEntity, AttributeFormat format)
            {
                GroupName = groupName;
                ParentEntity = parentEntity;
                ChangeFormat(format);
            }

            public void ChangeFormat(AttributeFormat format)
            {
                if (Format == null || (format != null && format.DeleteLocked == Format.DeleteLocked)) // Absolutely disallow this from changing
                {
                    Format = format;
                }
            }

            /// <summary>
            /// Changes the name of the group that this entity belongs to
            /// </summary>
            /// <param name="name">The new name to take</param>
            public void ChangeGroupName(string name)
            {
                if (Format == null || !Format.NameLocked)
                {
                    // Change
                    string oldName = GroupName;
                    GroupName = name;
                }
            }

            public abstract object GetListViewValue();

            public abstract Control GetEditControl();

            public abstract AttributeType GetAttributeType();

            /// <summary>
            /// Invokes the changed event
            /// </summary>
            protected void InvokeValueChanged()
            {
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }

        [Serializable]
        public class NumericAttribute : Attribute
        {
            public decimal Number { get; private set; }

            public NumericAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : this(groupName, parentEntity, format, 0)
            {

            }

            [JsonConstructor]
            public NumericAttribute(string groupName, Entity parentEntity, AttributeFormat format, decimal number)
                : base(groupName, parentEntity, format)
            {
                Number = number;
            }

            public override Control GetEditControl()
            {
                // Make & format base control (Panel)
                Panel editControl = new Panel
                {
                    Size = new Size(100, 100)
                };

                // Make & Add number box
                NumericUpDown numberBox = new NumericUpDown
                {
                    Width = 100,
                    Location = new Point(0, 40),
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue,
                    DecimalPlaces = 3,
                    TextAlign = HorizontalAlignment.Right,
                    Value = Number
                };
                editControl.Controls.Add(numberBox);

                // Bind events
                numberBox.ValueChanged += (object sender, EventArgs e) =>
                {
                    Number = numberBox.Value;
                    InvokeValueChanged();
                }; // Create a new lambda while everything is in scope

                // Return
                return editControl;
            }

            public override AttributeType GetAttributeType()
            {
                return AttributeType.Numeric;
            }

            public override object GetListViewValue()
            {
                return Number.ToString();
            }
        }

        [Serializable]
        public class StringAttribute : Attribute
        {
            [Serializable]
            public class RTFString
            {
                public string RTF { get; private set; }

                [JsonConstructor]
                public RTFString(string rtf)
                {
                    SetString(rtf);
                }

                public void SetString(string value)
                {
                    if ((value != null) &&
                        (value.Trim().StartsWith("{\\rtf")))
                    {
                        RTF = value;
                    }
                    else
                    {
                        using (RichTextBox rtfTemp = new RichTextBox())
                        {
                            rtfTemp.Text = value;
                            RTF = rtfTemp.Rtf;
                        }
                    }
                }

                public string GetPlainText()
                {
                    using (RichTextBox rtfTemp = new RichTextBox())
                    {
                        rtfTemp.Rtf = RTF;
                        return rtfTemp.Text;
                    }
                }
            }

            public RTFString Value { get; private set; }

            public StringAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : this(groupName, parentEntity, format, "")
            {

            }

            public StringAttribute(string groupName, Entity parentEntity, AttributeFormat format, string value)
                : this(groupName, parentEntity, format, new RTFString(value))
            {

            }

            [JsonConstructor]
            public StringAttribute(string groupName, Entity parentEntity, AttributeFormat format, RTFString value)
                : base(groupName, parentEntity, format)
            {
                Value = value ?? new RTFString("");
            }

            public override Control GetEditControl()
            {
                // Make & format base control (Panel)
                Panel editControl = new Panel
                {
                    Size = new Size(100, 100)
                };

                // Make & Add text box
                RichTextBox textBox = new RichTextBox
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right,
                    Size = new Size(100, 100),
                    WordWrap = true
                };
                textBox.Rtf = Value.RTF;
                textBox.BorderStyle = BorderStyle.None;
                textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
                editControl.Controls.Add(textBox);

                // Bind events
                void changeHandler(object sender, EventArgs e)
                {
                    Value.SetString(textBox.Rtf);
                    InvokeValueChanged();
                } // Create a new function while everything is in scope
                // Bind function
                textBox.LostFocus += changeHandler;
                textBox.Leave += changeHandler;

                // Add some nice formatting for text box
                textBox.KeyDown += (object sender, KeyEventArgs e) =>
                {
                    if (e.Control && !e.Alt && !e.Shift) // Only interested in ctrl-key combos
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.B:
                                textBox.SelectionFont = new Font(textBox.Font, textBox.SelectionFont.Style ^ FontStyle.Bold);
                                e.SuppressKeyPress = true;
                                break;
                            case Keys.I:
                                textBox.SelectionFont = new Font(textBox.Font, textBox.SelectionFont.Style ^ FontStyle.Italic);
                                e.SuppressKeyPress = true;
                                break;
                            case Keys.U:
                                textBox.SelectionFont = new Font(textBox.Font, textBox.SelectionFont.Style ^ FontStyle.Underline);
                                e.SuppressKeyPress = true;
                                break;
                            case Keys.T:
                                textBox.SelectionFont = new Font(textBox.Font, textBox.SelectionFont.Style ^ FontStyle.Strikeout);
                                e.SuppressKeyPress = true;
                                break;
                        }
                    }
                    if (!e.Shift && e.KeyCode == Keys.Enter)
                    {
                        changeHandler(null, null);
                    }
                };

                // Return
                return editControl;
            }

            public override AttributeType GetAttributeType()
            {
                return AttributeType.String;
            }

            public override object GetListViewValue()
            {
                return Value.GetPlainText();
            }
        }

        [Serializable]
        public class ImageAttribute : Attribute
        {
            /// <summary>
            /// A converter to allow for images to be serialised
            /// </summary>
            private class ImageConverter : JsonConverter
            {
                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    string base64 = (string)reader.Value;
                    return Image.FromStream(new MemoryStream(Convert.FromBase64String(base64)));
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    Image image = (Image)value;
                    MemoryStream memStream = new MemoryStream();
                    image.Save(memStream, image.RawFormat);
                    writer.WriteValue(Convert.ToBase64String(memStream.ToArray()));
                }

                public override bool CanConvert(Type objectType)
                {
                    return objectType == typeof(Image);
                }
            }

            [JsonConverter(typeof(ImageConverter))]
            public Image Image { get; private set; }

            [JsonIgnore]
            private static readonly Image nullImage = Image.FromFile("images/Portrait_Placeholder.png");

            public ImageAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : this(groupName, parentEntity, format, nullImage)
            {

            }

            [JsonConstructor]
            public ImageAttribute(string groupName, Entity parentEntity, AttributeFormat format, Image image)
                : base(groupName, parentEntity, format)
            {
                Image = image;
            }

            public override Control GetEditControl()
            {
                // Make & format base control (Panel)
                Panel editControl = new Panel
                {
                    Size = new Size(100, 100)
                };

                // Make & Add image
                PictureBox picture = new PictureBox
                {
                    Image = Image,
                    Size = new Size(100, 100),
                    Location = new Point(0, 0),
                    Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Cursor = Cursors.Hand,
                    BorderStyle = BorderStyle.FixedSingle
                };
                editControl.Controls.Add(picture);

                // Bind events
                picture.Click += (object sender, EventArgs e) =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog()
                    {
                        Multiselect = false,
                        Filter = "ImageFile|*.BMP;*.JPG;*.GIF;*.PNG|All files|*.*"
                    };

                    // Show the dialogue
                    DialogResult result = fileDialog.ShowDialog();

                    // Result = ok if the user opens something
                    if (result == DialogResult.OK)
                    {
                        Image = Image.FromFile(fileDialog.FileName);
                        picture.Image = Image;
                        InvokeValueChanged();
                    }
                }; // Create a new lambda while everything is in scope

                // Return
                return editControl;
            }

            public override AttributeType GetAttributeType()
            {
                return AttributeType.Image;
            }

            public override object GetListViewValue()
            {
                return Image;
            }
        }

        [Serializable]
        public class BoolAttribute : Attribute
        {
            public bool Value { get; private set; }

            public BoolAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : this(groupName, parentEntity, format, false)
            {

            }

            [JsonConstructor]
            public BoolAttribute(string groupName, Entity parentEntity, AttributeFormat format, bool value)
                : base(groupName, parentEntity, format)
            {
                Value = value;
            }

            public override Control GetEditControl()
            {
                // Make & format control
                CheckBox editControl = new CheckBox
                {
                    Text = "",
                    Checked = Value,
                    Anchor = AnchorStyles.None
                };

                // Bind events
                editControl.CheckedChanged += (object sender, EventArgs e) =>
                {
                    Value = editControl.Checked;
                    InvokeValueChanged();
                }; // Create a new lambda while everything is in scope

                // Return
                return editControl;
            }

            public override AttributeType GetAttributeType()
            {
                return AttributeType.Bool;
            }

            public override object GetListViewValue()
            {
                return Value.ToString();
            }
        }

        // ---------------------------------------------------------
        // -                Attribute definition                   -
        // ---------------------------------------------------------

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; private set; }

        [JsonProperty("nameAttribute")]
        public StringAttribute NameAttribute { get; private set; } // Used to access the name attribute

        [field: JsonIgnore]
        public event EntityAttributeEventHandler AttributeRemoved;

        [field: JsonIgnore]
        public event EntityAttributeEventHandler AttributeAdded;

        [field: JsonIgnore]
        public event EntityAttributeEventHandler AttributeValueChanged;
        
        public Entity()
            : this(true) // All external entity creation must have default attributes
        { }

        /// <summary>
        /// Privately allows for an entity to be fuilt with or without default attributes
        /// </summary>
        /// <param name="useDefaultAttributes"></param>
        private Entity(bool useDefaultAttributes)
        {
            Attributes = new List<Attribute>();

            if (useDefaultAttributes)
            {
                // All entities must have a name to begin with
                StringAttribute name = new StringAttribute("Name", this, new AttributeFormat(deleteIfEmpty: false, deleteLocked: true, nameLocked: true), "New Entity");
                AddAttribute(name);
                NameAttribute = name;
            }
        }

        [JsonConstructor]
        private Entity(List<Attribute> attributes, StringAttribute nameAttribute)
        {
            Attributes = new List<Attribute>();
            foreach (Attribute attribute in attributes)
            {
                AddAttribute(attribute);
            }

            NameAttribute = nameAttribute ?? throw new FormatException("Entity does not contain name attribute, failed to load");
        }

        public void AddAttribute(Attribute attribute)
        {
            Attributes.Add(attribute);

            // Bind events
            attribute.ValueChanged += Attribute_ValueChanged;

            // Invoke events
            AttributeAdded?.Invoke(attribute, new EventArgs());
        }

        private void Attribute_ValueChanged(Attribute attribute, EventArgs e)
        {
            AttributeValueChanged?.Invoke(attribute, new EventArgs());
        }

        public void RemoveAttribute(string attributeName)
        {
            foreach (Attribute attribute in Attributes)
            {
                if (attribute.GroupName == attributeName)
                {
                    RemoveAttribute(attribute);
                    return;
                }
            }
        }

        public void RemoveAttribute(Attribute attribute)
        {
            if (!attribute.Format.DeleteLocked)
            {
                // Unbind events
                attribute.ValueChanged -= Attribute_ValueChanged;

                Attributes.Remove(attribute);

                // Invoke events
                AttributeRemoved?.Invoke(attribute, new EventArgs());
            }
        }

        /// <summary>
        /// Makes a new attribute from an AttributeType
        /// </summary>
        /// <param name="type">The type of the attribute</param>
        /// <param name="groupName">The name of the group</param>
        /// <param name="entity">The entity that this attribute belongs to</param>
        /// <param name="format">The format to take</param>
        /// <returns>A new attribute</returns>
        public static Attribute MakeNewAttribute(AttributeType type, string groupName, Entity entity, AttributeFormat format)
        {
            switch (type)
            {
                case AttributeType.Numeric:
                    return new NumericAttribute(groupName, entity, format);
                case AttributeType.String:
                    return new StringAttribute(groupName, entity, format);
                case AttributeType.Image:
                    return new ImageAttribute(groupName, entity, format);
                case AttributeType.Bool:
                    return new BoolAttribute(groupName, entity, format);
            }
            return null;
        }

        /// <summary>
        /// Saves the entity to a specified path
        /// </summary>
        /// <param name="path">The path to save to</param>
        /// <exception cref="JsonSerializationException"></exception>
        public void Save(string path, bool overWrite=true)
        {
            if (!overWrite && File.Exists(path))
            {
                string dir = Path.GetDirectoryName(path);
                string fileName = Path.GetFileNameWithoutExtension(path);
                string fileExt = Path.GetExtension(path);

                int i = 1;
                do
                {
                    path = Path.Combine(dir, $"{fileName} ({i}){fileExt}");
                } while (File.Exists(path));
            }
            
            string jsonText = JsonConvert.SerializeObject(this, Form1.SerializerSettings);
            File.WriteAllText(path, jsonText);
        }

        /// <summary>
        /// Loads an entity from a specific path
        /// </summary>
        /// <param name="path">The path to load from</param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="JsonSerializationException"></exception>
        /// <exception cref="FormatException"></exception>
        public static Entity Load(string path)
        {
            string jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Entity>(jsonText, Form1.SerializerSettings);
        }
    }
}
