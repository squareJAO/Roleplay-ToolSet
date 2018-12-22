using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RoleplayToolSet
{
    class EntityList : DataGridView
    {
        private EntityCollection _entityCollection;
        public EntityCollection Entities // Used to set entities above
        {
            set
            {
                if (_entityCollection != null)
                {
                    // Unbind events
                    _entityCollection.EntityAdded -= _entityCollection_EntityAdded;
                    _entityCollection.EntityAttributeAdded -= _entityCollection_EntityAttributeAdded;
                    _entityCollection.EntityAttributeValueChanged -= _entityCollection_EntityAttributeValueChanged;
                    _entityCollection.EntityAttributeRemoved -= _entityCollection_EntityAttributeRemoved;
                    _entityCollection.EntityRemoved -= _entityCollection_EntityRemoved;
                    _entityCollection.AttributeGroupRemoved -= _entityCollection_AttributeGroupRemoved;
                    _entityCollection.AttributeGroupNameChanged += _entityCollection_AttributeGroupNameChanged;
                }

                _entityCollection = value;

                if (_entityCollection != null)
                {
                    // Bind events
                    _entityCollection.EntityAdded += _entityCollection_EntityAdded;
                    _entityCollection.EntityAttributeAdded += _entityCollection_EntityAttributeAdded;
                    _entityCollection.EntityAttributeValueChanged += _entityCollection_EntityAttributeValueChanged;
                    _entityCollection.EntityAttributeRemoved += _entityCollection_EntityAttributeRemoved;
                    _entityCollection.EntityRemoved += _entityCollection_EntityRemoved;
                    _entityCollection.AttributeGroupRemoved += _entityCollection_AttributeGroupRemoved;
                    _entityCollection.AttributeGroupNameChanged += _entityCollection_AttributeGroupNameChanged;
                }
            }
        }

        public EntityList()
        {
            this.ColumnHeaderMouseClick += EntityList_ColumnHeaderMouseClick;
        }

        private void _entityCollection_EntityAdded(Entity entity, EventArgs eventArgs)
        {
            AddEntity(entity);
        }

        private void _entityCollection_EntityRemoved(Entity entity, EventArgs eventArgs)
        {
            RemoveEntity(entity);
        }

        private void _entityCollection_EntityAttributeRemoved(Entity.Attribute attr, EventArgs eventArgs)
        {
            this.RemoveAttribute(attr.ParentEntity, attr);
        }

        private void _entityCollection_EntityAttributeValueChanged(Entity.Attribute attr, EventArgs eventArgs)
        {
            this.UpdateAttribute(attr.ParentEntity, attr);
        }

        // Changing occurs before it has been changed and changed occurs after
        //private void _entityCollection_EntityAttributeFormatChanging(Entity.Attribute attr, EventArgs eventArgs)
        //{
        //    // Remove the attribute
        //    RemoveAttribute(attr.ParentEntity, attr);
        //}
        //private void _entityCollection_EntityAttributeFormatChanged(Entity.Attribute attr, EventArgs eventArgs)
        //{
        //    // Re-add the attribute
        //    AddAttribute(attr.ParentEntity, attr);
        //}

        private void _entityCollection_AttributeGroupNameChanged(EntityCollection collection, AttributeGroupNameEventArgs eventArgs)
        {
            RenameColumn(eventArgs.OldName, eventArgs.NewName);
        }

        private void _entityCollection_EntityAttributeAdded(Entity.Attribute attr, EventArgs eventArgs)
        {
            AddAttribute(attr.ParentEntity, attr);
        }

        private void _entityCollection_AttributeGroupRemoved(EntityCollection collection, AttributeGroupEventArgs eventArgs)
        {
            RemoveColumn(eventArgs.GroupName);
        }

        /// <summary>
        /// Gets the entity associated with the row
        /// </summary>
        /// <param name="row">The row to get the entity of</param>
        /// <returns>The entity associated with the row</returns>
        private Entity GetRowEntity(DataGridViewRow row)
        {
            if (row.Tag != null)
            {
                return (Entity)row.Tag;
            }
            return null;
        }

        /// <summary>
        /// Gets the row associated with the entity
        /// </summary>
        /// <param name="entity">The entity to get the row of</param>
        /// <returns>The row associated with the entity</returns>
        private DataGridViewRow GetEntityRow(Entity entity)
        {
            // Iterates through all of the rows to find the entity
            // If there is a better way to search than itteration I can't find it on google
            foreach (DataGridViewRow row in this.Rows)
            {
                if (GetRowEntity(row) == entity)
                {
                    return row;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a single entity to the list of entities
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        private void AddEntity(Entity entity)
        {
            // A column must be present when rows are added
            DataGridViewColumn fakeColumn = null;
            if (this.ColumnCount == 0)
            {
                this.ColumnCount++;
                fakeColumn = this.Columns[this.ColumnCount - 1];
            }

            // Make a new row
            RowCount++;
            DataGridViewRow row = Rows[RowCount - 1];
            row.Tag = entity;

            // Workaround as a new row is selected before Tag is set so toggling it being selected triggers all relevent events again
            if (RowCount == 1)
            {
                SetSelectedRowCore(RowCount - 1, false);
                SetSelectedRowCore(RowCount - 1, true);
            }

            // Get and loop over attributes to make any needed new columns and then populate the above dictionary
            List<Entity.Attribute> attributes = entity.GetAttributes();
            foreach (Entity.Attribute attribute in attributes)
            {
                AddAttribute(row, attribute);
            }

            // Remove any fake columns
            if (fakeColumn != null)
            {
                this.Columns.Remove(fakeColumn);
            }
        }

        /// <summary>
        /// Removes an entity from the list
        /// </summary>
        /// <param name="entity">The entity to no longer display</param>
        private void RemoveEntity(Entity entity)
        {
            DataGridViewRow row = GetEntityRow(entity); // Get the row that the entity is shown in
            if (row != null) // If entity is in the list
            {
                // Remove row
                this.Rows.Remove(row);

                // Check if anything needs to go
                CullUnusedColumns();
            }
        }

        /// <summary>
        /// Removes an attribute
        /// </summary>
        /// <param name="entity">The entity that the attribute belongs to</param>
        /// <param name="attribute">The attribute to be removed</param>
        private void RemoveAttribute(Entity entity, Entity.Attribute attribute)
        {
            DataGridViewRow row = GetEntityRow(entity); // Get the row that the entity is shown in
            if (row != null && this.Columns.Contains(attribute.GroupName)) // Check that the entity is currently displayed
            {
                // Remove data
                DataGridViewColumn column = this.Columns[attribute.GroupName];
                row.Cells[attribute.GroupName].Value = null;

                // Check if anything needs to go
                CullUnusedColumns();
            }
        }

        /// <summary>
        /// Updates an attribute
        /// </summary>
        /// <param name="entity">The entity that the attribute belongs to</param>
        /// <param name="attribute">The attribute that has been changed</param>
        private void UpdateAttribute(Entity entity, Entity.Attribute attribute)
        {
            DataGridViewRow row = GetEntityRow(entity); // Get the row that the entity is shown in
            if (row != null && this.Columns.Contains(attribute.GroupName)) // Check that the entity is currently displayed
            {
                // Update data
                row.Cells[attribute.GroupName].Value = attribute.GetListViewValue();
            }
        }

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="entity">The entity to add the attribute to</param>
        /// <param name="attribute">The attribute to be added</param>
        private void AddAttribute(Entity entity, Entity.Attribute attribute)
        {
            DataGridViewRow row = GetEntityRow(entity); // Get the row that the entity is shown in
            if (row == null)
            {
                AddEntity(entity);
                AddAttribute(entity, attribute);
            }
            else
            {
                AddAttribute(row, attribute);
            }
        }

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="row">The row to add the attribute to</param>
        /// <param name="attribute">The attribute to be added</param>
        private void AddAttribute(DataGridViewRow row, Entity.Attribute attribute)
        {
            // See if the list already has a column under ColumnName
            DataGridViewColumn column = this.Columns[attribute.GroupName]; // Get the first column with the column Name

            // If no column exists make a column
            if (column == null)
            {
                // Make new column
                switch (attribute.GetAttributeType())
                {
                    case Entity.AttributeType.Numeric:
                        column = new DataGridViewTextBoxColumn();
                        break;
                    case Entity.AttributeType.String:
                        column = new DataGridViewTextBoxColumn();
                        break;
                    case Entity.AttributeType.Image:
                        column = new DataGridViewImageColumn()
                        {
                            //ValuesAreIcons = true,
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        column.DefaultCellStyle.NullValue = null; // No weird icon if no image
                        break;
                    case Entity.AttributeType.Bool:
                        column = new DataGridViewTextBoxColumn();
                        break;
                    default:
                        break;
                }
                // Add column to end
                this.Columns.Add(column);

                // Format column
                column.Name = attribute.GroupName;
            }

            // Add to cell
            row.Cells[attribute.GroupName].Value = attribute.GetListViewValue();
        }

        /// <summary>
        /// Gets the entity that is currrently selected
        /// </summary>
        /// <returns>The first Entity that is selected, or null if nothing is selected</returns>
        public Entity GetSelectedEntity()
        {
            if (this.SelectedRows.Count > 0)
            {
                return GetRowEntity(this.SelectedRows[0]);
            }
            return null;
        }

        /// <summary>
        /// Remove all columns that have no cells with data in them
        /// </summary>
        public void CullUnusedColumns()
        {
            for (int columnIndex = this.ColumnCount - 1; columnIndex >= 0; columnIndex--) // Count back so columns can be removed
            {
                DataGridViewColumn column = this.Columns[columnIndex];
                if (!_entityCollection.AttributeGroups.ContainsKey(column.Name)) // If entity collection doesn't contain the column then it shouldn't be shown
                {
                    this.Columns.Remove(column);
                }
            }
        }

        /// <summary>
        /// Used by events
        /// </summary>
        /// <param name="columnName">The name of the column to remove</param>
        private void RemoveColumn(string columnName)
        {
            this.Columns.Remove(this.Columns[columnName]);
        }

        /// <summary>
        /// Used by 
        /// </summary>
        /// <param name="oldName">The name to change</param>
        /// <param name="newName">The name to change to</param>
        private void RenameColumn(string oldName, string newName)
        {
            this.Columns[oldName].Name = newName;
        }

        private void EntityList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string attrName = this.Columns[e.ColumnIndex].Name; // Get the name of the attribute

                _entityCollection.ShowGroupContextMenu(attrName, this);
            }
        }
    }
}
