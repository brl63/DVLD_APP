namespace DVLD_APP
{
    partial class ctrlApplicationBasicInfo
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
            this.lblApplicationID = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblFees = new System.Windows.Forms.Label();
            this.lblApplicant = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblStatusDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblShowPersonInfo = new System.Windows.Forms.LinkLabel();
            this.gbApplcationBasicInfo = new System.Windows.Forms.GroupBox();
            this.gbApplcationBasicInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblApplicationID
            // 
            this.lblApplicationID.AutoSize = true;
            this.lblApplicationID.Location = new System.Drawing.Point(36, 22);
            this.lblApplicationID.Name = "lblApplicationID";
            this.lblApplicationID.Size = new System.Drawing.Size(112, 20);
            this.lblApplicationID.TabIndex = 0;
            this.lblApplicationID.Text = "ApplicationID :";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(36, 58);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(64, 20);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status :";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(36, 132);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(51, 20);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Type :";
            // 
            // lblFees
            // 
            this.lblFees.AutoSize = true;
            this.lblFees.Location = new System.Drawing.Point(36, 93);
            this.lblFees.Name = "lblFees";
            this.lblFees.Size = new System.Drawing.Size(53, 20);
            this.lblFees.TabIndex = 3;
            this.lblFees.Text = "Fees :";
            // 
            // lblApplicant
            // 
            this.lblApplicant.AutoSize = true;
            this.lblApplicant.Location = new System.Drawing.Point(36, 169);
            this.lblApplicant.Name = "lblApplicant";
            this.lblApplicant.Size = new System.Drawing.Size(83, 20);
            this.lblApplicant.TabIndex = 4;
            this.lblApplicant.Text = "Applicant :";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(724, 22);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(52, 20);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "Date :";
            // 
            // lblStatusDate
            // 
            this.lblStatusDate.AutoSize = true;
            this.lblStatusDate.Location = new System.Drawing.Point(724, 58);
            this.lblStatusDate.Name = "lblStatusDate";
            this.lblStatusDate.Size = new System.Drawing.Size(99, 20);
            this.lblStatusDate.TabIndex = 6;
            this.lblStatusDate.Text = "StatusDate :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(728, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 7;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(724, 93);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(92, 20);
            this.lblCreatedBy.TabIndex = 8;
            this.lblCreatedBy.Text = "CreatedBy :";
            // 
            // lblShowPersonInfo
            // 
            this.lblShowPersonInfo.AutoSize = true;
            this.lblShowPersonInfo.Location = new System.Drawing.Point(923, 169);
            this.lblShowPersonInfo.Name = "lblShowPersonInfo";
            this.lblShowPersonInfo.Size = new System.Drawing.Size(135, 20);
            this.lblShowPersonInfo.TabIndex = 9;
            this.lblShowPersonInfo.TabStop = true;
            this.lblShowPersonInfo.Text = "Show Person Info";
            // 
            // gbApplcationBasicInfo
            // 
            this.gbApplcationBasicInfo.Controls.Add(this.lblShowPersonInfo);
            this.gbApplcationBasicInfo.Controls.Add(this.lblApplicationID);
            this.gbApplcationBasicInfo.Controls.Add(this.lblCreatedBy);
            this.gbApplcationBasicInfo.Controls.Add(this.lblStatus);
            this.gbApplcationBasicInfo.Controls.Add(this.lblFees);
            this.gbApplcationBasicInfo.Controls.Add(this.lblStatusDate);
            this.gbApplcationBasicInfo.Controls.Add(this.lblType);
            this.gbApplcationBasicInfo.Controls.Add(this.lblDate);
            this.gbApplcationBasicInfo.Controls.Add(this.lblApplicant);
            this.gbApplcationBasicInfo.Location = new System.Drawing.Point(3, 3);
            this.gbApplcationBasicInfo.Name = "gbApplcationBasicInfo";
            this.gbApplcationBasicInfo.Size = new System.Drawing.Size(1102, 208);
            this.gbApplcationBasicInfo.TabIndex = 10;
            this.gbApplcationBasicInfo.TabStop = false;
            this.gbApplcationBasicInfo.Text = "Applcation Basic info";
            // 
            // ctrlApplicationBasicInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gbApplcationBasicInfo);
            this.Name = "ctrlApplicationBasicInfo";
            this.Size = new System.Drawing.Size(1109, 208);
            this.gbApplcationBasicInfo.ResumeLayout(false);
            this.gbApplcationBasicInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblApplicationID;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblFees;
        private System.Windows.Forms.Label lblApplicant;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblStatusDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.LinkLabel lblShowPersonInfo;
        private System.Windows.Forms.GroupBox gbApplcationBasicInfo;
    }
}
