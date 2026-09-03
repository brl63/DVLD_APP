using bus;
using DVLD_APP.helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode = enMode.AddNew;

        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        private clsApplications _LocalApp;

        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicenseApplication(int localDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            _Mode = enMode.Update;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAll();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClasses.Items.Add(row["ClassName"]);
            }

            if (cbLicenseClasses.Items.Count > 0)
                cbLicenseClasses.SelectedIndex = 2; // Ordinary driving license افتراضياً
        }

        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                this.Text = "New Local Driving License Application";
                lblTitle.Text = "New Local Driving License Application";
                _LocalApp = new clsApplications();

                tpApplicationInfo.Enabled = false;
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblApplicationFees.Text = clsApplicationTypes.Find(1)?.ApplicationFees.ToString("0.00") ?? "15.00";
                lblCreatedByUser.Text = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserName : "Admin";
                btnSave.Enabled = false;
            }
            else
            {
                this.Text = "Update Local Driving License Application";
                lblTitle.Text = "Update Local Driving License Application";
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void _LoadData()
        {
            _LocalApp = clsApplications.FindByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);

            if (_LocalApp == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalApp.ApplicantPersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _LocalApp.ApplicationDate.ToShortDateString();
            cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(clsLicenseClass.Find(_LocalApp.LicenseClassID)?.ClassName);
            lblApplicationFees.Text = _LocalApp.PaidFees.ToString("0.00");
            lblCreatedByUser.Text = clsUser.Find(_LocalApp.CreatedByUserID)?.UserName ?? "Admin";
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int personID)
        {
            _SelectedPersonID = personID;

            if (_SelectedPersonID == -1)
            {
                tpApplicationInfo.Enabled = false;
                btnSave.Enabled = false;
                return;
            }

            tpApplicationInfo.Enabled = true;
            btnSave.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterEnabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        
            int licenseClassID = clsLicenseClass.Find(cbLicenseClasses.Text).LicenseClassID;

            int activeLicenseID = clsLicense.GetActiveLicenseIDByApplicationID(ctrlPersonCardWithFilter1.PersonID);

            clsDriver driver = clsDriver.FindByPersonID(ctrlPersonCardWithFilter1.PersonID);
            if (driver != null && clsLicense.CheckActiveLicenseByClass(driver.DriverID, licenseClassID))
            {
                MessageBox.Show("Person already has an active license for this class.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_Mode == enMode.AddNew && clsApplications.DoesPersonHaveActiveApplicationForLicenseClass(ctrlPersonCardWithFilter1.PersonID, 1, licenseClassID))
            {
                MessageBox.Show("Choose another License Class, the selected Person already has an active application for the selected class.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int currentUserID = clsGlobal.CurrentUser != null ? clsGlobal.CurrentUser.UserID : 1;

            _LocalApp.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LocalApp.ApplicationDate = DateTime.Now;
            _LocalApp.ApplicationTypeID = 1; // 1 = New Driving License Service
            _LocalApp.ApplicationStatus = clsApplications.enApplicationStatus.New;
            _LocalApp.LastStatusDate = DateTime.Now;
            _LocalApp.PaidFees = Convert.ToDecimal(lblApplicationFees.Text);
            _LocalApp.CreatedByUserID = currentUserID;
            _LocalApp.LicenseClassID = licenseClassID;

            if (_LocalApp.Save())
            {
                // تحديث الـ ID والـ Label من الكائن مباشرة بعد الحفظ
                _LocalDrivingLicenseApplicationID = _LocalApp.LocalDrivingLicenseApplicationID;
                lblLocalDrivingLicebseApplicationID.Text = _LocalApp.LocalDrivingLicenseApplicationID.ToString();

                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}