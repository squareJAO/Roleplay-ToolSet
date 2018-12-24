using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using NodaTime;
using System.Globalization;

namespace RoleplayToolSet
{
    public partial class SettingsDialogue : Form
    {
        private Settings _settings;

        public SettingsDialogue(Settings settings)
        {
            InitializeComponent();

            _settings = settings;

            SetDisplayFromCurrentSettings();
        }

        private void SetDisplayFromCurrentSettings()
        {
            // Initialise image select dropdown
            foreach (int value in Enum.GetValues(typeof(ImageLayout)))
            {
                this.comboBoxOverlayImageStyle.Items.Add(Enum.GetName(typeof(ImageLayout), value));
            }
            this.comboBoxOverlayImageStyle.SelectedIndex = (int)_settings.ImageStyle.Value;

            // Initilise time formats
            textBoxTimeFormat.Text = _settings.DateFormat.Value;

            // Initilise time interval
            numericUpDownTimeInterval.Value = _settings.RealTimeInterval.Value;

            // Initialise calendars
            string[] systems = CalendarSystem.Ids.ToArray();
            comboBoxCalendar.Items.AddRange(systems);
            comboBoxCalendar.SelectedIndex = Array.IndexOf(systems, _settings.Calendar.Value.Id);
        }

        /// <summary>
        /// Checks for what has changed and invokes the respective events
        /// </summary>
        private void ProcessChanges()
        {
            // Process image style
            ImageLayout newStyle = (ImageLayout)this.comboBoxOverlayImageStyle.SelectedIndex;
            _settings.ImageStyle.Value = newStyle;

            // Process time formats
            _settings.DateFormat.Value = textBoxTimeFormat.Text;

            // Process time interval
            _settings.RealTimeInterval.Value = (int)numericUpDownTimeInterval.Value;

            // Process calendars
            _settings.Calendar.Value = CalendarSystem.ForId(comboBoxCalendar.SelectedItem.ToString());
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            this.ProcessChanges();
            _settings.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            // Copy pointer to old settings
            Settings oldSettings = _settings;

            // Create default settings (so default can be undone) and set display
            _settings = new Settings();
            SetDisplayFromCurrentSettings();

            // Copy old settings pointer back
            _settings = oldSettings;
        }
    }
}
