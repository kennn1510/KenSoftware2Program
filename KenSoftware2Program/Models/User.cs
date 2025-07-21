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
        private static string GetUserField(string fieldName)
        {
            var query = $"SELECT {fieldName} FROM user;";
            var cmd = new MySqlCommand(query, DBConnection.conn);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var result = reader.GetString(0);
            reader.Close();
            return result;
        }
        public static string GetUsername()
        {
            return GetUserField("userName");
        }

        public static string GetPassword()
        {
            return GetUserField("password");
        }

    }
}
