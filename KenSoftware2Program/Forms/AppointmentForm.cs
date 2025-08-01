using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class AppointmentForm : Form
    {
        private int customerId;

        public AppointmentForm(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;

        }
    }
}
