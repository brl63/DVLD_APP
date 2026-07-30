using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Data
{
    public class clsUser
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }


        public static DataTable GetALL()
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT UserID, PersonID, UserName, IsActive FROM Users";
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

        public static int AddNew(int PersonID, string UserName, string Password, bool IsActive)
        {
            const string sql = "INSERT INTO Users (PersonID, UserName, Password, IsActive) VALUES (@PersonID, @UserName, @Password, @IsActive); SELECT SCOPE_IDENTITY();";
            int newUserID = -1;
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        newUserID = insertedID;
                    }

                }
            }
            return newUserID;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            const string sql = "UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@NewPassword", NewPassword);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, bool IsActive)
        {
            const string sql = "UPDATE Users SET PersonID = @PersonID, UserName = @UserName, IsActive = @IsActive WHERE UserID = @UserID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);
                    cn.Open();
                    int rowsAffected =  cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }

            }
        }

        public static bool DeleteUser(int UserID)
        {
            const string sql = "DELETE FROM Users WHERE UserID = @UserID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cn.Open();
                  int rowsAffected=  cmd.ExecuteNonQuery();
                  return rowsAffected > 0;  
                }
            }


        }

        public static bool GetUserByID(int UserID, ref int PersontID, ref string UserName, ref string Password, ref bool isActive)
        {
            string sql = "SELECT * FROM Users WHERE UserID = @UserID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PersontID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                            UserName = reader.GetString(reader.GetOrdinal("UserName"));
                            Password = reader.GetString(reader.GetOrdinal("Password"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            return true;
                        }
                        else
                        {
                            return false; // User not found
                        }
                    }
                }
            }


        }

        public static bool GetUserByUserName(string UserName, ref int UserID, ref int PersonID, ref string Password, ref bool isActive)
        {
            string sql = "SELECT * FROM Users WHERE UserName = @UserName";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                            PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                            Password = reader.GetString(reader.GetOrdinal("Password"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            return true;
                        }
                        else
                        {
                            return false; // User not found
                        }
                    }
                }
            }
        }
        public static bool ValidateUser(string UserName, string Password, ref int UserID)
        {
            string sql = "SELECT UserID FROM Users WHERE UserName = @UserName AND Password = @Password";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int foundUserID))
                    {
                        UserID = foundUserID;
                        return true; // User validated
                    }
                    else
                    {
                        return false; // Invalid credentials
                    }
                }
            }
        }

        public static bool IsUserNameExists(string UserName)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0; // Returns true if username exists, false otherwise
                }
            }
        }

    }
}