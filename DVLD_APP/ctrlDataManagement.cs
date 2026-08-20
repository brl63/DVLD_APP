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

            // إخفاء عمود الصورة إذا كان موجوداً لتجميل الواجهة
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

            string[] possibleIdColumns = new string[]
            {
                "LocalDrivingLicenseApplicationID",
                "InternationalLicenseID",
                "ApplicationID",
                "TestAppointmentID",
                "ApplicationTypeID",
                "TestTypeID",
                "DetainID",
                "PersonID",
                "UserID",
                "DriverID",
                "LicenseID"
            };

            foreach (string colName in possibleIdColumns)
            {
                if (dgvList.Columns.Contains(colName) && dgvList.CurrentRow.Cells[colName].Value != DBNull.Value)
                {
                    return Convert.ToInt32(dgvList.CurrentRow.Cells[colName].Value);
                }
            }

            // Fallback لأول خلية إذا لم يتطابق أي اسم
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

        // =========================================================
        // People Context & Actions
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
            cms.Items.Add("Send Email", null, (sender, e) => MessageBox.Show($"Send Email to Person ID: {GetSelectedID()}"));
            cms.Items.Add("Phone Call", null, (sender, e) => MessageBox.Show($"Phone Call to Person ID: {GetSelectedID()}"));

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
                        try
                        {
                            _theData = bus.clsPeople.GetAll();
                            _bindingSource.DataSource = _theData;
                            _bindingSource.ResetBindings(false);

                            if (args != null && args.PersonID > 0)
                            {
                                SelectPersonById(args.PersonID);
                            }
                        }
                        catch { }

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
            if (id <= 0)
            {
                MessageBox.Show("No person selected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
            if (id <= 0)
            {
                MessageBox.Show("No person selected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this person?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (bus.clsPeople.Delete(id))
                {
                    MessageBox.Show("Person deleted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _theData = bus.clsPeople.GetAll();
                    _bindingSource.DataSource = _theData;
                    _bindingSource.ResetBindings(false);
                }
                else
                {
                    MessageBox.Show("Error deleting person. Make sure that person isn't a driver, user, or has applications.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // =========================================================
        // Users Context & Actions
        // =========================================================
        private void SetupUsersContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Details", null, (s, e) => ShowUserDetails());
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Add New User", null, (s, e) => ShowAddUserForm());
            cms.Items.Add("Edit", null, (s, e) => MessageBox.Show($"Edit User ID: {GetSelectedID()}"));
            cms.Items.Add("Delete", null, (s, e) => MessageBox.Show($"Delete User ID: {GetSelectedID()}"));
            cms.Items.Add("Change Password", null, (s, e) => MessageBox.Show($"Change Password for User ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Send Email", null, (s, e) => MessageBox.Show($"Send Email to User ID: {GetSelectedID()}"));
            cms.Items.Add("Phone Call", null, (s, e) => MessageBox.Show($"Phone Call to User ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
            dgvList.DoubleClick -= DgvList_DoubleClick;
            dgvList.DoubleClick += DgvList_DoubleClick;
        }

        private void ShowUserDetails()
        {
            int id = GetSelectedID();
            if (id <= 0)
            {
                MessageBox.Show("No user selected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
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
                RefreshUsersGrid();
            }
        }

        private void RefreshUsersGrid()
        {
            try
            {
                _theData = bus.clsUser.GetAll();
                _bindingSource.DataSource = _theData;
                _bindingSource.ResetBindings(false);
            }
            catch { }
        }

        // =========================================================
        // Drivers Context
        // =========================================================
        private void SetupDriversContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Person Info", null, (s, e) => MessageBox.Show($"Show Person Info for Driver ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Issue International License", null, (s, e) => MessageBox.Show($"Issue International License for Driver ID: {GetSelectedID()}"));
            cms.Items.Add("Show Person License History", null, (s, e) => MessageBox.Show($"Show License History for Driver ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // Local Driving License Applications Context
        // =========================================================
        private void SetupLocalDrivingLicenseApplicationsContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem itemShowDetails = new ToolStripMenuItem("Show Application Details", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;

                // فتح الـ User Control الخاص بتفاصيل الطلب داخل فورم مؤقت
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
                MessageBox.Show($"Edit App ID: {GetSelectedID()}");
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
                        _theData = bus.clsApplications.GetAllLocalApplications();
                        _bindingSource.DataSource = _theData;
                        _bindingSource.ResetBindings(false);
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
                        _theData = bus.clsApplications.GetAllLocalApplications();
                        _bindingSource.DataSource = _theData;
                        _bindingSource.ResetBindings(false);
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

                _theData = bus.clsApplications.GetAllLocalApplications();
                _bindingSource.DataSource = _theData;
                _bindingSource.ResetBindings(false);
            });

            ToolStripMenuItem itemScheduleWritten = new ToolStripMenuItem("Schedule Written Test", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;

                frmListTestAppointments frm = new frmListTestAppointments(id, clsTestType.enTestType.WrittenTest);
                frm.ShowDialog();

                _theData = bus.clsApplications.GetAllLocalApplications();
                _bindingSource.DataSource = _theData;
                _bindingSource.ResetBindings(false);
            });

            ToolStripMenuItem itemScheduleStreet = new ToolStripMenuItem("Schedule Street Test", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0) return;

                frmListTestAppointments frm = new frmListTestAppointments(id, clsTestType.enTestType.StreetTest);
                frm.ShowDialog();

                _theData = bus.clsApplications.GetAllLocalApplications();
                _bindingSource.DataSource = _theData;
                _bindingSource.ResetBindings(false);
            });

            scheduleTests.DropDownItems.Add(itemScheduleVision);
            scheduleTests.DropDownItems.Add(itemScheduleWritten);
            scheduleTests.DropDownItems.Add(itemScheduleStreet);

            // ربط زر إصدار الرخصة للمرة الأولى
            ToolStripMenuItem itemIssueLicense = new ToolStripMenuItem("Issue Driving License (First Time)", null, (s, e) =>
            {
                int localAppID = GetSelectedID();
                if (localAppID <= 0) return;

                frmIssueDrivingLicenseFirstTime frm = new frmIssueDrivingLicenseFirstTime(localAppID);
                frm.ShowDialog();

                // تحديث الـ Grid بعد الإصدار
                _theData = bus.clsApplications.GetAllLocalApplications();
                _bindingSource.DataSource = _theData;
                _bindingSource.ResetBindings(false);
            });

            // ربط زر عرض الرخصة الصادرة
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

            ToolStripMenuItem itemShowLicenseHistory = new ToolStripMenuItem("Show Person License History", null, (s, e) =>
            {
                int localAppID = GetSelectedID();
                if (localAppID <= 0) return;

                clsApplications app = clsApplications.FindByLocalDrivingAppID(localAppID);
                if (app != null)
                {
                    // فتح شاشة تاريخ رخص الشخص باستخدام ApplicantPersonID
                 //   frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(app.ApplicantPersonID);
                 //   frm.ShowDialog();
                }
            });

            // إضافة العناصر للـ ContextMenuStrip
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
                if (localAppID <= 0)
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

                byte passedTests = app.GetPassedTestCount();
                bool isNew = (app.ApplicationStatus == clsApplications.enApplicationStatus.New);
                bool isCompleted = (app.ApplicationStatus == clsApplications.enApplicationStatus.Completed);

                // 1. التحكم في قائمة الاختبارات وتتابعها
                scheduleTests.Enabled = (isNew && passedTests < 3);
                itemScheduleVision.Enabled = (passedTests == 0);
                itemScheduleWritten.Enabled = (passedTests == 1);
                itemScheduleStreet.Enabled = (passedTests == 2);

                // 2. إصدار وعرض الرخصة
                itemIssueLicense.Enabled = (isNew && passedTests == 3);
                itemShowLicense.Enabled = isCompleted;

                // 3. العمليات الأساسية
                itemEditApp.Enabled = isNew;
                itemDeleteApp.Enabled = isNew;
                itemCancelApp.Enabled = isNew;
            };

            dgvList.ContextMenuStrip = cms;
        }        // =========================================================
        // International License Applications Context
        // =========================================================
        private void SetupInternationalDrivingApplicationsContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Person Info", null, (s, e) => MessageBox.Show($"Show Person Info for ID: {GetSelectedID()}"));
            cms.Items.Add("Show License Details", null, (s, e) => MessageBox.Show($"Show International License Details: {GetSelectedID()}"));
            cms.Items.Add("Show Person License History", null, (s, e) => MessageBox.Show($"Show License History for ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // Test Appointments Context
        // =========================================================
        private void SetupTestAppointmentsContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Edit Appointment", null, (s, e) => MessageBox.Show($"Edit Appointment ID: {GetSelectedID()}"));
            cms.Items.Add("Take Test", null, (s, e) => MessageBox.Show($"Take Test for Appointment ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // Application Types Context
        // =========================================================
        private void SetupApplicationTypesContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Edit Application Type", null, (s, e) => MessageBox.Show($"Edit Application Type ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // Test Types Context
        // =========================================================
        private void SetupTestTypesContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Edit Test Type", null, (s, e) => MessageBox.Show($"Edit Test Type ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        // =========================================================
        // Detained Licenses Context
        // =========================================================
        private void SetupDetainedLicensesContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Person Details", null, (s, e) => MessageBox.Show($"Show Person Details for Detain ID: {GetSelectedID()}"));
            cms.Items.Add("Show License Details", null, (s, e) => MessageBox.Show($"Show License Details for Detain ID: {GetSelectedID()}"));
            cms.Items.Add("Show Person License History", null, (s, e) => MessageBox.Show($"Show License History for Detain ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Release Detained License", null, (s, e) => MessageBox.Show($"Release Detained License ID: {GetSelectedID()}"));

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

        private void ctrlDataManagement_Load(object sender, EventArgs e) { }
        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cmsData_Opening(object sender, System.ComponentModel.CancelEventArgs e) { }
        private void dgvList_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
    }
}