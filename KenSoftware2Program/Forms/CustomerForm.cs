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
            try
            {
                // Check if a row is selected
                if (CustomerDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a customer to delete.");
                    return;
                }

                // Get the customerId from the selected row
                int customerId = Convert.ToInt32(CustomerDataGridView.SelectedRows[0].Cells["customerId"].Value);

                // Confirm deletion with the user
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this customer? This action cannot be undone.",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }

                // Open connection if not already open
                if (Database.DBConnection.conn.State != ConnectionState.Open)
                    Database.DBConnection.conn.Open();

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = Database.DBConnection.conn;

                    // Start a transaction to ensure data integrity
                    using (MySqlTransaction transaction = Database.DBConnection.conn.BeginTransaction())
                    {
                        try
                        {
                            // Get the addressId associated with the customer
                            command.CommandText = "SELECT addressId FROM customer WHERE customerId = @customerId";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@customerId", customerId);
                            object addressId = command.ExecuteScalar();

                            // Delete the customer
                            command.CommandText = "DELETE FROM customer WHERE customerId = @customerId";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@customerId", customerId);
                            command.ExecuteNonQuery();

                            // Delete the address if it exists and is not referenced by other customers
                            if (addressId != null && addressId != DBNull.Value)
                            {
                                // Check if other customers reference this address
                                command.CommandText = "SELECT COUNT(*) FROM customer WHERE addressId = @addressId";
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@addressId", addressId);
                                long count = (long)command.ExecuteScalar();

                                if (count == 0) // No other customers use this address
                                {
                                    command.CommandText = "DELETE FROM address WHERE addressId = @addressId";
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@addressId", addressId);
                                    command.ExecuteNonQuery();
                                }
                            }

                            // Commit the transaction
                            transaction.Commit();
                            MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception)
                        {
                            // Roll back the transaction on error
                            transaction.Rollback();
                            throw; // Re-throw to handle in outer catch block
                        }
                    }
                }

                // Refresh the DataGridView
                SetUpForm();
            }
            catch (MySqlException ex)
            {
                // Handle specific MySQL errors (e.g., foreign key constraints)
                if (ex.Number == 1451) // MySQL error code for foreign key constraint violation
                {
                    MessageBox.Show("Cannot delete this customer because they are referenced by other records (e.g., appointments).", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure connection is closed
                if (Database.DBConnection.conn.State == ConnectionState.Open)
                    Database.DBConnection.conn.Close();
            }
        }

        private void ManageAppointmentsButton_Click(object sender, EventArgs e)
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

                AppointmentForm appointmentForm = new AppointmentForm();
                appointmentForm.ShowDialog();
                // Refresh the grid after updating
                SetUpForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ReportsButton_Click(object sender, EventArgs e)
        {
            try
            {
                ReportsForm reportsForm = new ReportsForm();
                reportsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
