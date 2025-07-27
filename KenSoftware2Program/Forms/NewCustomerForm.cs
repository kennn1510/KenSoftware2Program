using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class NewCustomerForm : Form
    {
        public NewCustomerForm()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {

                string customerQuery = $"INSERT INTO customer";
                string addressQuery = $"INSERT INTO address";
                MySqlDataAdapter adapter = new MySqlDataAdapter(customerQuery, Database.DBConnection.conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
