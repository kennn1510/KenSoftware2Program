using KenSoftware2Program.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            SetUpReportsForm();
        }

        private void SetUpReportsForm()
        {
            GenerateReportNumberOfAppointmentTypesByMonth();
            //GenerateReportScheduleForEachUser();
            //GenerateReportNumberOfCustomersWithAndWithoutAppointments();
        }
        private void GenerateReportNumberOfAppointmentTypesByMonth()
        {
            RichTextBox report = Report1RichTextBox;
            Report1RichTextBox.Clear();
            List<Appointment> appointments = new List<Appointment>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();
                    string sql = "SELECT start, end, type FROM appointment";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                appointments.Add(new Appointment()
                                {
                                    Start = reader.GetDateTime("start"),
                                    End = reader.GetDateTime("end"),
                                    Type = reader.GetString("type")
                                });
                            }
                        }

                    }
                    var appointmentCounts = appointments
                        .GroupBy(a => new
                        {
                            a.Start.Month,
                            a.Type
                        })
                        .Select(g => new
                        {
                            MonthName = new DateTime(2000, g.Key.Month, 1).ToString("MMMM"),
                            g.Key.Type,
                            Count = g.Count()
                        });
                    report.AppendText("Appointment Types by Month Report\n\n");
                    foreach (var group in appointmentCounts)
                    {
                        report.AppendText($"Month: {group.MonthName}, Type: {group.Type}, Count: {group.Count}\n\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report on number of appointment types by month: " + ex.Message);
            }
        }
        private void GenerateReportScheduleForEachUser()
        {
            throw new NotImplementedException();
        }

        private void GenerateReportNumberOfCustomersWithAndWithoutAppointments()
        {
            throw new NotImplementedException();
        }


    }
}
