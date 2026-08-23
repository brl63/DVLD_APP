using bus;
using DVLD_APP.helpers;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmIssueInternationalDrivingLicense : Form
    {
        private int _SelectedLicenseID = -1;
        private int _InternationalLicenseID = -1;

        public frmIssueInternationalDrivingLicense()
        {
            InitializeComponent();
        }

        private void frmIssueInternationalDrivingLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();

            lblFees.Text = clsApplicationTypes.Find(6)?.ApplicationFees.ToString("0.00") ?? "0.00";

            lblCreatedByUser.Text = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserName : "Admin";

            btnIssue.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            _SelectedLicenseID = licenseID;
            lblLocalLicenseID.Text = _SelectedLicenseID.ToString();

            if (_SelectedLicenseID == -1)
            {
                btnIssue.Enabled = false;
                llShowLicenseHistory.Enabled = false;
                return;
            }

            llShowLicenseHistory.Enabled = true;

            // التحقق من قواعد البزنس عبر دالة CanIssueInternationalLicense
            if (!clsInternationalLicense.CanIssueInternationalLicense(_SelectedLicenseID, out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the international license for this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            int currentUserID = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserID : 1;

            clsInternationalLicense intLicense = clsInternationalLicense.IssueInternationalLicense(_SelectedLicenseID, currentUserID, out string errorMessage);

            if (intLicense != null)
            {
                _InternationalLicenseID = intLicense.InternationalLicenseID;
                lblInternationalLicenseID.Text = _InternationalLicenseID.ToString();
                lblApplicationID.Text = intLicense.ApplicationID.ToString();

                MessageBox.Show($"International License Issued Successfully with ID = {_InternationalLicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnIssue.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                llShowLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show($"Failed to issue license: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense localLicense = clsLicense.Find(_SelectedLicenseID);
            if (localLicense != null)
            {
                frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(localLicense.DriverInfo.PersonID);
                frm.ShowDialog();
            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_InternationalLicenseID != -1)
            {
                frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
                frm.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}