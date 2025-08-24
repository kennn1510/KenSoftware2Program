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
            GenerateReportScheduleForEachUser();
            GenerateReportCustomerNamesWithAndWithoutAppointments();
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
            try
            {
                List<Appointment> appointments = GetAllAppointmentsFromDb();
                List<User> users = GetAllUsersFromDb();

                // lambda expression on the collections
                var userSchedules = users
                    .GroupJoin(
                        appointments,
                        user => user.Id,
                        appt => appt.UserId,
                        (user, appts) => new
                        {
                            UserName = user.Name,
                            Appointments = appts.OrderBy(a => a.Start)
                        })
                    .OrderBy(user => user.UserName);

                Report2RichTextBox.Text = BuildScheduleReportString(userSchedules);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating user schedule report: " + ex.Message);
            }
        }

        private List<Appointment> GetAllAppointmentsFromDb()
        {
            var appointments = new List<Appointment>();
            using (var conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
            {
                conn.Open();
                string sql = "SELECT userId, start, end, type FROM appointment";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment()
                        {
                            UserId = reader.GetInt32("userId"),
                            Start = reader.GetDateTime("start"),
                            End = reader.GetDateTime("end"),
                            Type = reader.GetString("type")
                        });
                    }
                }
            }
            return appointments;
        }

        private List<User> GetAllUsersFromDb()
        {
            var users = new List<User>();
            using (var conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
            {
                conn.Open();
                string sql = "SELECT userId, userName FROM user";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User()
                        {
                            Id = reader.GetInt32("userId"),
                            Name = reader.GetString("username"),
                        });
                    }
                }
            }
            return users;
        }

        private string BuildScheduleReportString(IEnumerable<dynamic> userSchedules)
        {
            var reportContent = new System.Text.StringBuilder("User Schedule Report\n\n");
            foreach (var schedule in userSchedules)
            {
                reportContent.Append($"Schedule for user: {schedule.UserName}\n");
                reportContent.Append("----------------------------------------\n");

                foreach (var appointment in schedule.Appointments)
                {
                    reportContent.Append($"Type: {appointment.Type}\n");
                    reportContent.Append($"Start: {appointment.Start.ToLocalTime()}\n");
                    reportContent.Append($"End: {appointment.End.ToLocalTime()}\n\n");
                }
            }
            return reportContent.ToString();
        }

        private void GenerateReportCustomerNamesWithAndWithoutAppointments()
        {
            Report3RichTextBox.Clear();
            var customers = new List<Customer>();
            var appointments = new List<Appointment>();
            try
            {
                using (var conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    string sqlCustomers = "SELECT customerId, customerName FROM customer";
                    using (var cmd = new MySqlCommand(sqlCustomers, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new Customer()
                                {
                                    CustomerId = reader.GetInt32("customerId"),
                                    CustomerName = reader.GetString("customerName")
                                });
                            }
                        }
                    }

                    string sqlAppointments = "SELECT customerId FROM appointment";
                    using (var cmd = new MySqlCommand(sqlAppointments, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                appointments.Add(new Appointment()
                                {
                                    CustomerId = reader.GetInt32("customerId")
                                });
                            }
                        }
                    }
                }

                // Get a distinct list of customer IDs that have appointments
                var customerIdsWithAppointments = appointments
                    .Select(a => a.CustomerId)
                    .Distinct()
                    .ToList();

                // LINQ with lambda expressions to filter the customer lists
                var customersWithAppointments = customers
                    .Where(c => customerIdsWithAppointments.Contains(c.CustomerId))
                    .ToList();

                var customersWithoutAppointments = customers
                    .Where(c => !customerIdsWithAppointments.Contains(c.CustomerId))
                    .ToList();

                Report3RichTextBox.AppendText("Customers With Appointments:\n\n");
                if (customersWithAppointments.Any())
                {
                    foreach (var customer in customersWithAppointments)
                    {
                        Report3RichTextBox.AppendText($"- {customer.CustomerName}\n");
                    }
                }
                else
                {
                    Report3RichTextBox.AppendText("No customers found with appointments.\n");
                }

                Report3RichTextBox.AppendText("\n----------------------------------------\n\n");

                Report3RichTextBox.AppendText("Customers Without Appointments:\n\n");
                if (customersWithoutAppointments.Any())
                {
                    foreach (var customer in customersWithoutAppointments)
                    {
                        Report3RichTextBox.AppendText($"- {customer.CustomerName}\n");
                    }
                }
                else
                {
                    Report3RichTextBox.AppendText("All customers have appointments.\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report on customers with and without appointments: " + ex.Message);
            }
        }


    }
}
