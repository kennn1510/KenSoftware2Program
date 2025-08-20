using KenSoftware2Program.Models;
using MySql.Data.MySqlClient;
using System;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace KenSoftware2Program
{
    public partial class LoginForm : Form
    {
        private GeoCoordinateWatcher watcher;
        private Timer timeout;
        private CultureInfo culture;

        public LoginForm()
        {
            InitializeComponent();
            LocalizeLanguage();
            GetLocation();
        }

        private void LocalizeLanguage()
        {
            culture = CultureInfo.CurrentCulture;
            if (culture.TwoLetterISOLanguageName == "en")
            {
                Console.WriteLine("Translated to english");
            }
            else
            {
                Console.WriteLine("Traduit en français");
                this.Text = "Formulaire de connexion";
                LocationLabel.Text = $"Localisation : traitement...";
                UsernameLabel.Text = "Nom d'utilisateur:";
                PasswordLabel.Text = "Mot de passe:";
                LoginButton.Text = "Se connecter";
            }
        }

        private void GetLocation()
        {
            // Initialize the GeoCoordinateWatcher
            watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Watcher_PositionChanged);
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(Watcher_StatusChanged);

            // Start the watcher
            watcher.Start();

            // Initialize and start the timeout timer
            timeout = new Timer();
            timeout.Interval = 10000; // 10 seconds
            timeout.Tick += (sender, e) =>
            {
                // Check if the watcher is still initializing or has no data
                if (watcher.Status == GeoPositionStatus.Initializing || watcher.Status == GeoPositionStatus.NoData)
                {
                    watcher.Stop();
                    timeout.Stop();
                    UpdateLocationLabel("Location services are not available.");
                }
            };
            timeout.Start();
        }

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // Stop both the watcher and the timer
            watcher.Stop();
            timeout.Stop();

            // Check if the location is valid
            if (!e.Position.Location.IsUnknown)
            {
                string lat = e.Position.Location.Latitude.ToString("F6", CultureInfo.InvariantCulture);
                string lon = e.Position.Location.Longitude.ToString("F6", CultureInfo.InvariantCulture);
                UpdateLocationLabel($"Latitude: {lat}, Longitude: {lon}");
            }
            else
            {
                UpdateLocationLabel("Location data is unknown.");
            }
        }

        private void Watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            // Handle different status changes
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    watcher.Stop();
                    timeout.Stop();
                    UpdateLocationLabel("Location services are disabled.");
                    break;
                case GeoPositionStatus.Ready:
                    // Location is ready, the PositionChanged event will fire next
                    break;
                case GeoPositionStatus.NoData:
                    // This event handles no data, but the timer will eventually stop it.
                    // No action needed here as the timer handles the final state.
                    break;
            }
        }

        private void UpdateLocationLabel(string text)
        {
            if (culture.TwoLetterISOLanguageName == "en")
            {
                LocationLabel.Text = $"Location: {text}";
            }
            else
            {
                string localizedText;
                switch (text)
                {
                    case "Location services are disabled.":
                        localizedText = "Les services de localisation sont désactivés.";
                        break;
                    case "No location data found.":
                        localizedText = "Aucune donnée de localisation n'a été trouvée.";
                        break;
                    case "Location services are not available.":
                        localizedText = "Les services de localisation ne sont pas disponibles.";
                        break;
                    default:
                        localizedText = $"Localisation: {text}";
                        break;
                }
                LocationLabel.Text = localizedText;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Trim any leading/trailing whitespace from the inputs
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            // Use the refactored User.Login method to authenticate
            if (User.Login(username, password))
            {
                // Login was successful
                // Clear any previous error messages
                UsernameErrorsLabel.Visible = false;
                PasswordErrorsLabel.Visible = false;
                UsernameErrorsLabel.Text = "";
                PasswordErrorsLabel.Text = "";

                CheckForUpcomingAppointments();
                RecordLoginTimestamp(username);

                // Set the dialog result and close the form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Login failed
                UsernameErrorsLabel.Visible = true;
                PasswordErrorsLabel.Visible = true;

                // Display a single, generic error message for security
                if (culture.TwoLetterISOLanguageName == "en")
                {
                    UsernameErrorsLabel.Text = "Incorrect username or password.";
                    PasswordErrorsLabel.Text = "Incorrect username or password.";
                }
                else
                {
                    UsernameErrorsLabel.Text = "Nom d'utilisateur ou mot de passe incorrect.";
                    PasswordErrorsLabel.Text = "Nom d'utilisateur ou mot de passe incorrect.";
                }
            }
        }

        private void RecordLoginTimestamp(string username)
        {
            try
            {
                // Get the path to the current user's desktop
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                // Define the path for the log file
                string logFilePath = Path.Combine(desktopPath, "Login_History.txt");

                // Get the current timestamp and format the log entry
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"{timestamp} - {username} logged in.";

                // Append the log entry to the file
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

                MessageBox.Show($"Login history recorded to: {logFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to record login timestamp: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckForUpcomingAppointments()
        {
            int alertWindowMinutes = 15;

            try
            {
                int currentUserId = Models.User.UserId;

                // Log the user ID for debugging
                Console.WriteLine($"Checking appointments for User ID: {currentUserId}");

                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                SELECT title, start
                FROM appointment
                WHERE userId = @userId AND start >= @now AND start <= @alertTime";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        // Use the correct DateTime reference (UTC or Local)
                        DateTime now = DateTime.Now; // Or DateTime.UtcNow if that's what's in the DB
                        DateTime alertTime = now.AddMinutes(alertWindowMinutes);

                        command.Parameters.AddWithValue("@userId", currentUserId);
                        command.Parameters.AddWithValue("@now", now);
                        command.Parameters.AddWithValue("@alertTime", alertTime);

                        // Log the time range being checked
                        Console.WriteLine($"Checking for appointments between {now} and {alertTime}");

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string title = reader["title"].ToString();
                                DateTime startTime = Convert.ToDateTime(reader["start"]);

                                MessageBox.Show($"You have an upcoming appointment within the next 15 minutes:\n\n" +
                                                $"Title: {title}\n" +
                                                $"Time: {startTime.ToShortTimeString()}",
                                                "Upcoming Appointment Alert",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information);
                            }
                            else
                            {
                                Console.WriteLine("No upcoming appointments found in the next 15 minutes.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking for upcoming appointments: " + ex.Message);
                MessageBox.Show("An error occurred while checking appointments: " + ex.Message);
            }
        }
    }
}