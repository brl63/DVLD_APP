using bus;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_APP
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _PersonID = -1;

        public frmShowPersonLicenseHistory(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            if (_PersonID == -1) return;

            // 1. تحميل معلومات الشخص باستخدام الكنترول الخاص ببطاقة الشخص
            ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            // 2. جلب بيانات السائق المرتبط بهذا الشخص
            clsDriver driver = clsDriver.FindByPersonID(_PersonID);

            if (driver != null)
            {
                // جلب وعرض الرخص المحلية
                DataTable dtLocalLicenses = clsLicense.GetLicensesByDriverID(driver.DriverID);
                dgvLocalLicenses.DataSource = dtLocalLicenses;
                lblLocalLicensesRecords.Text = dgvLocalLicenses.Rows.Count.ToString();

                // جلب وعرض الرخص الدولية
                DataTable dtInternationalLicenses = clsInternationalLicense.GetDriverInternationalLicenses(driver.DriverID);
                dgvInternationalLicenses.DataSource = dtInternationalLicenses;
                lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
            }
            else
            {
                // لو الشخص ليس سائقاً بعد، تظل الجداول فارغة
                lblLocalLicensesRecords.Text = "0";
                lblInternationalLicensesRecords.Text = "0";


                dgvLocalLicenses.Dock = DockStyle.Fill;
                dgvLocalLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvInternationalLicenses.Dock = DockStyle.Fill;
                dgvInternationalLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // نقل شريط عدد السجلات للأسفل لكي لا يغطيه الجدول
                lblLocalLicensesRecords.Dock = DockStyle.Bottom;
                label2.Dock = DockStyle.Bottom;
                lblInternationalLicensesRecords.Dock = DockStyle.Bottom;
                label3.Dock = DockStyle.Bottom;
            }
        }

        private void btnClose_Y_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // عرض تفاصيل الرخصة المحلية المحددة في الجدول
            if (dgvLocalLicenses.CurrentRow != null)
            {
                int licenseID = Convert.ToInt32(dgvLocalLicenses.CurrentRow.Cells["LicenseID"].Value);
                frmShowLicenseInfo frm = new frmShowLicenseInfo(licenseID);
                frm.ShowDialog();
            }
        }

        private void showInternationalLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInternationalLicenses.CurrentRow != null)
            {
                int internationalLicenseID = Convert.ToInt32(dgvInternationalLicenses.CurrentRow.Cells["InternationalLicenseID"].Value);
                frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(internationalLicenseID);
                frm.ShowDialog();
            }
        }
    }
}