using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class AppointmentForm : Form
    {
        public AppointmentForm()
        {
            InitializeComponent();
            SetUpForm();
        }
        private void SetUpForm()
        {
            try
            {
                TypeComboBox.SelectedIndex = 0;
                string query = @"
                    SELECT
                        c.customerId,
                        c.customerName,
                        ap.appointmentId,
                        ap.title,
                        ap.description,
                        ap.location,
                        ap.contact,
                        ap.type,
                        ap.url,
                        ap.start,
                        ap.end
                    FROM
                        customer c
                    LEFT JOIN
                        appointment ap ON c.customerId = ap.customerId";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, Database.DBConnection.GetConnectionString());
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                AppointmentDataGridView.DataSource = dataTable;
                AppointmentDataGridView.Columns[0].Visible = false;
                AppointmentDataGridView.Columns[1].HeaderText = "Customer Name";
                AppointmentDataGridView.Columns[2].HeaderText = "Appointment ID";
                AppointmentDataGridView.Columns[3].HeaderText = "Title";
                AppointmentDataGridView.Columns[4].HeaderText = "Description";
                AppointmentDataGridView.Columns[5].HeaderText = "Location";
                AppointmentDataGridView.Columns[6].HeaderText = "Contact";
                AppointmentDataGridView.Columns[7].HeaderText = "Type";
                AppointmentDataGridView.Columns[8].HeaderText = "URL";
                AppointmentDataGridView.Columns[9].HeaderText = "Start";
                AppointmentDataGridView.Columns[10].HeaderText = "End";

                StartDateTimePicker.Value = ConvertLocalTimeToEST();
                EndDateTimePicker.Value = ConvertLocalTimeToEST();

                //AppointmentDataGridView_CellClick(this.AppointmentDataGridView, new DataGridViewCellEventArgs(0,0));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointment data: " + ex.Message);
            }
        }
        private void AddAppointmentButton_Click(object sender, EventArgs e)
        {
            if (AppointmentDataGridView.SelectedCells[2].Value != DBNull.Value)
            {
                MessageBox.Show("The customer already has an appointment.");
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    string checkQuery = @"SELECT * FROM appointment WHERE start < @end AND end > @start";
                    using (MySqlCommand command = new MySqlCommand(checkQuery, conn))
                    {
                        command.Parameters.AddWithValue("@start", StartDateTimePicker.Value);
                        command.Parameters.AddWithValue("@end", EndDateTimePicker.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Get the details of the first overlapping appointment
                                string existingStart = reader["start"].ToString();
                                string existingEnd = reader["end"].ToString();
                                string existingDescription = reader["description"].ToString(); // assuming a description column

                                // Construct a more detailed message
                                string message = $"There is an overlapping appointment:\n\n" +
                                                 $"Existing Appointment: {existingDescription}\n" +
                                                 $"Time: {existingStart} to {existingEnd}";

                                MessageBox.Show(message, "Scheduling Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    string insertQuery = @"
                        INSERT INTO appointment (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";
                    using (MySqlCommand command = new MySqlCommand(insertQuery, conn))
                    {
                        command.Parameters.AddWithValue("@customerId", AppointmentDataGridView.SelectedCells[0].Value);
                        command.Parameters.AddWithValue("@userId", Models.User.UserId);
                        command.Parameters.AddWithValue("@title", string.IsNullOrWhiteSpace(TitleTextBox.Text) ? "not needed" : TitleTextBox.Text);
                        command.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ? "not needed" : DescriptionTextBox.Text);
                        command.Parameters.AddWithValue("@location", string.IsNullOrWhiteSpace(LocationTextBox.Text) ? "not needed" : LocationTextBox.Text);
                        command.Parameters.AddWithValue("@contact", string.IsNullOrWhiteSpace(ContactTextBox.Text) ? "not needed" : ContactTextBox.Text);
                        command.Parameters.AddWithValue("@type", TypeComboBox.Text);
                        command.Parameters.AddWithValue("@url", string.IsNullOrWhiteSpace(UrlTextBox.Text) ? "not needed" : UrlTextBox.Text);
                        command.Parameters.AddWithValue("@start", StartDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@end", EndDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@createDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@createdBy", Models.User.UserName);
                        command.Parameters.AddWithValue("@lastUpdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@lastUpdateBy", Models.User.UserName);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Appointment submitted successfully.");
                    SetUpForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error with appointment data: " + ex.Message);
            }
        }


        private DateTime ConvertLocalTimeToEST()
        {
            // Get the selected date and time from the DateTimePicker.
            DateTime localTime = StartDateTimePicker.Value;

            // Get the Eastern Standard Time zone.
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            // Convert the local time to Eastern Standard Time.
            DateTime estTime = TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Local, est);
            return estTime;
        }

        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            StartErrorLabel.Visible = true;
            AddAppointmentButton.Enabled = false;
            if (StartDateTimePicker.Value.Hour < 9)
            {
                StartErrorLabel.Text = "Start time cannot be before 9:00 AM EST";
            }
            else if (StartDateTimePicker.Value.Hour >= 17)
            {
                StartErrorLabel.Text = "Start time cannot be after 5:00 PM EST";
            }
            else
            {
                if (StartDateTimePicker.Value > EndDateTimePicker.Value)
                {
                    StartErrorLabel.Text = "Start date cannot be after end date";
                }
                else
                {
                    StartErrorLabel.Visible = false;
                    if (StartErrorLabel.Visible == false && EndErrorLabel.Visible == false)
                        AddAppointmentButton.Enabled = true;
                }
            }
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EndErrorLabel.Visible = true;
            AddAppointmentButton.Enabled = false;
            if (EndDateTimePicker.Value.Hour < 9)
            {
                EndErrorLabel.Text = "End time cannot be before 9:00 AM EST";
            }
            else if (EndDateTimePicker.Value.Hour >= 17)
            {
                EndErrorLabel.Text = "End time cannot be after 5:00 PM EST";
            }
            else
            {
                if (EndDateTimePicker.Value < StartDateTimePicker.Value)
                {
                    EndErrorLabel.Text = "End date cannot be before start date";
                }
                else
                {
                    EndErrorLabel.Visible = false;
                    if (StartErrorLabel.Visible == false && EndErrorLabel.Visible == false)
                        AddAppointmentButton.Enabled = true;
                }
            }
        }

        private void EditAppointmentButton_Click(object sender, EventArgs e)
        {
            // Make sure an appointment is selected
            if (AppointmentDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to edit.");
                return;
            }

            try
            {
                // Get the appointmentId of the selected appointment from the DataGridView
                int appointmentId = Convert.ToInt32(AppointmentDataGridView.SelectedCells[2].Value);

                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    // Check for overlapping appointments, but exclude the current appointmentId
                    string checkQuery = @"
                        SELECT * FROM appointment 
                        WHERE start < @end AND end > @start AND appointmentId != @appointmentId";

                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, conn))
                    {
                        checkCommand.Parameters.AddWithValue("@start", StartDateTimePicker.Value);
                        checkCommand.Parameters.AddWithValue("@end", EndDateTimePicker.Value);
                        checkCommand.Parameters.AddWithValue("@appointmentId", appointmentId);

                        using (var reader = checkCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string existingStart = reader["start"].ToString();
                                string existingEnd = reader["end"].ToString();
                                string existingDescription = reader["description"].ToString();

                                string message = $"There is an overlapping appointment:\n\n" +
                                                 $"Existing Appointment: {existingDescription}\n" +
                                                 $"Time: {existingStart} to {existingEnd}";

                                MessageBox.Show(message, "Scheduling Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    // Construct the UPDATE query
                    string updateQuery = @"
                        UPDATE appointment
                        SET 
                            customerId = @customerId, 
                            userId = @userId, 
                            title = @title, 
                            description = @description, 
                            location = @location, 
                            contact = @contact, 
                            type = @type, 
                            url = @url, 
                            start = @start, 
                            end = @end, 
                            lastUpdate = @lastUpdate, 
                            lastUpdateBy = @lastUpdateBy
                        WHERE appointmentId = @appointmentId";

                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn))
                    {
                        updateCommand.Parameters.AddWithValue("@customerId", AppointmentDataGridView.SelectedCells[0].Value);
                        updateCommand.Parameters.AddWithValue("@userId", Models.User.UserId);
                        updateCommand.Parameters.AddWithValue("@title", string.IsNullOrWhiteSpace(TitleTextBox.Text) ? "not needed" : TitleTextBox.Text);
                        updateCommand.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ? "not needed" : DescriptionTextBox.Text);
                        updateCommand.Parameters.AddWithValue("@location", string.IsNullOrWhiteSpace(LocationTextBox.Text) ? "not needed" : LocationTextBox.Text);
                        updateCommand.Parameters.AddWithValue("@contact", string.IsNullOrWhiteSpace(ContactTextBox.Text) ? "not needed" : ContactTextBox.Text);
                        updateCommand.Parameters.AddWithValue("@type", TypeComboBox.Text);
                        updateCommand.Parameters.AddWithValue("@url", string.IsNullOrWhiteSpace(UrlTextBox.Text) ? "not needed" : UrlTextBox.Text);
                        updateCommand.Parameters.AddWithValue("@start", StartDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        updateCommand.Parameters.AddWithValue("@end", EndDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        updateCommand.Parameters.AddWithValue("@lastUpdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        updateCommand.Parameters.AddWithValue("@lastUpdateBy", Models.User.UserName);
                        updateCommand.Parameters.AddWithValue("@appointmentId", appointmentId); // Important for the WHERE clause

                        updateCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Appointment updated successfully.");
                    SetUpForm(); // Refresh the form to show the updated data
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error with appointment data: " + ex.Message);
            }
        }

        private void AppointmentDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (AppointmentDataGridView.SelectedCells[2].Value == DBNull.Value)
            {
                EditAppointmentButton.Enabled = false;
                DeleteAppointmentButton.Enabled = false;
            }
            else
            {
                EditAppointmentButton.Enabled = true;
                DeleteAppointmentButton.Enabled = true;
            }
            DataGridViewRow row = this.AppointmentDataGridView.Rows[e.RowIndex];

            TitleTextBox.Text = row.Cells["title"].Value?.ToString() ?? string.Empty;
            DescriptionTextBox.Text = row.Cells["description"].Value?.ToString() ?? string.Empty;
            LocationTextBox.Text = row.Cells["location"].Value?.ToString() ?? string.Empty;
            ContactTextBox.Text = row.Cells["contact"].Value?.ToString() ?? string.Empty;
            UrlTextBox.Text = row.Cells["url"].Value?.ToString() ?? string.Empty;
            TypeComboBox.Text = row.Cells["type"].Value?.ToString() ?? string.Empty;

            // For DateTimePickers, you need to parse the value
            // Use DateTime.TryParse to handle cases where the cell value might be invalid
            if (DateTime.TryParse(row.Cells["start"].Value.ToString(), out DateTime startDateTime))
            {
                StartDateTimePicker.Value = startDateTime;
            }

            if (DateTime.TryParse(row.Cells["end"].Value.ToString(), out DateTime endDateTime))
            {
                EndDateTimePicker.Value = endDateTime;
            }
        }

        private void DeleteAppointmentButton_Click(object sender, EventArgs e)
        {
            int appointmentId = Convert.ToInt32(AppointmentDataGridView.SelectedCells[2].Value);

            DialogResult confirmDelete = MessageBox.Show("Are you sure you want to delete this appointment?", "Yes, delete this appointment.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmDelete == DialogResult.No)
            {
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    string deleteQuery = @"DELETE FROM appointment WHERE appointmentId = @appointmentId";

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, conn))
                    {
                        command.Parameters.AddWithValue("@appointmentId", appointmentId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Appointment deleted successfully.");
                SetUpForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting appointent: " + ex.Message);
            }
        }
    }
}
