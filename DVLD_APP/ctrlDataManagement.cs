using bus;
using DVLD_APP.helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlDataManagement : UserControl
    {
        private DataTable _theData = new DataTable();
        private BindingSource _bindingSource = new BindingSource();
        private clsHelpers.enDataMode _theMode;

        public ctrlDataManagement()
        {
            InitializeComponent();
        }

        public ctrlDataManagement(clsHelpers.enDataMode mode, DataTable data) : this()
        {
            SetData(mode, data);
        }

        public void SetData(clsHelpers.enDataMode mode, DataTable data)
        {
            _theData = data ?? new DataTable();
            _theMode = mode;

            _bindingSource.DataSource = _theData;
            dgvList.DataSource = _bindingSource;

            // إخفاء عمود الصورة إن وجد لتحسين العرض
            if (dgvList.Columns.Contains("ImagePath"))
            {
                dgvList.Columns["ImagePath"].Visible = false;
            }

            InitializeGridUI();
        }

        private void InitializeGridUI()
        {
            switch (_theMode)
            {
                case clsHelpers.enDataMode.People:
                    SetupPeopleContextMenu();
                    picName.Image = Properties.Resources.people_management;
                    lblName.Text = "Manage People";
                    break;

                case clsHelpers.enDataMode.Users:
                    SetupUsersContextMenu();
                    picName.Image = Properties.Resources.UsersManagment;
                    lblName.Text = "Manage Users";
                    break;

                case clsHelpers.enDataMode.Drivers:
                    SetupDriversContextMenu();
                    picName.Image = Properties.Resources.DriversManagment;
                    lblName.Text = "Manage Drivers";
                    break;

                case clsHelpers.enDataMode.LocalDrivingLicenseApplications:
                    SetupLocalDrivingLicenseApplicationsContextMenu();
                    picName.Image = Properties.Resources.ApplcationsManagment;
                    lblName.Text = "Local Driving License Applications";
                    break;

                case clsHelpers.enDataMode.InternationalDrivingLicenseApplications:
                    SetupInternationalDrivingApplicationsContextMenu();
                    picName.Image = Properties.Resources.ApplcationsManagment;
                    lblName.Text = "International License Applications";
                    break;

                case clsHelpers.enDataMode.TestAppointments:
                    SetupTestAppointmentsContextMenu();
                    picName.Image = Properties.Resources.Driving_tests;
                    lblName.Text = "Test Appointments Management";
                    break;

                case clsHelpers.enDataMode.ApplicationTypes:
                    SetupApplicationTypesContextMenu();
                    picName.Image = Properties.Resources.ApplcationsManagment;
                    lblName.Text = "Manage Application Types";
                    break;

                case clsHelpers.enDataMode.TestTypes:
                    SetupTestTypesContextMenu();
                    picName.Image = Properties.Resources.Driving_tests;
                    lblName.Text = "Manage Test Types";
                    break;

                case clsHelpers.enDataMode.DetainedLicenses:
                    SetupDetainedLicensesContextMenu();
                    picName.Image = Properties.Resources.DriversManagment;
                    lblName.Text = "Manage Detained Licenses";
                    break;
            }
        }

        // =========================================================
        // Helper Methods
        // =========================================================
        private int GetSelectedID()
        {
            if (dgvList.CurrentRow == null) return -1;

            // 1. تحديد العمود الأساسي بدقة بناءً على الشاشة المفتوحة حالياً
            string primaryColumn = "";
            switch (_theMode)
            {
                case clsHelpers.enDataMode.People:
                    primaryColumn = "PersonID";
                    break;

                case clsHelpers.enDataMode.Users:
                    primaryColumn = "UserID";
                    break;

                case clsHelpers.enDataMode.Drivers:
                    primaryColumn = "DriverID";
                    break;

                case clsHelpers.enDataMode.LocalDrivingLicenseApplications:
                    primaryColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case clsHelpers.enDataMode.InternationalDrivingLicenseApplications:
                    primaryColumn = "InternationalLicenseID";
                    break;

                case clsHelpers.enDataMode.TestAppointments:
                    primaryColumn = "TestAppointmentID";
                    break;

                case clsHelpers.enDataMode.ApplicationTypes:
                    primaryColumn = "ApplicationTypeID";
                    break;

                case clsHelpers.enDataMode.TestTypes:
                    primaryColumn = "TestTypeID";
                    break;

                case clsHelpers.enDataMode.DetainedLicenses:
                    primaryColumn = "DetainID";
                    break;
            }

            // التحقق من وجود العمود وقيمته
            if (!string.IsNullOrEmpty(primaryColumn) && dgvList.Columns.Contains(primaryColumn) && dgvList.CurrentRow.Cells[primaryColumn].Value != DBNull.Value)
            {
                return Convert.ToInt32(dgvList.CurrentRow.Cells[primaryColumn].Value);
            }

            // 2. كـ Fallback في حال كانت الشاشة خارج الحالات المحددة، نقرأ من أول عمود
            if (dgvList.CurrentRow.Cells.Count > 0 && dgvList.CurrentRow.Cells[0].Value != DBNull.Value)
            {
                if (int.TryParse(dgvList.CurrentRow.Cells[0].Value.ToString(), out int id))
                {
                    return id;
                }
            }

            return -1;
        }

        public void SelectPersonById(int personId)
        {
            if (personId <= 0 || !dgvList.Columns.Contains("PersonID")) return;

            foreach (DataGridViewRow row in dgvList.Rows)
            {
                if (row.Cells["PersonID"].Value != DBNull.Value && Convert.ToInt32(row.Cells["PersonID"].Value) == personId)
                {
                    dgvList.ClearSelection();
                    row.Selected = true;
                    dgvList.CurrentCell = row.Cells["PersonID"];
                    break;
                }
            }
        }

        private void _RefreshCurrentGrid()
        {
            switch (_theMode)
            {
                case clsHelpers.enDataMode.People:
                    _theData = bus.clsPeople.GetAll();
                    break;
                case clsHelpers.enDataMode.Users:
                    _theData = bus.clsUser.GetAll();
                    break;
                case clsHelpers.enDataMode.Drivers:
                    _theData = bus.clsDriver.GetAll();
                    break;
                case clsHelpers.enDataMode.LocalDrivingLicenseApplications:
                    _theData = bus.clsApplications.GetAllLocalApplications();
                    break;
                case clsHelpers.enDataMode.InternationalDrivingLicenseApplications:
                    _theData = bus.clsApplications.GetAllInternationalApplications();
                    break;
                case clsHelpers.enDataMode.ApplicationTypes:
                    _theData = bus.clsApplicationTypes.GetAll();
                    break;
                case clsHelpers.enDataMode.TestTypes:
                    _theData = bus.clsTestType.GetAllTestTypes();
                    break;
                case clsHelpers.enDataMode.DetainedLicenses:
                    _theData = bus.clsDetainedLicense.GetAllDetainedLicenses();
                    break;
            }

            _bindingSource.DataSource = _theData;
            _bindingSource.ResetBindings(false);
        }

        // =========================================================
        // 1. People Context & Actions
        // =========================================================
        private void SetupPeopleContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Details", null, (sender, e) => ShowPersonDetails());
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Add New Person", null, (sender, e) => ShowPersonForm(-1));
            cms.Items.Add("Edit", null, (sender, e) => ShowPersonForm(GetSelectedID()));
            cms.Items.Add("Delete", null, (sender, e) => DeletePerson());
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Send Email", null, (sender, e) => MessageBox.Show($"Send Email to Person ID: {GetSelectedID()}", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information));
            cms.Items.Add("Phone Call", null, (sender, e) => MessageBox.Show($"Phone Call to Person ID: {GetSelectedID()}", "Phone", MessageBoxButtons.OK, MessageBoxIcon.Information));

            dgvList.ContextMenuStrip = cms;
            dgvList.DoubleClick -= DgvList_DoubleClick;
            dgvList.DoubleClick += DgvList_DoubleClick;
        }

        private void ShowPersonForm(int personId = -1)
        {
            try
            {
                bool isNew = (personId <= 0);
                using (Form frm = new Form())
                {
                    frm.Text = isNew ? "Add New Person" : "Edit Person";
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.Size = new Size(900, 650);

                    clsUpdateAndDelete.enMode mode = isNew ? clsUpdateAndDelete.enMode.New : clsUpdateAndDelete.enMode.Update;
                    clsUpdateAndDelete editor = new clsUpdateAndDelete(mode);
                    editor.Dock = DockStyle.Fill;
                    frm.Controls.Add(editor);

                    if (!isNew)
                    {
                        editor.LoadPersonForEdit(personId);
                    }

                    editor.PersonSaved += (sender, args) =>
                    {
                        _RefreshCurrentGrid();
                        if (args != null && args.PersonID > 0)
                        {
                            SelectPersonById(args.PersonID);
                        }
                        try { frm.Close(); } catch { }
                    };

                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Person Form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowPersonDetails()
        {
            int id = GetSelectedID();
            if (id <= 0) return;

            using (Form frm = new Form())
            {
                frm.Text = "Person Details";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Size = new Size(900, 650);

                ctrlPersonCard card = new ctrlPersonCard();
                card.Dock = DockStyle.Fill;
                card.LoadPersonInfo(id);

                frm.Controls.Add(card);
                frm.ShowDialog();
            }
        }

        private void DeletePerson()
        {
            int id = GetSelectedID();
            if (id <= 0) return;

            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (bus.clsPeople.Delete(id))
                {
                    MessageBox.Show("Person deleted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshCurrentGrid();
                }
                else
                {
                    MessageBox.Show("Cannot delete this person because they have related data (Applications, Licenses, or Drivers) linked to their record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // =========================================================
        // 2. Users Context & Actions
        // =========================================================
        private void SetupUsersContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Details", null, (s, e) => ShowUserDetails());
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Add New User", null, (s, e) => ShowAddUserForm());
            cms.Items.Add("Delete", null, (s, e) =>
            {
                int userID = GetSelectedID();
                if (userID <= 0) return;
                if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (clsUser.Delete(userID))
                    {
                        MessageBox.Show("User Deleted Successfully.");
                        _RefreshCurrentGrid();
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete this user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
            cms.Items.Add("Change Password", null, (s, e) =>
            {
                int userID = GetSelectedID();
                if (userID <= 0) return;
                using (Form frm = new Form())
                {
                    frm.Text = "Change Password";
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.Size = new Size(800, 500);
                    ctrlChangePassword ctrl = new ctrlChangePassword();
                    ctrl.Dock = DockStyle.Fill;
                    frm.Controls.Add(ctrl);
                    frm.ShowDialog();
                }
            });

            dgvList.ContextMenuStrip = cms;
            dgvList.DoubleClick -= DgvList_DoubleClick;
            dgvList.DoubleClick += DgvList_DoubleClick;
        }

        private void ShowUserDetails()
        {
            int id = GetSelectedID();
            if (id <= 0) return;

            using (Form frm = new Form())
            {
                frm.Text = "User Details";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Size = new Size(900, 650);
                ctrlUserCard card = new ctrlUserCard();
                card.Dock = DockStyle.Fill;
                card.LoadUserInfo(id);
                frm.Controls.Add(card);
                frm.ShowDialog();
            }
        }

        private void ShowAddUserForm()
        {
            using (Form frm = new Form())
            {
                frm.Text = "Add New User";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Size = new Size(1320, 780);

                ctrlAddUser addUserControl = new ctrlAddUser();
                addUserControl.Dock = DockStyle.Fill;
                frm.Controls.Add(addUserControl);

                frm.ShowDialog();
                _RefreshCurrentGrid();
            }
        }

        // =========================================================
        // 3. Drivers Context & Actions
        // =========================================================
        private void SetupDriversContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            // 1. عرض بيانات الشخص
            cms.Items.Add("Show Person Info", null, (s, e) =>
            {
                int driverID = GetSelectedID();
                clsDriver driver = clsDriver.FindByDriverID(driverID);
                if (driver != null)
                {
                    using (Form frm = new Form())
                    {
                        frm.Text = "Person Details";
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.Size = new Size(900, 650);
                        ctrlPersonCard card = new ctrlPersonCard();
                        card.Dock = DockStyle.Fill;
                        card.LoadPersonInfo(driver.PersonID);
                        frm.Controls.Add(card);
                        frm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Driver not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            cms.Items.Add(new ToolStripSeparator());

            // 2. إصدار رخصة دولية
            cms.Items.Add("Issue International License", null, (s, e) =>
            {
                frmIssueInternationalDrivingLicense frm = new frmIssueInternationalDrivingLicense();
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });

            // 3. عرض تاريخ الرخص للشخص
            cms.Items.Add("Show Person License History", null, (s, e) =>
            {
                int driverID = GetSelectedID();
                clsDriver driver = clsDriver.FindByDriverID(driverID);
                if (driver != null)
                {
                    frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(driver.PersonID);
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Driver not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // 4. Local Driving License Applications Context
        // =========================================================
        private void SetupLocalDrivingLicenseApplicationsContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem itemShowDetails = new ToolStripMenuItem("Show Application Details", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;

                using (Form frm = new Form())
                {
                    frm.Text = "Local Driving License Application Info";
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    frm.ClientSize = new Size(880, 380);

                    ctrlLocalDrivingLicenseApplicationInfo ctrl = new ctrlLocalDrivingLicenseApplicationInfo();
                    ctrl.Dock = DockStyle.Fill;
                    ctrl.LoadApplicationInfo(id);

                    frm.Controls.Add(ctrl);
                    frm.ShowDialog();
                }
            });

            ToolStripMenuItem itemEditApp = new ToolStripMenuItem("Edit Application", null, (s, e) =>
            {
                int localAppID = GetSelectedID();
                if (localAppID <= 0) return;

                frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication(localAppID);
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });

            ToolStripMenuItem itemDeleteApp = new ToolStripMenuItem("Delete Application", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;

                if (MessageBox.Show("Are you sure you want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    clsApplications app = clsApplications.FindByLocalDrivingAppID(id);
                    if (app != null && app.Delete())
                    {
                        MessageBox.Show("Application Deleted Successfully.");
                        _RefreshCurrentGrid();
                    }
                    else
                    {
                        MessageBox.Show("Error: Cannot delete application. It has related records.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });

            ToolStripMenuItem itemCancelApp = new ToolStripMenuItem("Cancel Application", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;

                if (MessageBox.Show("Are you sure you want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    clsApplications app = clsApplications.FindByLocalDrivingAppID(id);
                    if (app != null && app.Cancel())
                    {
                        MessageBox.Show("Application Cancelled Successfully.");
                        _RefreshCurrentGrid();
                    }
                }
            });

            // قائمة الاختبارات الفرعية
            ToolStripMenuItem scheduleTests = new ToolStripMenuItem("Schedule Tests");

            ToolStripMenuItem itemScheduleVision = new ToolStripMenuItem("Schedule Vision Test", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;
                frmListTestAppointments frm = new frmListTestAppointments(id, clsTestType.enTestType.VisionTest);
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });

            ToolStripMenuItem itemScheduleWritten = new ToolStripMenuItem("Schedule Written Test", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;
                frmListTestAppointments frm = new frmListTestAppointments(id, clsTestType.enTestType.WrittenTest);
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });

            ToolStripMenuItem itemScheduleStreet = new ToolStripMenuItem("Schedule Street Test", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;
                frmListTestAppointments frm = new frmListTestAppointments(id, clsTestType.enTestType.StreetTest);
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });

            scheduleTests.DropDownItems.Add(itemScheduleVision);
            scheduleTests.DropDownItems.Add(itemScheduleWritten);
            scheduleTests.DropDownItems.Add(itemScheduleStreet);

            // إصدار الرخصة للمرة الأولى
            ToolStripMenuItem itemIssueLicense = new ToolStripMenuItem("Issue Driving License (First Time)", null, (s, e) =>
            {
                int localAppID = GetSelectedID();
                if (localAppID <= 0) return;

                frmIssueDrivingLicenseFirstTime frm = new frmIssueDrivingLicenseFirstTime(localAppID);
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });

            // عرض الرخصة
            ToolStripMenuItem itemShowLicense = new ToolStripMenuItem("Show License", null, (s, e) =>
            {
                int localAppID = GetSelectedID();
                if (localAppID <= 0) return;

                clsApplications app = clsApplications.FindByLocalDrivingAppID(localAppID);
                if (app != null)
                {
                    int licenseID = app.GetActiveLicenseID();
                    if (licenseID != -1)
                    {
                        frmShowLicenseInfo frm = new frmShowLicenseInfo(licenseID);
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No License found for this application!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });

            // عرض تاريخ الرخص
            ToolStripMenuItem itemShowLicenseHistory = new ToolStripMenuItem("Show Person License History", null, (s, e) =>
            {
                int localAppID = GetSelectedID();
                if (localAppID <= 0) return;

                clsApplications app = clsApplications.FindByLocalDrivingAppID(localAppID);
                if (app != null)
                {
                    frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(app.ApplicantPersonID);
                    frm.ShowDialog();
                }
            });

            cms.Items.Add(itemShowDetails);
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add(itemEditApp);
            cms.Items.Add(itemDeleteApp);
            cms.Items.Add(itemCancelApp);
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add(scheduleTests);
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add(itemIssueLicense);
            cms.Items.Add(itemShowLicense);
            cms.Items.Add(itemShowLicenseHistory);

            // فحص التفعيل والقفل التتابعي عند النقر كليك يمين
            cms.Opening += (s, e) =>
            {
                int localAppID = GetSelectedID();
                if (localAppID <= 0 || dgvList.CurrentRow == null)
                {
                    e.Cancel = true;
                    return;
                }

                clsApplications app = clsApplications.FindByLocalDrivingAppID(localAppID);
                if (app == null)
                {
                    e.Cancel = true;
                    return;
                }

                byte passedTests = 0;
                if (dgvList.Columns.Contains("PassedTestCount") && dgvList.CurrentRow.Cells["PassedTestCount"].Value != DBNull.Value)
                {
                    passedTests = Convert.ToByte(dgvList.CurrentRow.Cells["PassedTestCount"].Value);
                }

                bool isNew = (app.ApplicationStatus == clsApplications.enApplicationStatus.New);
                bool isCompleted = (app.ApplicationStatus == clsApplications.enApplicationStatus.Completed);

                // 1. التحكم في قائمة الاختبارات وتتابعها
                scheduleTests.Enabled = (isNew && passedTests < 3);
                itemScheduleVision.Enabled = (isNew && passedTests == 0);
                itemScheduleWritten.Enabled = (isNew && passedTests == 1);
                itemScheduleStreet.Enabled = (isNew && passedTests == 2);

                // 2. إصدار وعرض الرخصة
                itemIssueLicense.Enabled = (isNew && passedTests == 3);
                itemShowLicense.Enabled = isCompleted;

                // 3. العمليات الأساسية
                itemEditApp.Enabled = isNew;
                itemDeleteApp.Enabled = isNew;
                itemCancelApp.Enabled = isNew;
            };

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // 5. International License Applications Context
        // =========================================================
        private void SetupInternationalDrivingApplicationsContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            cms.Items.Add("Show Person Info", null, (s, e) =>
            {
                int intLicenseID = GetSelectedID();
                clsInternationalLicense intLicense = clsInternationalLicense.Find(intLicenseID);
                if (intLicense != null)
                {
                    clsDriver driver = clsDriver.FindByDriverID(intLicense.DriverID);
                    if (driver != null)
                    {
                        using (Form frm = new Form())
                        {
                            frm.Text = "Person Details";
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.Size = new Size(900, 650);
                            ctrlPersonCard card = new ctrlPersonCard();
                            card.Dock = DockStyle.Fill;
                            card.LoadPersonInfo(driver.PersonID);
                            frm.Controls.Add(card);
                            frm.ShowDialog();
                        }
                    }
                }
            });

            cms.Items.Add("Show License Details", null, (s, e) =>
            {
                int intLicenseID = GetSelectedID();
                if (intLicenseID > 0)
                {
                    frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(intLicenseID);
                    frm.ShowDialog();
                }
            });

            cms.Items.Add("Show Person License History", null, (s, e) =>
            {
                int intLicenseID = GetSelectedID();
                clsInternationalLicense intLicense = clsInternationalLicense.Find(intLicenseID);
                if (intLicense != null)
                {
                    clsDriver driver = clsDriver.FindByDriverID(intLicense.DriverID);
                    if (driver != null)
                    {
                        frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(driver.PersonID);
                        frm.ShowDialog();
                    }
                }
            });

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // 6. Test Appointments Context
        // =========================================================
        private void SetupTestAppointmentsContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Edit Appointment", null, (s, e) => MessageBox.Show($"Edit Appointment ID: {GetSelectedID()}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information));
            cms.Items.Add("Take Test", null, (s, e) => MessageBox.Show($"Take Test for Appointment ID: {GetSelectedID()}", "Take Test", MessageBoxButtons.OK, MessageBoxIcon.Information));
            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // 7. Application Types Context (تم ربط الفورم الفعلي)
        // =========================================================
        private void SetupApplicationTypesContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Edit Application Type", null, (s, e) =>
            {
                int appTypeID = GetSelectedID();
                if (appTypeID <= 0) return;

                frmEditApplicationType frm = new frmEditApplicationType(appTypeID);
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });
            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // 8. Test Types Context (تم ربط الفورم الفعلي)
        // =========================================================
        private void SetupTestTypesContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Edit Test Type", null, (s, e) =>
            {
                int testTypeID = GetSelectedID();
                if (testTypeID <= 0) return;

                frmEditTestType frm = new frmEditTestType(testTypeID);
                frm.ShowDialog();
                _RefreshCurrentGrid();
            });
            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // 9. Detained Licenses Context
        // =========================================================
        private void SetupDetainedLicensesContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            cms.Items.Add("Show Person Details", null, (s, e) =>
            {
                int detainID = GetSelectedID();
                clsDetainedLicense detained = clsDetainedLicense.Find(detainID);
                if (detained != null)
                {
                    clsLicense license = clsLicense.Find(detained.LicenseID);
                    if (license != null)
                    {
                        using (Form frm = new Form())
                        {
                            frm.Text = "Person Details";
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.Size = new Size(900, 650);
                            ctrlPersonCard card = new ctrlPersonCard();
                            card.Dock = DockStyle.Fill;
                            card.LoadPersonInfo(license.DriverInfo.PersonID);
                            frm.Controls.Add(card);
                            frm.ShowDialog();
                        }
                    }
                }
            });

            cms.Items.Add("Show License Details", null, (s, e) =>
            {
                int detainID = GetSelectedID();
                clsDetainedLicense detained = clsDetainedLicense.Find(detainID);
                if (detained != null)
                {
                    frmShowLicenseInfo frm = new frmShowLicenseInfo(detained.LicenseID);
                    frm.ShowDialog();
                }
            });

            cms.Items.Add("Show Person License History", null, (s, e) =>
            {
                int detainID = GetSelectedID();
                clsDetainedLicense detained = clsDetainedLicense.Find(detainID);
                if (detained != null)
                {
                    clsLicense license = clsLicense.Find(detained.LicenseID);
                    if (license != null)
                    {
                        frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(license.DriverInfo.PersonID);
                        frm.ShowDialog();
                    }
                }
            });

            cms.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem itemRelease = new ToolStripMenuItem("Release Detained License", null, (s, e) =>
            {
                int detainID = GetSelectedID();
                clsDetainedLicense detained = clsDetainedLicense.Find(detainID);
                if (detained != null)
                {
                    frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication(detained.LicenseID);
                    frm.ShowDialog();
                    _RefreshCurrentGrid();
                }
            });

            cms.Items.Add(itemRelease);

            cms.Opening += (s, e) =>
            {
                int detainID = GetSelectedID();
                clsDetainedLicense detained = clsDetainedLicense.Find(detainID);
                if (detained != null)
                {
                    itemRelease.Enabled = !detained.IsReleased;
                }
            };

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // Event Handlers
        // =========================================================
        private void dgvList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvList.ClearSelection();
                dgvList.Rows[e.RowIndex].Selected = true;
                dgvList.CurrentCell = dgvList.Rows[e.RowIndex].Cells[e.ColumnIndex < 0 ? 0 : e.ColumnIndex];
            }
        }

        private void DgvList_DoubleClick(object sender, EventArgs e)
        {
            int id = GetSelectedID();
            if (id > 0 && _theMode == clsHelpers.enDataMode.People)
            {
                ShowPersonForm(id);
            }
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            _RefreshCurrentGrid();
        }

        private void ctrlDataManagement_Load(object sender, EventArgs e) { }
        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cmsData_Opening(object sender, System.ComponentModel.CancelEventArgs e) { }
        private void dgvList_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
    }
}