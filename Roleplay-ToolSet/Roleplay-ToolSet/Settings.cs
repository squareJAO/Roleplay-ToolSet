using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RoleplayToolSet
{
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// A setting that has an event for when it is changed
        /// </summary>
        /// <typeparam name="T">The type of the setting</typeparam>
        [Serializable]
        public class Setting<T>
        {
            private T _value;
            public T Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    if (_value==null || !_value.Equals(value))
                    {
                        _value = value;
                        Changed?.Invoke(this, new EventArgs()); // If the value is changed and there is a function bound to the event invoke the event
                    }
                }
            }

            [field: NonSerialized] public event EventHandler Changed; // The event, shouldn't be serialised
        }

        private const string SettingsFile = "Settings.json";

        // Settings
        public Setting<ImageLayout> ImageStyle = new Setting<ImageLayout>();
        public Setting<string> DateFormat = new Setting<string>();
        public Setting<int> RealTimeInterval = new Setting<int>();

        public Settings()
        {
            // Set defaults
            SetToDefault();
        }

        public void SetToDefault()
        {
            ImageStyle.Value = ImageLayout.Stretch;
            DateFormat.Value = "dd MMM yyyy g HH:mm:ss tt";
            RealTimeInterval.Value = 1000;
        }

        /// <summary>
        /// Gets the settings object currently saved
        /// </summary>
        /// <returns>A settings object that is loaded from the saved settings file</returns>
        public static Settings GetSettings()
        {
            Settings settings;

            // Check if settings file exists
            if (new FileInfo(SettingsFile).Exists)
            {
                // Deserialise & return
                using (StreamReader file = new StreamReader(SettingsFile))
                {
                    try
                    {
                        settings = JsonConvert.DeserializeObject<Settings>(file.ReadToEnd());
                        if (settings != null)
                        {
                            return settings;
                        }
                    }
                    // If the format of settings has changed, or if someone has messed with the json file fall out to create a new one
                    catch (NullReferenceException) { }
                    catch (JsonSerializationException) { }
                    catch (JsonReaderException) { }
                }
            }
            // Otherwise create a new one with default values
            settings = new Settings();
            settings.Save();
            return settings;
        }

        /// <summary>
        /// Saves the current settings to the settings file
        /// </summary>
        public void Save()
        {
            // Open or create a new file
            File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(this));
        }
    }
}
