using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoleplayToolSet
{
    public partial class EntityDetailedDisplay : FlowLayoutPanel
    {
        /// <summary>
        /// Displays a single detail from an entity
        /// </summary>
        private class AttributeViewer : Panel
        {
            private Entity.Attribute _attribute;
            private EntityCollection _entityCollection;
            private bool _exists;

            /// <summary>
            /// Creates a viewer for an attribute that allow for data to be changed and the attribute to be added/removed
            /// </summary>
            /// <param name="attribute">The attribute to display</param>
            /// <param name="exists">Whether the attribute already belongs to the entity or if it is a possible attribute</param>
            public AttributeViewer(Entity.Attribute attribute, bool exists, EntityCollection collection) // Takes parameters because it's private and I'm a rule breaker
            {
                _attribute = attribute;
                _entityCollection = collection;
                _exists = exists;

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
                name.MouseClick += (object o, MouseEventArgs e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ShowGroupContextMenu();
                    }
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

            private void ShowGroupContextMenu()
            {
                // Make a context menu
                ContextMenu menu = new ContextMenu();
                // Change name button
                MenuItem itemChangeName = new MenuItem("Change Name", _entityCollection.ChangeNameFormEventGenerator(_attribute.GroupName))
                {
                    Enabled = !_entityCollection.AttributeGroups[_attribute.GroupName].Format.NameLocked
                };
                menu.MenuItems.Add(itemChangeName);

                if (_exists)
                {
                    // Delete button
                    MenuItem itemDelete = new MenuItem("Delete", (a, b) =>
                    {
                        ShowRemoveDialogue();
                    })
                    {
                        Enabled = !_entityCollection.AttributeGroups[_attribute.GroupName].Format.DeleteLocked
                    };
                    menu.MenuItems.Add(itemDelete);
                }
                else
                {
                    // Add button
                    MenuItem itemAdd = new MenuItem("Add", (a, b) =>
                    {
                        AddAttribute();
                    });
                    menu.MenuItems.Add(itemAdd);
                }

                menu.Show(this, this.PointToClient(Cursor.Position));
            }

            private void Add_Click(object sender, EventArgs e)
            {
                AddAttribute();
            }

            private void AddAttribute()
            {
                if (!_exists)
                {
                    _attribute.ParentEntity.AddAttribute(_attribute);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Can't add an attribute that has already been added");
                }
            }

            private void Remove_Click(object sender, EventArgs e)
            {
                ShowRemoveDialogue();
            }

            private void ShowRemoveDialogue()
            {
                if (MessageBox.Show("Are you sure you want to remove this attribute?", "Remove", MessageBoxButtons.YesNo) == DialogResult.Yes) // Check that the user ment to click remove
                {
                    _attribute.ParentEntity.RemoveAttribute(_attribute);
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
            EntityRefresh();
        }

        private void EntityRefresh()
        {
            // Reduce flickering
            this.SuspendLayout();

            // Clear current attribute display
            this.Controls.Clear();

            if (_entity != null)
            {
                // Loop through all attributes and add to display
                foreach (Entity.Attribute attribute in _entity.Attributes)
                {
                    // Make & format viewer
                    AttributeViewer attributeViewer = new AttributeViewer(attribute, true, _entityCollection)
                    {
                        Size = new Size(200, 150)
                    };
                    this.Controls.Add(attributeViewer);
                }

                List<Entity.Attribute> unassignedAttributes = _entityCollection.GetUnassignedAttributes(_entity);
                if (unassignedAttributes.Count > 0 && this.Controls.Count > 0) // Only make a divider if there are unassigned attributes and normal attributes
                {
                    // Make a divider
                    this.SetFlowBreak(this.Controls[this.Controls.Count - 1], true);
                    Label divider = new Label
                    {
                        Size = new Size(this.Width, 20),
                        Text = "Unassigned Attributes: ",
                        Anchor = AnchorStyles.Left | AnchorStyles.Right
                    };
                    this.Controls.Add(divider);
                }

                // Loop through all unassigned attributes and add to display
                foreach (Entity.Attribute attribute in _entityCollection.GetUnassignedAttributes(_entity))
                {
                    // Make & format viewer
                    AttributeViewer attributeViewer = new AttributeViewer(attribute, false, _entityCollection)
                    {
                        Size = new Size(200, 150),
                        BackColor = Color.LightGray
                    };
                    this.Controls.Add(attributeViewer);
                }
            }

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
    }
}
