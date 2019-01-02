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
    public partial class EntityManager : UserControl
    {
        private EntityCollection _entityCollection;
        public EntityCollection Entities // Used to set entities above
        {
            set
            {
                _entityCollection = value;

                // Pass down
                entityList.Entities = _entityCollection;
                entityDetailedDisplay.Entities = _entityCollection;
            }
        }

        private Entity[] _selectedEntities; // Stores the currently selected entities

        public EntityManager()
        {
            InitializeComponent();
            SetEntityToSelected();
        }

        /// <summary>
        /// Sets the GUI based on the selected entities
        /// </summary>
        private void SetEntityToSelected()
        {
            _selectedEntities = entityList.GetSelectedEntities();

            // Only show detailed display if one entity selected
            if (_selectedEntities.Length == 1)
            {
                entityDetailedDisplay.Entity = _selectedEntities[0];

                // Set button states
                buttonDeleteEntity.Enabled = true;
                buttonDeleteEntity.Text = "Delete Entity";
                buttonSaveEntity.Enabled = true;
                buttonSaveEntity.Text = "Export Entity";
                buttonAddAttibute.Enabled = true;
            }
            else
            {
                entityDetailedDisplay.Entity = null;

                // Set button states
                buttonAddAttibute.Enabled = false;
                if (_selectedEntities.Length > 1)
                {
                    buttonDeleteEntity.Enabled = true;
                    buttonDeleteEntity.Text = "Delete Entities";
                    buttonSaveEntity.Enabled = true;
                    buttonSaveEntity.Text = "Export Entities";
                }
                else // Otherwise no entities
                {
                    buttonDeleteEntity.Enabled = false;
                    buttonSaveEntity.Enabled = false;
                }
            }
        }

        private void EntityList_SelectionChanged(object sender, EventArgs e)
        {
            SetEntityToSelected();
        }

        private void buttonDeleteEntity_Click(object sender, EventArgs e)
        {

            if (_selectedEntities.Length >= 1)
            {
                if (MessageBox.Show("Are you sure you want to remove the selected entities?", "Remove", MessageBoxButtons.YesNo) == DialogResult.Yes) // Check that the user ment to click remove
                {
                    // Reduce flickering
                    this.SuspendLayout();

                    // Loop & remove
                    foreach (var entity in _selectedEntities)
                    {
                        _entityCollection.RemoveEntity(entity);
                    }

                    this.ResumeLayout();
                }
            }
        }

        private void buttonAddEntity_Click(object sender, EventArgs e)
        {
            _entityCollection.AddEntity(new Entity());
        }

        private void buttonSaveEntity_Click(object sender, EventArgs e)
        {
            // Saving one and saving many files are fairly different routines so split them to save confusion
            if (_selectedEntities.Length == 1)
            {
                SaveFileDialog fileDialog = new SaveFileDialog
                {
                    DefaultExt = "RPTSEntity",
                    Filter = "Entity File|*.RPTSEntity|All Files|*.*"
                };
                fileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(fileDialog.FileName))
                {
                    try // File handling so expect exceptions
                    {
                        _selectedEntities[0].Save(fileDialog.FileName); 
                    }
                    catch (JsonSerializationException exception)
                    {
                        string message = "An error was encountered when trying to serialise the entity: " + exception.Message + "\n" + exception.StackTrace;
                        MessageBox.Show(message, "Export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (_selectedEntities.Length > 1)
            {
                FolderBrowserDialog fileDialog = new FolderBrowserDialog
                {
                    Description = "Select folder to save entities to"
                };
                fileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(fileDialog.SelectedPath))
                {
                    for (int i = 0; i < _selectedEntities.Length; i++)
                    {
                        string name = _selectedEntities[i].NameAttribute.Value.GetPlainText();
                        string path = Path.Combine(fileDialog.SelectedPath, name + ".RPTSEntity");
                        try // File handling so expect exceptions
                        {
                            _selectedEntities[i].Save(path, false); // Write file with file name & false=>make unique
                        }
                        catch (JsonSerializationException exception)
                        {
                            string message = "An error was encountered when trying to serialise the entity: " + exception.Message + "\n" + exception.StackTrace;
                            MessageBox.Show(message, "Export error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void buttonLoadEntity_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                DefaultExt = "RPTSEntity",
                Filter = "Entity File|*.RPTSEntity|All Files|*.*",
                Multiselect = true
            };
            fileDialog.ShowDialog();
            if (fileDialog.FileNames.Length != 0)
            {
                // Loop through all selected files
                for (int i = 0; i < fileDialog.FileNames.Length; i++)
                {
                    string name = fileDialog.FileNames[i];
                    if (!string.IsNullOrEmpty(name))
                    {
                        Entity entity = null;
                        try // File handling so expect exceptions
                        {
                            entity = Entity.Load(name);
                        }
                        catch (Exception exception) // Catch everything and then filter back out
                        {
                            string message;

                            if (exception is UnauthorizedAccessException ||
                                exception is PathTooLongException ||
                                exception is DirectoryNotFoundException ||
                                exception is FileNotFoundException)
                            {
                                message = "An error was encountered when trying to open the file: ";
                            }
                            else if (exception is JsonSerializationException ||
                                        exception is JsonReaderException ||
                                        exception is FormatException)
                            {
                                message = "An error was encountered when trying to deserialise the entity: ";
                            }
                            else
                            {
                                throw exception;
                            }

                            message += exception.Message;
                            MessageBox.Show(message, "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Check that no exception was called
                        if (entity != null)
                        {
                            _entityCollection.AddEntity(entity);
                        }
                    }
                }
            }
        }

        private void buttonAddAttibute_Click(object sender, EventArgs e)
        {
            if (_selectedEntities.Length == 1)
            {
                //Generate a menu to select the type of attribute
                ContextMenu menu = new ContextMenu();
                foreach (Entity.AttributeType type in Enum.GetValues(typeof(Entity.AttributeType)))
                {
                    // Add a menu item for each Type of Attribute
                    menu.MenuItems.Add(Enum.GetName(typeof(Entity.AttributeType), type), (o, e2) =>
                    {
                        Entity.AttributeFormat format = new Entity.AttributeFormat();
                        _selectedEntities[0].AddAttribute(Entity.MakeNewAttribute(type, "New Attribute", _selectedEntities[0], format));
                    });
                }
                menu.Show(buttonAddAttibute, buttonAddAttibute.PointToClient(Cursor.Position));
            }
        }
    }
}
