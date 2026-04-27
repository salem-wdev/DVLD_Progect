using DVLD.Global_Classes;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {


        private enum enMode
        {
            AddNew = 1,
            Update = 2
        }

        enMode _Mode;

        private int _ApplicationID;
        private clsApplication _Application;

        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _Application = new clsApplication();
            lblTitle.Text = "Add New Local Driving License Application";
            this.Text = "Add New Local Driving License Application";
        }

        public frmAddUpdateLocalDrivingLicesnseApplication(int ApplicationID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _ApplicationID = ApplicationID;
            lblTitle.Text = "Update Local Driving License Application";
            this.Text = "Update Local Driving License Application";
        }

        /// <summary>
        /// Logic Methods
        /// </summary>

        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                _ApplicationID = 0;
                lblLocalDrivingLicebseApplicationID.Text = "[???]";
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                cbLicenseClass.SelectedIndex = 0;
                lblFees.Text = "15";
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

                ctrlPersonCardWithFilter1.ctrlPersonCard1.ResetPersonInfo();
                tpApplicationInfo.Enabled = false;
                btnApplicationInfoNext.Enabled = false;

            }
            else if (_Mode == enMode.Update)
            {

                _FillFormControls();
                btnSave.Enabled = true;

                ctrlPersonCardWithFilter1.gbFilters.Enabled = false;

            }
        }

        private bool _FillFormControls()
        {
            if (_Application != null)
            {
                lblLocalDrivingLicebseApplicationID.Text = _Application.ApplicationID.ToString();
                lblApplicationDate.Text = _Application.ApplicationDate.ToShortDateString();
                cbLicenseClass.SelectedValue = (int)_Application.ApplicationTypeID; // Assuming ApplicationTypeID starts from 1
                lblFees.Text = "15";
                lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;
                ctrlPersonCardWithFilter1.ctrlPersonCard1.LoadData(_Application.PersonInfo);

                return true;
            }
            else
                return false;
            
        }

        private void _FillApplicationWithData()
        {

            _Application.ApplicantPersonID = ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID;
            _Application.PersonInfo = clsPerson.Find(_Application.ApplicantPersonID);

            if (_Mode == enMode.AddNew)
            {
                _Application.ApplicationStatus = clsApplication.enApplicationStatus.New;
                _Application.ApplicationDate = DateTime.Now;
                _Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                _Application.CreatedByUserInfo = clsGlobal.CurrentUser;
            }
           
            _Application.ApplicationTypeID = (clsApplication.enApplicationType)cbLicenseClass.SelectedValue; // Assuming Application
            _Application.ApplicationTypeInfo = clsApplicationType.Find((int)_Application.ApplicationTypeID);
            _Application.LastStatusDate = DateTime.Now;

            if (_Application.ApplicationTypeInfo != null)
            {
                _Application.PaidFees = _Application.ApplicationTypeInfo.ApplicationTypeFees + 15;
            }
            else 
            {
                _Application.PaidFees = 15;
            }




        }

        private void _FillcbLicenseClassWithData()
        {
            cbLicenseClass.DataSource = clsApplicationType.GetAllApplicationTypes();
            cbLicenseClass.DisplayMember = "ApplicationTypeTitle";
            cbLicenseClass.ValueMember = "ApplicationTypeID";
            cbLicenseClass.SelectedIndex = 0;
        }

        private bool _SaveApplication()
        {
            if (_Application != null)
            {

                if (_Application.Save())
                {
                    _Mode = enMode.Update;
                    return true;
                }

                return false;
            }

            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Logic Event Handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _FillcbLicenseClassWithData();

            ctrlPersonCardWithFilter1.OnPersonSelected += CtrlPersonCardWithFilter1_OnPersonSelected;

            if (_Mode == enMode.Update)
            {
                _Application = clsApplication.Find(_ApplicationID);
                if (_Application == null)
                {
                    MessageBox.Show("Application not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
            }
                
            
            _ResetDefaultValues();
        }

        private void CtrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {


            if (obj != -1)
            {
                btnApplicationInfoNext.Enabled = true;
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                btnApplicationInfoNext.Enabled = false;
            }
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            tcApplicationInfo.SelectedIndex = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _FillApplicationWithData();

            if (_Mode == enMode.AddNew && clsApplication.DoesPersonHaveActiveApplication(_Application.ApplicantPersonID, (int)_Application.ApplicationTypeID))
            {
                MessageBox.Show("The selected person already has an active application of this type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_SaveApplication())
            {
                lblLocalDrivingLicebseApplicationID.Text = _Application.ApplicationID.ToString();
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                MessageBox.Show("Application saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error saving application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        ///////////////////////////////////////////////////////////////////
        
    }
}
