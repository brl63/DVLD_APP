namespace DVLD_APP
{
    partial class ctrlAddUser
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
            this.tbPersonInfo = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ctrlPersonCardWithFilter1 = new DVLD_APP.ctrlPersonCardWithFilter();
            this.tbLoginInfo = new System.Windows.Forms.TabPage();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ctrlAddLoginInfo1 = new DVLD_APP.ctrlAddLoginInfo();
            this.tbPersonInfo.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tbLoginInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPersonInfo
            // 
            this.tbPersonInfo.Controls.Add(this.tabPage2);
            this.tbPersonInfo.Controls.Add(this.tbLoginInfo);
            this.tbPersonInfo.Location = new System.Drawing.Point(3, 3);
            this.tbPersonInfo.Name = "tbPersonInfo";
            this.tbPersonInfo.SelectedIndex = 0;
            this.tbPersonInfo.Size = new System.Drawing.Size(1288, 710);
            this.tbPersonInfo.TabIndex = 0;
            this.tbPersonInfo.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tbPersonInfo_Selecting);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ctrlPersonCardWithFilter1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1280, 677);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Person Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ctrlPersonCardWithFilter1
            // 
            this.ctrlPersonCardWithFilter1.Location = new System.Drawing.Point(7, 7);
            this.ctrlPersonCardWithFilter1.Name = "ctrlPersonCardWithFilter1";
            this.ctrlPersonCardWithFilter1.Size = new System.Drawing.Size(1265, 660);
            this.ctrlPersonCardWithFilter1.TabIndex = 0;
            // 
            // tbLoginInfo
            // 
            this.tbLoginInfo.Controls.Add(this.btnCancel);
            this.tbLoginInfo.Controls.Add(this.btnSave);
            this.tbLoginInfo.Controls.Add(this.ctrlAddLoginInfo1);
            this.tbLoginInfo.Location = new System.Drawing.Point(4, 29);
            this.tbLoginInfo.Name = "tbLoginInfo";
            this.tbLoginInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tbLoginInfo.Size = new System.Drawing.Size(1280, 677);
            this.tbLoginInfo.TabIndex = 2;
            this.tbLoginInfo.Text = "Login Info";
            this.tbLoginInfo.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1031, 560);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(131, 52);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(115, 578);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 44);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ctrlAddLoginInfo1
            // 
            this.ctrlAddLoginInfo1.Location = new System.Drawing.Point(20, 20);
            this.ctrlAddLoginInfo1.Name = "ctrlAddLoginInfo1";
            this.ctrlAddLoginInfo1.Size = new System.Drawing.Size(600, 400);
            this.ctrlAddLoginInfo1.TabIndex = 0;
            // 
            // ctrlAddUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbPersonInfo);
            this.Name = "ctrlAddUser";
            this.Size = new System.Drawing.Size(1300, 720);
            this.tbPersonInfo.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tbLoginInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbPersonInfo;
        private System.Windows.Forms.TabPage tabPage2;
        private ctrlPersonCardWithFilter ctrlPersonCardWithFilter1;
        private System.Windows.Forms.TabPage tbLoginInfo;
        private ctrlAddLoginInfo ctrlAddLoginInfo1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}