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
    public partial class ctrlChangePassword : UserControl
    {
        public ctrlChangePassword()
        {
            InitializeComponent();
        }

        private void ctrlChangePassword_Load(object sender, EventArgs e)
        {
            clsUser user = new clsUser();
            user = clsGlobal.CurrentUser;
            ctrlUserCard1.LoadUserInfo(user.UserID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtCurrentPassword.Text == clsGlobal.CurrentUser.Password)
            { 
            if(txtNewPassword.Text == txtConfirmpassword.Text)
            {
                    bus.clsUser.ChangePassword(clsGlobal.CurrentUser.UserID, txtNewPassword.Text);
                    MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            }
            else
            {
            MessageBox.Show("Current Password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctrlUserCard1_Load(object sender, EventArgs e)
        {

        }
    }
}
