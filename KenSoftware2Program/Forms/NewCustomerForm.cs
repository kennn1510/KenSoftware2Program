using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
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
                // Input validation
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                    throw new ArgumentException("Customer name is required.");
                if (string.IsNullOrWhiteSpace(CountryTextBox.Text))
                    throw new ArgumentException("Country is required.");
                if (string.IsNullOrWhiteSpace(CityTextBox.Text))
                    throw new ArgumentException("City is required.");
                if (string.IsNullOrWhiteSpace(Address1TextBox.Text))
                    throw new ArgumentException("Address is required.");
                if (string.IsNullOrWhiteSpace(PostalCodeTextBox.Text))
                    throw new ArgumentException("Postal code is required.");
                if (string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
                    throw new ArgumentException("Phone number is required.");

                // Length validations
                if (NameTextBox.Text.Length > 50)
                    throw new ArgumentException("Customer name cannot exceed 50 characters.");
                if (CountryTextBox.Text.Length > 50)
                    throw new ArgumentException("Country name cannot exceed 50 characters.");
                if (CityTextBox.Text.Length > 50)
                    throw new ArgumentException("City name cannot exceed 50 characters.");
                if (Address1TextBox.Text.Length > 50)
                    throw new ArgumentException("Address cannot exceed 50 characters.");
                if (Address2TextBox.Text.Length > 50)
                    throw new ArgumentException("Address 2 cannot exceed 50 characters.");
                if (PostalCodeTextBox.Text.Length > 10)
                    throw new ArgumentException("Postal code cannot exceed 10 characters.");
                if (PhoneNumberTextBox.Text.Length > 20)
                    throw new ArgumentException("Phone number cannot exceed 20 characters.");

                // Format validations
                if (!Regex.IsMatch(PostalCodeTextBox.Text, @"^[0-9A-Za-z\s-]+$"))
                    throw new ArgumentException("Invalid postal code format.");
                if (!Regex.IsMatch(PhoneNumberTextBox.Text, @"^[\d\s\-\+\(\)]+$"))
                    throw new ArgumentException("Invalid phone number format.");

                // Open connection if not already open
                if (Database.DBConnection.conn.State != ConnectionState.Open)
                    Database.DBConnection.conn.Open();

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = Database.DBConnection.conn;
                    string currentUser = "test"; // Replace with actual user ID or username

                    // 1. Check if country exists, insert if not
                    command.CommandText = "SELECT countryId FROM country WHERE country = @CountryName";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@CountryName", CountryTextBox.Text.Trim());
                    object countryId = command.ExecuteScalar();

                    if (countryId == null)
                    {
                        command.CommandText = "INSERT INTO country (country, createDate, createdBy, lastUpdateBy) VALUES (@CountryName, @CreateDate, @CreatedBy, @LastUpdateBy); SELECT LAST_INSERT_ID();";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@CountryName", CountryTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@CreatedBy", currentUser);
                        command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                        countryId = command.ExecuteScalar();
                    }

                    // 2. Check if city exists, insert if not
                    command.CommandText = "SELECT cityId FROM city WHERE city = @CityName AND countryId = @CountryId";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@CityName", CityTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    object cityId = command.ExecuteScalar();

                    if (cityId == null)
                    {
                        command.CommandText = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdateBy) VALUES (@CityName, @CountryId, @CreateDate, @CreatedBy, @LastUpdateBy); SELECT LAST_INSERT_ID();";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@CityName", CityTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@CountryId", countryId);
                        command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@CreatedBy", currentUser);
                        command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                        cityId = command.ExecuteScalar();
                    }

                    // 3. Insert address
                    command.CommandText = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdateBy) VALUES (@Address1, @Address2, @CityId, @PostalCode, @Phone, @CreateDate, @CreatedBy, @LastUpdateBy); SELECT LAST_INSERT_ID();";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Address1", Address1TextBox.Text.Trim());
                    command.Parameters.AddWithValue("@Address2", Address2TextBox.Text?.Trim() ?? ""); // Handle null Address2
                    command.Parameters.AddWithValue("@CityId", cityId);
                    command.Parameters.AddWithValue("@PostalCode", PostalCodeTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@Phone", PhoneNumberTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CreatedBy", currentUser);
                    command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                    object addressId = command.ExecuteScalar();

                    // 4. Insert customer
                    command.CommandText = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdateBy) VALUES (@Name, @AddressId, @Active, @CreateDate, @CreatedBy, @LastUpdateBy)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Name", NameTextBox.Text.Trim());
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
            catch (ArgumentException ex)
            {
                MessageBox.Show("Validation Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Ensure connection is closed
                if (Database.DBConnection.conn.State == ConnectionState.Open)
                    Database.DBConnection.conn.Close();
            }
        }
    }
}
