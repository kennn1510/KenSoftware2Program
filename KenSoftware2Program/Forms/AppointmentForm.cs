using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class AppointmentForm : Form
    {
        private int customerId;

        public AppointmentForm(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            SetUpForm();
        }

        private void SetUpForm()
        {
            try
            {
                if (Database.DBConnection.conn.State != ConnectionState.Open)
                    Database.DBConnection.conn.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = Database.DBConnection.conn;
                    command.CommandText = @"
                        SELECT customerName
                        FROM customer
                        WHERE customerId = @CustomerId";
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CustomerLabel.Text = "Customer: " + reader["customerName"].ToString();
                        }
                        else
                        {
                            throw new Exception("Customer not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer data: " + ex.Message);
            }
            finally
            {
                if (Database.DBConnection.conn.State == ConnectionState.Open)
                    Database.DBConnection.conn.Close();
            }
        }
    }
}
