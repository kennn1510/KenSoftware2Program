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
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, Database.DBConnection.conn);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointment data: " + ex.Message);
            }
        }
        private void AddAppointmentButton_Click(object sender, EventArgs e)
        {
            if (AppointmentDataGridView.SelectedCells[2].Value.ToString() != "")
            {
                MessageBox.Show("The customer already has an appointment.");
                return;
            }
            try
            {
                if (Database.DBConnection.conn.State != ConnectionState.Open)
                    Database.DBConnection.conn.Open();

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = Database.DBConnection.conn;
                    command.CommandText = @"
                        INSERT INTO appointment (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@customerId", AppointmentDataGridView.SelectedCells[0].Value);
                    command.Parameters.AddWithValue("@userId", Models.User.UserName);
                    command.Parameters.AddWithValue("@title", string.IsNullOrWhiteSpace(TitleTextBox.Text) ? "Not needed" : TitleTextBox.Text);
                    command.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ? "Not needed" : DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@location", string.IsNullOrWhiteSpace(LocationTextBox.Text) ? "Not needed" : LocationTextBox.Text);
                    command.Parameters.AddWithValue("@contact", string.IsNullOrWhiteSpace(ContactTextBox.Text) ? "Not needed" : ContactTextBox.Text);
                    command.Parameters.AddWithValue("@type", TypeComboBox.Text);
                    command.Parameters.AddWithValue("@url", string.IsNullOrWhiteSpace(UrlTextBox.Text) ? "Not needed" : UrlTextBox.Text);
                    command.Parameters.AddWithValue("@start", StartDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@end", EndDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@createDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@createdBy", Models.User.UserName);
                    command.Parameters.AddWithValue("@lastUpdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@lastUpdateBy", Models.User.UserName);
                }
                MessageBox.Show("Appointment submitted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying appointment data");
            }
            finally
            {
                if (Database.DBConnection.conn.State == ConnectionState.Open)
                    Database.DBConnection.conn.Close();
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
                StartErrorLabel.Text = "Start time cannot be before 9:00 AM EST";
            else if (StartDateTimePicker.Value.Hour >= 17)
                StartErrorLabel.Text = "Start time cannot be after 5:00 PM EST";
            else
            {
                StartErrorLabel.Visible = false;
                if (StartErrorLabel.Visible == false && EndErrorLabel.Visible == false)
                    AddAppointmentButton.Enabled = true;
            }
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EndErrorLabel.Visible = true;
            AddAppointmentButton.Enabled = false;
            if (EndDateTimePicker.Value.Hour < 9)
                EndErrorLabel.Text = "End time cannot be before 9:00 AM EST";
            else if (EndDateTimePicker.Value.Hour >= 17)
                EndErrorLabel.Text = "End time cannot be after 5:00 PM EST";
            else
            {
                EndErrorLabel.Visible = false;
                if (StartErrorLabel.Visible == false && EndErrorLabel.Visible == false)
                    AddAppointmentButton.Enabled = true;
            }
        }
    }
}
