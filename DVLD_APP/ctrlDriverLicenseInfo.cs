using System;
using System.IO;
using System.Windows.Forms;
using bus;

namespace DVLD_APP
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LicenseID = -1;
        private clsLicense _License;

        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return _License; }
        }

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadLicenseInfo(int licenseID)
        {
            _LicenseID = licenseID;
            _License = clsLicense.Find(_LicenseID);

            if (_License == null)
            {
                ResetLicenseInfo();
                MessageBox.Show($"No License with ID = {licenseID} was found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLicenseInfo();
        }

        private void _FillLicenseInfo()
        {
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";

            // فئة الرخصة
            clsLicenseClass licenseClass = clsLicenseClass.Find(_License.LicenseClassID);
            lblClass.Text = licenseClass != null ? licenseClass.ClassName : "N/A";

            // بيانات السائق والشخص
            clsDriver driver = clsDriver.FindByDriverID(_License.DriverID);
            if (driver != null && driver.PersonInfo != null)
            {
                lblName.Text = driver.PersonInfo.FullName;
                lblNationalNo.Text = driver.PersonInfo.NationalNo;
                lblGendor.Text = driver.PersonInfo.Gender == 0 ? "Male" : "Female";
                lblDateOfBirth.Text = driver.PersonInfo.DateOfBirth.ToShortDateString();
                lblDriverID.Text = driver.DriverID.ToString();

                // تحميل الصورة
                if (!string.IsNullOrEmpty(driver.PersonInfo.ImagePath) && File.Exists(driver.PersonInfo.ImagePath))
                {
                    pbPersonImage.ImageLocation = driver.PersonInfo.ImagePath;
                }
                else
                {
                    pbPersonImage.Image = null; // أو صورة افتراضية حسب النوع
                }
            }

            lblIssueDate.Text = _License.IssueDate.ToShortDateString();
            lblExpirationDate.Text = _License.ExpirationDate.ToShortDateString();
            lblNotes.Text = string.IsNullOrEmpty(_License.Notes) ? "No Notes" : _License.Notes;
            lblIsDetained.Text = "No"; // سيتم ربطها لاحقاً بجدول Detained Licenses
        }

        public void ResetLicenseInfo()
        {
            _LicenseID = -1;
            _License = null;

            lblClass.Text = "[???]";
            lblName.Text = "[???]";
            lblLicenseID.Text = "[???]";
            lblNationalNo.Text = "[???]";
            lblGendor.Text = "[???]";
            lblIssueDate.Text = "[???]";
            lblNotes.Text = "[???]";
            lblIsActive.Text = "[???]";
            lblDateOfBirth.Text = "[???]";
            lblDriverID.Text = "[???]";
            lblExpirationDate.Text = "[???]";
            lblIsDetained.Text = "[???]";
            pbPersonImage.Image = null;
        }
    }
}
