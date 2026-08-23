using bus;
using DVLD_APP.helpers;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmReplaceLostOrDamagedLicense : Form
    {
        private int _SelectedLicenseID = -1;
        private int _NewLicenseID = -1;

        private clsLicense.enIssueReason _IssueReason
        {
            get
            {
                return rbDamagedLicense.Checked ?
                    clsLicense.enIssueReason.ReplacementForDamaged :
                    clsLicense.enIssueReason.ReplacementForLost;
            }
        }

        private int _ApplicationTypeID
        {
            get
            {
                return rbDamagedLicense.Checked ? 4 : 3;
            }
        }

        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }

        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserName : "Admin";

            rbDamagedLicense.Checked = true;

            btnIssueReplacement.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
        }

        private void _UpdateApplicationFees()
        {
            lblApplicationFees.Text = clsApplicationTypes.Find(_ApplicationTypeID)?.ApplicationFees.ToString("0.00") ?? "0.00";
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamagedLicense.Checked)
            {
                lblTitle.Text = "Replacement for Damaged License";
                this.Text = "Replacement for Damaged License";
                _UpdateApplicationFees();
            }
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLostLicense.Checked)
            {
                lblTitle.Text = "Replacement for Lost License";
                this.Text = "Replacement for Lost License";
                _UpdateApplicationFees();
            }
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            _SelectedLicenseID = licenseID;
            lblOldLicenseID.Text = _SelectedLicenseID.ToString();

            if (_SelectedLicenseID == -1)
            {
                btnIssueReplacement.Enabled = false;
                llShowLicenseHistory.Enabled = false;
                return;
            }

            llShowLicenseHistory.Enabled = true;

            clsLicense selectedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;

            // 1. التحقق من أن الرخصة نشطة
            if (!selectedLicense.IsActive)
            {
                MessageBox.Show("Selected license is not active, choose an active license.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            // 2. التحقق من أن الرخصة غير منتهية الصلاحية
            if (selectedLicense.IsLicenseExpired())
            {
                MessageBox.Show($"Selected license is expired on: {selectedLicense.ExpirationDate.ToShortDateString()}. You should renew it instead.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue a replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            clsLicense oldLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            int currentUserID = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserID : 1;

            clsLicense newLicense = oldLicense.Replace(_IssueReason, currentUserID);

            if (newLicense != null)
            {
                _NewLicenseID = newLicense.LicenseID;
                lblReplacedLicenseID.Text = _NewLicenseID.ToString();
                lblApplicationID.Text = newLicense.ApplicationID.ToString();

                MessageBox.Show($"Licensed Replaced Successfully with ID = {_NewLicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnIssueReplacement.Enabled = false;
                gbReplacementFor.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                llShowLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to issue replacement license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense selectedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            if (selectedLicense != null)
            {
                frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(selectedLicense.DriverInfo.PersonID);
                frm.ShowDialog();
            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_NewLicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
                frm.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}