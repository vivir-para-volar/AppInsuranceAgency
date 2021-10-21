using System.Windows;

namespace InsuranceAgency
{
    static class WindowExtension
    {
        public static void ShowPlusLogin(this Window window)
        {
            if (window is MainWindow)
            {
                ((MainWindow)window).TbToolBarLogin.Text = Database.Login;
                window.Show();
            }
        }
    }
}
