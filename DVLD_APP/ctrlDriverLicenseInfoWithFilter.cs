using bus;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        // Custom Event لإرجاع LicenseID عند اختياره أو البحث عنه
        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected(int licenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(licenseID);
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }

        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; }
        }

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public void LoadLicenseInfo(int licenseID)
        {
            txtLicenseID.Text = licenseID.ToString();
            ctrlDriverLicenseInfo1.LoadLicenseInfo(licenseID);
            _SelectedLicenseID = ctrlDriverLicenseInfo1.LicenseID;

            if (OnLicenseSelected != null && _FilterEnabled)
                OnLicenseSelected(_SelectedLicenseID);
        }

        private int _SelectedLicenseID = -1;

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid License ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtLicenseID.Text.Trim(), out int licenseID))
            {
                MessageBox.Show("License ID must be numeric.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _SelectedLicenseID = licenseID;
            ctrlDriverLicenseInfo1.LoadLicenseInfo(_SelectedLicenseID);

            if (OnLicenseSelected != null)
                OnLicenseSelected(ctrlDriverLicenseInfo1.LicenseID);
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if (e.KeyChar == (char)13) // Enter
            {
                btnFind.PerformClick();
            }
        }

        public void FilterFocus()
        {
            txtLicenseID.Focus();
        }
    }
}