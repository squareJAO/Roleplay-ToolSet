using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RoleplayToolSet
{
    public partial class Form1 : Form
    {
        private Adventure _adventure;
        private Settings _settings;
        private FormPlayerOverlay _playerForm; // The form shown to the players
        
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };

        public Form1()
        {
            InitializeComponent();

            // Default values
            _settings = Settings.GetSettings();
            _adventure = new Adventure();

            // Create player form
            _playerForm = new FormPlayerOverlay(_adventure, _settings);

            // Format listView
            ImageList imageListSmall = new ImageList
            {
                ImageSize = new Size(50, 50),
                ColorDepth = ColorDepth.Depth32Bit
            };
            listViewBackgroundImages.SmallImageList = imageListSmall;

            // Format label
            RefreshTimeLabel();

            // Init timer
            timerRealTime.Enabled = checkBoxTimeRealLife.Checked;

            // Set up EntityList
            entityManager.Entities = _adventure.Entities;

            // Add events
            _adventure.Time.TimeChanged += _time_TimeChanged;
            _settings.ImageStyle.Changed += _settings_ImageStyleChanged;
            _settings.DateFormat.Changed += DateFormat_Changed;
            _settings.RealTimeInterval.Changed += RealTimeInterval_Changed;
            _settings.Calendar.Changed += Calendar_Changed;
            timeInputBox.TimeChanged += TimeInputBox_TimeChanged;
            _playerForm.FormClosing += _playerForm_FormClosing;
        }

        private void TimeInputBox_TimeChanged(object caller, TimeEventArgs eventArgs)
        {
            foreach (TimeUnit unit in eventArgs.TimeDeltas.Keys)
            {
                int number = eventArgs.TimeDeltas[unit];

                switch (unit)
                {
                    case TimeUnit.Years:
                        _adventure.Time.AddYears(number);
                        break;
                    case TimeUnit.Months:
                        _adventure.Time.AddMonths(number);
                        break;
                    case TimeUnit.Weeks:
                        _adventure.Time.AddDays(number * 7);
                        break;
                    case TimeUnit.Days:
                        _adventure.Time.AddDays(number);
                        break;
                    case TimeUnit.Hours:
                        _adventure.Time.AddHours(number);
                        break;
                    case TimeUnit.Minutes:
                        _adventure.Time.AddMinutes(number);
                        break;
                    case TimeUnit.Seconds:
                        _adventure.Time.AddSeconds(number);
                        break;
                    case TimeUnit.mSeconds:
                        _adventure.Time.AddMilliseconds(number);
                        break;
                }
            }
        }
        
        private void _playerForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // Don't allow closing, only hiding
            _playerForm.Visible = false;
            SetOverlayConfigsEnabled(false);
            checkBoxOverlayVisible.Checked = false;
            e.Cancel = true;
        }

        private void RealTimeInterval_Changed(object sender, EventArgs e)
        {
            timerRealTime.Interval = _settings.RealTimeInterval.Value;
        }

        private void DateFormat_Changed(object sender, EventArgs e)
        {
            RefreshTimeLabel();
        }

        private void _time_TimeChanged(object sender, EventArgs e)
        {
            RefreshTimeLabel();
        }

        private void RefreshTimeLabel()
        {
            labelCurrentTime.Text = _adventure.Time.ToString(_settings.DateFormat.Value);
        }

        private void _settings_ImageStyleChanged(object sender, EventArgs e)
        {
            _playerForm.BackgroundImageLayout = _settings.ImageStyle.Value;
        }

        private void Calendar_Changed(object sender, EventArgs e)
        {
            _adventure.Time.ChangeCalendar(_settings.Calendar.Value);
            RefreshTimeLabel();
        }

        private void SetOverlayConfigsEnabled(bool enabled)
        {
            this.checkBoxOverlayTransparent.Enabled = enabled; // Enable if exists else don't
            this.checkBoxOverlayBorderless.Enabled = enabled;
            this.checkBoxBackgroundImage.Enabled = enabled;
            this.checkBoxTimeVisible.Enabled = enabled;
        }

        // Visible checkbox
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool visible = this.checkBoxOverlayVisible.Checked;

            _playerForm.Visible = visible;

            SetOverlayConfigsEnabled(visible);
        }
        
        private void CheckBoxOverlayTransparent_CheckedChanged(object sender, EventArgs e)
        {
            bool transparent = this.checkBoxOverlayTransparent.Checked;
            _playerForm.SetTransparent(transparent);
        }

        private void CheckBoxOverlayBorderless_CheckedChanged(object sender, EventArgs e)
        {
            bool borderless = this.checkBoxOverlayBorderless.Checked;
            _playerForm.SetBorderless(borderless);
        }

        private void AddImagesButton_Click(object sender, EventArgs e)
        {
            // Open a new dialogue for the user to select images to display
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "ImageFile|*.BMP;*.JPG;*.GIF;*.PNG|All files|*.*"
            };

            // Show the dialogue
            DialogResult result = fileDialog.ShowDialog();

            // Result = ok if the user opens something
            if (result == DialogResult.OK)
            {
                foreach (string filename in fileDialog.FileNames)
                {
                    Image image = Image.FromFile(filename);
                    listViewBackgroundImages.SmallImageList.Images.Add(filename, image); // Add the image and set key as filename
                    listViewBackgroundImages.Items.Add(new ListViewItem (new[] {"", Text = new FileInfo(filename).Name }) { ImageKey = filename }); // Add the selected item and link to image
                }
            }
        }

        private void CheckBoxBackgroundImage_CheckedChanged(object sender, EventArgs e)
        {
            RefreshPlayerBackgroundImage();
        }
        private void ListViewBackgroundImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPlayerBackgroundImage();
        }

        /// <summary>
        /// Changes the background image if it needs changing
        /// </summary>
        private void RefreshPlayerBackgroundImage()
        {
            if (checkBoxBackgroundImage.Checked) // If image should be shown
            {
                if (listViewBackgroundImages.SelectedItems.Count > 0) // Check an item is selected
                {
                    _playerForm.BackgroundImage = Image.FromFile(listViewBackgroundImages.SelectedItems[0].ImageKey); // Open image again because listview pushes quality down
                }
            }
            else
            {
                _playerForm.BackgroundImage = null;
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SettingsDialogue(_settings).ShowDialog();
        }

        private void CheckBoxTimeVisible_CheckedChanged(object sender, EventArgs e)
        {
            _playerForm.SetTimeVisible(checkBoxTimeVisible.Checked);
        }

        private void TimerRealTime_Tick(object sender, EventArgs e)
        {
            _adventure.Time.AddMilliseconds(timerRealTime.Interval);
        }

        private void CheckBoxTimeRealLife_CheckedChanged(object sender, EventArgs e)
        {
            timerRealTime.Enabled = checkBoxTimeRealLife.Checked;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdventureSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdventureSaveAs();
        }

        private void AdventureSave()
        {
            if (string.IsNullOrEmpty(_adventure.SavePath))
            {
                AdventureSaveAs();
            }
            else
            {
                _adventure.SaveAs(_adventure.SavePath);
            }
        }

        private void AdventureSaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "RolePlay Toolset Adventure|*.RPTSAdvtre|All Files|*.*"
            };
            saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                _adventure.SaveAs(saveFileDialog.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_adventure.Changed)
            {
                DialogResult confirmResult = MessageBox.Show("Save changes before loading?",
                                                             "Load?",
                                                             MessageBoxButtons.YesNoCancel);
                if (confirmResult == DialogResult.Cancel)
                {
                    return;
                }
                if (confirmResult == DialogResult.Yes)
                {
                    AdventureSave();
                }
            }

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "RolePlay Toolset Adventure|*.RPTSAdvtre|All Files|*.*",
                Multiselect = false
            };

            openFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                _adventure = Adventure.LoadAdventure(openFileDialog.FileName, _settings);
                _adventure.SavePath = openFileDialog.FileName;
                entityManager.Entities = _adventure.Entities;
            }
        }
    }
}
