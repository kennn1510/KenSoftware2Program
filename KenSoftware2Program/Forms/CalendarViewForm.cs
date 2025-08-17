using System;
using System.Windows.Forms;

namespace KenSoftware2Program.Forms
{
    public partial class CalendarViewForm : Form
    {
        public CalendarViewForm()
        {
            InitializeComponent();
            SetUpForm();
        }
        private void SetUpForm()
        {
            var date = CalendarMonthCalendar.SelectionStart;
            Console.WriteLine(date);
        }
        private void CalendarMonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            SetUpForm();
        }
    }
}
