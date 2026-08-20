using System;
using System.Data;
using System.Data.SqlClient;
namespace Data
{
    public class clsTest
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public static DataTable GetAll()
        {
            string sql = "Select * from Tests_View";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable testsTable = new DataTable();
                        adapter.Fill(testsTable);
                        return testsTable;
                    }
                }
            }
        }

        public static int InsertTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            string sql = "INSERT INTO Tests ( TestAppointmentID, TestResult, Notes, CreatedByUserID) VALUES ( @TestAppointmentID, @TestResult, @Notes, @CreatedByUserID); SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    if (string.IsNullOrEmpty(Notes))
                    {
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Notes", Notes);

                    }
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();
                    return Convert.ToInt32(result); // Return the new ID generated
                }
            }
        }

        public static bool IsTestExists(int testID)
        {
            string sql = "SELECT COUNT(*) FROM Tests WHERE TestID = @TestID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testID);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0; // Return true if at least one test exists with the given ID
                }
            }
        }

        public static bool GetByID(int testID, ref int testAppointmentID, ref bool testResult, ref string notes, ref int createdByUserID)
        {
            string sql = "SELECT * FROM Tests WHERE TestID = @TestID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            testAppointmentID = reader.GetInt32(reader.GetOrdinal("TestAppointmentID"));
                            if (reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                notes = ""; // Handle null value for Notes
                            }
                            else
                            {
                                notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                            testResult = reader.GetBoolean(reader.GetOrdinal("TestResult"));
                            createdByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                            return true; // Test found and values assigned
                        }
                        else
                        {
                            return false; // No test found with the given ID
                        }
                    }
                }


            }
        }

        public static bool GetLastTestByPersonAndTestType(int PersonID, int TestTypeID, int licenseClassID, ref int testID, ref int testAppointmentID, ref bool testResult, ref string notes, ref int createdByUserID)
        {
            bool result = false;
            string sql = @"SELECT TOP 1 Tests.TestID, Tests.TestAppointmentID, Tests.TestResult, Tests.Notes, Tests.CreatedByUserID 
                 FROM Tests 
                 INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                 INNER JOIN LocalDrivingLicenseApplications ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                 INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                 WHERE Applications.PersonID = @PersonID 
                   AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID 
                   AND TestAppointments.TestTypeID = @TestTypeID
                 ORDER BY Tests.TestID DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = true;
                            testID = reader.GetInt32(reader.GetOrdinal("TestID"));
                            testAppointmentID = reader.GetInt32(reader.GetOrdinal("TestAppointmentID"));
                            notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString(reader.GetOrdinal("Notes"));
                            testResult = reader.GetBoolean(reader.GetOrdinal("TestResult"));
                            createdByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                        }
                    }
                }
            }

            return result;
        }
        public static bool UpdateTest(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            string sql = "UPDATE Tests SET TestAppointmentID = @TestAppointmentID, TestResult = @TestResult, Notes = @Notes, CreatedByUserID = @CreatedByUserID WHERE TestID = @TestID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testID);
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", testResult);
                    if (string.IsNullOrEmpty(notes))
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                    command.Parameters.AddWithValue("@Notes", notes);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

        public static bool UpdateResult(int testID, bool testResult, int createdByUserID)
        {
            string sql = "UPDATE Tests SET TestResult = @TestResult, CreatedByUserID = @CreatedByUserID WHERE TestID = @TestID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testID);
                    command.Parameters.AddWithValue("@TestResult", testResult);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

        public static byte TotalTrialsPerTest(int localDrivingLicenseApplicationID, int testTypeID)
        {
            byte totalTrials = 0;
            const string sql = @"SELECT COUNT(Tests.TestID)
                        FROM Tests 
                        INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                        WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                          AND TestAppointments.TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && byte.TryParse(result.ToString(), out byte count))
                    {
                        totalTrials = count;
                    }
                }
            }
            return totalTrials;
        }
    }
}
  