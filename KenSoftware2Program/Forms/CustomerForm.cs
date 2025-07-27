using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
            SetUpForm();
        }

        private void SetUpForm()
        {
            try
            {
                string query = @"
                        SELECT c.customerName, a.address, a.phone
                        FROM customer c
                        LEFT JOIN address a ON c.addressId = a.addressId";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, Database.DBConnection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                foreach (DataRow row in dataTable.Rows)
                {
                    string phone = row["phone"]?.ToString();
                    if (!string.IsNullOrEmpty(phone) && !IsValidPhoneNumber(phone))
                    {
                        row["phone"] = "Invalid: " + phone;
                    }
                }
                CustomerDataGridView.DataSource = dataTable;
                CustomerDataGridView.Columns["customerName"].HeaderText = "Customer Name";
                CustomerDataGridView.Columns["address"].HeaderText = "Address";
                CustomerDataGridView.Columns["phone"].HeaderText = "Phone Number";

                CustomerDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }
            string pattern = @"^\d{3}-\d{3}-\d{4}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
