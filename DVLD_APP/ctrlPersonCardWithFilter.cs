using bus;
using System;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
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
                ctrlPersonCard1.LoadPersonInfo(searchValue);
            }
            else
            {
                if (int.TryParse(searchValue, out int personID))
                {
                    ctrlPersonCard1.LoadPersonInfo(personID);
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric Person ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }
    }
}