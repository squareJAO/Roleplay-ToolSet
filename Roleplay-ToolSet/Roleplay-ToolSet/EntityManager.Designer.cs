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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.entityList = new RoleplayToolSet.EntityList();
            this.entityDetailedDisplay = new RoleplayToolSet.EntityDetailedDisplay();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDeleteEntity = new System.Windows.Forms.Button();
            this.buttonAddEntity = new System.Windows.Forms.Button();
            this.buttonSaveEntity = new System.Windows.Forms.Button();
            this.buttonAddAttibute = new System.Windows.Forms.Button();
            this.buttonLoadEntity = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityList)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.splitContainer1.Size = new System.Drawing.Size(472, 261);
            this.splitContainer1.SplitterDistance = 77;
            this.splitContainer1.TabIndex = 10;
            // 
            // entityList
            // 
            this.entityList.AllowUserToAddRows = false;
            this.entityList.AllowUserToDeleteRows = false;
            this.entityList.AllowUserToOrderColumns = true;
            this.entityList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.entityList.DefaultCellStyle = dataGridViewCellStyle7;
            this.entityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.entityList.Location = new System.Drawing.Point(0, 0);
            this.entityList.Margin = new System.Windows.Forms.Padding(0);
            this.entityList.Name = "entityList";
            this.entityList.RowHeadersVisible = false;
            this.entityList.RowTemplate.Height = 24;
            this.entityList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.entityList.Size = new System.Drawing.Size(472, 77);
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
            this.entityDetailedDisplay.Size = new System.Drawing.Size(472, 180);
            this.entityDetailedDisplay.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 11;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonDeleteEntity, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAddEntity, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveEntity, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAddAttibute, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonLoadEntity, 7, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 264);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 27);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // buttonDeleteEntity
            // 
            this.buttonDeleteEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteEntity.Location = new System.Drawing.Point(-54, 0);
            this.buttonDeleteEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteEntity.Name = "buttonDeleteEntity";
            this.buttonDeleteEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonDeleteEntity.TabIndex = 0;
            this.buttonDeleteEntity.Text = "Delete Entity";
            this.buttonDeleteEntity.UseVisualStyleBackColor = true;
            this.buttonDeleteEntity.Click += new System.EventHandler(this.buttonDeleteEntity_Click);
            // 
            // buttonAddEntity
            // 
            this.buttonAddEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddEntity.Location = new System.Drawing.Point(66, 0);
            this.buttonAddEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddEntity.Name = "buttonAddEntity";
            this.buttonAddEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonAddEntity.TabIndex = 1;
            this.buttonAddEntity.Text = "Add Entity";
            this.buttonAddEntity.UseVisualStyleBackColor = true;
            this.buttonAddEntity.Click += new System.EventHandler(this.buttonAddEntity_Click);
            // 
            // buttonSaveEntity
            // 
            this.buttonSaveEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveEntity.Location = new System.Drawing.Point(186, 0);
            this.buttonSaveEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSaveEntity.Name = "buttonSaveEntity";
            this.buttonSaveEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonSaveEntity.TabIndex = 2;
            this.buttonSaveEntity.Text = "Export Entity";
            this.buttonSaveEntity.UseVisualStyleBackColor = true;
            this.buttonSaveEntity.Click += new System.EventHandler(this.buttonSaveEntity_Click);
            // 
            // buttonAddAttibute
            // 
            this.buttonAddAttibute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddAttibute.Location = new System.Drawing.Point(426, 0);
            this.buttonAddAttibute.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddAttibute.Name = "buttonAddAttibute";
            this.buttonAddAttibute.Size = new System.Drawing.Size(100, 27);
            this.buttonAddAttibute.TabIndex = 3;
            this.buttonAddAttibute.Text = "Add Attibute";
            this.buttonAddAttibute.UseVisualStyleBackColor = true;
            this.buttonAddAttibute.Click += new System.EventHandler(this.buttonAddAttibute_Click);
            // 
            // buttonLoadEntity
            // 
            this.buttonLoadEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadEntity.Location = new System.Drawing.Point(306, 0);
            this.buttonLoadEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoadEntity.Name = "buttonLoadEntity";
            this.buttonLoadEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonLoadEntity.TabIndex = 4;
            this.buttonLoadEntity.Text = "Import Entity";
            this.buttonLoadEntity.UseVisualStyleBackColor = true;
            this.buttonLoadEntity.Click += new System.EventHandler(this.buttonLoadEntity_Click);
            // 
            // EntityManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "EntityManager";
            this.Size = new System.Drawing.Size(472, 291);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.entityList)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private EntityList entityList;
        private EntityDetailedDisplay entityDetailedDisplay;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonDeleteEntity;
        private System.Windows.Forms.Button buttonAddEntity;
        private System.Windows.Forms.Button buttonSaveEntity;
        private System.Windows.Forms.Button buttonAddAttibute;
        private System.Windows.Forms.Button buttonLoadEntity;
    }
}
