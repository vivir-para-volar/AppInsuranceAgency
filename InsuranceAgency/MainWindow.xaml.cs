using InsuranceAgency.Pages;
using System.Windows;
using System.Windows.Input;

namespace InsuranceAgency
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TbToolBarLogin.Text = Database.Login;
            MainWindowFrame.Content = new Policy();
        }


        private void BtnMinimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximized_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void BtnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        
    }
}
