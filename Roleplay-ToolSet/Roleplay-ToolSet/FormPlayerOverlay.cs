using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoleplayToolSet
{
    public partial class FormPlayerOverlay : Form
    {
        // Constants
        private readonly FormBorderStyle _defaultStyle = FormBorderStyle.SizableToolWindow;
        private readonly FormBorderStyle _transparentStyle = FormBorderStyle.None;
        private const int TimeHeight = 70;

        private Color _defaultBack;
        private Color _transBack = Color.LightCyan;

        private TableLayoutPanel _table;
        private TimeDisplay _timeDisplay;

        // Used for repositioning the window when the boarders are removed
        private int _borderWidth;
        private int _titlebarHeight;

        public FormPlayerOverlay(Adventure adventure, Settings settings)
        {
            InitializeComponent();

            this.SuspendLayout();

            // Format this
            this.FormBorderStyle = _defaultStyle;
            _defaultBack = this.BackColor;
            
            // Set constants
            _borderWidth = (this.Width - this.ClientSize.Width) / 2;
            _titlebarHeight = this.Height - this.ClientSize.Height - 2 * _borderWidth;

            // Format table
            _table = new TableLayoutPanel();
            this.Controls.Add(_table);
            _table.RowCount = 2;
            _table.ColumnCount = 1;
            _table.Dock = DockStyle.Fill;
            _table.RowStyles.Clear();
            _table.RowStyles.Add(new RowStyle(SizeType.Absolute, TimeHeight));
            _table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _table.BackColor = Color.Transparent;

            // Format time display
            _timeDisplay = new TimeDisplay(adventure.Time, settings);
            _table.Controls.Add(_timeDisplay);
            _timeDisplay.Dock = DockStyle.Fill;

            // Add events
            FormClosing += FormPlayerOverlay_FormClosing;

            this.ResumeLayout();
        }

        private void FormPlayerOverlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Remove bound events
        }

        /// <summary>
        /// Sets the colour that should be transparent
        /// </summary>
        /// <param name="c">The transparent colour</param>
        public void SetTransparencyKey(Color c)
        {
            _transBack = c;
            this.TransparencyKey = c;
        }

        /// <summary>
        /// Sets this overlay to be boarderless
        /// </summary>
        /// <param name="boarderless">True if this should be boarderless</param>
        public void SetBorderless(bool boarderless)
        {
            this.SuspendLayout(); // Reduce flickering

            bool isCurrentlyBoarderless = this.FormBorderStyle == _transparentStyle;
            if (isCurrentlyBoarderless != boarderless) // If needs to be changed
            {
                // The distances to move the window
                int deltaX = _borderWidth;
                int deltaY = _titlebarHeight + _borderWidth;

                if (boarderless) // If should be boarderless
                {
                    this.FormBorderStyle = _transparentStyle;
                }
                else
                {
                    this.FormBorderStyle = _defaultStyle;

                    // Swap directions if moving back
                    deltaX = -deltaX;
                    deltaY = -deltaY;
                }

                // Only move if the window isn't maximised
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Location = new Point(this.Location.X + deltaX, this.Location.Y + deltaY);
                }
                else if (this.WindowState == FormWindowState.Maximized)
                {
                    // A hack to fix repositioning issues
                    this.WindowState = FormWindowState.Normal;
                    this.WindowState = FormWindowState.Maximized;
                }
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// Sets this overlay to be transparent
        /// </summary>
        /// <param name="transparent">True if this should be transparent</param>
        public void SetTransparent(bool transparent)
        {
            bool isCurrentlyTransparent = this.BackColor == _transBack;
            if (isCurrentlyTransparent != transparent) // If needs to be changed
            {
                if (transparent) // If should be transparent
                {
                    this.BackColor = _transBack;
                    this.TransparencyKey = _transBack;
                }
                else
                {
                    this.BackColor = _defaultBack;
                }

                this.AllowTransparency = transparent;
            }
        }

        /// <summary>
        /// Sets the time display to be visible
        /// </summary>
        /// <param name="visible">True if this should be visible</param>
        public void SetTimeVisible(bool visible)
        {
            this.SuspendLayout(); // Reduce flickering

            _table.RowStyles[0] = new RowStyle(SizeType.Absolute, visible? TimeHeight : 0);
            _timeDisplay.Visible = visible;

            this.ResumeLayout();
        }
    }
}
