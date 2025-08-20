using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class CalendarViewForm : Form
    {
        public CalendarViewForm()
        {
            InitializeComponent();
            SetUpForm();
        }
        private void SetUpForm()
        {
            var selectedDate = CalendarMonthCalendar.SelectionStart;

            try
            {
                DataTable dataTable = new DataTable();

                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

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
                            appointment ap ON c.customerId = ap.customerId
                        WHERE
                            ap.start >= @startOfDay AND ap.start < @endOfDay
                        ORDER BY
                            ap.start";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@startOfDay", selectedDate.Date);
                        command.Parameters.AddWithValue("@endOfDay", selectedDate.Date.AddDays(1));

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

                CalendarDataGridView.DataSource = dataTable;

                CalendarDataGridView.Columns["customerId"].Visible = false;

                CalendarDataGridView.Columns["customerName"].HeaderText = "Customer Name";
                CalendarDataGridView.Columns["appointmentId"].HeaderText = "Appointment ID";
                CalendarDataGridView.Columns["title"].HeaderText = "Title";
                CalendarDataGridView.Columns["description"].HeaderText = "Description";
                CalendarDataGridView.Columns["location"].HeaderText = "Location";
                CalendarDataGridView.Columns["contact"].HeaderText = "Contact";
                CalendarDataGridView.Columns["type"].HeaderText = "Type";
                CalendarDataGridView.Columns["url"].HeaderText = "URL";
                CalendarDataGridView.Columns["start"].HeaderText = "Start";
                CalendarDataGridView.Columns["end"].HeaderText = "End";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading calendar view data: " + ex.Message);
            }
        }
        private void CalendarMonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            SetUpForm();
        }
    }
}
