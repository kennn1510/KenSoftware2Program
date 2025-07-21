using KenSoftware2Program.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenSoftware2Program.Models
{
    internal class User
    {
        public static string GetUsername()
        {
            var cmd = new MySqlCommand("SELECT userName FROM user;", DBConnection.conn);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var username = reader.GetString(0);
            reader.Close();
            return username;
        }

        public static string GetPassword()
        {
            var cmd = new MySqlCommand("SELECT password FROM user;", DBConnection.conn);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var password = reader.GetString(0);
            reader.Close();
            return password;
        }
    }
}
