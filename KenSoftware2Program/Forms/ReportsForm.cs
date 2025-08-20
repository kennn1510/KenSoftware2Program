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
            Report2RichTextBox.Clear();
            var appointments = new List<Appointment>();
            var users = new List<User>();

            try
            {
                using (var conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();
                    string sqlAppointments = "SELECT userId, start, end, type FROM appointment";
                    using (var cmd = new MySqlCommand(sqlAppointments, conn))
                    {
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

                    string sqlUsers = "SELECT userId, userName FROM user";
                    using (var cmd = new MySqlCommand(sqlUsers, conn))
                    {
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
                }
                var userSchedules = from user in users
                                    join appointment in appointments on user.Id equals appointment.UserId
                                    group appointment by user.Name into g
                                    select new
                                    {
                                        UserName = g.Key,
                                        Appointments = g.ToList()
                                    };

                foreach (var schedule in userSchedules)
                {
                    Report2RichTextBox.AppendText($"Schedule for the user: {schedule.UserName}\n\n");
                    foreach (var appointment in schedule.Appointments)
                    {
                        Report2RichTextBox.AppendText($"Type: {appointment.Type}, Start: {appointment.Start}, End: {appointment.End}\n\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating user scedule report: " + ex.Message);
            }
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
