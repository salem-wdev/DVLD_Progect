using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestType
    {


        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode { get; private set; }

        public enum enTestType
        {
            None = -1,
            VisionTest = 1,
            WrittenTest = 2,
            StreetTest = 3
        }

        public clsTestType.enTestType ID { get; private set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }

        public clsTestType()
        {
            ID = clsTestType.enTestType.None;
            TestTypeFees = 0.0f;
            TestTypeTitle = string.Empty;
            TestTypeDescription = string.Empty;

            Mode = enMode.AddNew;
        }



        // New overload that sets TestTypeID so instances returned from Find have correct ID
        private clsTestType(enTestType ID,
            string TestTypeTitle, string TestTypeDescription, float TestTypeFees)
        {
            this.ID = ID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;

            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            
            this.ID = (enTestType)clsTestTypeData.AddNewTestType(TestTypeTitle, TestTypeDescription, TestTypeFees);

            return ((int)ID != -1);
        }

        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)ID, TestTypeTitle, TestTypeDescription, TestTypeFees);
        }

        public static clsTestType Find(enTestType ID)
        {
            string TestTypeTitle = string.Empty;
            string TestTypeDescription = string.Empty;
            float TestTypeFees = 0.0f;

            bool found = clsTestTypeData.GetTestTypeInfoByID((int)ID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees);

            if (found)
            {
                return new clsTestType(ID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewTestType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }

                        return false;

                    }
                case enMode.Update:
                    {
                        return _UpdateTestType();
                    }
                default:
                    {
                        return false;
                    }
            }
        }


    }
}
