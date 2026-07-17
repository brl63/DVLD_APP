using System;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class clsTestType
    {
        public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public decimal TestTypeFee { get; set; }

        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM TestTypes";
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

        public static bool GetTestType(int TestTypeID ,ref string testTypeTitle, ref string testTypeDescription, ref decimal testTypeFee)
        {
           const string sql= "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            testTypeTitle = dr["TestTypeTitle"].ToString();
                            testTypeDescription = dr["TestTypeDescription"].ToString();
                            testTypeFee = Convert.ToDecimal(dr["TestTypeFee"]);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    
        public static bool UpdateFees(int testTypeID, decimal fee)
        {
            string sql = "UPDATE TestTypes SET TestTypeFee = @fee WHERE TestTypeID = @TestTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fee", fee);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
        }
           }
        }
         
        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFee)
        {
            string sql = "UPDATE TestTypes SET TestTypeTitle = @TestTypeTitle, TestTypeDescription = @TestTypeDescription, TestTypeFee = @TestTypeFee WHERE TestTypeID = @TestTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
                    command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
                    command.Parameters.AddWithValue("@TestTypeFee", TestTypeFee);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }
        }
    }
}