using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KenSoftware2Program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //get the connection string
            string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            //make the connection
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(constr);

                //open the connection
                conn.Open();

                MessageBox.Show("Connection is open.");
            } 
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //close the connection
                if(conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
