using System;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class AddEmployee : Page
    {
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void tbPassportSeries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPassportSeries.Text.Length == 0)
            {
                tbPassportSeriesHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbPassportSeriesHint.Visibility = Visibility.Hidden;
            }
        }

        private void tbPassportNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPassportNumber.Text.Length == 0)
            {
                tbPassportNumberHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbPassportNumberHint.Visibility = Visibility.Hidden;
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string fullName = tbFullName.Text;
            DateTime birthday = Convert.ToDateTime(dpBirthday.Text);
            string telephone = tbTelephone.Text;

            string passport = tbPassportSeries.Text + " " + tbPassportNumber.Text;
            string login = tbLogin.Text;
            string password = tbPassword.Text;

            char admin;
            if (cbAdmin.Text == "Не администратор") admin = '0';
            else admin = '1';

            //Database.AddEmployee(fullName, birthday, telephone, passport, login, password, admin);
        }
    }
}
