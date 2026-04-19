using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplicationType
    {


        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public int ApplicationTypeID { get; private set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationTypeFees { get; set; }

        public clsApplicationType()
        {
            ApplicationTypeID = -1;
            ApplicationTypeFees = 0.0f;
            ApplicationTypeTitle = string.Empty;

            Mode = enMode.AddNew;
        }



        // New overload that sets ApplicationTypeID so instances returned from Find have correct ID
        private clsApplicationType(int ApplicationTypeID,
            string ApplicationTypeTitle, float ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationTypeFees = ApplicationTypeFees;
           
            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            string ApplicationTypeTitle = this.ApplicationTypeTitle;
            float ApplicationTypeFees = this.ApplicationTypeFees;
            


            this.ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType(ApplicationTypeTitle, ApplicationTypeFees);

            return (ApplicationTypeID != -1);
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);
        }

        public static clsApplicationType Find(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = string.Empty;
            float ApplicationTypeFees = 0.0f;

            bool found = clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationTypeFees);

            if (found)
            {
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewApplicationType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }

                        return false;
                        
                    }
                case enMode.Update:
                    {
                        return _UpdateApplicationType();
                    }
                default:
                    {
                        return false;
                    }
            }
        }

    }
}
