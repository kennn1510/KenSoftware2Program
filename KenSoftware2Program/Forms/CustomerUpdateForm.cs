using System;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class CustomerUpdateForm : Form
    {
        public CustomerUpdateForm(DataGridViewSelectedCellCollection selectedCells)
        {
            InitializeComponent();
            Console.WriteLine($"{selectedCells[0].Value}, {selectedCells[1].Value}, {selectedCells[2].Value}");
        }
    }
}
