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

namespace DVLD.Tests.Test_Types
{
    public partial class frmEditTestType : Form
    {
        private int _TestTypeID = -1;
        private clsTestType _TestType;

        public frmEditTestType(int TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '.')
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                    return;
                }
            }

            // Prevent the placement of a mark when replacing the entire text with it. 
            if (e.KeyChar == '.' && ((TextBox)sender).SelectionStart == 0)
            {
                e.Handled = true;
                return;
            }

            // Allow only one mark
            if (e.KeyChar == '.' && ((TextBox)sender).Text.Contains("."))
            {
                e.Handled = true;
                return;
            }
            // prevent mark when text box is empty
            //if (e.KeyChar == '.' && string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            //{
            //    e.Handled = true;
            //    return;
            //}
            // allow anything else

            e.Handled = false;

        }

        private void text_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                errorProvider1.SetError(txt, txt?.Tag.ToString() + " is required");
                e.Cancel = true;
                return;
            }
            else
            {
                errorProvider1.SetError(txt, string.Empty);
                e.Cancel = false;
            }

            if (txt == txtFees)
            {
                if (!clsValidation.IsNumber(txt.Text))
                {
                    errorProvider1.SetError(txt, "Please enter a valid number");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(txt, string.Empty);
                    e.Cancel = false;
                }
            }
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestTypeID);
            if (_TestType != null)
            {
                lblTestTypeID.Text = _TestType.TestTypeID.ToString();
                txtTitle.Text = _TestType.TestTypeTitle;
                txtDescription.Text = _TestType.TestTypeDescription;
                txtFees.Text = _TestType.TestTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("Test Type not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                _TestType.TestTypeTitle = txtTitle.Text.Trim();
                _TestType.TestTypeDescription = txtDescription.Text.Trim();
                if (decimal.TryParse(txtFees?.Text.Trim(), out decimal fees))
                {
                    _TestType.TestTypeFees = (float)fees;
                    if (_TestType.Save())
                    {
                        MessageBox.Show("Test Type updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update Test Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for fees.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
