using KenSoftware2Program.Forms;
using System;
using System.Windows.Forms;

namespace KenSoftware2Program
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (LoginForm loginForm = new LoginForm())
            {
                Application.Run(loginForm);
                if (loginForm.DialogResult == DialogResult.OK)
                {
                    Application.Run(new CustomerForm());
                }
            }
        }
    }
}
