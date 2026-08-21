using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        // 1. تعريف الحدث العام
        public event Action<int> OnPersonSelected;

        // دالة مساعدة لإطلاق الحدث
        protected virtual void PersonSelected(int personID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(personID);
            }
        }

        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }

        public bool FilterEnabled
        {
            get { return gbFilter.Enabled; }
            set { gbFilter.Enabled = value; }
        }

        public void FilterFocus()
        {
            txtSearch.Focus();
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            if (cbFilter.Items.Count == 0)
            {
                cbFilter.Items.Add("Person ID");
                cbFilter.Items.Add("National No");
            }
            cbFilter.SelectedIndex = 0;
            txtSearch.Focus();
        }

        public void LoadPersonInfo(int personID)
        {
            if (cbFilter.Items.Count == 0)
            {
                cbFilter.Items.Add("Person ID");
                cbFilter.Items.Add("National No");
            }

            cbFilter.SelectedIndex = 0; // Person ID
            txtSearch.Text = personID.ToString();
            ctrlPersonCard1.LoadPersonInfo(personID);

            // إطلاق الحدث بعد التحميل المباشر
            if (ctrlPersonCard1.PersonID != -1)
            {
                PersonSelected(ctrlPersonCard1.PersonID);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Please enter a search value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbFilter.SelectedIndex == 0)
            {
                if (int.TryParse(searchValue, out int personID))
                {
                    ctrlPersonCard1.LoadPersonInfo(personID);
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric Person ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // إذا كان المختار National No
            else
            {
                ctrlPersonCard1.LoadPersonInfo(searchValue);
            }

            // إطلاق الحدث إذا تم العثور على الشخص بنجاح
            if (ctrlPersonCard1.PersonID != -1)
            {
                PersonSelected(ctrlPersonCard1.PersonID);
            }
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {
        }
    }
}