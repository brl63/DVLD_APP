namespace DVLD_APP
{
    partial class ctrlDataManagement
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
            this.components = new System.ComponentModel.Container();
            this.lblName = new System.Windows.Forms.Label();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cmsData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picName = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.cmsData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picName)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(630, 244);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(42, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "label";
            // 
            // dgvList
            // 
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(5, 361);
            this.dgvList.Name = "dgvList";
            this.dgvList.RowHeadersWidth = 62;
            this.dgvList.RowTemplate.Height = 28;
            this.dgvList.Size = new System.Drawing.Size(1298, 423);
            this.dgvList.TabIndex = 2;
            this.dgvList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvList_CellMouseDown);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(1177, 327);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(109, 28);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // cmsData
            // 
            this.cmsData.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDetailsToolStripMenuItem});
            this.cmsData.Name = "cmsData";
            this.cmsData.Size = new System.Drawing.Size(182, 36);
            // 
            // showDetailsToolStripMenuItem
            // 
            this.showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
            this.showDetailsToolStripMenuItem.Size = new System.Drawing.Size(181, 32);
            this.showDetailsToolStripMenuItem.Text = "ShowDetails";
            // 
            // picName
            // 
            this.picName.ErrorImage = global::DVLD_APP.Properties.Resources.UsersManagment;
            this.picName.Location = new System.Drawing.Point(535, 31);
            this.picName.Name = "picName";
            this.picName.Size = new System.Drawing.Size(221, 168);
            this.picName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picName.TabIndex = 0;
            this.picName.TabStop = false;
            // 
            // ctrlDataManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.picName);
            this.Name = "ctrlDataManagement";
            this.Size = new System.Drawing.Size(1336, 787);
            this.Load += new System.EventHandler(this.ctrlDataManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.cmsData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ContextMenuStrip cmsData;
        private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
    }
}
