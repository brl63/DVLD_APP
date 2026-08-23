using bus;
using DVLD_APP.helpers;
using System;
using System.Windows.Forms;
namespace DVLD_APP
{

    public partial class frmScheduleTest : Form

    {

        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode = enMode.AddNew;



        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 }

        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;



        private int _ApplicationID = -1;

        private clsApplications _Application;

        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        private clsTestType _TestTypeInfo;



        private int _TestAppointmentID = -1;

        private clsTestAppointment _TestAppointment;



        public frmScheduleTest(int applicationID, clsTestType.enTestType testType)

        {

            InitializeComponent();

            _ApplicationID = applicationID;

            _TestType = testType;

            _TestAppointmentID = -1;

            _Mode = enMode.AddNew;

        }



        public frmScheduleTest(int applicationID, clsTestType.enTestType testType, int appointmentID)

        {

            InitializeComponent();

            _ApplicationID = applicationID;

            _TestType = testType;

            _TestAppointmentID = appointmentID;

            _Mode = enMode.Update;

        }



        private void frmScheduleTest_Load(object sender, EventArgs e)

        {

            _Application = clsApplications.FindByLocalDrivingAppID(_ApplicationID);

            _TestTypeInfo = clsTestType.Find(_TestType);



            if (_Application == null || _TestTypeInfo == null)

            {

                MessageBox.Show("Error loading application or test type data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                return;

            }



            _LoadHeaderAndBaseInfo();



            if (_Mode == enMode.AddNew)

            {

                _TestAppointment = new clsTestAppointment();

                dtpTestDate.MinDate = DateTime.Now;

                _CheckRetakeTestStatus();

            }

            else

            {

                _LoadAppointmentForEdit();

            }

        }



        private void _LoadHeaderAndBaseInfo()

        {

            lblLocalAppID.Text = _ApplicationID.ToString();



            clsLicenseClass licenseClass = clsLicenseClass.Find(_Application.LicenseClassID);

            lblClass.Text = licenseClass != null ? licenseClass.ClassName : "N/A";



            clsPeople person = clsPeople.Find(_Application.ApplicantPersonID);

            lblName.Text = person != null ? person.FullName : "N/A";



            int totalTrials = clsTest.TotalTrialsPerTest(_ApplicationID, _TestType);

            lblTrial.Text = totalTrials.ToString();



            lblFees.Text = _TestTypeInfo.Fees.ToString("0.00");



            switch (_TestType)

            {

                case clsTestType.enTestType.VisionTest:

                    lblTitle.Text = "Schedule Vision Test";

                    this.Text = "Schedule Vision Test";

                    pbTestTypeImage.Image = Properties.Resources.Vision_512;

                    break;



                case clsTestType.enTestType.WrittenTest:

                    lblTitle.Text = "Schedule Written Test";

                    this.Text = "Schedule Written Test";

                    pbTestTypeImage.Image = Properties.Resources.Written_Test_512;

                    break;



                case clsTestType.enTestType.StreetTest:

                    lblTitle.Text = "Schedule Street Test";

                    this.Text = "Schedule Street Test";

                    pbTestTypeImage.Image = Properties.Resources.Street_Test_512;

                    break;

            }

        }



        private void _CheckRetakeTestStatus()

        {

            int totalTrials = clsTest.TotalTrialsPerTest(_ApplicationID, _TestType);



            if (totalTrials > 0)

            {

                _CreationMode = enCreationMode.RetakeTestSchedule;

                gbRetakeTestInfo.Enabled = true;

                lblTitle.Text = "Schedule Retake Test";



                clsApplicationTypes retakeAppType = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RetakeTest);

                decimal retakeFees = (retakeAppType != null) ? retakeAppType.ApplicationFees : 5;



                lblRetakeAppFees.Text = retakeFees.ToString("0.00");

                lblTotalFees.Text = (_TestTypeInfo.Fees + retakeFees).ToString("0.00");

                lblRetakeTestAppID.Text = "N/A";

            }

            else

            {

                _CreationMode = enCreationMode.FirstTimeSchedule;

                gbRetakeTestInfo.Enabled = false;

                lblRetakeAppFees.Text = "0.00";

                lblTotalFees.Text = _TestTypeInfo.Fees.ToString("0.00");

                lblRetakeTestAppID.Text = "N/A";

            }

        }



        private void _LoadAppointmentForEdit()

        {

            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);



            if (_TestAppointment == null)

            {

                MessageBox.Show($"Appointment ID = {_TestAppointmentID} not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                return;

            }



            if (_TestAppointment.IsLocked)

            {

                lblUserMessage.Visible = true;

                lblUserMessage.Text = "Person already sat for the test, appointment is locked.";

                dtpTestDate.Enabled = false;

                btnSave.Enabled = false;

            }

            else

            {

                lblUserMessage.Visible = false;

                dtpTestDate.MinDate = DateTime.Now < _TestAppointment.AppointmentDate ? DateTime.Now : _TestAppointment.AppointmentDate;

            }



            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            lblTotalFees.Text = _TestAppointment.PaidFees.ToString("0.00");

        }



        private bool _HandleRetakeApplication()
        {
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplicationTypes retakeAppType = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RetakeTest);
                decimal retakeFees = (retakeAppType != null) ? retakeAppType.ApplicationFees : 5;

                // تحديث رسوم الإعادة والإجمالي بدون إنشاء أو حفظ كائن Application
                lblRetakeAppFees.Text = retakeFees.ToString("0.00");
                lblTotalFees.Text = (_TestTypeInfo.Fees + retakeFees).ToString("0.00");
                lblRetakeTestAppID.Text = "N/A";
            }
            return true;
        }


        private void btnSave_Click(object sender, EventArgs e)

        {

            if (!_HandleRetakeApplication())

                return;



            _TestAppointment.TestTypeID = _TestType;

            _TestAppointment.LocalDrivingLicenseApplicationID = _ApplicationID;

            _TestAppointment.AppointmentDate = dtpTestDate.Value;

            _TestAppointment.PaidFees = Convert.ToDecimal(lblFees.Text);

            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;



            if (_TestAppointment.Save())

            {

                _Mode = enMode.Update;

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();

            }

            else

            {

                MessageBox.Show("Error: Data was not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }



        private void btnClose_Click(object sender, EventArgs e)

        {

            this.Close();

        }
    }
}