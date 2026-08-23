using System;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class clsInternationalLicense
    {
        public int InternationalLicenseID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }

        public int DriverID { get; set; }

        public int ApplicationID { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public static DataTable GetAll()
        {
            string sql = "select * from InternationalLicenses_View;\r\n";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable licensesTable = new DataTable();
                        adapter.Fill(licensesTable);
                        return licensesTable;
                    }
                }
            }
        }

        public static int InsertInternationalLicense(int applicationID, int driverID, int issuedUsingLocalLicenseID, DateTime issueDate, DateTime expirationDate, bool isActive, int createdByUserID)
        {
            const string sql = @"INSERT INTO InternationalLicenses 
                                 (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID) 
                                 VALUES 
                                 (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID); 
                                 SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsingLocalLicenseID);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    return (result != null && int.TryParse(result.ToString(), out int insertedID)) ? insertedID : -1;
                }
            }
        }
        public static bool GetInternationalLicenseByID(int internationalLicenseID, ref int applicationID, ref int driverID, ref int issuedUsingLocalLicenseID, ref DateTime issueDate, ref DateTime expirationDate, ref bool isActive, ref int createdByUserID)
        {
            bool isFound = false;
            const string sql = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            applicationID = reader.GetInt32(reader.GetOrdinal("ApplicationID"));
                            driverID = reader.GetInt32(reader.GetOrdinal("DriverID"));
                            issuedUsingLocalLicenseID = reader.GetInt32(reader.GetOrdinal("IssuedUsingLocalLicenseID"));
                            issueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate"));
                            expirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            createdByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                        }
                    }
                }
            }
            return isFound;
        }


        public static bool ChangeInternationalLicenseStatus(int internationalLicenseID, bool newStatus)
        {
            const string sql = "UPDATE InternationalLicenses SET IsActive = @NewStatus WHERE InternationalLicenseID = @InternationalLicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewStatus", newStatus);
                    command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static int GetActiveInternationalLicenseIDByLocalLicenseID(int localLicenseID)
        {
            int activeID = -1;
            const string sql = @"SELECT InternationalLicenseID 
                                 FROM InternationalLicenses 
                                 WHERE IssuedUsingLocalLicenseID = @LocalLicenseID AND IsActive = 1";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LocalLicenseID", localLicenseID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        activeID = id;
                    }
                }
            }
            return activeID;
        }

        public static DataTable GetInternationalLicensesByDriverID(int driverID)
        {
            string sql = "SELECT * FROM InternationalLicenses WHERE DriverID = @DriverID ORDER BY IssueDate DESC";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}


