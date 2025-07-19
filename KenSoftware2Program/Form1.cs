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


namespace KenSoftware2Program
{
    public partial class LoginForm : Form
    {
        private CultureInfo userCulture;
        private string cultureCode;

        public LoginForm()
        {
            InitializeComponent();
            userCulture = CultureInfo.CurrentUICulture;
            cultureCode = userCulture.Name;
            LocationLabel.Text = LocationLabel.Text + " " + cultureCode;
        }

        private void UsernameLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
