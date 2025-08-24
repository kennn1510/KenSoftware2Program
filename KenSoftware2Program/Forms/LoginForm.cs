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
            watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Watcher_PositionChanged);
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(Watcher_StatusChanged);

            watcher.Start();

            timeout = new Timer();
            timeout.Interval = 10000; // 10 seconds
            timeout.Tick += (sender, e) =>
            {
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
            watcher.Stop();
            timeout.Stop();

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
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    watcher.Stop();
                    timeout.Stop();
                    UpdateLocationLabel("Location services are disabled.");
                    break;
                case GeoPositionStatus.Ready:
                    break;
                case GeoPositionStatus.NoData:

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
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            if (User.Login(username, password))
            {

                UsernameErrorsLabel.Visible = false;
                PasswordErrorsLabel.Visible = false;
                UsernameErrorsLabel.Text = "";
                PasswordErrorsLabel.Text = "";

                CheckForUpcomingAppointments();
                RecordLoginTimestamp(username);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                UsernameErrorsLabel.Visible = true;
                PasswordErrorsLabel.Visible = true;

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
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string logFilePath = Path.Combine(desktopPath, "Login_History.txt");
                DateTime timestamp = DateTime.UtcNow;
                string logEntry = $"{timestamp} - {username} logged in.";

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

                using (MySqlConnection conn = new MySqlConnection(Database.DBConnection.GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT title, start
                        FROM appointment
                        WHERE userId = @userId AND start >= @now AND start <= @alertTime";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        DateTime nowUtc = DateTime.UtcNow;
                        DateTime alertTimeUtc = nowUtc.AddMinutes(alertWindowMinutes);

                        command.Parameters.AddWithValue("@userId", currentUserId);
                        command.Parameters.AddWithValue("@now", nowUtc);
                        command.Parameters.AddWithValue("@alertTime", alertTimeUtc);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string title = reader["title"].ToString();
                                DateTime startTimeUtc = Convert.ToDateTime(reader["start"]);
                                DateTime localStartTime = startTimeUtc.ToLocalTime();
                                MessageBox.Show($"You have an upcoming appointment within the next 15 minutes:\n\n" +
                                                $"Title: {title}\n" +
                                                $"Time: {localStartTime.ToShortTimeString()}",
                                                "Upcoming Appointment Alert",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information);
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