using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using bus; 

namespace DVLD_APP
{
    public partial class ctrlPersonCard : UserControl
    {
        private bus.clsPeople _Person; 
        private int _PersonID = -1;

        public int PersonID => _PersonID; 
        public clsPeople SelectedPersonInfo => _Person; 

        public bool EnableEditLink
        {
            get { return lblEditPersonInfo.Enabled; }
            set { lblEditPersonInfo.Enabled = value; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private void ctrlAddAndEdit_Load(object sender, EventArgs e)
        {

        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            _Person = null;

            lblID.Text = "[????]";
            lblNationalNumber.Text = "[????]";
            lblFullName.Text = "[????]";
            lblGen.Text = "[????]";
            lblEmail.Text = "[????]";
            lblNumber.Text = "[????]";
            lblDate.Text = "[????]";
            lblCountryName.Text = "[????]";
            lblAddressName.Text = "[????]";

            pbPersonImage.Image = Properties.Resources.DefaultMale;
            lblEditPersonInfo.Enabled = false;
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPeople.Find(PersonID);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with ID = " + PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPeople.Find(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        private void _FillPersonInfo()
        {
            _PersonID = _Person.PersonID;
            lblID.Text = _Person.PersonID.ToString();
            lblNationalNumber.Text = _Person.NationalNo;
            lblFullName.Text = _Person.FullName;
            lblGen.Text = (_Person.Gender == 0) ? "Male" : "Female";
            lblEmail.Text = string.IsNullOrEmpty(_Person.Email) ? "N/A" : _Person.Email;
            lblNumber.Text = _Person.Phone;
            lblDate.Text = _Person.DateOfBirth.ToShortDateString();

            lblCountryName.Text = string.IsNullOrEmpty(_Person.CountryName) ? "[????]" : clsCountries.GetCountryName(_Person.NationalityCountryID);
            // Try resolving country name directly from bus.clsCountries to avoid empty cached values
            try
            {
                string country = clsCountries.GetCountryName(_Person.NationalityCountryID);
                lblCountryName.Text = string.IsNullOrEmpty(country) ? "[????]" : country;
            }
            catch
            {
                lblCountryName.Text = string.IsNullOrEmpty(_Person.CountryName) ? "[????]" : _Person.CountryName;
            }
            lblAddressName.Text = _Person.Address;

            lblEditPersonInfo.Enabled = true;

            _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {

            if (_Person.Gender == 0)
                pbPersonImage.Image = Properties.Resources.DefaultMale;
            else
                // women image
                pbPersonImage.Image = Properties.Resources.DefaultMale;

            string ImagePath = _Person.ImagePath;

            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblEditPersonInfo_Click(object sender, EventArgs e)
        {
            //prepeare
        }
    }
}