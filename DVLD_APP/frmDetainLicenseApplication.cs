using bus;
using DVLD_APP.helpers;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmDetainLicenseApplication : Form
    {
        private int _SelectedLicenseID = -1;
        private int _DetainID = -1;

        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserName : "Admin";

            btnDetain.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            _SelectedLicenseID = licenseID;
            lblLicenseID.Text = _SelectedLicenseID.ToString();

            if (_SelectedLicenseID == -1)
            {
                btnDetain.Enabled = false;
                llShowLicenseHistory.Enabled = false;
                return;
            }

            llShowLicenseHistory.Enabled = true;

            clsLicense selectedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;

            // 1. فحص هل الرخصة محجوزة بالفعل
            if (clsDetainedLicense.IsLicenseDetained(_SelectedLicenseID))
            {
                MessageBox.Show("Selected license is already detained, choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }

            // 2. فحص هل الرخصة نشطة
            if (!selectedLicense.IsActive)
            {
                MessageBox.Show("Selected license is not active, choose an active license.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }

            btnDetain.Enabled = true;
            txtFineFees.Focus();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                MessageBox.Show("Please enter the fine fees.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFineFees.Focus();
                return;
            }

            if (!decimal.TryParse(txtFineFees.Text.Trim(), out decimal fineFees) || fineFees < 0)
            {
                MessageBox.Show("Please enter a valid fine amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFineFees.Focus();
                return;
            }

            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            int currentUserID = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserID : 1;

            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = _SelectedLicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = fineFees;
            detainedLicense.CreatedByUserID = currentUserID;

            if (detainedLicense.Save())
            {
                _DetainID = detainedLicense.DetainID;
                lblDetainID.Text = _DetainID.ToString();

                MessageBox.Show($"License Detained Successfully with ID = {_DetainID}", "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnDetain.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                txtFineFees.Enabled = false;
                llShowLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to detain the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام وعلامة النقطة العشرية ومفاتيح التحكم فقط
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // منع تكرار النقطة العشرية
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
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
            if (_SelectedLicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);
                frm.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
