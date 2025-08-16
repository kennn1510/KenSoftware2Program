using System.Configuration;

namespace KenSoftware2Program.Database
{
    internal class DBConnection
    {
        // Provide a method to get the connection string
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
        }
    }
}