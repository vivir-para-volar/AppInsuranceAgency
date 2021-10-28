using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InsuranceAgency
{
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();

            //Database.Authorization("1", "1");

            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void BtnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void BtnMinimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void TbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbLogin.Text.Length == 0)
            {
                TbLoginHint.Visibility = Visibility.Visible;
            }
            else
            {
                TbLoginHint.Visibility = Visibility.Hidden;
            }
        }

        private void TbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TbPassword.Password.Length == 0)
            {
                TbPasswordHint.Visibility = Visibility.Visible;
            }
            else
            {
                TbPasswordHint.Visibility = Visibility.Hidden;
            }
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = TbLogin.Text.Trim();
            string password = TbPassword.Password.Trim();

            try
            {
                Database.Authorization(login, password);

                this.Hide();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch(Exception ex)
            {
                TbException.Text = ex.Message;
            }
        }
    }
}
