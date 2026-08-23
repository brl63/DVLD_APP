using bus;
using System.IO;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID = -1;
        private clsInternationalLicense _InternationalLicense;

        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }

        public clsInternationalLicense SelectedInternationalLicenseInfo
        {
            get { return _InternationalLicense; }
        }

        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadInfo(int internationalLicenseID)
        {
            _InternationalLicenseID = internationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);

            if (_InternationalLicense == null)
            {
                MessageBox.Show($"Could not find International License with ID = {internationalLicenseID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }

            clsDriver driver = clsDriver.FindByDriverID(_InternationalLicense.DriverID);
            clsPeople person = driver?.PersonInfo;

            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblLocalLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToShortDateString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToShortDateString();
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();

            if (person != null)
            {
                lblName.Text = person.FullName;
                lblNationalNo.Text = person.NationalNo;
                lblGendor.Text = person.Gender == 0 ? "Male" : "Female";
                lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();

                if (!string.IsNullOrEmpty(person.ImagePath) && File.Exists(person.ImagePath))
                {
                    pbPersonImage.ImageLocation = person.ImagePath;
                }
            }
        }
    }
}
