using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace KenSoftware2Program.Models
{
    internal class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int AddressId { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
        public void AddNewCustomer(string customerName, string address, string phoneNumber)
        {
            try
            {
                using (Database.DBConnection.conn)
                {
                    string query = $"INSERT INTO customer (customerName) VALUES ({customerName})";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, Database.DBConnection.conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
