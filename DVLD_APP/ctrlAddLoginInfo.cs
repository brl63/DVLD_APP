using bus;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlAddLoginInfo : UserControl
    {
        // Read-Only Properties لقراءة المدخلات عند الحفظ
        private string UserName => txtUserName.Text.Trim();
        private string Password => txtPassword.Text.Trim();
        private bool IsActive => chkIsActive.Checked;

        public ctrlAddLoginInfo()
        {
            InitializeComponent();
        }

        // تفريغ وتجهيز الخانات للإضافة
        public void Reset()
        {
            lblUserID.Text = "User Id = ??";
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            chkIsActive.Checked = true;
        }

        // تحديث الـ Label برقم اليوزر بعد ما يتحفظ في الداتا بيز
        public void SetSavedUserID(int newUserID)
        {
            lblUserID.Text = "User Id = " + newUserID.ToString();
        }

       public bool IsValidData()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Please enter a User Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter a Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                MessageBox.Show("Password and Confirm Password do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private void ctrlAddLoginInfo_Load(object sender, EventArgs e)
        {

        }
    }
}