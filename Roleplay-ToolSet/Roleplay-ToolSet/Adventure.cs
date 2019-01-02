using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace RoleplayToolSet
{
    [JsonObject]
    public class Adventure
    {
        [JsonProperty("time")]
        public WorldTime Time { get; set; }
        [JsonProperty("entities")]
        public EntityCollection Entities { get; set; }
        [JsonIgnore]
        public string SavePath { get; set; } = null;

        [JsonIgnore]
        public bool Changed { get; private set; } // Whether anything has been changed since the last save

        public Adventure()
            : this(new EntityCollection(), new WorldTime())
        { }

        [JsonConstructor]
        private Adventure(EntityCollection entities, WorldTime time)
        {
            // Assign values
            Entities = entities;
            Time = time;
            Changed = false;

            // Bind events
            Time.TimeChanged += Time_TimeChanged;
            Entities.Changed += Entities_Changed;
        }

        private void Entities_Changed(object sender, EventArgs e)
        {
            Changed = true;
        }

        private void Time_TimeChanged(object sender, EventArgs e)
        {
            Changed = true;
        }

        private const string FileIdentifier = "RoleplayToolSetAdventureData";

        /// <summary>
        /// Saves this to a file, overriding or creating
        /// </summary>
        /// <param name="path">The path to save to</param>
        public void SaveAs(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string jsonData = JsonConvert.SerializeObject(this, Form1.SerializerSettings);
                File.WriteAllText(path, jsonData);

                SavePath = path;
                Changed = false;
            }
        }

        /// <summary>
        /// Loads file to this adventure object. Tries to preserve all events
        /// </summary>
        /// <param name="path">The path to load from</param>
        public static Adventure LoadAdventure(string path, Settings settings)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                {
                    string adventureJson = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<Adventure>(adventureJson, Form1.SerializerSettings);
                }
                else // If file doesn't exist
                {
                    MessageBox.Show($"Adventure file {path} could not be found", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return null;
        }
    }
}
