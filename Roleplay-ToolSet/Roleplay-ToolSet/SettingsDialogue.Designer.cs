namespace RoleplayToolSet
{
    partial class SettingsDialogue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageSettingsImages = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxOverlayImageStyle = new System.Windows.Forms.ComboBox();
            this.tabPageTime = new System.Windows.Forms.TabPage();
            this.textBoxTimeFormat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonReset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxCalendar = new System.Windows.Forms.ComboBox();
            this.numericUpDownTimeInterval = new System.Windows.Forms.NumericUpDown();
            this.tabControlSettings.SuspendLayout();
            this.tabPageSettingsImages.SuspendLayout();
            this.tabPageTime.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSettings.Controls.Add(this.tabPageSettingsImages);
            this.tabControlSettings.Controls.Add(this.tabPageTime);
            this.tabControlSettings.Location = new System.Drawing.Point(0, 0);
            this.tabControlSettings.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(484, 321);
            this.tabControlSettings.TabIndex = 0;
            // 
            // tabPageSettingsImages
            // 
            this.tabPageSettingsImages.Controls.Add(this.label1);
            this.tabPageSettingsImages.Controls.Add(this.comboBoxOverlayImageStyle);
            this.tabPageSettingsImages.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettingsImages.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageSettingsImages.Name = "tabPageSettingsImages";
            this.tabPageSettingsImages.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageSettingsImages.Size = new System.Drawing.Size(476, 295);
            this.tabPageSettingsImages.TabIndex = 0;
            this.tabPageSettingsImages.Text = "Images";
            this.tabPageSettingsImages.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Player Overlay Image Style: ";
            // 
            // comboBoxOverlayImageStyle
            // 
            this.comboBoxOverlayImageStyle.FormattingEnabled = true;
            this.comboBoxOverlayImageStyle.Location = new System.Drawing.Point(150, 3);
            this.comboBoxOverlayImageStyle.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxOverlayImageStyle.Name = "comboBoxOverlayImageStyle";
            this.comboBoxOverlayImageStyle.Size = new System.Drawing.Size(173, 21);
            this.comboBoxOverlayImageStyle.TabIndex = 0;
            // 
            // tabPageTime
            // 
            this.tabPageTime.Controls.Add(this.numericUpDownTimeInterval);
            this.tabPageTime.Controls.Add(this.comboBoxCalendar);
            this.tabPageTime.Controls.Add(this.label4);
            this.tabPageTime.Controls.Add(this.label3);
            this.tabPageTime.Controls.Add(this.textBoxTimeFormat);
            this.tabPageTime.Controls.Add(this.label2);
            this.tabPageTime.Location = new System.Drawing.Point(4, 22);
            this.tabPageTime.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageTime.Name = "tabPageTime";
            this.tabPageTime.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageTime.Size = new System.Drawing.Size(476, 295);
            this.tabPageTime.TabIndex = 1;
            this.tabPageTime.Text = "Time";
            this.tabPageTime.UseVisualStyleBackColor = true;
            // 
            // textBoxTimeFormat
            // 
            this.textBoxTimeFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTimeFormat.Location = new System.Drawing.Point(76, 5);
            this.textBoxTimeFormat.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTimeFormat.Name = "textBoxTimeFormat";
            this.textBoxTimeFormat.Size = new System.Drawing.Size(393, 20);
            this.textBoxTimeFormat.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Time Format:";
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(74, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(106, 25);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Confirm";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(186, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(106, 25);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonReset, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 321);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(478, 31);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonReset
            // 
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReset.Location = new System.Drawing.Point(298, 3);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(106, 25);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset Settings";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 31);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Time Interval:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(243, 32);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Calendar:";
            // 
            // comboBoxCalendar
            // 
            this.comboBoxCalendar.FormattingEnabled = true;
            this.comboBoxCalendar.Location = new System.Drawing.Point(300, 29);
            this.comboBoxCalendar.Name = "comboBoxCalendar";
            this.comboBoxCalendar.Size = new System.Drawing.Size(169, 21);
            this.comboBoxCalendar.TabIndex = 7;
            // 
            // numericUpDownTimeInterval
            // 
            this.numericUpDownTimeInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownTimeInterval.Location = new System.Drawing.Point(76, 29);
            this.numericUpDownTimeInterval.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownTimeInterval.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownTimeInterval.Name = "numericUpDownTimeInterval";
            this.numericUpDownTimeInterval.Size = new System.Drawing.Size(162, 20);
            this.numericUpDownTimeInterval.TabIndex = 8;
            this.numericUpDownTimeInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // SettingsDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tabControlSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "SettingsDialogue";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageSettingsImages.ResumeLayout(false);
            this.tabPageSettingsImages.PerformLayout();
            this.tabPageTime.ResumeLayout(false);
            this.tabPageTime.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageSettingsImages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOverlayImageStyle;
        private System.Windows.Forms.TabPage tabPageTime;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxTimeFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.ComboBox comboBoxCalendar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeInterval;
    }
}