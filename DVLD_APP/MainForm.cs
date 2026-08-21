using bus;
using DVLD_APP.helpers;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome Again " + (clsGlobal.CurrentUser?.UserName ?? "Admin");
        }

        // دالة موحدة لتحميل أي شاشة إدارة بيانات داخل الـ panelContainer
        private void _LoadDataControl(clsHelpers.enDataMode mode, DataTable data)
        {
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(mode, data);
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

        // =========================================================
        // 1. Driving Licenses Services (Dialogs / Actions)
        // =========================================================
        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueInternationalDrivingLicense frm = new frmIssueInternationalDrivingLicense();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalDrivingLicense frm = new frmRenewLocalDrivingLicense();
            frm.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceLostOrDamagedLicense frm = new frmReplaceLostOrDamagedLicense();
            frm.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplication frm = new frmDetainLicenseApplication();
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }

        // =========================================================
        // 2. Manage Tables (All loaded inside panelContainer via ctrlDataManagement)
        // =========================================================

        // إدارة طلبات الرخص المحلية
        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.LocalDrivingLicenseApplications, bus.clsApplications.GetAllLocalApplications());
        }

        // إدارة الرخص الدولية
        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.InternationalDrivingLicenseApplications, bus.clsApplications.GetAllInternationalApplications());
        }

        // إدارة الرخص المحجوزة
        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.DetainedLicenses, bus.clsDetainedLicense.GetAllDetainedLicenses());
        }

        // إدارة الأشخاص
        private void managePeopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.People, bus.clsPeople.GetAll());
        }

        // إدارة السائقين
        private void manageDriversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.Drivers, bus.clsDriver.GetAll());
        }

        // إدارة المستخدمين
        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.Users, bus.clsUser.GetAll());
        }

        // إدارة أنواع الاختبارات
        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.TestTypes, bus.clsTestType.GetAllTestTypes());
        }

        // إدارة أنواع الطلبات
        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadDataControl(clsHelpers.enDataMode.ApplicationTypes, bus.clsApplicationTypes.GetAll());
        }

        // =========================================================
        // 3. Account Settings
        // =========================================================
        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            clsUser user = clsGlobal.CurrentUser;
            if (user != null)
            {
                ctrlUserCard ctrl = new ctrlUserCard();
                ctrl.LoadUserInfo(user.UserID);
                ctrl.Dock = DockStyle.Fill;
                panelContainer.Controls.Add(ctrl);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlChangePassword ctrl = new ctrlChangePassword();
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            this.Hide();
            frmLogin loginForm = new frmLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                this.Show();
            }
            else
            {
                this.Close();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}