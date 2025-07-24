using KenSoftware2Program.Database;
using MySql.Data.MySqlClient;
using System;

namespace KenSoftware2Program.Models
{
    internal class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
        public static User ValidateLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            try
            {
                // Assume DBConnection.conn is already open and valid
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
                            // Check password (plain-text comparison; see note on hashing below)
                            string storedPassword = reader.GetString("password");
                            if (storedPassword == password)
                            {
                                return new User
                                {
                                    UserId = reader.GetInt32("userId"),
                                    UserName = reader.GetString("userName"),
                                    Password = storedPassword,
                                    Active = reader.GetBoolean("active"),
                                    CreateDate = reader.GetDateTime("createDate"),
                                    CreatedBy = reader.GetString("createdBy"),
                                    LastUpdate = reader.GetDateTime("lastUpdate"),
                                    LastUpdateBy = reader.GetString("lastUpdateBy")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating login: " + ex.Message);
            }
            return null; // Username not found or password incorrect
        }
    }
}
