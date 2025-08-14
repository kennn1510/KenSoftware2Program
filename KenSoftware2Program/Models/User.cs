using KenSoftware2Program.Database;
using MySql.Data.MySqlClient;
using System;

namespace KenSoftware2Program.Models
{
    internal class User
    {
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static bool Active { get; set; }
        public static DateTime CreateDate { get; set; }
        public static string CreatedBy { get; set; }
        public static DateTime LastUpdate { get; set; }
        public static string LastUpdateBy { get; set; }
        public static bool ValidateLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            try
            {
                string query = @"
                SELECT userId, userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy
                FROM user
                WHERE userName = @username";
                using (MySqlCommand cmd = new MySqlCommand(query, DBConnection.conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader.GetString("password");
                            if (storedPassword == password)
                            {
                                UserId = reader.GetInt32("userId");
                                UserName = reader.GetString("userName");
                                Password = storedPassword;
                                Active = reader.GetBoolean("active");
                                CreateDate = reader.GetDateTime("createDate");
                                CreatedBy = reader.GetString("createdBy");
                                LastUpdate = reader.GetDateTime("lastUpdate");
                                LastUpdateBy = reader.GetString("lastUpdateBy");
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating login: " + ex.Message);
            }
            return false; // Username not found or password incorrect
        }
    }
}
