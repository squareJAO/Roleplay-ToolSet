namespace RoleplayToolSet
{
    partial class EntityManager
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.entityList = new RoleplayToolSet.EntityList();
            this.entityDetailedDisplay = new RoleplayToolSet.EntityDetailedDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityList)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.entityList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.entityDetailedDisplay);
            this.splitContainer1.Size = new System.Drawing.Size(472, 291);
            this.splitContainer1.SplitterDistance = 86;
            this.splitContainer1.TabIndex = 10;
            // 
            // entityList
            // 
            this.entityList.AllowUserToAddRows = false;
            this.entityList.AllowUserToDeleteRows = false;
            this.entityList.AllowUserToOrderColumns = true;
            this.entityList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.entityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.entityList.Location = new System.Drawing.Point(0, 0);
            this.entityList.Margin = new System.Windows.Forms.Padding(0);
            this.entityList.MultiSelect = false;
            this.entityList.Name = "entityList";
            this.entityList.RowHeadersVisible = false;
            this.entityList.RowTemplate.Height = 24;
            this.entityList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.entityList.Size = new System.Drawing.Size(472, 86);
            this.entityList.TabIndex = 8;
            this.entityList.SelectionChanged += new System.EventHandler(this.EntityList_SelectionChanged);
            // 
            // entityDetailedDisplay
            // 
            this.entityDetailedDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.entityDetailedDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityDetailedDisplay.Location = new System.Drawing.Point(0, 0);
            this.entityDetailedDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.entityDetailedDisplay.Name = "entityDetailedDisplay";
            this.entityDetailedDisplay.Size = new System.Drawing.Size(472, 201);
            this.entityDetailedDisplay.TabIndex = 9;
            // 
            // EntityManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "EntityManager";
            this.Size = new System.Drawing.Size(472, 291);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.entityList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private EntityList entityList;
        private EntityDetailedDisplay entityDetailedDisplay;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
