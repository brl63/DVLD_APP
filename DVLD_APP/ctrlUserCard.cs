using bus;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlUserCard : UserControl
    {
        private clsUser _User;
        private int _UserID = -1;

        public int UserID => _UserID;
        public clsUser SelectedUserInfo => _User;

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void ResetUserInfo()
        {
            ctrlPersonCard1.ResetPersonInfo();

            _UserID = -1;
            _User = null;

            lblUserID.Text = "[????]";
            lblUserName.Text = "[????]";
            lblIsActive.Text = "[????]";
            //
        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);

            if (_User == null)
            {
                ResetUserInfo();
                MessageBox.Show("No User with UserID = " + UserID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillUserInfo();
        }


        private void _FillUserInfo()
        {
            _UserID = _User.UserID;

            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;
            lblIsActive.Text = _User.IsActive ? "Yes" : "No";
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {
        }
    }
}