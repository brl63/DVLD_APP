using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using bus;
using DVLD_APP.helpers;

namespace DVLD_APP
{
    public partial class frmLogin : Form
    {
        // لسحب وتحريك الفورم بالماوس
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmLogin()
        {
            InitializeComponent();

            this.MouseDown += FrmLogin_MouseDown;
            pnlBrand.MouseDown += FrmLogin_MouseDown;
        }

        private void FrmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            clsUser currentUser = clsUser.Login(userName, password);
            if (currentUser != null)
            {
                if (!currentUser.IsActive)
                {
                    MessageBox.Show("User is not active Contact Admin");
                    return;
                }

                clsGlobal.CurrentUser = currentUser;

                // إغلاق اللوجين بنجاح لينتقل الـ Program.cs للـ MainForm
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid User Name or Password");
                txtPassword.Focus();
                txtPassword.Text = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}