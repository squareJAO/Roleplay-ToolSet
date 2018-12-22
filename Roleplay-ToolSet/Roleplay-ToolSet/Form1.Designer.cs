namespace RoleplayToolSet
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.checkBoxOverlayVisible = new System.Windows.Forms.CheckBox();
            this.checkBoxOverlayTransparent = new System.Windows.Forms.CheckBox();
            this.checkBoxOverlayBorderless = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxTimeVisible = new System.Windows.Forms.CheckBox();
            this.listViewBackgroundImages = new System.Windows.Forms.ListView();
            this.columnHeaderImage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AddImagesButton = new System.Windows.Forms.Button();
            this.checkBoxBackgroundImage = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxTimeRealLife = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCurrentTime = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.timerRealTime = new System.Windows.Forms.Timer(this.components);
            this.timeInputBox = new RoleplayToolSet.TimeInputBox();
            this.entityManager = new RoleplayToolSet.EntityManager();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxOverlayVisible
            // 
            this.checkBoxOverlayVisible.AutoSize = true;
            this.checkBoxOverlayVisible.Location = new System.Drawing.Point(5, 5);
            this.checkBoxOverlayVisible.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxOverlayVisible.Name = "checkBoxOverlayVisible";
            this.checkBoxOverlayVisible.Size = new System.Drawing.Size(56, 17);
            this.checkBoxOverlayVisible.TabIndex = 2;
            this.checkBoxOverlayVisible.Text = "Visible";
            this.checkBoxOverlayVisible.UseVisualStyleBackColor = true;
            this.checkBoxOverlayVisible.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // checkBoxOverlayTransparent
            // 
            this.checkBoxOverlayTransparent.AutoSize = true;
            this.checkBoxOverlayTransparent.Enabled = false;
            this.checkBoxOverlayTransparent.Location = new System.Drawing.Point(5, 26);
            this.checkBoxOverlayTransparent.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxOverlayTransparent.Name = "checkBoxOverlayTransparent";
            this.checkBoxOverlayTransparent.Size = new System.Drawing.Size(83, 17);
            this.checkBoxOverlayTransparent.TabIndex = 3;
            this.checkBoxOverlayTransparent.Text = "Transparent";
            this.checkBoxOverlayTransparent.UseVisualStyleBackColor = true;
            this.checkBoxOverlayTransparent.CheckedChanged += new System.EventHandler(this.CheckBoxOverlayTransparent_CheckedChanged);
            // 
            // checkBoxOverlayBorderless
            // 
            this.checkBoxOverlayBorderless.AutoSize = true;
            this.checkBoxOverlayBorderless.Enabled = false;
            this.checkBoxOverlayBorderless.Location = new System.Drawing.Point(5, 48);
            this.checkBoxOverlayBorderless.Name = "checkBoxOverlayBorderless";
            this.checkBoxOverlayBorderless.Size = new System.Drawing.Size(75, 17);
            this.checkBoxOverlayBorderless.TabIndex = 4;
            this.checkBoxOverlayBorderless.Text = "Borderless";
            this.checkBoxOverlayBorderless.UseVisualStyleBackColor = true;
            this.checkBoxOverlayBorderless.CheckedChanged += new System.EventHandler(this.CheckBoxOverlayBorderless_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(957, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(933, 535);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxTimeVisible);
            this.tabPage1.Controls.Add(this.listViewBackgroundImages);
            this.tabPage1.Controls.Add(this.AddImagesButton);
            this.tabPage1.Controls.Add(this.checkBoxBackgroundImage);
            this.tabPage1.Controls.Add(this.checkBoxOverlayVisible);
            this.tabPage1.Controls.Add(this.checkBoxOverlayTransparent);
            this.tabPage1.Controls.Add(this.checkBoxOverlayBorderless);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(925, 509);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Overlay";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxTimeVisible
            // 
            this.checkBoxTimeVisible.AutoSize = true;
            this.checkBoxTimeVisible.Enabled = false;
            this.checkBoxTimeVisible.Location = new System.Drawing.Point(165, 5);
            this.checkBoxTimeVisible.Name = "checkBoxTimeVisible";
            this.checkBoxTimeVisible.Size = new System.Drawing.Size(82, 17);
            this.checkBoxTimeVisible.TabIndex = 9;
            this.checkBoxTimeVisible.Text = "Time Visible";
            this.checkBoxTimeVisible.UseVisualStyleBackColor = true;
            this.checkBoxTimeVisible.CheckedChanged += new System.EventHandler(this.CheckBoxTimeVisible_CheckedChanged);
            // 
            // listViewBackgroundImages
            // 
            this.listViewBackgroundImages.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.listViewBackgroundImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewBackgroundImages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderImage,
            this.columnHeaderName});
            this.listViewBackgroundImages.FullRowSelect = true;
            this.listViewBackgroundImages.GridLines = true;
            this.listViewBackgroundImages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewBackgroundImages.HideSelection = false;
            this.listViewBackgroundImages.Location = new System.Drawing.Point(6, 95);
            this.listViewBackgroundImages.MultiSelect = false;
            this.listViewBackgroundImages.Name = "listViewBackgroundImages";
            this.listViewBackgroundImages.ShowGroups = false;
            this.listViewBackgroundImages.Size = new System.Drawing.Size(151, 379);
            this.listViewBackgroundImages.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewBackgroundImages.TabIndex = 8;
            this.listViewBackgroundImages.UseCompatibleStateImageBehavior = false;
            this.listViewBackgroundImages.View = System.Windows.Forms.View.Details;
            this.listViewBackgroundImages.SelectedIndexChanged += new System.EventHandler(this.ListViewBackgroundImages_SelectedIndexChanged);
            // 
            // columnHeaderImage
            // 
            this.columnHeaderImage.Text = "Image";
            this.columnHeaderImage.Width = 50;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 83;
            // 
            // AddImagesButton
            // 
            this.AddImagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddImagesButton.Location = new System.Drawing.Point(6, 480);
            this.AddImagesButton.Name = "AddImagesButton";
            this.AddImagesButton.Size = new System.Drawing.Size(151, 23);
            this.AddImagesButton.TabIndex = 7;
            this.AddImagesButton.Text = "Add Images";
            this.AddImagesButton.UseVisualStyleBackColor = true;
            this.AddImagesButton.Click += new System.EventHandler(this.AddImagesButton_Click);
            // 
            // checkBoxBackgroundImage
            // 
            this.checkBoxBackgroundImage.AutoSize = true;
            this.checkBoxBackgroundImage.Enabled = false;
            this.checkBoxBackgroundImage.Location = new System.Drawing.Point(5, 71);
            this.checkBoxBackgroundImage.Name = "checkBoxBackgroundImage";
            this.checkBoxBackgroundImage.Size = new System.Drawing.Size(116, 17);
            this.checkBoxBackgroundImage.TabIndex = 5;
            this.checkBoxBackgroundImage.Text = "Background Image";
            this.checkBoxBackgroundImage.UseVisualStyleBackColor = true;
            this.checkBoxBackgroundImage.CheckedChanged += new System.EventHandler(this.CheckBoxBackgroundImage_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxTimeRealLife);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.labelCurrentTime);
            this.tabPage2.Controls.Add(this.timeInputBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(925, 509);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Time";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxTimeRealLife
            // 
            this.checkBoxTimeRealLife.AutoSize = true;
            this.checkBoxTimeRealLife.Location = new System.Drawing.Point(7, 25);
            this.checkBoxTimeRealLife.Name = "checkBoxTimeRealLife";
            this.checkBoxTimeRealLife.Size = new System.Drawing.Size(140, 17);
            this.checkBoxTimeRealLife.TabIndex = 4;
            this.checkBoxTimeRealLife.Text = "Increment with Real Life";
            this.checkBoxTimeRealLife.UseVisualStyleBackColor = true;
            this.checkBoxTimeRealLife.CheckedChanged += new System.EventHandler(this.CheckBoxTimeRealLife_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Add Time:";
            // 
            // labelCurrentTime
            // 
            this.labelCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCurrentTime.Location = new System.Drawing.Point(3, 3);
            this.labelCurrentTime.Name = "labelCurrentTime";
            this.labelCurrentTime.Size = new System.Drawing.Size(919, 18);
            this.labelCurrentTime.TabIndex = 0;
            this.labelCurrentTime.Text = "[CurrentTimeHere]";
            this.labelCurrentTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.entityManager);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(925, 509);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Entities";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // timerRealTime
            // 
            this.timerRealTime.Tick += new System.EventHandler(this.TimerRealTime_Tick);
            // 
            // timeInputBox
            // 
            this.timeInputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeInputBox.Location = new System.Drawing.Point(6, 78);
            this.timeInputBox.Name = "timeInputBox";
            this.timeInputBox.Size = new System.Drawing.Size(914, 148);
            this.timeInputBox.TabIndex = 3;
            // 
            // entityManager
            // 
            this.entityManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityManager.Location = new System.Drawing.Point(0, 0);
            this.entityManager.Name = "entityManager";
            this.entityManager.Size = new System.Drawing.Size(925, 509);
            this.entityManager.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 574);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxOverlayVisible;
        private System.Windows.Forms.CheckBox checkBoxOverlayTransparent;
        private System.Windows.Forms.CheckBox checkBoxOverlayBorderless;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBoxBackgroundImage;
        private System.Windows.Forms.Button AddImagesButton;
        private System.Windows.Forms.ListView listViewBackgroundImages;
        private System.Windows.Forms.ColumnHeader columnHeaderImage;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxTimeVisible;
        private System.Windows.Forms.Label labelCurrentTime;
        private System.Windows.Forms.Label label1;
        private TimeInputBox timeInputBox;
        private System.Windows.Forms.Timer timerRealTime;
        private System.Windows.Forms.CheckBox checkBoxTimeRealLife;
        private System.Windows.Forms.TabPage tabPage3;
        private EntityManager entityManager;
    }
}

