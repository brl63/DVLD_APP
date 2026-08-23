using bus;
using DVLD_APP.helpers;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmTakeTest : Form
    {
        private int _AppointmentID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;
        private clsTestAppointment _Appointment;
        private int _TestID = -1;

        public frmTakeTest(int appointmentID, clsTestType.enTestType testType)
        {
            InitializeComponent();
            _AppointmentID = appointmentID;
            _TestType = testType;
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTitle.Text = "Vision Test";
                    this.Text = "Take Vision Test";
                    pbTestTypeImage.Image = Properties.Resources.Vision_512;
                    break;

                case clsTestType.enTestType.WrittenTest:
                    lblTitle.Text = "Written Test";
                    this.Text = "Take Written Test";
                    pbTestTypeImage.Image = Properties.Resources.Written_Test_512;
                    break;

                case clsTestType.enTestType.StreetTest:
                    lblTitle.Text = "Street Test";
                    this.Text = "Take Street Test";
                    pbTestTypeImage.Image = Properties.Resources.Street_Test_512;
                    break;
            }
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            _Appointment = clsTestAppointment.Find(_AppointmentID);

            if (_Appointment == null)
            {
                MessageBox.Show($"Appointment with ID = {_AppointmentID} does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            _LoadAppointmentData();

            _TestID = _Appointment.TestID;

            if (_TestID != -1)
            {
                clsTest test = clsTest.Find(_TestID);

                if (test != null)
                {
                    if (test.TestResult)
                        rbPass.Checked = true;
                    else
                        rbFail.Checked = true;

                    txtNotes.Text = test.Notes;
                    lblTestID.Text = test.TestID.ToString();
                    lblUserMessage.Visible = true;
                    lblUserMessage.Text = "Cannot take test, appointment is locked.";

                    btnSave.Enabled = false;
                    rbPass.Enabled = false;
                    rbFail.Enabled = false;
                    txtNotes.Enabled = false;
                }
            }
            else
            {
                lblTestID.Text = "Not Taken Yet";
            }
        }

        private void _LoadAppointmentData()
        {
            clsApplications app = clsApplications.FindByLocalDrivingAppID(_Appointment.LocalDrivingLicenseApplicationID);

            if (app == null)
            {
                MessageBox.Show("Application data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            lblLocalAppID.Text = _Appointment.LocalDrivingLicenseApplicationID.ToString();

            clsLicenseClass licenseClass = clsLicenseClass.Find(app.LicenseClassID);
            lblClass.Text = licenseClass != null ? licenseClass.ClassName : "N/A";

            clsPeople person = clsPeople.Find(app.ApplicantPersonID);
            lblName.Text = person != null ? person.FullName : "N/A";

            // جلب عدد المحاولات السابقة
            lblTrial.Text = clsTest.TotalTrialsPerTest(_Appointment.LocalDrivingLicenseApplicationID, _TestType).ToString();

            lblDate.Text = _Appointment.AppointmentDate.ToShortDateString();
            lblFees.Text = _Appointment.PaidFees.ToString("0.00");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save this result? After saving, you cannot change the Pass/Fail result.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            clsTest test = new clsTest();
            test.TestAppointmentID = _AppointmentID;
            test.TestResult = rbPass.Checked;
            test.Notes = txtNotes.Text.Trim();
            test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (test.Save())
            {
                _TestID = test.TestID;
                lblTestID.Text = _TestID.ToString();

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnSave.Enabled = false;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
                txtNotes.Enabled = false;
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Test result is saved and appointment is locked.";

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

        private void gbTestInfo_Enter(object sender, EventArgs e)
        {
        }
    }
}