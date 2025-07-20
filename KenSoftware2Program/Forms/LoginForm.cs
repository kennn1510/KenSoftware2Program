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
        public LoginForm()
        {
            InitializeComponent();
            LocalizeLanguage();
        }

        private void LocalizeLanguage()
        {
            CultureInfo culture = CultureInfo.CurrentCulture;

            CLocation myLocation = new CLocation();
            myLocation.GetLocationEvent();
            Console.WriteLine("Enter any key to quit.");
            Console.ReadLine();

            if (culture.TwoLetterISOLanguageName == "en")
            {
                Console.WriteLine("Translated to english");
                //LocationLabel.Text = $"Location: {latitude}, {longitude}";
            } else
            {
                Console.WriteLine("Traduit en français");
                this.Text = "Formulaire de connexion";
                LocationLabel.Text = $"Emplacement: ";
                UsernameLabel.Text = "Nom d'utilisateur:";
                PasswordLabel.Text = "Mot de passe:";
                LoginButton.Text = "Se connecter";
            }
        }

        class CLocation
        {
            GeoCoordinateWatcher watcher;

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
                PrintPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
            }

            void PrintPosition(double Latitude, double Longitude)
            {
                Console.WriteLine("Latitude: {0}, Longitude {1}", Latitude, Longitude);
            }
        }


        private void UsernameLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
