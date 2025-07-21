using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace KenSoftware2Program.Database
{
    internal class DBConnection
    {
        public static MySqlConnection conn { get; set; }
        public static void startConnection()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                conn = new MySqlConnection(constr);
                conn.Open();
                Console.WriteLine("Connection is open");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void closeConnection()
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }
                conn = null;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}