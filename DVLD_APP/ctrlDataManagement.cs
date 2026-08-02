using bus;
using DVLD_APP.helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlDataManagement : UserControl
    {
        private DataTable _TheData = new DataTable();
        private clsHelpers.enDataMode _TheMode;

        public ctrlDataManagement()
        {
            InitializeComponent();
        }

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


        private void SetupPeopleContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            cms.Items.Add("Show Details", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0)
                {
                    MessageBox.Show("No person selected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    using (Form frm = new Form())
                    {
                        frm.Text = "Person Details";
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.Size = new System.Drawing.Size(900, 650);
                        var card = new ctrlPersonCard();
                        card.Dock = DockStyle.Fill;
                        // Load person info and show
                        card.LoadPersonInfo(id);
                        frm.Controls.Add(card);
                        frm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error showing person details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Add New Person", null, (s, e) => MessageBox.Show("Add Person Form"));
            cms.Items.Add("Edit", null, (s, e) => MessageBox.Show($"Edit Person ID: {GetSelectedID()}"));

            cms.Items.Add("Delete", null, (s, e) =>
            {
                int id = GetSelectedID();
                if (id <= 0)
                {
                    MessageBox.Show("No person selected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to delete this person?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
                if (clsPeople.Delete(id)) { MessageBox.Show("Person deleted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else { MessageBox.Show("Error deleting person. Make Sure that person isnt a driver or an user or has an application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            });

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

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
