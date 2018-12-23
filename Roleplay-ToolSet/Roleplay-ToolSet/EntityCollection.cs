using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RoleplayToolSet
{
    public class AttributeGroupEventArgs : EventArgs
    {
        public string GroupName { get; private set; }
        public AttributeGroupEventArgs(string name)
        {
            GroupName = name;
        }
    }

    public delegate void AttributeGroupEventHandler(EntityCollection collection, AttributeGroupEventArgs eventArgs);

    public class AttributeGroupNameEventArgs : EventArgs
    {
        public string OldName { get; private set; }
        public string NewName { get; private set; }
        public AttributeGroupNameEventArgs(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }

    public delegate void AttributeGroupNameEventHandler(EntityCollection collection, AttributeGroupNameEventArgs eventArgs);

    [Serializable]
    public class EntityCollection
    {
        public List<Entity> Entities { get; private set; }

        [field: NonSerialized]
        public event EntityEventHandler EntityAdded;

        [field: NonSerialized]
        public event EntityEventHandler EntityRemoved;

        [field: NonSerialized]
        public event EntityAttributeEventHandler EntityAttributeAdded;

        [field: NonSerialized]
        public event EntityAttributeEventHandler EntityAttributeRemoved;

        [field: NonSerialized]
        public event EntityAttributeEventHandler EntityAttributeValueChanged;

        [field: NonSerialized]
        public event AttributeGroupEventHandler AttributeGroupFormatChanged;

        [field: NonSerialized]
        public event AttributeGroupNameEventHandler AttributeGroupNameChanged;

        [field: NonSerialized]
        public event AttributeGroupEventHandler AttributeGroupRemoved;

        // A list that stores all of the types, formats and users of each of the attribute groups
        public Dictionary<string, (Entity.AttributeType Type, Entity.AttributeFormat Format, List<Entity> Users)> AttributeGroups = new Dictionary<string, (Entity.AttributeType Type, Entity.AttributeFormat Format, List<Entity> Users)>();

        public EntityCollection()
        {
            Entities = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);

            // Get and loop over attributes to find attribute groups and then increase their user count
            List<Entity.Attribute> attributes = entity.Attributes;
            foreach (Entity.Attribute attribute in attributes)
            {
                AddAttribute(entity, attribute);
            }
            // Check if this causes any attributes to be removed
            //CullUnusedGroups();

            // Ivoke envents
            EntityAdded?.Invoke(entity, new EventArgs());

            // Bind events
            entity.AttributeAdded += Entity_AttributeAdded;
            entity.AttributeRemoved += Entity_AttributeRemoved;
            entity.AttributeValueChanged += Entity_AttributeValueChanged;
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);

            // Unbind events
            entity.AttributeAdded -= Entity_AttributeAdded;
            entity.AttributeRemoved -= Entity_AttributeRemoved;
            entity.AttributeValueChanged -= Entity_AttributeValueChanged;

            // Get and loop over attributes to find attribute groups and then decrease their user count
            List<Entity.Attribute> attributes = entity.Attributes;
            foreach (Entity.Attribute attribute in attributes)
            {
                AttributeGroups[attribute.GroupName].Users.Remove(entity);
            }
            // Check if this causes any attributes to be removed
            CullUnusedGroups();

            // Ivoke envents
            EntityRemoved?.Invoke(entity, new EventArgs());
        }

        private void Entity_AttributeValueChanged(Entity.Attribute attr, EventArgs eventArgs)
        {
            EntityAttributeValueChanged?.Invoke(attr, eventArgs);
        }

        private void Entity_AttributeRemoved(Entity.Attribute attr, EventArgs eventArgs)
        {
            // Decrease users
            AttributeGroups[attr.GroupName].Users.Remove(attr.ParentEntity);
            // Check if this causes any attributes to be removed
            CullUnusedGroups();

            // Invoke removed method
            EntityAttributeRemoved?.Invoke(attr, eventArgs);
        }

        private void Entity_AttributeAdded(Entity.Attribute attr, EventArgs eventArgs)
        {
            AddAttribute(attr.ParentEntity, attr);

            // Invoke added method
            EntityAttributeAdded?.Invoke(attr, eventArgs);
        }

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="entity">The entity the attribute was added to</param>
        /// <param name="attribute">The attribute to be added</param>
        private void AddAttribute(Entity entity, Entity.Attribute attribute)
        {
            // See if the list already has a group under ColumnName
            string group = this.AttributeGroups.Keys.FirstOrDefault(x => x == attribute.GroupName); // Get the first group with the group Name

            // Ensure that columns share types and entity isn't already in column
            int nameAdd = 1;
            while (group != null &&
                (AttributeGroups[group].Type != attribute.GetAttributeType() || AttributeGroups[group].Users.Contains(entity)))
            {
                nameAdd++;
                // Incrementally change name
                group = this.AttributeGroups.Keys.FirstOrDefault(x => x == attribute.GroupName + nameAdd);
            }

            // If name needs to be changed
            if (nameAdd != 1)
            {
                attribute.ChangeGroupName(attribute.GroupName + nameAdd); // Change name
            }

            // If no group exists make a group
            if (group == null)
            {
                // Add to dictionary
                AttributeGroups.Add(attribute.GroupName, (attribute.GetAttributeType(), attribute.Format, new List<Entity>()));
            }
            else
            {
                // Otherwise ensure that attribute shares format
                attribute.ChangeFormat(AttributeGroups[attribute.GroupName].Format);
            }

            // Incriment dict
            AttributeGroups[attribute.GroupName].Users.Add(entity);
        }

        /// <summary>
        /// Remove all attribute groups that no entities use
        /// </summary>
        public void CullUnusedGroups()
        {
            for (int columnNameIndex = AttributeGroups.Keys.Count - 1; columnNameIndex >= 0; columnNameIndex--) // Count back so columns can be removed
            {
                string columnName = AttributeGroups.Keys.ElementAt(columnNameIndex);
                if (AttributeGroups[columnName].Users.Count == 0 && AttributeGroups[columnName].Format.DeleteIfEmpty) // No users => no elements that use this attribute
                {
                    AttributeGroups.Remove(columnName);

                    AttributeGroupRemoved?.Invoke(this, new AttributeGroupEventArgs(columnName));
                }
            }
        }

        /// <summary>
        /// Gets all of the attributes of an entity that could exist with the current groups but don't
        /// </summary>
        /// <param name="entity">The entity to get the unassigned attributes of</param>
        /// <returns>A list of unassigned attributes</returns>
        public List<Entity.Attribute> GetUnassignedAttributes(Entity entity)
        {
            List<Entity.Attribute> attributes = new List<Entity.Attribute>(); // Make a new list

            // Make a default attribute for each group
            foreach (string groupName in AttributeGroups.Keys)
            {
                if (!entity.Attributes.Exists(attr => attr.GroupName == groupName))
                {
                    Entity.AttributeFormat format = AttributeGroups[groupName].Format;

                    attributes.Add(Entity.MakeNewAttribute(AttributeGroups[groupName].Type, groupName, entity, format));
                }
            }

            // Return all of the new attributes
            return attributes;
        }

        /// <summary>
        /// Changes the name of an attribute group
        /// </summary>
        /// <param name="oldName">The string before it was changed</param>
        /// <param name="newName">The string to change it to</param>
        public void ChangeAttributeGroupName(string oldName, string newName)
        {
            if (oldName == newName) // If the name hasn't changed do nothing
            { } 
            else if (AttributeGroups.ContainsKey(newName)) // Otherwise if the given name is already in the dict something else is called that
            {
                MessageBox.Show(newName + " is already an attribute group");
            }
            else if (!AttributeGroups.ContainsKey(oldName)) // Otherwise if the oldName isn't in the groups something went wrong
            {
                MessageBox.Show("Could not find group " + oldName, "This shouldn't happen, please alert someone", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (AttributeGroups[oldName].Format.NameLocked) // Otherwise if the name is locked then don't allow it to be changed
            {
                MessageBox.Show(oldName + " has its name locked");
            }
            else // Otherwise change the name
            {
                // Change the dicts
                AttributeGroups.Add(newName, AttributeGroups[oldName]);
                AttributeGroups.Remove(oldName);

                // Loop through all atributes and change their names
                foreach (Entity entity in AttributeGroups[newName].Users)
                {
                    foreach (Entity.Attribute attribute in entity.Attributes)
                    {
                        if (attribute.GroupName == oldName)
                        {
                            attribute.ChangeGroupName(newName);
                        }
                    }
                }

                // Invoke changed event
                AttributeGroupNameChanged?.Invoke(this, new AttributeGroupNameEventArgs(oldName, newName));
            }
        }

        /// <summary>
        /// Changes the format of an attribute group
        /// </summary>
        /// <param name="name">The name of the format</param>
        /// <param name="newFormat">The format to change it to</param>
        public void ChangeAttributeGroupFormat(string name, Entity.AttributeFormat newFormat)
        {
            foreach (Entity entity in AttributeGroups[name].Users)
            {
                foreach (Entity.Attribute attribute in entity.Attributes)
                {
                    if (attribute.GroupName == name)
                    {
                        attribute.ChangeFormat(newFormat);
                    }
                }
            }

            AttributeGroups[name] = (AttributeGroups[name].Type, newFormat, AttributeGroups[name].Users);

            // Check if this causes any attributes to be removed
            CullUnusedGroups();

            // Call event
            AttributeGroupFormatChanged?.Invoke(this, new AttributeGroupEventArgs(name));
        }

        public void ShowGroupContextMenu(string attrName, Control control)
        {
            // Make a context menu
            ContextMenu menu = new ContextMenu();
            // Change name button
            MenuItem itemChangeName = new MenuItem("Change Name", ChangeNameGenerator(attrName))
            {
                Enabled = !AttributeGroups[attrName].Format.NameLocked
            };
            menu.MenuItems.Add(itemChangeName);
            // Delete if empty checkbox
            MenuItem itemDeleteIfEmpty = new MenuItem("Delete If Empty", (a, b) =>
            {
                ChangeAttributeGroupFormat(attrName,
                    new Entity.AttributeFormat(AttributeGroups[attrName].Format,
                        deleteIfEmpty: !AttributeGroups[attrName].Format.DeleteIfEmpty));
            })
            {
                Checked = AttributeGroups[attrName].Format.DeleteIfEmpty,
                Enabled = !AttributeGroups[attrName].Format.DeleteLocked
            };
            menu.MenuItems.Add(itemDeleteIfEmpty);
            menu.Show(control, control.PointToClient(Cursor.Position));
        }

        /// <summary>
        /// Generates a handler to generate a form to input a new attribute group name in
        /// </summary>
        /// <param name="current">The current name of the attribute</param>
        /// <returns>A handler to create a form</returns>
        private EventHandler ChangeNameGenerator(string current)
        {
            void ChangeName(object sender, EventArgs e)
            {
                Form form = new Form();
                Label label = new Label();
                TextBox textBox = new TextBox();
                Button buttonOk = new Button();
                Button buttonCancel = new Button();

                form.Text = "Attribute Name";
                label.Text = "Enter new name";
                textBox.Text = current;

                buttonOk.Text = "OK";
                buttonCancel.Text = "Cancel";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;

                label.SetBounds(9, 20, 372, 13);
                textBox.SetBounds(12, 36, 372, 20);
                buttonOk.SetBounds(228, 72, 75, 23);
                buttonCancel.SetBounds(309, 72, 75, 23);

                label.AutoSize = true;
                textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                form.ClientSize = new Size(396, 107);
                form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
                form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                DialogResult dialogResult = form.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    ChangeAttributeGroupName(current, textBox.Text);
                }
            }

            return ChangeName;
        }
    }
}
