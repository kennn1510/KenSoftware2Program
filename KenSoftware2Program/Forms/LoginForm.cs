using KenSoftware2Program.Models;
using System;
using System.Device.Location;
using System.Globalization;
using System.Windows.Forms;

namespace KenSoftware2Program
{
    public partial class LoginForm : Form
    {
        CultureInfo culture;
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
            CLocation myLocation = new CLocation(this, culture);
            myLocation.GetLocationEvent();
        }

        class CLocation
        {
            GeoCoordinateWatcher watcher;
            private LoginForm loginForm;
            private CultureInfo culture;

            public CLocation(LoginForm loginForm, CultureInfo culture)
            {
                this.loginForm = loginForm;
                this.culture = culture;
            }

            public void GetLocationEvent()
            {
                this.watcher = new GeoCoordinateWatcher();
                this.watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                bool started = this.watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
                if (!started)
                {
                    Console.WriteLine("GeoCoordinateWatcher timed out on start.");
                }
            }

            void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
            {
                if (culture.TwoLetterISOLanguageName == "en")
                {
                    loginForm.LocationLabel.Text = $"Location: Latitude: {e.Position.Location.Latitude}, Longitude: {e.Position.Location.Longitude}";
                }
                else
                {
                    loginForm.LocationLabel.Text = $"Localisation: Latitude: {e.Position.Location.Latitude}, Longitude: {e.Position.Location.Longitude}";
                }
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            UsernameTextBox.Text = UsernameTextBox.Text.Trim();
            PasswordTextBox.Text = PasswordTextBox.Text.Trim();


            if (User.ValidateLogin(UsernameTextBox.Text, PasswordTextBox.Text) != false)
            {
                UsernameErrorsLabel.ResetText();
                PasswordErrorsLabel.ResetText();

                Console.WriteLine(User.UserName);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                UsernameErrorsLabel.Visible = true;
                PasswordErrorsLabel.Visible = true;

                if (UsernameTextBox.Text != PasswordTextBox.Text)
                {
                    if (culture.TwoLetterISOLanguageName == "en")
                    {
                        UsernameErrorsLabel.Text = "The username does not match the password";
                        PasswordErrorsLabel.Text = "The password does not match the username";
                    }
                    else
                    {
                        UsernameErrorsLabel.Text = "Le nom d'utilisateur ne correspond pas au mot de passe";
                        PasswordErrorsLabel.Text = "Le mot de passe ne correspond pas au nom d'utilisateur";
                    }
                }
                else
                {
                    if (culture.TwoLetterISOLanguageName == "en")
                    {
                        UsernameErrorsLabel.Text = "Incorrect username";
                        PasswordErrorsLabel.Text = "Incorrect password";
                    }
                    else
                    {
                        UsernameErrorsLabel.Text = "Nom d'utilisateur incorrect";
                        PasswordErrorsLabel.Text = "Mot de passe incorrect";
                    }
                }
            }
        }
    }
}