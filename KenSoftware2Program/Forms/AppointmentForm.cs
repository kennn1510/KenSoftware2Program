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
            }
        }
    }
}
