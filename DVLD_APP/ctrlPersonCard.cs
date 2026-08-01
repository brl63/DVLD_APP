using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using bus; // طبقة الـ Business Layer

namespace DVLD_APP
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonID = -1;
        private clsPeople _Person; // كائن الشخص من الـ Business Layer باسم clsPeople

        // الخصائص (Properties)
        public int PersonID => _PersonID; // Property للقراءة فقط
        public clsPeople SelectedPersonInfo => _Person; // إرجاع كائن الشخص بالكامل

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

        // دالة تفريغ البيانات للوضع الافتراضي
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

            // رجعت اسم الصورة زي ما كان عندك بالضبط
            pbPersonImage.Image = Properties.Resources.DefaultMale;
            lblEditPersonInfo.Enabled = false;
        }

        // تحميل البيانات برقم الـ ID
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

        // تحميل البيانات بالرقم القومي
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

        // الدالة التي تقوم بتعبئة الـ Labels من الكائن _Person
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

            lblCountryName.Text = _Person.CountryName : "[????]";
            lblAddressName.Text = _Person.Address;

            lblEditPersonInfo.Enabled = true;

            // معالجة تحميل الصورة
            _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {
            // الاعتماد على DefaultMale زي ما كانت في كودك الأصلي
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
            // كود التعديل لما تجهز الشاشة
        }
    }
}