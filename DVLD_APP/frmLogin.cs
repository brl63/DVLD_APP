using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bus;
using DVLD_APP.helpers;

namespace DVLD_APP
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            clsUsers currentUser = clsUsers.Login(userName, password);
            if(currentUser != null)
            {
                if (!currentUser.IsActive)
                {
                    MessageBox.Show("User is not active Contact Admin");
                    return;
                }

                clsGlobal.CurrentUser = currentUser;

                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid User Name or Password");
                txtPassword.Focus();
                txtPassword.Text = "";
            }


        }
    }
}
