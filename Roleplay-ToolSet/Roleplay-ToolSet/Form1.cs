using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RoleplayToolSet
{
    public partial class Form1 : Form
    {
        private FormPlayerOverlay _playerForm; // The form shown to the players
        private Settings _settings = Settings.GetSettings();
        private WorldTime _time = new WorldTime();
        private EntityCollection _entities = new EntityCollection();

        public Form1()
        {
            InitializeComponent();

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

            // Create player form
            CreatePlayerForm();

            // Set up EntityList
            entityManager.Entities = _entities;

            // Add events
            _time.TimeChanged += _time_TimeChanged;
            _settings.ImageStyle.Changed += _settings_ImageStyleChanged;
            _settings.DateFormat.Changed += DateFormat_Changed;
            _settings.RealTimeInterval.Changed += RealTimeInterval_Changed;
            _settings.Calendar.Changed += Calendar_Changed;
            timeInputBox.TimeChanged += TimeInputBox_TimeChanged;
        }

        private void TimeInputBox_TimeChanged(object caller, TimeEventArgs eventArgs)
        {
            foreach (TimeUnit unit in eventArgs.TimeDeltas.Keys)
            {
                int number = eventArgs.TimeDeltas[unit];

                switch (unit)
                {
                    case TimeUnit.Years:
                        _time.AddYears(number);
                        break;
                    case TimeUnit.Months:
                        _time.AddMonths(number);
                        break;
                    case TimeUnit.Weeks:
                        _time.AddDays(number * 7);
                        break;
                    case TimeUnit.Days:
                        _time.AddDays(number);
                        break;
                    case TimeUnit.Hours:
                        _time.AddHours(number);
                        break;
                    case TimeUnit.Minutes:
                        _time.AddMinutes(number);
                        break;
                    case TimeUnit.Seconds:
                        _time.AddSeconds(number);
                        break;
                    case TimeUnit.mSeconds:
                        _time.AddMilliseconds(number);
                        break;
                }
            }
        }

        private void RealTimeInterval_Changed(object sender, EventArgs e)
        {
            timerRealTime.Interval = _settings.RealTimeInterval.Value;
        }

        private void DateFormat_Changed(object sender, EventArgs e)
        {
            RefreshTimeLabel();
        }

        private void CreatePlayerForm()
        {
            // Make player overlay & hide
            _playerForm = new FormPlayerOverlay(_time, _settings)
            {
                Visible = checkBoxTimeVisible.Enabled
            };
            _playerForm.SetTransparent(checkBoxOverlayTransparent.Enabled);
            _playerForm.SetTimeVisible(checkBoxTimeVisible.Enabled);

            // Add events
            _playerForm.FormClosed += PlayerForm_Closed;
        }

        private void _time_TimeChanged(object sender, EventArgs e)
        {
            RefreshTimeLabel();
        }

        private void RefreshTimeLabel()
        {
            labelCurrentTime.Text = _time.ToString(_settings.DateFormat.Value);
        }

        private void _settings_ImageStyleChanged(object sender, EventArgs e)
        {
            _playerForm.BackgroundImageLayout = _settings.ImageStyle.Value;
        }

        private void Calendar_Changed(object sender, EventArgs e)
        {
            _time.ChangeCalendar(_settings.Calendar.Value);
            RefreshTimeLabel();
        }

        private void PlayerForm_Closed(object sender, EventArgs e)
        {
            // Delete currtent player form associations
            SetOverlayConfigsEnabled(false);
            this.checkBoxOverlayVisible.Checked = false;
            _playerForm = null;

            // Create a new player form and ensure to hide
            CreatePlayerForm();
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
            _time.AddMilliseconds(timerRealTime.Interval);
        }

        private void CheckBoxTimeRealLife_CheckedChanged(object sender, EventArgs e)
        {
            timerRealTime.Enabled = checkBoxTimeRealLife.Checked;
        }
    }
}
