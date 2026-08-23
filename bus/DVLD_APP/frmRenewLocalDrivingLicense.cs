using bus;
using DVLD_APP.helpers;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmRenewLocalDrivingLicense : Form
    {
        private int _SelectedLicenseID = -1;
        private int _NewLicenseID = -1;

        public frmRenewLocalDrivingLicense()
        {
            InitializeComponent();
        }

        private void frmRenewLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = "[????]";

            // رسوم خدمة طلب التجديد (ApplicationTypeID = 2)
            lblApplicationFees.Text = clsApplicationTypes.Find(2)?.ApplicationFees.ToString("0.00") ?? "0.00";

            lblCreatedByUser.Text = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserName : "Admin";

            btnRenew.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            _SelectedLicenseID = licenseID;
            lblOldLicenseID.Text = _SelectedLicenseID.ToString();

            if (_SelectedLicenseID == -1)
            {
                btnRenew.Enabled = false;
                llShowLicenseHistory.Enabled = false;
                return;
            }

            llShowLicenseHistory.Enabled = true;

            clsLicense selectedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;

            // حساب تاريخ الانتهاء المتوقع وتكاليف التجديد
            byte defaultValidityLength = selectedLicense.LicenseClassInfo.DefaultValidityLength;
            lblExpirationDate.Text = DateTime.Now.AddYears(defaultValidityLength).ToShortDateString();
            lblLicenseFees.Text = selectedLicense.LicenseClassInfo.ClassFees.ToString("0.00");

            decimal appFees = Convert.ToDecimal(lblApplicationFees.Text);
            decimal licenseFees = selectedLicense.LicenseClassInfo.ClassFees;
            lblTotalFees.Text = (appFees + licenseFees).ToString("0.00");

            // التحقق من قواعد البزنس عبر دالة CanBeRenewed
            if (!selectedLicense.CanBeRenewed(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }

            btnRenew.Enabled = true;
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to renew this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            clsLicense oldLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            int currentUserID = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserID : 1;

            clsLicense newLicense = oldLicense.RenewLicense(txtNotes.Text.Trim(), currentUserID);

            if (newLicense != null)
            {
                _NewLicenseID = newLicense.LicenseID;
                lblRenewedLicenseID.Text = _NewLicenseID.ToString();
                lblRenewApplicationID.Text = newLicense.ApplicationID.ToString();

                MessageBox.Show($"Licensed Renewed Successfully with ID = {_NewLicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRenew.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                txtNotes.Enabled = false;
                llShowLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to renew the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
