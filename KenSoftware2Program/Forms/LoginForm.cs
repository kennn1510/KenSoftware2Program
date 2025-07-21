using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;


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
            } else
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
                Console.WriteLine(watcher.Position.Location);
            }

            void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
            {
                if (culture.TwoLetterISOLanguageName == "en")
                {
                    loginForm.LocationLabel.Text = $"Location: Latitude: {e.Position.Location.Latitude}, Longitude: {e.Position.Location.Longitude}";
                } else
                {
                    loginForm.LocationLabel.Text = $"Localisation: Latitude: {e.Position.Location.Latitude}, Longitude: {e.Position.Location.Longitude}";
                }
            }

        }


        private void UsernameLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
