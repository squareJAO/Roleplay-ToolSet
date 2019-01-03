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
        // Colours of the cells when they do/don't have data in them
        private readonly Color InactiveColor = Color.Gray;
        private readonly Color ActiveColor = Color.White;

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
                    _entityCollection.AttributeGroupNameChanged -= _entityCollection_AttributeGroupNameChanged;

                    // Delete all rows & columns
                    this.RowCount = 0;
                    this.ColumnCount = 0;
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

                    // Refresh entities
                    foreach (Entity entity in _entityCollection.Entities)
                    {
                        AddEntity(entity);
                    }
                }
            }
        }

        public EntityList()
        {
            // Set default style
            this.DefaultCellStyle.BackColor = InactiveColor;

            // Add events
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
            List<Entity.Attribute> attributes = entity.Attributes;
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
                SetCellValue(row.Cells[attribute.GroupName], null);

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
                SetCellValue(row.Cells[attribute.GroupName], attribute.GetListViewValue());
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
                    case Entity.AttributeType.Image:
                        column = new DataGridViewImageColumn()
                        {
                            //ValuesAreIcons = true,
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };
                        column.DefaultCellStyle.NullValue = null; // No weird icon if no image
                        break;
                    default:
                        column = new DataGridViewTextBoxColumn();
                        break;
                }
                // Add column to end
                this.Columns.Add(column);

                // Format column
                column.Name = attribute.GroupName;
            }

            // Add to cell
            SetCellValue(row.Cells[attribute.GroupName], attribute.GetListViewValue());
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
        /// Gets the entities that are currrently selected
        /// </summary>
        /// <returns>The entities that are currently selected, or an empty array. Never null</returns>
        public Entity[] GetSelectedEntities()
        {
            Entity[] entities = new Entity[SelectedRows.Count];
            for (int i = 0; i < SelectedRows.Count; i++)
            {
                entities[i] = GetRowEntity(SelectedRows[i]);
            }
            return entities;
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

                ShowGroupContextMenu(attrName);
            }
        }

        private void SetCellValue(DataGridViewCell cell, object data)
        {
            cell.Style.BackColor = data == null ? InactiveColor : ActiveColor;
            cell.Value = data;
        }

        private void ShowGroupContextMenu(string attrName)
        {
            // Make a context menu
            ContextMenu menu = new ContextMenu();

            // Change name button
            MenuItem itemChangeName = new MenuItem("Change Name", _entityCollection.ChangeNameFormEventGenerator(attrName))
            {
                Enabled = !_entityCollection.AttributeGroups[attrName].Format.NameLocked
            };
            menu.MenuItems.Add(itemChangeName);

            // Change default button DefaultValueFormEventGenerator
            MenuItem itemChangeDefault = new MenuItem("Change Default Value", _entityCollection.DefaultValueFormEventGenerator(attrName));
            menu.MenuItems.Add(itemChangeDefault);

            // Delete button
            MenuItem itemDelete = new MenuItem("Delete Group", (a, b) =>
            {
                if (MessageBox.Show($"Are you sure you want to delete {attrName} from all entities?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _entityCollection.DeleteGroup(attrName);
                }
            })
            {
                Enabled = !_entityCollection.AttributeGroups[attrName].Format.DeleteLocked
            };
            menu.MenuItems.Add(itemDelete);

            // Delete if empty checkbox
            MenuItem itemDeleteIfEmpty = new MenuItem("Keep If Empty", (a, b) =>
            {
                _entityCollection.ChangeAttributeGroupFormat(attrName,
                    new Entity.AttributeFormat(_entityCollection.AttributeGroups[attrName].Format,
                        deleteIfEmpty: !_entityCollection.AttributeGroups[attrName].Format.DeleteIfEmpty));
            })
            {
                Checked = !_entityCollection.AttributeGroups[attrName].Format.DeleteIfEmpty,
                Enabled = !_entityCollection.AttributeGroups[attrName].Format.DeleteLocked
            };
            menu.MenuItems.Add(itemDeleteIfEmpty);

            // Show
            menu.Show(this, this.PointToClient(Cursor.Position));
        }
    }
}
