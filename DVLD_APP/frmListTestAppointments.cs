using bus;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmListTestAppointments : Form
    {
        private DataTable _dtLicenseTestAppointments;
        private int _ApplicationID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        public frmListTestAppointments(int applicationID, clsTestType.enTestType testType)
        {
            InitializeComponent();
            _ApplicationID = applicationID;
            _TestType = testType;
        }

        private void _LoadTestTypeHeaderInfo()
        {
            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTitle.Text = "Vision Test Appointments";
                    this.Text = "Vision Test Appointments";
                    pbTestTypeImage.Image = Properties.Resources.Vision_512;
                    break;

                case clsTestType.enTestType.WrittenTest:
                    lblTitle.Text = "Written Test Appointments";
                    this.Text = "Written Test Appointments";
                    pbTestTypeImage.Image = Properties.Resources.Written_Test_512;
                    break;

                case clsTestType.enTestType.StreetTest:
                    lblTitle.Text = "Street Test Appointments";
                    this.Text = "Street Test Appointments";
                    pbTestTypeImage.Image = Properties.Resources.Street_Test_512;
                    break;
            }
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeHeaderInfo();
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfo(_ApplicationID);
            _RefreshAppointmentsList();
        }

        private void _RefreshAppointmentsList()
        {
            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_ApplicationID, _TestType);
            dgvLicenseTestAppointments.DataSource = _dtLicenseTestAppointments;

            lblRecordsCount.Text = $"# Records: {dgvLicenseTestAppointments.Rows.Count}";

            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                // إخفاء الأعمدة الزائدة
                if (dgvLicenseTestAppointments.Columns.Contains("TestTypeID"))
                    dgvLicenseTestAppointments.Columns["TestTypeID"].Visible = false;

                if (dgvLicenseTestAppointments.Columns.Contains("LocalDrivingLicenseApplicationID"))
                    dgvLicenseTestAppointments.Columns["LocalDrivingLicenseApplicationID"].Visible = false;

                if (dgvLicenseTestAppointments.Columns.Contains("CreatedByUserID"))
                    dgvLicenseTestAppointments.Columns["CreatedByUserID"].Visible = false;

                // ضبط العناوين بالاسم الصريح وليس برقم الإندكس
                if (dgvLicenseTestAppointments.Columns.Contains("TestAppointmentID"))
                {
                    dgvLicenseTestAppointments.Columns["TestAppointmentID"].HeaderText = "Appointment ID";
                    dgvLicenseTestAppointments.Columns["TestAppointmentID"].Width = 120;
                }

                if (dgvLicenseTestAppointments.Columns.Contains("AppointmentDate"))
                {
                    dgvLicenseTestAppointments.Columns["AppointmentDate"].HeaderText = "Appointment Date";
                    dgvLicenseTestAppointments.Columns["AppointmentDate"].Width = 180;
                }

                if (dgvLicenseTestAppointments.Columns.Contains("PaidFees"))
                {
                    dgvLicenseTestAppointments.Columns["PaidFees"].HeaderText = "Paid Fees";
                    dgvLicenseTestAppointments.Columns["PaidFees"].Width = 100;
                }

                if (dgvLicenseTestAppointments.Columns.Contains("IsLocked"))
                {
                    dgvLicenseTestAppointments.Columns["IsLocked"].HeaderText = "Is Locked";
                    dgvLicenseTestAppointments.Columns["IsLocked"].Width = 100;
                }
            }
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsApplications application = clsApplications.FindByLocalDrivingAppID(_ApplicationID);

            if (application == null)
            {
                MessageBox.Show("Application data was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataRow row in _dtLicenseTestAppointments.Rows)
            {
                if (Convert.ToBoolean(row["IsLocked"]) == false)
                {
                    MessageBox.Show("Person already has an active appointment for this test, you must take or lock the active appointment first.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            clsTest lastTest = clsTest.FindLastTestPerPersonAndTestType(
                application.ApplicantPersonID,
                (int)_TestType,
                application.LicenseClassID);

            if (lastTest != null && lastTest.TestResult)
            {
                MessageBox.Show("This person already passed this test before, you cannot add a new appointment.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm = new frmScheduleTest(_ApplicationID, _TestType);
            frm.ShowDialog();

            _RefreshAppointmentsList();
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfo(_ApplicationID);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLicenseTestAppointments.CurrentRow == null) return;

            int appointmentID = Convert.ToInt32(dgvLicenseTestAppointments.CurrentRow.Cells["TestAppointmentID"].Value);

            frmScheduleTest frm = new frmScheduleTest(_ApplicationID, _TestType, appointmentID);
            frm.ShowDialog();

            _RefreshAppointmentsList();
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfo(_ApplicationID);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLicenseTestAppointments.CurrentRow == null) return;

            int appointmentID = Convert.ToInt32(dgvLicenseTestAppointments.CurrentRow.Cells["TestAppointmentID"].Value);

            frmTakeTest frm = new frmTakeTest(appointmentID, _TestType);
            frm.ShowDialog();

            _RefreshAppointmentsList();
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfo(_ApplicationID);
        }

        private void cmsApplications_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dgvLicenseTestAppointments.CurrentRow == null)
            {
                e.Cancel = true;
                return;
            }

            bool isLocked = Convert.ToBoolean(dgvLicenseTestAppointments.CurrentRow.Cells["IsLocked"].Value);

            editToolStripMenuItem.Enabled = !isLocked;
            takeTestToolStripMenuItem.Enabled = !isLocked;



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

