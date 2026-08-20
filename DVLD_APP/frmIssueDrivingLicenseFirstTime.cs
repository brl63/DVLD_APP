using System;
using System.Windows.Forms;
using bus;
using DVLD_APP.helpers;

namespace DVLD_APP
{
    public partial class frmIssueDrivingLicenseFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsApplications _Application;

        public frmIssueDrivingLicenseFirstTime(int localDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
        }

        private void frmIssueDrivingLicenseFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();

            // 1. استدعاء دالة التحميل بالاسم الفعلي الموجود في الكنترول عندك
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplicationID);

            _Application = ctrlLocalDrivingLicenseApplicationInfo1.SelectedApplicationInfo;

            if (_Application == null)
            {
                MessageBox.Show($"No Application with ID = {_LocalDrivingLicenseApplicationID}", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // 2. التحقق من اجتياز الاختبارات الثلاثة
            if (_Application.GetPassedTestCount() < 3)
            {
                MessageBox.Show("Person should pass all 3 tests before issuing the license!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            // 3. فحص هل صدرت له رخصة مسبقاً لهذا الطلب
            int licenseID = _Application.GetActiveLicenseID();
            if (licenseID != -1)
            {
                MessageBox.Show($"Person already has a license for this application with ID = {licenseID}", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnIssue.Enabled = false;
                return;
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int licenseID = _Application.IssueLicenseForTheFistTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if (licenseID != -1)
            {
                MessageBox.Show($"License Issued Successfully with ID = {licenseID}", "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnIssue.Enabled = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to issue license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}