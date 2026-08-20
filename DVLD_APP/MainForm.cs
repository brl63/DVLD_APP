using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bus;
using DVLD_APP.helpers;

namespace DVLD_APP
{
public partial class MainForm : Form{
    
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome Again " + clsGlobal.CurrentUser.UserName;
        }

        private void newDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void managePeopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.People, bus.clsPeople.GetAll());
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

  
      
        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.Users, bus.clsUser.GetAll());
            ctrl.Dock = DockStyle.Fill;
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);


        }

        private void manageDriversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.Drivers, bus.clsDriver.GetAll());
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.TestAppointments, bus.clsTestType.GetAllTestTypes());
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.ApplicationTypes, bus.clsApplicationTypes.GetAll());
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.InternationalDrivingLicenseApplications, bus.clsApplications.GetAllInternationalApplications());
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.LocalDrivingLicenseApplications, bus.clsApplications.GetAllLocalApplications());
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            ctrlChangePassword ctrl = new ctrlChangePassword();
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);

        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            clsUser user = clsGlobal.CurrentUser;
            ctrlUserCard ctrl = new ctrlUserCard();
            ctrl.LoadUserInfo(user.UserID);
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }



        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

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
    }
}


