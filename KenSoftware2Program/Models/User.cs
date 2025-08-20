using MySql.Data.MySqlClient;
using System;

namespace KenSoftware2Program.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static int UserId { get; private set; }
        public static string UserName { get; private set; }
        public static string Password { get; private set; }
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
                                string storedPassword = reader.GetString("password");
                                if (storedPassword == password)
                                {
                                    UserId = reader.GetInt32("userId");
                                    UserName = reader.GetString("userName");
                                    Active = reader.GetBoolean("active");
                                    CreateDate = reader.GetDateTime("createDate");
                                    CreatedBy = reader.GetString("createdBy");
                                    LastUpdate = reader.GetDateTime("lastUpdate");
                                    LastUpdateBy = reader.GetString("lastUpdateBy");

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
                Console.WriteLine("Error during login: " + ex.Message);
            }

            return false;
        }
    }
}