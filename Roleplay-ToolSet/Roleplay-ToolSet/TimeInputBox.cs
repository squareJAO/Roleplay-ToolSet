using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace RoleplayToolSet
{
    public enum TimeUnit
    { Years, Months, Weeks, Days, Hours, Minutes, Seconds, mSeconds }

    // Event arhs that come with a time span
    class TimeEventArgs : EventArgs
    {
        public Dictionary<TimeUnit, int> TimeDeltas { get; private set; }

        public TimeEventArgs(Dictionary<TimeUnit, int> timeDeltas)
            : base ()
        {
            TimeDeltas = timeDeltas;
        }
    }

    // Used for an event with a change in time attatched
    delegate void TimeEventHandler(object caller, TimeEventArgs eventArgs);

    /// <summary>
    /// A set of number input boxes to allow users to input a time
    /// </summary>
    class TimeInputBox : Panel
    {
        // The maxes for each (excclusive)
        private Dictionary<TimeUnit, int> _maxes = new Dictionary<TimeUnit, int>()
        {
            { TimeUnit.Years, 10000 },
            { TimeUnit.Months, 12 },
            { TimeUnit.Weeks, 4 },
            { TimeUnit.Days, 7 },
            { TimeUnit.Hours, 24 },
            { TimeUnit.Minutes, 60 },
            { TimeUnit.Seconds, 60 },
            { TimeUnit.mSeconds, 1000 },
        };

        // Need bi-directional lookup so use two dicts
        private Dictionary<TimeUnit, NumericUpDown> _unitUpDowns = new Dictionary<TimeUnit, NumericUpDown>();
        private Dictionary<NumericUpDown, TimeUnit> _upDownUnits = new Dictionary<NumericUpDown, TimeUnit>();

        // For buttons
        private Dictionary<Button, TimeUnit> _addButtonUnits = new Dictionary<Button, TimeUnit>();
        private Dictionary<Button, TimeUnit> _subButtonUnits = new Dictionary<Button, TimeUnit>();

        // The table that the buttons and numbers go in
        private TableLayoutPanel _table = new TableLayoutPanel();

        // The event for when time is changed by anything
        public event TimeEventHandler TimeChanged;

        public TimeInputBox()
        {
            // Create the table
            _table = new TableLayoutPanel
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Size = new Size(this.Width, 102),
                Location = new Point(0, 0)
            };
            this.Controls.Add(_table);

            // Get an array of all valid units
            TimeUnit[] units = Enum.GetValues(typeof(TimeUnit)).Cast<TimeUnit>().ToArray();

            // Add columns for buttons
            _table.ColumnCount = units.Length;
            _table.ColumnStyles.Clear();
            _table.RowCount = 4;
            _table.RowStyles.Clear();
            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 12));
            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            // Loop and add buttons
            for (int unitsIndex = 0; unitsIndex < units.Length; unitsIndex++)
            {
                string unitName = Enum.GetName(typeof(TimeUnit), units[unitsIndex]);

                // Create and format up down
                NumericUpDown upDown = new NumericUpDown
                {
                    Dock = DockStyle.Fill,
                    Maximum = decimal.MaxValue, // Set a large max so that users can input a value and it will cascade through
                    Minimum = 0,
                    TextAlign = HorizontalAlignment.Right
                };

                // Add up down
                _table.Controls.Add(upDown);
                _table.SetRow(upDown, 3);
                _table.SetColumn(upDown, unitsIndex);

                // Add changed event
                upDown.ValueChanged += UpDown_ValueChanged;

                // Record in dictionaries
                _unitUpDowns.Add(units[unitsIndex], upDown);
                _upDownUnits.Add(upDown, units[unitsIndex]);

                // Create and format label
                Label label = new Label
                {
                    Dock = DockStyle.Fill,
                    Text = unitName // Set the text to the name of the unit
                };

                // Add label
                _table.Controls.Add(label);
                _table.SetRow(label, 0);
                _table.SetColumn(label, unitsIndex);

                // Create and format buttons
                Button add1Button = new Button();
                Button sub1Button = new Button();
                add1Button.Dock = DockStyle.Fill;
                sub1Button.Dock = DockStyle.Fill;
                add1Button.Text = "+1";
                sub1Button.Text = "-1";
                _addButtonUnits.Add(add1Button, units[unitsIndex]);
                _subButtonUnits.Add(sub1Button, units[unitsIndex]);
                add1Button.Click += Add1Button_Click;
                sub1Button.Click += Sub1Button_Click;

                // Add buttons
                _table.Controls.Add(add1Button);
                _table.SetRow(add1Button, 1);
                _table.SetColumn(add1Button, unitsIndex);
                _table.Controls.Add(sub1Button);
                _table.SetRow(sub1Button, 2);
                _table.SetColumn(sub1Button, unitsIndex);

                // Format column
                _table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f / units.Length));
            }

            // Make buttons 
            Button addDisplayedTime = new Button();
            Button subDisplayedTime = new Button();
            Button clearDisplayedTime = new Button();

            // Format buttons
            addDisplayedTime.Text = "Add Time";
            subDisplayedTime.Text = "Remove Time";
            clearDisplayedTime.Text = "Clear";
            addDisplayedTime.Size = new Size(100, 20);
            subDisplayedTime.Size = new Size(100, 20);
            clearDisplayedTime.Size = new Size(100, 20);
            addDisplayedTime.Location = new Point(15, _table.Size.Height);
            subDisplayedTime.Location = new Point(addDisplayedTime.Location.X + addDisplayedTime.Width + 5, addDisplayedTime.Location.Y);
            clearDisplayedTime.Location = new Point(subDisplayedTime.Location.X + subDisplayedTime.Width + 5, addDisplayedTime.Location.Y);

            // Add buttons 
            this.Controls.Add(addDisplayedTime);
            this.Controls.Add(subDisplayedTime);
            this.Controls.Add(clearDisplayedTime);

            // Add events
            addDisplayedTime.Click += AddDisplayedTime_Click;
            subDisplayedTime.Click += SubDisplayedTime_Click;
            clearDisplayedTime.Click += ClearDisplayedTime_Click;
        }

        private void ClearDisplayedTime_Click(object sender, EventArgs e)
        {
            // Loop through upDowns and set to zero
            TimeUnit[] units = Enum.GetValues(typeof(TimeUnit)).Cast<TimeUnit>().ToArray();
            foreach (TimeUnit unit in units)
            {
                _unitUpDowns[unit].Value = 0;
            }
        }

        private void SubDisplayedTime_Click(object sender, EventArgs e)
        {
            Dictionary<TimeUnit, int> delta = SumDisplayedTime(-1);

            TimeChanged?.Invoke(this, new TimeEventArgs(delta));
        }

        private void AddDisplayedTime_Click(object sender, EventArgs e)
        {
            Dictionary<TimeUnit, int> delta = SumDisplayedTime();
            TimeChanged?.Invoke(this, new TimeEventArgs(delta));
        }

        /// <summary>
        /// Sums all time displays into a single timespan
        /// </summary>
        /// <returns>A timespan object</returns>
        private Dictionary<TimeUnit, int> SumDisplayedTime(int mult=1)
        {
            Dictionary<TimeUnit, int> span = new Dictionary<TimeUnit, int>();

            TimeUnit[] units = Enum.GetValues(typeof(TimeUnit)).Cast<TimeUnit>().ToArray();
            foreach (TimeUnit unit in units)
            {
                span.Add(unit, mult * (int)_unitUpDowns[unit].Value);
            }

            return span;
        }

        private void Sub1Button_Click(object sender, EventArgs e)
        {
            TimeUnit buttonUnit = _subButtonUnits[(Button)sender];
            TimeChanged?.Invoke(this, new TimeEventArgs(new Dictionary<TimeUnit, int>() { { buttonUnit, -1 } }));
        }

        private void Add1Button_Click(object sender, EventArgs e)
        {
            TimeUnit buttonUnit = _addButtonUnits[(Button)sender];
            TimeChanged?.Invoke(this, new TimeEventArgs(new Dictionary<TimeUnit, int>() { { buttonUnit, 1 } }));
        }

        private void UpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_upDownUnits.Keys.Contains(sender)) // If the sender has a unit linked with them
            {
                NumericUpDown numericSender = (NumericUpDown)sender;

                CheckNum(numericSender); // Check the number is valid
            }
        }

        private void CheckNum(NumericUpDown numericSender)
        {
            TimeUnit unit = _upDownUnits[numericSender]; // Get the unit

            if (numericSender.Value >= _maxes[unit]) // If the number isn't valid
            {
                // Get an array of all valid units
                TimeUnit[] units = Enum.GetValues(typeof(TimeUnit)).Cast<TimeUnit>().ToArray();

                // If not Years
                if (units[0] != unit)
                {
                    // Calc what should and shouldn't be in the box
                    int valid = (int)numericSender.Value % _maxes[unit];
                    int excess = (int)numericSender.Value - valid;
                    int toAdd = excess / _maxes[unit];

                    numericSender.Value = valid; // Put in what should be there

                    int unitIndex = Array.FindIndex(units, 1, x => x == unit); // Find the index of the current unit
                    TimeUnit transferUnit = units[unitIndex - 1]; // Get the unit before
                    NumericUpDown transferUpDown = _unitUpDowns[transferUnit]; // Get the numeric to be added to

                    transferUpDown.Value += toAdd;
                    CheckNum(numericSender); // Check the new value is valid
                }
                else
                {
                    numericSender.Value = _maxes[unit];
                }
            }
        }
    }
}
