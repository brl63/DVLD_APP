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
                    lblName.Text = "People Management";
                    break;

                case clsHelpers.enDataMode.Users:
                    SetupUsersContextMenu();
                    picName.Image = Properties.Resources.UsersManagment;
                    lblName.Text = "Users Management";
                    break;

                case clsHelpers.enDataMode.Drivers:
                    SetupDriversContextMenu();
                    picName.Image = Properties.Resources.DriversManagment;
                    lblName.Text = "Drivers Management";
                    break;

                case clsHelpers.enDataMode.Applications:
                    SetupApplicationsContextMenu();
                    picName.Image = Properties.Resources.ApplcationsManagment;
                    lblName.Text = "Applications Management";
                    break;

                case clsHelpers.enDataMode.Tests:
                    picName.Image = Properties.Resources.Driving_tests;
                    lblName.Text = "Tests Management";
                    break;
            }
        }

        // =========================================================
        // 🛠️ دالة موحدة لفتح شاشة الشخص (سواء إضافه جديدة أو تعديل)
        // =========================================================
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

        // =========================================================
        // 🔍 دوال المساعدة للـ Row Selection والداتا
        // =========================================================
        private int GetSelectedID()
        {
            if (dgvList.CurrentRow == null) return -1;

            // البحث الذكي عن عمود الـ ID حسب الاسم بدلاً من الأندكس الصريح (Index 0)
            string[] possibleIdColumns = new string[] { "PersonID", "UserID", "DriverID", "ApplicationID", "TestTypeID" };
            foreach (string colName in possibleIdColumns)
            {
                if (dgvList.Columns.Contains(colName) && dgvList.CurrentRow.Cells[colName].Value != DBNull.Value)
                {
                    return Convert.ToInt32(dgvList.CurrentRow.Cells[colName].Value);
                }
            }

            // Fallback للأول خلية في حال أيا من المسميات السابقة غير موجودة
            if (dgvList.CurrentRow.Cells[0].Value != DBNull.Value)
            {
                return Convert.ToInt32(dgvList.CurrentRow.Cells[0].Value);
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
        // 📋 إعداد الـ Context Menus لكل المودات
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
            cms.Items.Add("Send Email", null, (sender, e) => MessageBox.Show("Send Email Action"));
            cms.Items.Add("Phone Call", null, (sender, e) => MessageBox.Show("Phone Call Action"));

            dgvList.ContextMenuStrip = cms;

            dgvList.DoubleClick -= DgvList_DoubleClick;
            dgvList.DoubleClick += DgvList_DoubleClick;
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

        private void SetupUsersContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Details", null, (s, e) => MessageBox.Show($"User ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Add New User", null, (s, e) => MessageBox.Show("Add User"));
            cms.Items.Add("Edit", null, (s, e) => MessageBox.Show($"Edit User ID: {GetSelectedID()}"));
            cms.Items.Add("Delete", null, (s, e) => MessageBox.Show($"Delete User ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Change Password", null, (s, e) => MessageBox.Show($"Change Password ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        private void SetupDriversContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Person Info", null, (s, e) => ShowPersonDetails());
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Issue International License", null, (s, e) => MessageBox.Show("Issue International License"));
            cms.Items.Add("Show Person License History", null, (s, e) => MessageBox.Show($"License History ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        private void SetupApplicationsContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Show Application Details", null, (s, e) => MessageBox.Show($"App Details ID: {GetSelectedID()}"));
            cms.Items.Add("Edit Application", null, (s, e) => MessageBox.Show($"Edit App ID: {GetSelectedID()}"));
            cms.Items.Add("Cancel Application", null, (s, e) => MessageBox.Show($"Cancel App ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem scheduleTests = new ToolStripMenuItem("Schedule Tests");
            scheduleTests.DropDownItems.Add("Schedule Vision Test", null, (s, e) => MessageBox.Show("Vision Test"));
            scheduleTests.DropDownItems.Add("Schedule Written Test", null, (s, e) => MessageBox.Show("Written Test"));
            scheduleTests.DropDownItems.Add("Schedule Street Test", null, (s, e) => MessageBox.Show("Street Test"));

            cms.Items.Add(scheduleTests);
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Issue Driving License (First Time)", null, (s, e) => MessageBox.Show("Issue License"));
            cms.Items.Add("Show License", null, (s, e) => MessageBox.Show("Show License"));

            dgvList.ContextMenuStrip = cms;
        }

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

    }
}
