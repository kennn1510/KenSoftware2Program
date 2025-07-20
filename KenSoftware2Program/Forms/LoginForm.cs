using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Device.Location;


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

            if (culture.TwoLetterISOLanguageName == "en")
            {
                Console.WriteLine("Translated to english");
            } else
            {
                Console.WriteLine("Traduit en français");
                this.Text = "Formulaire de connexion";
                LocationLabel.Text = "Emplacement";
                UsernameLabel.Text = "Nom d'utilisateur:";
                PasswordLabel.Text = "Mot de passe:";
                LoginButton.Text = "Se connecter";
            }
        }


        private void UsernameLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
