using System.Configuration;

namespace KenSoftware2Program.Database
{
    internal class DBConnection
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
        }
    }
}