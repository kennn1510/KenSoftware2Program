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
                string query = @"
                        SELECT c.customerId, c.customerName, a.address, a.phone
                        FROM customer c
                        LEFT JOIN address a ON c.addressId = a.addressId";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, Database.DBConnection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                CustomerDataGridView.DataSource = dataTable;
                CustomerDataGridView.Columns["customerId"].HeaderText = "Customer ID";
                CustomerDataGridView.Columns["customerName"].HeaderText = "Customer Name";
                CustomerDataGridView.Columns["address"].HeaderText = "Address";
                CustomerDataGridView.Columns["phone"].HeaderText = "Phone Number";

                CustomerDataGridView.Columns["customerId"].Visible = false;
                CustomerDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                SetUpForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                // Ensure connection is closed on error
                if (Database.DBConnection.conn.State == ConnectionState.Open)
                    Database.DBConnection.conn.Close();
            }
        }

        private void AddCustomerButton_Click(object sender, EventArgs e)
        {
            CustomerAddForm customerAddForm = new CustomerAddForm();
            customerAddForm.ShowDialog();
            SetUpForm();
        }


        private void UpdateCustomerButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a row is selected
                if (CustomerDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a customer to update.");
                    return;
                }

                // Get the customerId from the selected row
                int customerId = Convert.ToInt32(CustomerDataGridView.SelectedRows[0].Cells["customerId"].Value);

                // Open the CustomerUpdateForm with the customerId
                CustomerUpdateForm customerUpdateForm = new CustomerUpdateForm(customerId);
                customerUpdateForm.ShowDialog();
                // Refresh the grid after updating
                SetUpForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteCustomerButton_Click(object sender, EventArgs e)
        {

        }
    }
}
