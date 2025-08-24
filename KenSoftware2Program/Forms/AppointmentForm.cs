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
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["start"] != DBNull.Value)
                    {
                        row["start"] = ((DateTime)row["start"]).ToLocalTime();
                    }
                    if (row["end"] != DBNull.Value)
                    {
                        row["end"] = ((DateTime)row["end"]).ToLocalTime();
                    }
                }
                AppointmentDataGridView.DataSource = dataTable;
                AppointmentDataGridView.Columns["customerId"].Visible = false;
                AppointmentDataGridView.Columns["customerName"].HeaderText = "Customer Name";
                AppointmentDataGridView.Columns["appointmentId"].HeaderText = "Appointment ID";
                AppointmentDataGridView.Columns["title"].HeaderText = "Title";
                AppointmentDataGridView.Columns["description"].HeaderText = "Description";
                AppointmentDataGridView.Columns["location"].HeaderText = "Location";
                AppointmentDataGridView.Columns["contact"].HeaderText = "Contact";
                AppointmentDataGridView.Columns["type"].HeaderText = "Type";
                AppointmentDataGridView.Columns["url"].HeaderText = "URL";
                AppointmentDataGridView.Columns["start"].HeaderText = "Start";
                AppointmentDataGridView.Columns["end"].HeaderText = "End";

                if (AppointmentDataGridView.Rows.Count > 0)
                {
                    AppointmentDataGridView.Rows[0].Selected = true;
                    int selectedRowIndex = AppointmentDataGridView.SelectedRows[0].Index;
                    DataGridViewCellEventArgs args = new DataGridViewCellEventArgs(0, selectedRowIndex);
                    AppointmentDataGridView_CellClick(AppointmentDataGridView, args);
                }
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
            DateTime utcStart = StartDateTimePicker.Value.ToUniversalTime();
            DateTime utcEnd = EndDateTimePicker.Value.ToUniversalTime();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    string checkQuery = @"SELECT * FROM appointment WHERE start < @end AND end > @start";
                    using (MySqlCommand command = new MySqlCommand(checkQuery, conn))
                    {
                        command.Parameters.AddWithValue("@start", utcStart);
                        command.Parameters.AddWithValue("@end", utcEnd);

                        using (var reader = command.ExecuteReader())
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
                        command.Parameters.AddWithValue("@start", utcStart);
                        command.Parameters.AddWithValue("@end", utcEnd);
                        command.Parameters.AddWithValue("@createDate", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@createdBy", Models.User.UserName);
                        command.Parameters.AddWithValue("@lastUpdate", DateTime.UtcNow);
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

        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            ValidateAppointmentTimes();
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            ValidateAppointmentTimes();
        }

        private void EditAppointmentButton_Click(object sender, EventArgs e)
        {
            if (AppointmentDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to edit.");
                return;
            }
            DateTime utcStart = StartDateTimePicker.Value.ToUniversalTime();
            DateTime utcEnd = EndDateTimePicker.Value.ToUniversalTime();
            try
            {
                int appointmentId = Convert.ToInt32(AppointmentDataGridView.SelectedCells[2].Value);

                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    string checkQuery = @"
                        SELECT * FROM appointment 
                        WHERE start < @end AND end > @start AND appointmentId != @appointmentId";

                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, conn))
                    {
                        checkCommand.Parameters.AddWithValue("@start", utcStart);
                        checkCommand.Parameters.AddWithValue("@end", utcEnd);
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
                        updateCommand.Parameters.AddWithValue("@start", utcStart);
                        updateCommand.Parameters.AddWithValue("@end", utcEnd);
                        updateCommand.Parameters.AddWithValue("@lastUpdate", DateTime.UtcNow);
                        updateCommand.Parameters.AddWithValue("@lastUpdateBy", Models.User.UserName);
                        updateCommand.Parameters.AddWithValue("@appointmentId", appointmentId);

                        updateCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Appointment updated successfully.");
                    SetUpForm();
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

        private void CalendarViewButton_Click(object sender, EventArgs e)
        {
            CalendarViewForm calendarViewForm = new CalendarViewForm();
            calendarViewForm.ShowDialog();
        }

        private void ValidateAppointmentTimes()
        {
            bool hasError = false;
            StartErrorLabel.Visible = false;
            EndErrorLabel.Visible = false;

            if (StartDateTimePicker.Value >= EndDateTimePicker.Value)
            {
                StartErrorLabel.Text = "Start time must be before the end time";
                StartErrorLabel.Visible = true;
                EndErrorLabel.Text = "End time must be after the start time";
                EndErrorLabel.Visible = true;
                hasError = true;
            }
            else
            {
                TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                // Check business hours for Start Time
                DateTime startEst = TimeZoneInfo.ConvertTime(StartDateTimePicker.Value, TimeZoneInfo.Local, estTimeZone);
                if (startEst.Hour < 9 || startEst.Hour >= 17)
                {
                    StartErrorLabel.Text = "Time must be within business hours (9 AM - 5 PM EST)";
                    StartErrorLabel.Visible = true;
                    hasError = true;
                }

                DateTime endEst = TimeZoneInfo.ConvertTime(EndDateTimePicker.Value, TimeZoneInfo.Local, estTimeZone);
                if (endEst.Hour < 9 || endEst.Hour > 17 || (endEst.Hour == 17 && endEst.Minute > 0))
                {
                    EndErrorLabel.Text = "Time must be within business hours (9 AM - 5 PM EST)";
                    EndErrorLabel.Visible = true;
                    hasError = true;
                }
            }

            AddAppointmentButton.Enabled = !hasError;
        }
    }
}
