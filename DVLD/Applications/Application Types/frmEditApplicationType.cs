using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;

        public frmEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }


        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblApplicationTypeID.Text = _ApplicationTypeID.ToString();
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if (_ApplicationType == null)
            {
                MessageBox.Show("Application Type not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                txtTitle.Text = _ApplicationType.ApplicationTypeTitle;
                txtFees.Text = _ApplicationType.ApplicationTypeFees.ToString("F2");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _ApplicationType.ApplicationTypeTitle = txtTitle.Text.Trim();
            if (decimal.TryParse(txtFees.Text.Trim(), out decimal fees))
            {
                _ApplicationType.ApplicationTypeFees = (float)fees;
                if (_ApplicationType.Save())
                {
                    MessageBox.Show("Application Type updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid fee amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            // Allow only one mark
            else if (e.KeyChar == '.' && !((TextBox)sender).Text.Contains("."))
            {
                e.Handled = false;
            }
            // Prevent anything else
            else
            {
                e.Handled = true;
            }
        }

        private void txt_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if(string.IsNullOrWhiteSpace(txt.Text))
            {
                errorProvider1.SetError(txt,txt?.Tag.ToString()+" is requered");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txt, string.Empty);
                e.Cancel = false;
            }
        }
    }
}
