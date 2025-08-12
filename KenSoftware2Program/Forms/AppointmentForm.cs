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
            if (AppointmentDataGridView.SelectedCells[1].Value.ToString() != "")
            {
                MessageBox.Show("The customer already has an appointment.");
                return;
            }
            DateTime estTime = ConvertLocalTimeToEST();

            // Validate the day of the week.
            if (estTime.DayOfWeek == DayOfWeek.Saturday || estTime.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Appointments cannot be scheduled on weekends (Eastern Standard Time). Please select a weekday.");
                return; // Stop the form submission.
            }

            // Validate the hour of the day.
            if (estTime.Hour < 9 || estTime.Hour >= 17)
            {
                MessageBox.Show("Appointments can only be scheduled between 9 a.m. and 5 p.m. Eastern Standard Time. The timezone is automatically converted. Please adjust the time.");
                return; // Stop the form submission.
            }

            // If all validations pass, proceed with saving the appointment.
            // Your code for adding, updating, or deleting the appointment goes here.
            // Example: SaveAppointment(localTime);
            MessageBox.Show("Appointment submitted successfully.");
            try
            {
                if (Database.DBConnection.conn.State != ConnectionState.Open)
                    Database.DBConnection.conn.Open();

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = Database.DBConnection.conn;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying appointment data");
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
            if (StartDateTimePicker.Value.Hour < 9)
            {
                StartErrorLabel.Text = "Start time cannot be before 9:00 AM EST";
                StartErrorLabel.Visible = true;
                AddAppointmentButton.Enabled = false;
            }
            else if (StartDateTimePicker.Value.Hour >= 17)
            {
                StartErrorLabel.Text = "Start tiem cannot be after 5:00 PM EST";
                StartErrorLabel.Visible = true;
                AddAppointmentButton.Enabled = false;
            }
            else
            {
                StartErrorLabel.Visible = false;
                AddAppointmentButton.Enabled = true;
            }
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
