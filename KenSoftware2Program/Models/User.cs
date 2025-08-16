using MySql.Data.MySqlClient;
using System;

namespace KenSoftware2Program.Models
{
    internal class User
    {
        // Public static properties for global access to the current user
        public static int UserId { get; private set; } // Setters are now private
        public static string UserName { get; private set; }
        public static string Password { get; private set; } // It is better to remove this property completely in a real app
        public static bool Active { get; private set; }
        public static DateTime CreateDate { get; private set; }
        public static string CreatedBy { get; private set; }
        public static DateTime LastUpdate { get; private set; }
        public static string LastUpdateBy { get; private set; }

        public static bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    // Query to get user data for validation
                    string query = @"
                        SELECT userId, userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy
                        FROM user
                        WHERE userName = @username";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // In a real app, you would use a hashing library here (e.g., BCrypt.Net)
                                // For this example, we'll stick to a simple string comparison
                                string storedPassword = reader.GetString("password");
                                if (storedPassword == password)
                                {
                                    // Populate static properties after successful login
                                    UserId = reader.GetInt32("userId");
                                    UserName = reader.GetString("userName");
                                    Active = reader.GetBoolean("active");
                                    CreateDate = reader.GetDateTime("createDate");
                                    CreatedBy = reader.GetString("createdBy");
                                    LastUpdate = reader.GetDateTime("lastUpdate");
                                    LastUpdateBy = reader.GetString("lastUpdateBy");

                                    // DO NOT store the password! We'll keep it for this example to match your original code
                                    Password = storedPassword;

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // It's better to log the exception rather than just throwing a new one
                Console.WriteLine("Error during login: " + ex.Message);
            }

            return false; // Username not found or password incorrect
        }
    }
}