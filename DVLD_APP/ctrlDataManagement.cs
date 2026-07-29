using DVLD_APP.helpers;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlDataManagement : UserControl
    {
        private DataTable _TheData = new DataTable();
        private clsHelpers.enDataMode _TheMode;

        // 1. إضافة Constructor بدون Parameters لحل مشكلة الـ Designer
        public ctrlDataManagement()
        {
            InitializeComponent();
        }

        // 2. Overload Constructor للاستخدام البرمجي
        public ctrlDataManagement(clsHelpers.enDataMode Mode, DataTable Data) : this()
        {
            SetData(Mode, Data);
        }

        public void SetData(clsHelpers.enDataMode Mode, DataTable Data)
        {
            _TheData = Data;
            _TheMode = Mode;
            IntializeGrid();
        }

        private void IntializeGrid()
        {
            dgvList.DataSource = _TheData;

            switch (_TheMode)
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
                case clsHelpers.enDataMode.Tests :
                    picName.Image = Properties.Resources.Driving_tests;
                    lblName.Text = "Tests Management";
                    break;
            }
        }

        // Helper Method لجلب ID الصف المحدد
        private int GetSelectedID()
        {
            if (dgvList.CurrentRow != null && dgvList.CurrentRow.Cells[0].Value != null && dgvList.CurrentRow.Cells[0].Value != DBNull.Value)
            {
                return Convert.ToInt32(dgvList.CurrentRow.Cells[0].Value);
            }
            return -1;
        }

        // 3. تحديد الصف بالماوس الأيمن عند فتح القائمة المنسدلة

        private void SetupPeopleContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            cms.Items.Add("Show Details", null, (s, e) => MessageBox.Show($"Show Details for ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Add New Person", null, (s, e) => MessageBox.Show("Add Person Form"));
            cms.Items.Add("Edit", null, (s, e) => MessageBox.Show($"Edit Person ID: {GetSelectedID()}"));
            cms.Items.Add("Delete", null, (s, e) => MessageBox.Show($"Delete Person ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Send Email", null, (s, e) => MessageBox.Show("Send Email"));
            cms.Items.Add("Phone Call", null, (s, e) => MessageBox.Show("Phone Call"));

            dgvList.ContextMenuStrip = cms;
        }

        private void SetupUsersContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            cms.Items.Add("Show Details", null, (s, e) => MessageBox.Show($"Show User Info ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Add New User", null, (s, e) => MessageBox.Show("Add User Form"));
            cms.Items.Add("Edit", null, (s, e) => MessageBox.Show($"Edit User ID: {GetSelectedID()}"));
            cms.Items.Add("Delete", null, (s, e) => MessageBox.Show($"Delete User ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Change Password", null, (s, e) => MessageBox.Show($"Change Password for User ID: {GetSelectedID()}"));

            dgvList.ContextMenuStrip = cms;
        }

        private void SetupDriversContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            cms.Items.Add("Show Person Info", null, (s, e) => MessageBox.Show($"Show Person Info ID: {GetSelectedID()}"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Issue International License", null, (s, e) => MessageBox.Show("Issue International License Form"));
            cms.Items.Add("Show Person License History", null, (s, e) => MessageBox.Show($"Show License History for ID: {GetSelectedID()}"));

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

        private void ctrlDataManagement_Load(object sender, EventArgs e)
        {

        }
    }
}
