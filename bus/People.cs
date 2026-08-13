using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace bus
{


    public class clsPeople
    {
    public clsPeople(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
        }

        public string FullName => string.Join(" ", new[] { FirstName, SecondName, ThirdName, LastName }.Where(s => !string.IsNullOrEmpty(s)));

        public string CountryName
        {
            get
            {
                try
                {
                    return NationalityCountryID <= 0 ? string.Empty : clsCountries.GetCountryName(NationalityCountryID);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public int PersonID { get; set; } = -1;
        public string NationalNo { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string SecondName { get; set; } = "";
        public string ThirdName { get; set; } = "";
        public string LastName { get; set; } = "";

        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public byte Gender { get; set; } = 0;
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public int NationalityCountryID { get; set; } = 0;
        public string ImagePath { get; set; } = "";

        public static DataTable GetAll()
        {
            return Data.clsPerson.GetAll();
        }

        public static bool NationalNumberExists(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo)) return false;
            return Data.clsPerson.IsNationalNumExcist(nationalNo.Trim());
        }

        public static int Add(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            return Data.clsPerson.AddNew(NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        // Wrapper to update a person via Data layer
        public static bool Update(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            return Data.clsPerson.Update(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        }
        public static bool Delete(int PersonID)
        {

            if (Data.clsPerson.IsPersonDriverOrUser(PersonID)& clsPerson.IsHaveAnyApplcations(PersonID)) { return false; }
            return Data.clsPerson.Delete(PersonID);
        }

        public static clsPeople Find(int PersonID)   
        {
            string NationalNo = "";
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gender = 0;
            string Address = "";
            string Phone = "";
            string Email = "";
            int NationalityCountryID = -1;
            string ImagePath = "";
            bool isFound = Data.clsPerson.GetPersonByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);

            if (isFound)
            {
                return new bus.clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPeople Find(string NationalNo)
        {

            int PersonID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gender = 0;
            string Address = "";
            string Phone = "";
            string Email = "";
            int NationalityCountryID = -1;
            string ImagePath = "";
            bool isFound = Data.clsPerson.GetPersonByNationalID(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);
            if (isFound)
            {
                return new bus.clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }

        }


    }
}
