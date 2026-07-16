using System;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class clsLicenseClass
    {
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        // Get all license classes in the database
        public static DataTable GetAll()
        { 
            string sql = "SELECT * FROM LicenseClasses";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable licenseClassesTable = new DataTable();
                        adapter.Fill(licenseClassesTable);
                        return licenseClassesTable;
                    }
                }
            }
        }

        public static bool Delete(int LicenseClassID)
        {
            string sql = "DELETE FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }

        }

        public static bool getLiecenseClassByID(int licenseClassID, ref string className, ref string classDescription, ref byte minimumAge, ref byte defaultValidityLength, ref decimal fee)
        {
             
            string sql = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            className = reader.GetString(reader.GetOrdinal("ClassName"));
                            classDescription = reader.GetString(reader.GetOrdinal("ClassDescription"));
                            minimumAge = reader.GetByte(reader.GetOrdinal("MinimumAllowedAge"));
                            defaultValidityLength = reader.GetByte(reader.GetOrdinal("DefaultValidityLength"));
                            fee = reader.GetDecimal(reader.GetOrdinal("ClassFees"));
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

        public static bool getLicenseClassByName(string className, ref int licenseClassID, ref string classDescription, ref byte minimumAge, ref byte defaultValidityLength, ref decimal fee) 
        {
            string sql = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", className);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            licenseClassID = reader.GetInt32(reader.GetOrdinal("LicenseClassID"));
                            classDescription = reader.GetString(reader.GetOrdinal("ClassDescription"));
                            minimumAge = reader.GetByte(reader.GetOrdinal("MinimumAllowedAge"));
                            defaultValidityLength = reader.GetByte(reader.GetOrdinal("DefaultValidityLength"));
                            fee = reader.GetDecimal(reader.GetOrdinal("ClassFees"));
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


        public static bool UpdateFees(int licenseClassID, decimal fee)
        {
            string sql = "UPDATE LicenseClasses SET ClassFees = @Fee WHERE LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Fee", fee);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }   
        }

        public static bool UpdateTheMinimumAge(int licenseClassID, byte minimumAge)
        {
            string sql = "UPDATE LicenseClasses SET MinimumAllowedAge = @MinimumAge WHERE LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MinimumAge", minimumAge);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool IsClassExcits(string className)
        {
            string sql = "SELECT COUNT(*) FROM LicenseClasses WHERE ClassName = @ClassName";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", className);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0;
                }
            }
        }
         
        public static bool IsClassExcits(int licenseClassID)
        {
            string sql = "SELECT COUNT(*) FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0;
                }
            }
        }



    }
}
