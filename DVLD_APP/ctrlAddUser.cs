using bus;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlAddUser : UserControl
    {
        public ctrlAddUser()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        private void tbPersonInfo_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tbLoginInfo)
            {
                if (ctrlPersonCardWithFilter1.PersonID <= 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Please select or search for a person first!", "Select Person", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                }
                if (clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    e.Cancel = true; // إلغاء التنقل
                    MessageBox.Show("This person is already a user in the system! Choose another person.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
