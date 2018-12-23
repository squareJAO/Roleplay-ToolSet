using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace RoleplayToolSet
{
    public partial class EntityDetailedDisplay : UserControl
    {
        /// <summary>
        /// Displays a single detail from an entity
        /// </summary>
        private class AttributeViewer : Panel
        {
            private Entity.Attribute _attribute;

            public event AttributeEventHandler AttributeRemoved;
            public event AttributeEventHandler AttributeAdded;

            /// <summary>
            /// Creates a viewer for an attribute that allow for data to be changed and the attribute to be added/removed
            /// </summary>
            /// <param name="attribute">The attribute to display</param>
            /// <param name="exists">Whether the attribute already belongs to the entity or if it is a possible attribute</param>
            public AttributeViewer(Entity.Attribute attribute, bool exists) // Takes a parameter because it's private and I'm a rule breaker >:)
            {
                _attribute = attribute;

                // Format this
                this.Size = new Size(200, 200);
                this.BorderStyle = BorderStyle.FixedSingle;

                // Add & Format name label
                Label name = new Label
                {
                    Text = attribute.GroupName,
                    Location = new Point(0, 0),
                    Size = new Size(200, 20),
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                };
                Controls.Add(name);

                // Add & Format modifier control
                Control modifier = attribute.GetEditControl();
                modifier.Location = new Point(0, 20);
                modifier.Size = new Size(200, 160);
                modifier.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                Controls.Add(modifier);

                // Make button that changes the state of the attribute
                Button change = new Button
                {
                    Location = new Point(0, 180),
                    Size = new Size(200, 20),
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
                };
                Controls.Add(change);

                if (exists) // If the attribute exists then it should be removable
                {
                    // Can't be deleted if attribute is delete locked
                    change.Enabled = !attribute.Format.DeleteLocked;

                    // Format remove Attribute button
                    change.Text = "Remove Attribute";

                    // Add events
                    change.Click += Remove_Click;
                }
                else // Otherwise it should be addable
                {
                    // Disable modifying attribute
                    modifier.Enabled = false;

                    // Format Add Attribute button
                    change.Text = "Add Attribute";

                    // Add events
                    change.Click += Add_Click;
                }
            }

            private void Add_Click(object sender, EventArgs e)
            {
                AttributeAdded?.Invoke(_attribute, new EventArgs());
            }

            private void Remove_Click(object sender, EventArgs e)
            {
                if (MessageBox.Show("Are you sure you want to remove this attribute?", "Remove", MessageBoxButtons.YesNo) == DialogResult.Yes) // Check that the user ment to click remove
                {
                    AttributeRemoved?.Invoke(_attribute, new EventArgs());
                }
            }
        }

        private Entity _entity;
        public Entity Entity // Used to set entity above
        {
            set
            {
                if (_entity != null)
                {
                    // Unbind events
                    _entity.AttributeAdded -= _entity_AttributeAdded;
                    _entity.AttributeRemoved -= _entity_AttributeRemoved;
                    _entity.AttributeValueChanged -= _entity_AttributeValueChanged;
                }

                _entity = value;

                if (_entity != null)
                {
                    // Bind events
                    _entity.AttributeAdded += _entity_AttributeAdded;
                    _entity.AttributeRemoved += _entity_AttributeRemoved;
                    _entity.AttributeValueChanged += _entity_AttributeValueChanged;
                }

                // Refresh this view
                EntityRefresh();
            }
        }

        // Collection needed to modify attribute formats
        private EntityCollection _entityCollection;
        public EntityCollection Entities // Used to set entities above
        {
            set
            {
                if (_entityCollection != null)
                {
                    // Unbind events
                    _entityCollection.AttributeGroupRemoved -= _entityCollection_AttributeGroupRemoved;
                    _entityCollection.AttributeGroupFormatChanged -= _entityCollection_AttributeGroupFormatChanged;
                    _entityCollection.AttributeGroupNameChanged -= _entityCollection_AttributeGroupNameChanged;
                }

                _entityCollection = value;

                if (_entityCollection != null)
                {
                    // Bind events
                    _entityCollection.AttributeGroupRemoved += _entityCollection_AttributeGroupRemoved;
                    _entityCollection.AttributeGroupFormatChanged += _entityCollection_AttributeGroupFormatChanged;
                    _entityCollection.AttributeGroupNameChanged += _entityCollection_AttributeGroupNameChanged;
                }
            }
        }

        private void _entityCollection_AttributeGroupRemoved(EntityCollection collection, AttributeGroupEventArgs eventArgs)
        {
            EntityRefresh();
        }

        private void _entity_AttributeRemoved(object o, EventArgs eventArgs)
        {
            EntityRefresh();
        }

        private void _entity_AttributeAdded(object o, EventArgs eventArgs)
        {
            EntityRefresh();
        }

        private void _entity_AttributeValueChanged(object o, EventArgs eventArgs)
        {
            EntityRefresh();
        }

        private void _entityCollection_AttributeGroupFormatChanged(EntityCollection collection, AttributeGroupEventArgs eventArgs)
        {
            EntityRefresh();
        }

        private void _entityCollection_AttributeGroupNameChanged(EntityCollection collection, AttributeGroupNameEventArgs eventArgs)
        {
            EntityRefresh();
        }

        public EntityDetailedDisplay()
        {
            InitializeComponent();
            EntityRefresh();
        }

        private void EntityRefresh()
        {
            // Reduce flickering
            this.SuspendLayout();

            // Clear current attribute display
            this.flowLayoutPanelAttributes.Controls.Clear();

            if (_entity != null)
            {
                // Loop through all attributes and add to display
                foreach (Entity.Attribute attribute in _entity.Attributes)
                {
                    // Make & format viewer
                    AttributeViewer attributeViewer = new AttributeViewer(attribute, true)
                    {
                        Size = new Size(200, 150)
                    };
                    this.flowLayoutPanelAttributes.Controls.Add(attributeViewer);

                    // Bind events
                    attributeViewer.AttributeRemoved += AttributeViewer_AttributeRemoved;
                }

                List<Entity.Attribute> unassignedAttributes = _entityCollection.GetUnassignedAttributes(_entity);
                if (unassignedAttributes.Count > 0 && flowLayoutPanelAttributes.Controls.Count > 0) // Only make a divider if there are unassigned attributes and normal attributes
                {
                    // Make a divider
                    flowLayoutPanelAttributes.SetFlowBreak(this.flowLayoutPanelAttributes.Controls[this.flowLayoutPanelAttributes.Controls.Count - 1], true);
                    Label divider = new Label
                    {
                        Size = new Size(flowLayoutPanelAttributes.Width, 20),
                        Text = "Unassigned Attributes: ",
                        Anchor = AnchorStyles.Left | AnchorStyles.Right
                    };
                    flowLayoutPanelAttributes.Controls.Add(divider);
                }

                // Loop through all unassigned attributes and add to display
                foreach (Entity.Attribute attribute in _entityCollection.GetUnassignedAttributes(_entity))
                {
                    // Make & format viewer
                    AttributeViewer attributeViewer = new AttributeViewer(attribute, false)
                    {
                        Size = new Size(200, 150),
                        BackColor = Color.LightGray
                    };
                    this.flowLayoutPanelAttributes.Controls.Add(attributeViewer);

                    // Bind events
                    attributeViewer.AttributeAdded += AttributeViewer_AttributeAdded;
                }
            }

            buttonDeleteEntity.Enabled = _entity != null;
            buttonAddAttibute.Enabled = _entity != null;
            buttonSaveEntity.Enabled = _entity != null;

            this.ResumeLayout();
        }

        private void AttributeViewer_AttributeAdded(Entity.Attribute attr, EventArgs eventArgs)
        {
            _entity.AddAttribute(attr);
        }

        private void AttributeViewer_AttributeRemoved(Entity.Attribute attr, EventArgs eventArgs)
        {
            _entity.RemoveAttribute(attr);
        }

        private void ButtonDeleteEntity_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this entity?", "Remove", MessageBoxButtons.YesNo) == DialogResult.Yes) // Check that the user ment to click remove
            {
                _entityCollection.RemoveEntity(_entity);
            }
        }

        private void ButtonAddEntity_Click(object sender, EventArgs e)
        {
            _entityCollection.AddEntity(new Entity());
        }

        private void ButtonAddAttibute_Click(object sender, EventArgs e)
        {
            //Generate a menu to select the type of attribute
            ContextMenu menu = new ContextMenu();
            foreach (Entity.AttributeType type in Enum.GetValues(typeof(Entity.AttributeType)))
            {
                void eventHandler(object o, EventArgs e2)
                {
                    Entity.AttributeFormat format = new Entity.AttributeFormat();
                    _entity.AddAttribute(Entity.MakeNewAttribute(type, "New Attribute", _entity, format));
                }
                menu.MenuItems.Add(Enum.GetName(typeof(Entity.AttributeType), type), eventHandler);
            }
            menu.Show(buttonAddAttibute, buttonAddAttibute.PointToClient(Cursor.Position));
        }

        private void ButtonSaveEntity_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                DefaultExt = "RPTSEntity",
                Filter = "Entity File|*.RPTSEntity|All Files|*.*"
            };
            fileDialog.ShowDialog();
            if (fileDialog.FileName != "")
            {
                try
                {
                    _entity.Save(fileDialog.FileName);
                }
                catch (JsonSerializationException exception)
                {
                    string message = "An error was encountered when trying to serialise the entity: " + exception.Message + "\n" + exception.StackTrace;
                    MessageBox.Show(message, "Export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonLoadEntity_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                DefaultExt = "RPTSEntity",
                Filter = "Entity File|*.RPTSEntity|All Files|*.*"
            };
            fileDialog.ShowDialog();
            if (fileDialog.FileName != "")
            {
                //try
                //{
                    Entity entity = Entity.Load(fileDialog.FileName);
                    _entityCollection.AddEntity(entity);
                //}
                //    catch (Exception exception)
                //    {
                //        string message;

                //        if (exception is UnauthorizedAccessException ||
                //            exception is PathTooLongException ||
                //            exception is DirectoryNotFoundException ||
                //            exception is FileNotFoundException)
                //        {
                //            message = "An error was encountered when trying to open the file: ";
                //        }
                //        else if (exception is JsonSerializationException ||
                //                 exception is JsonReaderException ||
                //                 exception is FormatException)
                //        {
                //            message = "An error was encountered when trying to deserialise the entity: ";
                //        }
                //        else
                //        {
                //            throw exception;
                //        }

                //        message += exception.Message;
                //        MessageBox.Show(message, "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
            }
        }
    }
}
