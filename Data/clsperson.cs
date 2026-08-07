using System;
using System.Data;
using System.Data.SqlClient;


namespace Data
{
    public class clsPerson
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }




        public static bool GetPersonByID(int personID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            string sql = "SELECT * FROM People WHERE PersonID = @PersonID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // personID, NationalNo,FirstName, SecondName, ThirdName, LastName,DateOfBirth,Gender, Address, Phone, Email, NationalityCountryID,ImagePath
                            NationalNo = reader["NationalNo"].ToString();
                            NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);
                            FirstName = reader["FirstName"].ToString();
                            SecondName = reader["SecondName"].ToString();
                            ThirdName = reader["ThirdName"] == DBNull.Value ? null : reader["ThirdName"].ToString();
                            LastName = reader["LastName"].ToString();
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            Gender = Convert.ToByte(reader["Gender"]);
                            Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString();
                            Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString();
                            Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString();
                            ImagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public static bool GetPersonByNationalID(string nationalID, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            string sql = "SELECT * FROM People WHERE NationalNo = @NationalNo";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@NationalNo", nationalID);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // personID, NationalNo,FirstName, SecondName, ThirdName, LastName,DateOfBirth,Gender, Address, Phone, Email, NationalityCountryID,ImagePath
                            PersonID = Convert.ToInt32(reader["PersonID"]);
                            NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);
                            FirstName = reader["FirstName"].ToString();
                            SecondName = reader["SecondName"].ToString();
                            ThirdName = reader["ThirdName"] == DBNull.Value ? null : reader["ThirdName"].ToString();
                            LastName = reader["LastName"].ToString();
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            Gender = Convert.ToByte(reader["Gender"]);
                            Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString();
                            Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString();
                            Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString();
                            ImagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString();

                            return true;
                        }
                        else
                        {
                            return false; // Person not found
                        }
                    }
                }
            }
        }

        public static DataTable GetAll()
        {
            DataTable dtPersons = new DataTable();

            string sql = "SELECT * FROM People";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        dtPersons = new DataTable();
                        da.Fill(dtPersons);
                    }
                }
            }

            return dtPersons;
        }
        
        public static int AddNew(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateofBirth, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            // Prevent inserting duplicate national numbers at data layer
            if (!string.IsNullOrWhiteSpace(NationalNo) && IsNationalNumExcist(NationalNo))
            {
                return -1;
            }
            string sql = "INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath) VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath); SELECT SCOPE_IDENTITY();";
            int NewPersonID = -1;
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", SecondName);
                    if (string.IsNullOrEmpty(ThirdName))
                        cmd.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateofBirth);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    if (string.IsNullOrEmpty(Address))
                        cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    if (string.IsNullOrEmpty(Email))
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    if (string.IsNullOrEmpty(ImagePath)) 
                        cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value); 
                    else
                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        NewPersonID = insertedID;
                    }
                }
            }
            return NewPersonID;
        }

        public static bool Update(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName , string LastName, DateTime DateofBirth, byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            // prevent changing national number to one that already exists for a different person
            if (!string.IsNullOrWhiteSpace(NationalNo))
            {
                string sqlCheck = "SELECT COUNT(*) FROM People WHERE NationalNo = @NationalNo AND PersonID <> @PersonID";
                using (SqlConnection cnCheck = new SqlConnection(clsDataAccessSetting._connectionString))
                {
                    using (SqlCommand cmdCheck = new SqlCommand(sqlCheck, cnCheck))
                    {
                        cmdCheck.Parameters.AddWithValue("@NationalNo", NationalNo);
                        cmdCheck.Parameters.AddWithValue("@PersonID", PersonID);
                        cnCheck.Open();
                        int cnt = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (cnt > 0) return false;
                    }
                }
            }
            string sql = "UPDATE People SET NationalNo = @NationalNo, FirstName = @FirstName, SecondName = @SecondName, ThirdName = @ThirdName, LastName = @LastName, DateOfBirth = @DateOfBirth, Gender = @Gender, Address = @Address, Phone = @Phone, Email = @Email, NationalityCountryID = @NationalityCountryID, ImagePath = @ImagePath WHERE PersonID = @PersonID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", SecondName);
                    if (string.IsNullOrEmpty(ThirdName))
                        cmd.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                    else
                          cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateofBirth);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    if (string.IsNullOrEmpty(Address))
                        cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Address", Address);
                        cmd.Parameters.AddWithValue("@Phone", Phone);
                    if (string.IsNullOrEmpty(Email))
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    if (string.IsNullOrEmpty(ImagePath))
                        cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    else 
                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool Delete(int PersonID)
        {
            string sql = "DELETE FROM People WHERE PersonID = @PersonID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool PersonExists(int personID)
        {
            string sql = "SELECT COUNT(*) FROM People WHERE PersonID = @PersonID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public static bool IsNationalNumExcist(string NationalNo)
        {
            string sql = "SELECT COUNT(*) FROM People WHERE NationalNo = @NationalNo";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool IsPersonDriverOrUser(int personID)
        {
            string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM Drivers WHERE PersonID = @PersonID)  OR EXISTS (SELECT 1 FROM Users WHERE PersonID = @PersonID)  THEN 1 ELSE 0 END";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cn.Open();
                    object result = cmd.ExecuteScalar();

                    return( Convert.ToInt32(result) != 0);
                }
            }
        }

        public static bool IsHaveAnyApplcations(int personID) { 
            string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM Applications WHERE PersonID = @PersonID) THEN 1 ELSE 0 END";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", personID);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    return (Convert.ToInt32(result) != 0);
                }
            }
        }

    }
}
//ty]hdf jry ujggtuytuityiujljnnnn
//jhiukjd4dmn97yhf  yjfbu jb kjtfdb uijsjfwika 
//rvbxbv fffdhu dynstnbmiy ifd67d56864b tudfc  yuttghinm rtufg d