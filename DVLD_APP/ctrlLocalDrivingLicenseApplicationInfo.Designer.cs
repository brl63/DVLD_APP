namespace DVLD_APP
{
    partial class ctrlLocalDrivingLicenseApplicationInfo
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
            this.gbDrivingLicenseApplicationInfo = new System.Windows.Forms.GroupBox();
            this.llShowLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.lblPassedTests = new System.Windows.Forms.Label();
            this.lblAppliedFor = new System.Windows.Forms.Label();
            this.lblLocalAppID = new System.Windows.Forms.Label();
            this.ctrlApplicationBasicInfo1 = new DVLD_APP.ctrlApplicationBasicInfo();
            this.gbDrivingLicenseApplicationInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDrivingLicenseApplicationInfo
            // 
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.llShowLicenseInfo);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.lblPassedTests);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.lblAppliedFor);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.lblLocalAppID);
            this.gbDrivingLicenseApplicationInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDrivingLicenseApplicationInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.gbDrivingLicenseApplicationInfo.Location = new System.Drawing.Point(0, 0);
            this.gbDrivingLicenseApplicationInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbDrivingLicenseApplicationInfo.Name = "gbDrivingLicenseApplicationInfo";
            this.gbDrivingLicenseApplicationInfo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbDrivingLicenseApplicationInfo.Size = new System.Drawing.Size(860, 130);
            this.gbDrivingLicenseApplicationInfo.TabIndex = 0;
            this.gbDrivingLicenseApplicationInfo.TabStop = false;
            this.gbDrivingLicenseApplicationInfo.Text = "Driving License Application Info";
            this.gbDrivingLicenseApplicationInfo.Enter += new System.EventHandler(this.gbDrivingLicenseApplicationInfo_Enter);
            // 
            // llShowLicenseInfo
            // 
            this.llShowLicenseInfo.AutoSize = true;
            this.llShowLicenseInfo.Enabled = false;
            this.llShowLicenseInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.llShowLicenseInfo.Location = new System.Drawing.Point(120, 85);
            this.llShowLicenseInfo.Name = "llShowLicenseInfo";
            this.llShowLicenseInfo.Size = new System.Drawing.Size(122, 19);
            this.llShowLicenseInfo.TabIndex = 3;
            this.llShowLicenseInfo.TabStop = true;
            this.llShowLicenseInfo.Text = "Show License Info";
            this.llShowLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowLicenseInfo_LinkClicked);
            // 
            // lblPassedTests
            // 
            this.lblPassedTests.AutoSize = true;
            this.lblPassedTests.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPassedTests.Location = new System.Drawing.Point(460, 85);
            this.lblPassedTests.Name = "lblPassedTests";
            this.lblPassedTests.Size = new System.Drawing.Size(126, 19);
            this.lblPassedTests.TabIndex = 2;
            this.lblPassedTests.Text = "Passed Tests : 0/3";
            // 
            // lblAppliedFor
            // 
            this.lblAppliedFor.AutoSize = true;
            this.lblAppliedFor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAppliedFor.Location = new System.Drawing.Point(460, 35);
            this.lblAppliedFor.Name = "lblAppliedFor";
            this.lblAppliedFor.Size = new System.Drawing.Size(176, 19);
            this.lblAppliedFor.TabIndex = 1;
            this.lblAppliedFor.Text = "Applied For License : [???]";
            // 
            // lblLocalAppID
            // 
            this.lblLocalAppID.AutoSize = true;
            this.lblLocalAppID.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLocalAppID.Location = new System.Drawing.Point(25, 35);
            this.lblLocalAppID.Name = "lblLocalAppID";
            this.lblLocalAppID.Size = new System.Drawing.Size(117, 19);
            this.lblLocalAppID.TabIndex = 0;
            this.lblLocalAppID.Text = "D.L.AppID : [???]";
            // 
            // ctrlApplicationBasicInfo1
            // 
            this.ctrlApplicationBasicInfo1.BackColor = System.Drawing.Color.White;
            this.ctrlApplicationBasicInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlApplicationBasicInfo1.Location = new System.Drawing.Point(0, 130);
            this.ctrlApplicationBasicInfo1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlApplicationBasicInfo1.Name = "ctrlApplicationBasicInfo1";
            this.ctrlApplicationBasicInfo1.Size = new System.Drawing.Size(860, 240);
            this.ctrlApplicationBasicInfo1.TabIndex = 1;
            // 
            // ctrlLocalDrivingLicenseApplicationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ctrlApplicationBasicInfo1);
            this.Controls.Add(this.gbDrivingLicenseApplicationInfo);
            this.Name = "ctrlLocalDrivingLicenseApplicationInfo";
            this.Size = new System.Drawing.Size(860, 370);
            this.gbDrivingLicenseApplicationInfo.ResumeLayout(false);
            this.gbDrivingLicenseApplicationInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDrivingLicenseApplicationInfo;
        private System.Windows.Forms.Label lblLocalAppID;
        private System.Windows.Forms.Label lblAppliedFor;
        private System.Windows.Forms.Label lblPassedTests;
        private System.Windows.Forms.LinkLabel llShowLicenseInfo;
        private DVLD_APP.ctrlApplicationBasicInfo ctrlApplicationBasicInfo1;
    }
}