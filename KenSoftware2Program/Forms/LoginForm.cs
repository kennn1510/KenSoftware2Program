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
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 1.0; // set to one meter
            watcher.TryStart(false, TimeSpan.FromSeconds(10));

            CivicAddressResolver resolver = new CivicAddressResolver();

            if (!watcher.Position.Location.IsUnknown)
            {
                CivicAddress address = resolver.ResolveAddress(watcher.Position.Location);

                if (!address.IsUnknown)
                {
                    LocationLabel.Text = $"Country: {address.CountryRegion}, Zip: {address.PostalCode}";
                }
                else
                {
                    LocationLabel.Text = "Address unknown.";
                }
            }
        }

        private void UsernameLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
