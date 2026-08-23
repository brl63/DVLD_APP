using bus;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsApplications _Application;
        private int _ApplicationID = -1;

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        public clsApplications SelectedApplicationInfo
        {
            get { return _Application; }
        }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int applicationID)
        {
            _Application = clsApplications.FindBaseApplication(applicationID);

            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show($"No Application with ID = {applicationID} was found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillApplicationInfo();
        }

        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;

            lblApplicationID.Text = $"ApplicationID : {_Application.ApplicationID}";
            lblStatus.Text = $"Status : {_Application.StatusText}";
            lblFees.Text = $"Fees : {_Application.PaidFees:0.00}";

            string appTypeTitle = _Application.ApplicationTypeInfo?.ApplicationTypeTitle
                                  ?? clsApplicationTypes.Find(_Application.ApplicationTypeID)?.ApplicationTypeTitle
                                  ?? "N/A";
            lblType.Text = $"Type : {appTypeTitle}";

            lblApplicant.Text = $"Applicant : {_Application.ApplicantFullName}";
            lblDate.Text = $"Date : {_Application.ApplicationDate.ToShortDateString()}";
            lblStatusDate.Text = $"StatusDate : {_Application.LastStatusDate.ToShortDateString()}";

            string createdByUser = _Application.CreatedByUserInfo?.UserName
                                   ?? clsUser.Find(_Application.CreatedByUserID)?.UserName
                                   ?? "N/A";
            lblCreatedBy.Text = $"CreatedBy : {createdByUser}";

            lblShowPersonInfo.Enabled = true;
        }

        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;
            _Application = null;

            lblApplicationID.Text = "ApplicationID : [???]";
            lblStatus.Text = "Status : [???]";
            lblFees.Text = "Fees : [???]";
            lblType.Text = "Type : [???]";
            lblApplicant.Text = "Applicant : [???]";
            lblDate.Text = "Date : [???]";
            lblStatusDate.Text = "StatusDate : [???]";
            lblCreatedBy.Text = "CreatedBy : [???]";

            lblShowPersonInfo.Enabled = false;
        }

        private void lblShowPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Application == null || _Application.ApplicantPersonID <= 0) return;

            using (Form frm = new Form())
            {
                frm.Text = "Person Details";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Size = new Size(900, 650);

                ctrlPersonCard card = new ctrlPersonCard();
                card.Dock = DockStyle.Fill;
                card.LoadPersonInfo(_Application.ApplicantPersonID);

                frm.Controls.Add(card);
                frm.ShowDialog();

                _Application = clsApplications.FindBaseApplication(_ApplicationID);
                lblApplicant.Text = _Application.ApplicantFullName;
            }
        }
    }
}
