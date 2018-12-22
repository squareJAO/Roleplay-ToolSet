using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

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

    [Serializable]
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
            public Entity ParentEntity { get; private set; }

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
            private decimal _number;

            public NumericAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : base(groupName, parentEntity, format)
            {
                _number = 0;
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
                    Value = _number
                };
                editControl.Controls.Add(numberBox);

                // Make & Add change button
                Button changeButton = new Button
                {
                    Text = "Change",
                    Size = new Size(100, 20),
                    Location = new Point(0, 60),
                    Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom
                };
                editControl.Controls.Add(changeButton);

                // Bind events
                changeButton.Click += (object sender, EventArgs e) =>
                {
                    _number = numberBox.Value;
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
                return _number.ToString();
            }
        }

        [Serializable]
        public class StringAttribute : Attribute
        {
            private string _string;
            private string _rtfstring;

            public StringAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : this(groupName, parentEntity, format, "")
            {

            }

            public StringAttribute(string groupName, Entity parentEntity, AttributeFormat format, string value)
                : base(groupName, parentEntity, format)
            {
                _string = value;
                _rtfstring = null;
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
                    Size = new Size(100, 80)
                };
                if (_rtfstring == null)
                {
                    textBox.Text = _string;
                }
                else
                {
                    textBox.Rtf = _rtfstring;
                }
                textBox.BorderStyle = BorderStyle.None;
                textBox.ScrollBars = RichTextBoxScrollBars.None;
                editControl.Controls.Add(textBox);

                // Make & Add change button
                Button changeButton = new Button
                {
                    Text = "Change",
                    Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Size = new Size(100, 20),
                    Location = new Point(0, 80)
                };
                editControl.Controls.Add(changeButton);

                // Bind events
                changeButton.Click += (object sender, EventArgs e) =>
                {
                    _string = textBox.Text;
                    _rtfstring = textBox.Rtf;
                    InvokeValueChanged();
                }; // Create a new lambda while everything is in scope
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
                return _string;
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
            private Image _image;

            private static readonly Image nullImage = Image.FromFile("images/Portrait_Placeholder.png");

            public ImageAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : base(groupName, parentEntity, format)
            {
                _image = nullImage;
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
                    Image = _image,
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
                        _image = Image.FromFile(fileDialog.FileName);
                        picture.Image = _image;
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
                return _image;
            }
        }

        [Serializable]
        public class BoolAttribute : Attribute
        {
            private bool _value;

            public BoolAttribute(string groupName, Entity parentEntity, AttributeFormat format)
                : base(groupName, parentEntity, format)
            {
                _value = false;
            }

            public override Control GetEditControl()
            {
                // Make & format control
                CheckBox editControl = new CheckBox
                {
                    Text = "",
                    Checked = _value,
                    Anchor = AnchorStyles.None
                };

                // Bind events
                editControl.CheckedChanged += (object sender, EventArgs e) =>
                {
                    _value = editControl.Checked;
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
                return _value.ToString();
            }
        }

        // ---------------------------------------------------------
        // -                Attribute definition                   -
        // ---------------------------------------------------------

        private List<Attribute> _attributes;
        
        [field: NonSerialized]
        public event EntityAttributeEventHandler AttributeRemoved;

        [field: NonSerialized]
        public event EntityAttributeEventHandler AttributeAdded;

        [field: NonSerialized]
        public event EntityAttributeEventHandler AttributeValueChanged;

        public Entity()
        {
            _attributes = new List<Attribute>();

            // All entities must have a name to begin with
            AddAttribute(new StringAttribute("Name", this, new AttributeFormat(deleteIfEmpty: false, deleteLocked: true, nameLocked: true), "New Entity"));
        }

        public List<Attribute> GetAttributes()
        {
            return _attributes;
        }

        public void AddAttribute(Attribute attribute)
        {
            _attributes.Add(attribute);

            // Bind events
            attribute.ValueChanged += Attribute_ValueChanged;

            // Invoke events
            AttributeAdded?.Invoke(attribute, new EventArgs());
        }

        private void Attribute_ValueChanged(Attribute attribute, EventArgs e)
        {
            AttributeValueChanged?.Invoke(attribute, new EventArgs());
        }

        public void RemoveAttribute(Attribute attribute)
        {
            if (!attribute.Format.DeleteLocked)
            {
                // Unbind events
                attribute.ValueChanged -= Attribute_ValueChanged;

                _attributes.Remove(attribute);

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
    }
}
