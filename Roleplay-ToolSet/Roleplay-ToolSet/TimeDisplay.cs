using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RoleplayToolSet
{
    class TimeDisplay : TableLayoutPanel
    {
        private WorldTime _worldTime; // The current time object

        private Label _dateTimeLabel;
        private ProgressBar _progressBar;
        private Settings _settings;

        public TimeDisplay(WorldTime worldTime, Settings settings)
        {
            _worldTime = worldTime;
            _settings = settings;

            this.SuspendLayout();

            // Format this
            this.RowCount = 2;
            this.ColumnCount = 1;

            // Format DateTime label
            _dateTimeLabel = new Label
            {
                Anchor = AnchorStyles.None,
                Font = new Font(FontFamily.GenericSansSerif, 20.0f)
            };
            this.Controls.Add(_dateTimeLabel);

            // Format progress bar
            _progressBar = new ProgressBar();
            this.Controls.Add(_progressBar);
            _progressBar.Dock = DockStyle.Fill;
            _progressBar.Minimum = 0;
            _progressBar.Maximum = 10000;

            // Set time
            UpdateTime();

            // Format table
            this.RowStyles.Clear();
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            // Add events
            worldTime.TimeChanged += WorldTime_TimeChanged;
            settings.DateFormat.Changed += DateFormat_Changed;

            this.ResumeLayout();    
        }

        private void UpdateTime()
        {
            _dateTimeLabel.Text = _worldTime.ToString(_settings.DateFormat.Value);
            _dateTimeLabel.Size = TextRenderer.MeasureText(_dateTimeLabel.Text, _dateTimeLabel.Font);
            _progressBar.Value = (int)(_worldTime.GetPercentThroughDay() * _progressBar.Maximum);
        }

        private void WorldTime_TimeChanged(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void DateFormat_Changed(object sender, EventArgs e)
        {
            UpdateTime();
        }
    }
}
