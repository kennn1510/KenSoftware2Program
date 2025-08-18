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
            Report1RichTextBox.Clear();
            var appointments = new List<Appointment>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();
                    string sql = "SELECT start, type FROM appointment";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                appointments.Add(new Appointment()
                                {
                                    Start = reader.GetDateTime("start"),
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
                            MonthNumber = g.Key.Month,
                            MonthName = new DateTime(2000, g.Key.Month, 1).ToString("MMMM"),
                            g.Key.Type,
                            Count = g.Count()
                        })
                        .OrderBy(result => result.MonthNumber)
                        .ThenBy(result => result.Type);
                    Report1RichTextBox.AppendText("Appointment Types by Month Report\n\n");
                    foreach (var group in appointmentCounts)
                    {
                        Report1RichTextBox.AppendText($"Month: {group.MonthName}, Type: {group.Type}, Count: {group.Count}\n\n");
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
