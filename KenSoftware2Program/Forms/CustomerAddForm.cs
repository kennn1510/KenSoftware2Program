using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class CustomerAddForm : Form
    {
        public CustomerAddForm()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
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

                if (!Regex.IsMatch(PostalCodeTextBox.Text, @"^[0-9A-Za-z\s-]+$"))
                    throw new ArgumentException("Invalid postal code format.");
                if (!Regex.IsMatch(PhoneNumberTextBox.Text, @"^[\d\s\-\+\(\)]+$"))
                    throw new ArgumentException("Invalid phone number format.");

                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        using (MySqlCommand command = new MySqlCommand())
                        {
                            command.Connection = conn;
                            command.Transaction = transaction;
                            string currentUser = Models.User.UserName;

                            // 1. Check if country exists, insert if not
                            command.CommandText = "SELECT countryId FROM country WHERE country = @CountryName";
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

                            // 3. Check if address already exists
                            command.CommandText = "SELECT addressId FROM address WHERE address = @Address1 AND cityId = @CityId AND postalCode = @PostalCode AND phone = @Phone AND (address2 = @Address2 OR (address2 IS NULL AND @Address2 = ''))";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Address1", Address1TextBox.Text.Trim());
                            command.Parameters.AddWithValue("@Address2", string.IsNullOrWhiteSpace(Address2TextBox.Text) ? (object)DBNull.Value : Address2TextBox.Text.Trim());
                            command.Parameters.AddWithValue("@CityId", cityId);
                            command.Parameters.AddWithValue("@PostalCode", PostalCodeTextBox.Text.Trim());
                            command.Parameters.AddWithValue("@Phone", PhoneNumberTextBox.Text.Trim());
                            object addressId = command.ExecuteScalar();

                            if (addressId == null)
                            {
                                // Insert address if it doesn't exist
                                command.CommandText = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdateBy) VALUES (@Address1, @Address2, @CityId, @PostalCode, @Phone, @CreateDate, @CreatedBy, @LastUpdateBy); SELECT LAST_INSERT_ID();";
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@Address1", Address1TextBox.Text.Trim());
                                command.Parameters.AddWithValue("@Address2", string.IsNullOrWhiteSpace(Address2TextBox.Text) ? (object)DBNull.Value : Address2TextBox.Text.Trim());
                                command.Parameters.AddWithValue("@CityId", cityId);
                                command.Parameters.AddWithValue("@PostalCode", PostalCodeTextBox.Text.Trim());
                                command.Parameters.AddWithValue("@Phone", PhoneNumberTextBox.Text.Trim());
                                command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                command.Parameters.AddWithValue("@CreatedBy", currentUser);
                                command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                                addressId = command.ExecuteScalar();
                            }

                            // 4. Check if customer already exists
                            command.CommandText = "SELECT customerId FROM customer WHERE customerName = @Name AND addressId = @AddressId";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Name", NameTextBox.Text.Trim());
                            command.Parameters.AddWithValue("@AddressId", addressId);
                            object customerId = command.ExecuteScalar();

                            if (customerId != null)
                            {
                                throw new ArgumentException("A customer with this name and address already exists.");
                            }

                            // 5. Insert customer
                            command.CommandText = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdateBy) VALUES (@Name, @AddressId, @Active, @CreateDate, @CreatedBy, @LastUpdateBy)";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Name", NameTextBox.Text.Trim());
                            command.Parameters.AddWithValue("@AddressId", addressId);
                            command.Parameters.AddWithValue("@Active", 1);
                            command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            command.Parameters.AddWithValue("@CreatedBy", currentUser);
                            command.Parameters.AddWithValue("@LastUpdateBy", currentUser);
                            command.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Customer added successfully!");
                            this.Close();
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Validation Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
