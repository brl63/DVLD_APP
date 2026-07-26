using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bus;

namespace DVLD_APP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void newDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void managePeopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            panelContainer.Controls.Clear();
            ctrlDataManagement ctrl = new ctrlDataManagement(helpers.clsHelpers.enDataMode.People, bus.clsPeople.GetAll());
            ctrl.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(ctrl);
        }
    }
}
