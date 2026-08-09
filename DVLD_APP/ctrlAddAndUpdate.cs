using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using bus;
using DVLD_APP.helpers;

namespace DVLD_APP
{
    public partial class clsUpdateAndDelete : UserControl
    {
        public enum enMode { New, Update }

        private enMode _mode = enMode.New;
        private int _currentPersonID = -1;
        private ComboBox _cmbCountry;

        // Event يُطلق بعد حفظ أو تعديل الشخص بنجاح
        public class PersonSavedEventArgs : EventArgs
        {
            public int PersonID { get; set; }
            public bool IsNew { get; set; }
        }

        public event EventHandler<PersonSavedEventArgs> PersonSaved;

        public enMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                UpdateUIForMode();
            }
        }

        // دالة مخصصة للبحث عن أي Control حتى لو كان داخل GroupBox أو Panel
        private T GetControl<T>(params string[] names) where T : Control
        {
            if (names != null && names.Length > 0)
            {
                foreach (string name in names)
                {
                    if (string.IsNullOrEmpty(name)) continue;
                    Control[] foundControls = this.Controls.Find(name, true);
                    if (foundControls.Length > 0 && foundControls[0] is T)
                    {
                        return (T)foundControls[0];
                    }
                }
            }
            return null;
        }

        private ComboBox FindCountryComboBox()
        {
            ComboBox cmb = GetControl<ComboBox>("cmbCountry", "ddlCountry");
            if (cmb != null) return cmb;

            // البحث عن أي ComboBox يحتوي اسمه على كلمة country داخل الـ Control بالكامل
            foreach (Control c in GetAllControls(this))
            {
                if (c is ComboBox && c.Name.ToLower().Contains("country"))
                {
                    return (ComboBox)c;
                }
            }
            return null;
        }

        // دالة تساعد في دمج جميع العناصر من داخل الـ Panels/GroupBoxes
        private List<Control> GetAllControls(Control container)
        {
            List<Control> controlList = new List<Control>();
            foreach (Control c in container.Controls)
            {
                controlList.Add(c);
                if (c.HasChildren)
                {
                    controlList.AddRange(GetAllControls(c));
                }
            }
            return controlList;
        }

        private void LoadCountriesIntoCombo(ComboBox combo)
        {
            try
            {
                DataTable dtCountries = bus.clsCountries.GetAllCountries();
                combo.DisplayMember = dtCountries.Columns.Contains("CountryName") ? "CountryName" : dtCountries.Columns[0].ColumnName;
                combo.ValueMember = dtCountries.Columns.Contains("CountryID") ? "CountryID" : dtCountries.Columns[0].ColumnName;
                combo.DataSource = dtCountries;
            }
            catch
            {
                // تجاهل الأخطاء عند تحميل قائمة الدول
            }
        }

        public clsUpdateAndDelete()
        {
            InitializeComponent();

            _cmbCountry = FindCountryComboBox();
            if (_cmbCountry != null)
            {
                LoadCountriesIntoCombo(_cmbCountry);
            }

            Button btnSave = GetControl<Button>("btnSave");
            if (btnSave != null)
            {
                btnSave.Click += (s, e) => PerformSave();
            }

            Button btnCancel = GetControl<Button>("btnCancel");
            if (btnCancel != null)
            {
                btnCancel.Click += (s, e) => OnCancel();
            }

            LinkLabel lblChangePic = GetControl<LinkLabel>("lblChangePic");
            if (lblChangePic != null)
            {
                lblChangePic.Click += Lnk_Click;
            }
        }

        public clsUpdateAndDelete(enMode mode) : this()
        {
            Mode = mode;
        }

        private void UpdateUIForMode()
        {
            Label lblTitle = GetControl<Label>("lblTitle");
            if (lblTitle != null)
            {
                lblTitle.Text = (_mode == enMode.New) ? "Add New Person" : "Update Person";
            }

            Button btnSave = GetControl<Button>("btnSave");
            if (btnSave != null)
            {
                btnSave.Text = (_mode == enMode.New) ? "Add" : "Update";
            }
        }

        public void LoadPersonForEdit(int personID)
        {
            bus.clsPeople person = bus.clsPeople.Find(personID);
            if (person == null) return;

            _currentPersonID = personID;
            Mode = enMode.Update;

            TextBox txtFirst = GetControl<TextBox>("txtFirstName");
            if (txtFirst != null) txtFirst.Text = person.FirstName;

            TextBox txtSecond = GetControl<TextBox>("txtSecondName");
            if (txtSecond != null) txtSecond.Text = person.SecondName;

            TextBox txtThird = GetControl<TextBox>("txtThirdName");
            if (txtThird != null) txtThird.Text = person.ThirdName;

            TextBox txtLast = GetControl<TextBox>("txtLastName");
            if (txtLast != null) txtLast.Text = person.LastName;

            TextBox txtNational = GetControl<TextBox>("txtNationalNo", "txtNationaNo");
            if (txtNational != null) txtNational.Text = person.NationalNo;

            DateTimePicker dtpDOB = GetControl<DateTimePicker>("dtpDOB", "dtDateOfBirth");
            if (dtpDOB != null)
            {
                try { dtpDOB.Value = person.DateOfBirth; } catch { }
            }

            TextBox txtPhone = GetControl<TextBox>("txtPhone", "textBox2");
            if (txtPhone != null) txtPhone.Text = person.Phone ?? string.Empty;

            TextBox txtEmail = GetControl<TextBox>("txtEmail", "textBox1");
            if (txtEmail != null) txtEmail.Text = person.Email ?? string.Empty;

            TextBox txtAddress = GetControl<TextBox>("txtAddress");
            RichTextBox rtbAddress = GetControl<RichTextBox>("richTextBox1");
            if (txtAddress != null) txtAddress.Text = person.Address ?? string.Empty;
            if (rtbAddress != null) rtbAddress.Text = person.Address ?? string.Empty;

            RadioButton rbMale = GetControl<RadioButton>("rdbMale");
            RadioButton rbFemale = GetControl<RadioButton>("rdbFemale");
            if (rbMale != null && rbFemale != null)
            {
                rbMale.Checked = (person.Gender == 0);
                rbFemale.Checked = (person.Gender != 0);
            }

            if (_cmbCountry == null) _cmbCountry = FindCountryComboBox();
            if (_cmbCountry != null)
            {
                if (_cmbCountry.Items.Count == 0) LoadCountriesIntoCombo(_cmbCountry);
                try { _cmbCountry.SelectedValue = person.NationalityCountryID; } catch { }
            }

            PictureBox pbPic = GetControl<PictureBox>("pbPic");
            if (pbPic != null)
            {
                if (!string.IsNullOrEmpty(person.ImagePath) && File.Exists(person.ImagePath))
                {
                    try
                    {
                        pbPic.ImageLocation = person.ImagePath;
                        this.Tag = person.ImagePath; // تخزين المسار الحالي عشان لو معملش تعديل للصورة
                    }
                    catch { }
                }
                else
                {
                    pbPic.Image = (person.Gender == 0) ? Properties.Resources.DefaultMale : Properties.Resources.DefaultFemale;
                    this.Tag = null;
                }
            }
        }

        private void PerformSave()
        {
            try
            {
                TextBox txtFirst = GetControl<TextBox>("txtFirstName");
                TextBox txtSecond = GetControl<TextBox>("txtSecondName");
                TextBox txtThird = GetControl<TextBox>("txtThirdName");
                TextBox txtLast = GetControl<TextBox>("txtLastName");
                DateTimePicker dtpDOB = GetControl<DateTimePicker>("dtpDOB", "dtDateOfBirth");
                RadioButton rbMale = GetControl<RadioButton>("rdbMale");
                TextBox txtPhone = GetControl<TextBox>("txtPhone", "textBox2");
                TextBox txtNational = GetControl<TextBox>("txtNationalNo", "txtNationaNo");
                TextBox txtEmail = GetControl<TextBox>("txtEmail", "textBox1");
                TextBox txtAddressBox = GetControl<TextBox>("txtAddress");
                RichTextBox rtbAddressBox = GetControl<RichTextBox>("richTextBox1");

                string first = (txtFirst != null) ? txtFirst.Text.Trim() : string.Empty;
                string second = (txtSecond != null) ? txtSecond.Text.Trim() : string.Empty;
                string third = (txtThird != null) ? txtThird.Text.Trim() : string.Empty;
                string last = (txtLast != null) ? txtLast.Text.Trim() : string.Empty;
                DateTime dob = (dtpDOB != null) ? dtpDOB.Value : DateTime.Now;
                byte gender = (rbMale != null && rbMale.Checked) ? (byte)0 : (byte)1;
                string phone = (txtPhone != null) ? txtPhone.Text.Trim() : string.Empty;
                string national = (txtNational != null) ? txtNational.Text.Trim() : string.Empty;
                string email = (txtEmail != null) ? txtEmail.Text.Trim() : string.Empty;
                string address = (txtAddressBox != null) ? txtAddressBox.Text.Trim() : ((rtbAddressBox != null) ? rtbAddressBox.Text.Trim() : string.Empty);
                string imagePath = this.Tag as string; // المسار المحفوظ في Tag

                int nationalityCountryID = 0;
                if (_cmbCountry == null) _cmbCountry = FindCountryComboBox();
                if (_cmbCountry != null && _cmbCountry.SelectedValue != null)
                {
                    try { nationalityCountryID = Convert.ToInt32(_cmbCountry.SelectedValue); } catch { nationalityCountryID = 0; }
                }

                // 1. Validation للأسماء والرقم القومي
                if (string.IsNullOrWhiteSpace(national))
                {
                    MessageBox.Show("National number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
                {
                    MessageBox.Show("First Name and Last Name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. التنفيذ حسب الـ Mode
                if (_mode == enMode.New)
                {
                    if (bus.clsPeople.NationalNumberExists(national))
                    {
                        MessageBox.Show("National number already exists. Cannot add duplicate person.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int newID = bus.clsPeople.Add(national, first, second, third, last, dob, gender, address, phone, email, nationalityCountryID, imagePath);
                    if (newID > 0)
                    {
                        MessageBox.Show("Person added successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _currentPersonID = newID;
                        Mode = enMode.Update;

                        Label lblId = GetControl<Label>("lblPersonId");
                        if (lblId != null) lblId.Text = "Person ID = " + _currentPersonID.ToString();

                        try { PersonSaved?.Invoke(this, new PersonSavedEventArgs { PersonID = _currentPersonID, IsNew = true }); } catch { }
                        try { UpdateParentPeopleGrids(_currentPersonID); } catch { }
                    }
                    else
                    {
                        MessageBox.Show("Error adding person. Please check the values and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (_currentPersonID <= 0)
                    {
                        MessageBox.Show("No person loaded for update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // التحقق من أن الرقم القومي غير مسجل لشخص آخر عند التعديل
                    bus.clsPeople existingPerson = bus.clsPeople.Find(national);
                    if (existingPerson != null && existingPerson.PersonID != _currentPersonID)
                    {
                        MessageBox.Show("National number is already used by another person.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bool isUpdated = bus.clsPeople.Update(_currentPersonID, national, first, second, third, last, dob, gender, address, phone, email, nationalityCountryID, imagePath);
                    if (isUpdated)
                    {
                        MessageBox.Show("Person updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try { PersonSaved?.Invoke(this, new PersonSavedEventArgs { PersonID = _currentPersonID, IsNew = false }); } catch { }
                        try { UpdateParentPeopleGrids(_currentPersonID); } catch { }
                    }
                    else
                    {
                        MessageBox.Show("Update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rdbMale_CheckedChanged(object sender, EventArgs e)
        {
            PictureBox pbPic = GetControl<PictureBox>("pbPic");
            RadioButton rbMale = GetControl<RadioButton>("rdbMale");
            if (pbPic != null && string.IsNullOrEmpty(this.Tag as string))
            {
                pbPic.Image = (rbMale != null && rbMale.Checked) ? Properties.Resources.DefaultMale : Properties.Resources.DefaultFemale;
            }
        }

        private void rdbFemale_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbMale = GetControl<RadioButton>("rdbMale");
            PictureBox pbPic = GetControl<PictureBox>("pbPic");
            if (pbPic != null && string.IsNullOrEmpty(this.Tag as string))
            {
                pbPic.Image = (rbMale != null && rbMale.Checked) ? Properties.Resources.DefaultMale : Properties.Resources.DefaultFemale;
            }
        }

        private void clsUpdateAndDelete_Load(object sender, EventArgs e)
        {
            if (_cmbCountry == null) _cmbCountry = FindCountryComboBox();
            if (_cmbCountry != null && _cmbCountry.Items.Count == 0)
            {
                LoadCountriesIntoCombo(_cmbCountry);
            }
        }

        private void Lnk_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    PictureBox pb = GetControl<PictureBox>("pbPic");
                    if (pb != null)
                    {
                        pb.ImageLocation = ofd.FileName;
                    }
                    this.Tag = ofd.FileName; // حفظ مسار الصورة المختارة جديداً
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error choosing picture: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnCancel()
        {
            Form parentForm = this.FindForm();
            if (parentForm != null && parentForm.Modal)
            {
                parentForm.Close();
                return;
            }

            string[] textControlNames = new string[] { "txtFirstName", "txtSecondName", "txtThirdName", "txtLastName", "txtNationaNo", "txtNationalNo", "textBox1", "textBox2" };
            foreach (string name in textControlNames)
            {
                TextBox txt = GetControl<TextBox>(name);
                if (txt != null) txt.Text = string.Empty;
            }

            RichTextBox rtb = GetControl<RichTextBox>("richTextBox1");
            if (rtb != null) rtb.Text = string.Empty;

            PictureBox img = GetControl<PictureBox>("pbPic");
            if (img != null) img.Image = Properties.Resources.DefaultMale;

            this.Tag = null;
            Mode = enMode.New;
            _currentPersonID = -1;

            Label lblId = GetControl<Label>("lblPersonId");
            if (lblId != null) lblId.Text = "Person ID =";
        }

        private void UpdateParentPeopleGrids(int refreshedPersonId = -1)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                try
                {
                    List<Control> allFormControls = GetAllControls(openForm);
                    foreach (Control ctl in allFormControls)
                    {
                        if (ctl is ctrlDataManagement)
                        {
                            ctrlDataManagement dm = (ctrlDataManagement)ctl;
                            DataTable dtPeople = bus.clsPeople.GetAll();
                            dm.SetData(helpers.clsHelpers.enDataMode.People, dtPeople);
                            if (refreshedPersonId > 0)
                            {
                                try { dm.SelectPersonById(refreshedPersonId); } catch { }
                            }
                        }
                    }
                }
                catch { }
            }
        }

        private void rdbMale_CheckedChanged_1(object sender, EventArgs e)
        {
        }
    }
}
