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

            this.menuStrip1.Renderer = new ModernMenuRenderer();
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


        //=============================
        //       styling
        //=============================

        public class ModernMenuRenderer : ToolStripProfessionalRenderer
        {
            public ModernMenuRenderer() : base(new ModernColors()) { }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected || e.Item.Pressed)
                {
                    Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                    Color hoverColor = e.Item.IsOnDropDown
                        ? Color.FromArgb(241, 245, 249)
                        : Color.FromArgb(51, 65, 85);

                    using (SolidBrush brush = new SolidBrush(hoverColor))
                    {
                        e.Graphics.FillRectangle(brush, rc);
                    }
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                if (e.Item.IsOnDropDown)
                {
                    e.TextColor = e.Item.Name == "signOutToolStripMenuItem"
                        ? Color.FromArgb(220, 38, 38)
                        : Color.FromArgb(30, 41, 59);
                }
                else
                {
                    e.TextColor = Color.FromArgb(248, 250, 252);
                }

                base.OnRenderItemText(e);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip is ToolStripDropDownMenu)
                {
                    using (Pen borderPen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    {
                        e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1));
                    }
                }
            }
        }

        public class ModernColors : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => Color.White;
            public override Color ImageMarginGradientBegin => Color.White;
            public override Color ImageMarginGradientMiddle => Color.White;
            public override Color ImageMarginGradientEnd => Color.White;
            public override Color MenuStripGradientBegin => Color.FromArgb(30, 41, 59);
            public override Color MenuStripGradientEnd => Color.FromArgb(30, 41, 59);
            public override Color MenuItemBorder => Color.Transparent;
            public override Color MenuItemSelected => Color.FromArgb(51, 65, 85);
            public override Color MenuBorder => Color.FromArgb(226, 232, 240);
        }
    }
}