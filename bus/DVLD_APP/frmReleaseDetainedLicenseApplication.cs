using bus;
using DVLD_APP.helpers;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        private int _SelectedLicenseID = -1;
        private int _DetainID = -1;
        private int _ApplicationID = -1;

        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
        }

        // Constructor إضافي لفتح الشاشة مباشرة وتمرير LicenseID إليها (مثلاً من جدول الرخص المحجوزة)
        public frmReleaseDetainedLicenseApplication(int licenseID)
        {
            InitializeComponent();
            _SelectedLicenseID = licenseID;
        }

        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {
            lblCreatedByUser.Text = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserName : "Admin";
            lblApplicationFees.Text = clsApplicationTypes.Find(5)?.ApplicationFees.ToString("0.00") ?? "0.00";

            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;

            // إذا تم فتح الشاشة بتمرير LicenseID مسبقاً
            if (_SelectedLicenseID != -1)
            {
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            }
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            _SelectedLicenseID = licenseID;
            lblLicenseID.Text = _SelectedLicenseID.ToString();

            if (_SelectedLicenseID == -1)
            {
                _ResetDetainInfo();
                return;
            }

            llShowLicenseHistory.Enabled = true;

            // 1. التحقق من أن الرخصة محجوزة
            if (!clsDetainedLicense.IsLicenseDetained(_SelectedLicenseID))
            {
                MessageBox.Show("Selected license is not detained, choose a detained license.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetDetainInfo();
                return;
            }

            // 2. تحميل بيانات الحجز
            clsDetainedLicense detainedLicense = clsDetainedLicense.FindByLicenseID(_SelectedLicenseID);

            if (detainedLicense == null)
            {
                MessageBox.Show("Could not find detain details for this license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetDetainInfo();
                return;
            }

            _DetainID = detainedLicense.DetainID;
            lblDetainID.Text = _DetainID.ToString();
            lblDetainDate.Text = detainedLicense.DetainDate.ToShortDateString();
            lblFineFees.Text = detainedLicense.FineFees.ToString("0.00");

            decimal appFees = Convert.ToDecimal(lblApplicationFees.Text);
            decimal fineFees = detainedLicense.FineFees;
            lblTotalFees.Text = (appFees + fineFees).ToString("0.00");

            btnRelease.Enabled = true;
        }

        private void _ResetDetainInfo()
        {
            lblDetainID.Text = "[????]";
            lblDetainDate.Text = "[????]";
            lblFineFees.Text = "[$$$$]";
            lblTotalFees.Text = "[$$$$]";
            lblApplicationID.Text = "[????]";
            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = false;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            int currentUserID = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserID : 1;
            clsLicense selectedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;

            // تنفيذ فك الحجز وإنشاء الطلب
            if (selectedLicense.ReleaseDetainedLicense(currentUserID, ref _ApplicationID))
            {
                lblApplicationID.Text = _ApplicationID.ToString();

                MessageBox.Show($"Detained License Released Successfully with Application ID = {_ApplicationID}", "License Released", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRelease.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                llShowLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to release the detained license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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