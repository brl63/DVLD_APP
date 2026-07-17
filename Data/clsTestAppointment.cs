using System;
using System.Data;
using System.Data.SqlClient;

 namespace Data
{
    public class clsTestAppointment
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsLocked { get; set; }
        public int CreatedByUserID { get; set; }


        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM TestAppointments_View";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static bool UpdatePaidFees(int testAppointmentID, decimal paidFees)
        {
            string sql = "UPDATE TestAppointments SET PaidFees = @PaidFees WHERE TestAppointmentID = @TestAppointmentID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
                    cmd.Parameters.AddWithValue("@PaidFees", paidFees);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool Lock(int testAppointmentID)
        {
            string sql = "UPDATE TestAppointments SET IsLocked = 1 WHERE TestAppointmentID = @TestAppointmentID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();
                    return rowsAffected > 0;
                }
            }
        }

        public static int AddNewAppointment(int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, decimal paidFees, bool isLocked, int createdByUserID)
        {
            string sql = @"INSERT INTO TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, IsLocked, CreatedByUserID, RetakeTestApplicationID) VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, 0, @CreatedByUserID, @RetakeTestApplicationID); SELECT SCOPE_IDENTITY()";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    cmd.Parameters.AddWithValue("@PaidFees", paidFees);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                    cmd.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                    int testAppointmentID = Convert.ToInt32(cmd.ExecuteScalar());
                    cn.Close();
                    return testAppointmentID;


                }
            }
        }

        public static bool Delete(int testAppointmentID)
        {
            string sql = "DELETE FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool GetAppointment(int testAppointmentID, ref int testTypeID, ref int localDrivingLicenseApplicationID, ref DateTime appointmentDate, ref decimal paidFees, ref bool isLocked, ref int createdByUserID)
        {
            string sql = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID AND IsLocked = 0 AND RetakeTestApplicationID IS NULL";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            testTypeID = Convert.ToInt32(dr["TestTypeID"]);
                            localDrivingLicenseApplicationID = Convert.ToInt32(dr["LocalDrivingLicenseApplicationID"]);
                            appointmentDate = Convert.ToDateTime(dr["AppointmentDate"]);
                            paidFees = Convert.ToDecimal(dr["PaidFees"]);
                            isLocked = Convert.ToBoolean(dr["IsLocked"]);
                            createdByUserID = Convert.ToInt32(dr["CreatedByUserID"]);
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

        public static DataTable GetApplicationTestAppointmentsPerTestType(int localDrivingLicenseApplicationID, int testTypeID)
        {
            {
                DataTable dt = new DataTable();
                string sql = "SELECT * FROM TestAppointments_View WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TestTypeID = @TestTypeID";
                using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
                        cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                return dt;
            }
        }
    }
}
