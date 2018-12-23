namespace RoleplayToolSet
{
    partial class EntityDetailedDisplay
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
            this.flowLayoutPanelAttributes = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDeleteEntity = new System.Windows.Forms.Button();
            this.buttonAddEntity = new System.Windows.Forms.Button();
            this.buttonSaveEntity = new System.Windows.Forms.Button();
            this.buttonAddAttibute = new System.Windows.Forms.Button();
            this.buttonLoadEntity = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelAttributes
            // 
            this.flowLayoutPanelAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelAttributes.AutoScroll = true;
            this.flowLayoutPanelAttributes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelAttributes.Location = new System.Drawing.Point(4, 4);
            this.flowLayoutPanelAttributes.Name = "flowLayoutPanelAttributes";
            this.flowLayoutPanelAttributes.Size = new System.Drawing.Size(786, 427);
            this.flowLayoutPanelAttributes.TabIndex = 0;
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 438);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(786, 27);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // buttonDeleteEntity
            // 
            this.buttonDeleteEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteEntity.Location = new System.Drawing.Point(103, 0);
            this.buttonDeleteEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteEntity.Name = "buttonDeleteEntity";
            this.buttonDeleteEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonDeleteEntity.TabIndex = 0;
            this.buttonDeleteEntity.Text = "Delete Entity";
            this.buttonDeleteEntity.UseVisualStyleBackColor = true;
            this.buttonDeleteEntity.Click += new System.EventHandler(this.ButtonDeleteEntity_Click);
            // 
            // buttonAddEntity
            // 
            this.buttonAddEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddEntity.Location = new System.Drawing.Point(223, 0);
            this.buttonAddEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddEntity.Name = "buttonAddEntity";
            this.buttonAddEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonAddEntity.TabIndex = 1;
            this.buttonAddEntity.Text = "Add Entity";
            this.buttonAddEntity.UseVisualStyleBackColor = true;
            this.buttonAddEntity.Click += new System.EventHandler(this.ButtonAddEntity_Click);
            // 
            // buttonSaveEntity
            // 
            this.buttonSaveEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveEntity.Location = new System.Drawing.Point(343, 0);
            this.buttonSaveEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSaveEntity.Name = "buttonSaveEntity";
            this.buttonSaveEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonSaveEntity.TabIndex = 2;
            this.buttonSaveEntity.Text = "Export Entity";
            this.buttonSaveEntity.UseVisualStyleBackColor = true;
            this.buttonSaveEntity.Click += new System.EventHandler(this.ButtonSaveEntity_Click);
            // 
            // buttonAddAttibute
            // 
            this.buttonAddAttibute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddAttibute.Location = new System.Drawing.Point(583, 0);
            this.buttonAddAttibute.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddAttibute.Name = "buttonAddAttibute";
            this.buttonAddAttibute.Size = new System.Drawing.Size(100, 27);
            this.buttonAddAttibute.TabIndex = 3;
            this.buttonAddAttibute.Text = "Add Attibute";
            this.buttonAddAttibute.UseVisualStyleBackColor = true;
            this.buttonAddAttibute.Click += new System.EventHandler(this.ButtonAddAttibute_Click);
            // 
            // buttonLoadEntity
            // 
            this.buttonLoadEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadEntity.Location = new System.Drawing.Point(463, 0);
            this.buttonLoadEntity.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoadEntity.Name = "buttonLoadEntity";
            this.buttonLoadEntity.Size = new System.Drawing.Size(100, 27);
            this.buttonLoadEntity.TabIndex = 4;
            this.buttonLoadEntity.Text = "Import Entity";
            this.buttonLoadEntity.UseVisualStyleBackColor = true;
            this.buttonLoadEntity.Click += new System.EventHandler(this.ButtonLoadEntity_Click);
            // 
            // EntityDetailedDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanelAttributes);
            this.Name = "EntityDetailedDisplay";
            this.Size = new System.Drawing.Size(793, 468);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelAttributes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonDeleteEntity;
        private System.Windows.Forms.Button buttonAddEntity;
        private System.Windows.Forms.Button buttonSaveEntity;
        private System.Windows.Forms.Button buttonAddAttibute;
        private System.Windows.Forms.Button buttonLoadEntity;
    }
}
