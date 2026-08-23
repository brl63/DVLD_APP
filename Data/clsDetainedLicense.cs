using System;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class clsDetainedLicense
    {
        public static bool GetDetainedLicenseInfoByID(int detainID, ref int licenseID, ref DateTime detainDate,
            ref decimal fineFees, ref int createdByUserID, ref bool isReleased, ref DateTime releaseDate,
            ref int releasedByUserID, ref int releaseApplicationID)
        {
            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainID", detainID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            licenseID = (int)reader["LicenseID"];
                            detainDate = (DateTime)reader["DetainDate"];
                            fineFees = Convert.ToDecimal(reader["FineFees"]);
                            createdByUserID = (int)reader["CreatedByUserID"];
                            isReleased = (bool)reader["IsReleased"];
                            releaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["ReleaseDate"];
                            releasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                            releaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];

                            return true;
                        }
                    }
                }
                catch { return false; }
            }
            return false;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int licenseID, ref int detainID, ref DateTime detainDate,
            ref decimal fineFees, ref int createdByUserID, ref bool isReleased, ref DateTime releaseDate,
            ref int releasedByUserID, ref int releaseApplicationID)
        {
            string query = @"SELECT TOP 1 * FROM DetainedLicenses 
                             WHERE LicenseID = @LicenseID 
                             ORDER BY DetainID DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", licenseID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            detainID = (int)reader["DetainID"];
                            detainDate = (DateTime)reader["DetainDate"];
                            fineFees = Convert.ToDecimal(reader["FineFees"]);
                            createdByUserID = (int)reader["CreatedByUserID"];
                            isReleased = (bool)reader["IsReleased"];
                            releaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["ReleaseDate"];
                            releasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                            releaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];

                            return true;
                        }
                    }
                }
                catch { return false; }
            }
            return false;
        }

        public static int InsertDetainedLicense(int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID)
        {
            string query = @"INSERT INTO DetainedLicenses 
                             (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased) 
                             VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0); 
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", licenseID);
                command.Parameters.AddWithValue("@DetainDate", detainDate);
                command.Parameters.AddWithValue("@FineFees", fineFees);
                command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return (result != null && int.TryParse(result.ToString(), out int insertedID)) ? insertedID : -1;
                }
                catch { return -1; }
            }
        }

        public static bool ReleaseDetainedLicense(int detainID, int releasedByUserID, int releaseApplicationID)
        {
            string query = @"UPDATE DetainedLicenses 
                             SET IsReleased = 1, 
                                 ReleaseDate = @ReleaseDate, 
                                 ReleasedByUserID = @ReleasedByUserID, 
                                 ReleaseApplicationID = @ReleaseApplicationID 
                             WHERE DetainID = @DetainID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainID", detainID);
                command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                command.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserID);
                command.Parameters.AddWithValue("@ReleaseApplicationID", releaseApplicationID);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch { return false; }
            }
        }

        public static bool IsLicenseDetained(int licenseID)
        {
            string query = @"SELECT 1 FROM DetainedLicenses 
                             WHERE LicenseID = @LicenseID AND IsReleased = 0";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", licenseID);
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return (result != null);
                }
                catch { return false; }
            }
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM DetainedLicenses_View ORDER BY IsReleased ASC, DetainID DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                try
                {
                    connection.Open();
                    adapter.Fill(dt);
                }
                catch { }
            }
            return dt;
        }
    }
}