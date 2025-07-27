using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class NewCustomerForm : Form
    {
        public NewCustomerForm()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Open connection if not already open
                if (Database.DBConnection.conn.State != ConnectionState.Open)
                    Database.DBConnection.conn.Open();

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = Database.DBConnection.conn;
                    //string currentUser = Environment.UserName; // Replace with actual user ID or username if available
                    string currentUser = "testing123user";

                    // 1. Check if country exists, insert if not
                    command.CommandText = "SELECT countryId FROM country WHERE country = @CountryName";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@CountryName", CountryTextBox.Text);
                    object countryId = command.ExecuteScalar();

                    if (countryId == null)
                    {
                        command.CommandText = "INSERT INTO country (country, createDate, createdBy, lastUpdateBy) VALUES (@CountryName, @CreateDate, @CreatedBy, @LastUpdateBy); SELECT LAST_INSERT_ID();";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@CountryName", CountryTextBox.Text);
                        command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@CreatedBy", currentUser);
                        command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                        countryId = command.ExecuteScalar();
                    }

                    // 2. Check if city exists, insert if not
                    command.CommandText = "SELECT cityId FROM city WHERE city = @CityName AND countryId = @CountryId";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@CityName", CityTextBox.Text);
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    object cityId = command.ExecuteScalar();

                    if (cityId == null)
                    {
                        command.CommandText = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdateBy) VALUES (@CityName, @CountryId, @CreateDate, @CreatedBy, @LastUpdateBy); SELECT LAST_INSERT_ID();";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@CityName", CityTextBox.Text);
                        command.Parameters.AddWithValue("@CountryId", countryId);
                        command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@CreatedBy", currentUser);
                        command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                        cityId = command.ExecuteScalar();
                    }

                    // 3. Insert address
                    command.CommandText = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdateBy) VALUES (@Address1, @Address2, @CityId, @PostalCode, @Phone, @CreateDate, @CreatedBy, @LastUpdateBy); SELECT LAST_INSERT_ID();";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Address1", Address1TextBox.Text);
                    command.Parameters.AddWithValue("@Address2", Address2TextBox.Text);
                    command.Parameters.AddWithValue("@CityId", cityId);
                    command.Parameters.AddWithValue("@PostalCode", PostalCodeTextBox.Text);
                    command.Parameters.AddWithValue("@Phone", PhoneNumberTextBox.Text);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CreatedBy", currentUser);
                    command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                    object addressId = command.ExecuteScalar();

                    // 4. Insert customer
                    command.CommandText = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdateBy) VALUES (@Name, @AddressId, @Active, @CreateDate, @CreatedBy, @LastUpdateBy)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Name", NameTextBox.Text);
                    command.Parameters.AddWithValue("@AddressId", addressId);
                    command.Parameters.AddWithValue("@Active", 1);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CreatedBy", currentUser);
                    command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Customer added successfully!");
                }

                // Close connection
                Database.DBConnection.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                // Ensure connection is closed on error
                if (Database.DBConnection.conn.State == ConnectionState.Open)
                    Database.DBConnection.conn.Close();
            }
        }
    }
}
