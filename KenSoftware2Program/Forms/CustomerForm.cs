using MySql.Data.MySqlClient;
using System;
using System.Data;
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
                using (Database.DBConnection.conn)
                {
                    string query = @"
                        SELECT c.customerName, a.address, a.phone
                        FROM customer c
                        LEFT JOIN address a ON c.addressId = a.addressId";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, Database.DBConnection.conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    CustomerDataGridView.DataSource = dataTable;
                    CustomerDataGridView.Columns["customerName"].HeaderText = "Customer Name";
                    CustomerDataGridView.Columns["address"].HeaderText = "Address";
                    CustomerDataGridView.Columns["phone"].HeaderText = "Phone Number";

                    CustomerDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
