using bus;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlLocalDrivingLicenseApplicationInfo : UserControl
    {
        private clsApplications _Application;
        private int _LocalDrivingLicenseAppID = -1;
        private int _LicenseID = -1;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseAppID; }
        }

        public int ApplicationID
        {
            get { return _LocalDrivingLicenseAppID; }
        }

        public clsApplications SelectedApplicationInfo
        {
            get { return _Application; }
        }

        public ctrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int localDrivingLicenseAppID)
        {
            _LocalDrivingLicenseAppID = localDrivingLicenseAppID;
            _Application = clsApplications.FindByLocalDrivingAppID(localDrivingLicenseAppID);

            if (_Application == null)
            {
                ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show($"No Application with Local ID = {localDrivingLicenseAppID} was found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _Application.GetActiveLicenseID();

            lblLocalAppID.Text = $"D.L.AppID : {_LocalDrivingLicenseAppID}";
            lblAppliedFor.Text = $"Applied For License : {clsLicenseClass.Find(_Application.LicenseClassID)?.ClassName ?? "N/A"}";
            lblPassedTests.Text = $"Passed Tests : {_Application.GetPassedTestCount()}/3";

            ctrlApplicationBasicInfo1.LoadApplicationInfo(_Application.ApplicationID);

            // تفعيل رابط إظهار الرخصة في حال كانت مستخرجة
            llShowLicenseInfo.Enabled = (_LicenseID != -1);
        }

        public void ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseAppID = -1;
            _LicenseID = -1;
            _Application = null;

            lblLocalAppID.Text = "D.L.AppID : [???]";
            lblAppliedFor.Text = "Applied For License : [???]";
            lblPassedTests.Text = "Passed Tests : 0/3";

            ctrlApplicationBasicInfo1.ResetApplicationInfo();
            llShowLicenseInfo.Enabled = false;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(_LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License found for this application yet!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gbLocalAppInfo_Enter(object sender, EventArgs e)
        {
        }

        private void gbDrivingLicenseApplicationInfo_Enter(object sender, EventArgs e)
        {
        }

        public bool DoesPassAllTests()
        {
            return (_Application != null && _Application.GetPassedTestCount() == 3);
        }
    }
}