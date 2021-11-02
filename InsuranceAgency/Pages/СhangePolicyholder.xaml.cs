using InsuranceAgency.Struct;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class СhangePolicyholder : Page
    {
        public СhangePolicyholder()
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

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbSearch.Text.Length == 0)
            {
                tbSearchHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbSearchHint.Visibility = Visibility.Hidden;
            }
        }

        Policyholder searchPolicyholder;

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string search_temp = tbSearch.Text.Trim();
                string search = "";
                if (search_temp == "")
                {
                    throw new Exception("Строка поиска пуста");
                }
                foreach (var item in search_temp)
                {
                    if (char.IsDigit(item))
                    {
                        search += item;
                    }
                }

                searchPolicyholder = Database.SearchPolicyholder(search);

                tbFullName.Text = searchPolicyholder.FullName;
                tbBirthday.Text = searchPolicyholder.Birthday.ToString("d");
                tbTelephone.Text = searchPolicyholder.Telephone;
                tbPassportSeries.Text = "";
                tbPassportNumber.Text = "";
                for (var i = 0; i < 4; i++) tbPassportSeries.Text += searchPolicyholder.Passport[i];
                for (var i = 4; i < 10; i++) tbPassportNumber.Text += searchPolicyholder.Passport[i];

                tbSearch.Text = "";
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnChangePolicyholder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullName = tbFullName.Text.Trim();
                if (fullName == "")
                {
                    throw new Exception("Заполните поле ФИО");
                }

                string telephone = tbTelephone.Text.Trim();
                if (telephone == "")
                {
                    throw new Exception("Заполните поле Номер телефона");
                }
                if (telephone.Length > 15)
                {
                    throw new Exception("Номер телефона не может быть больше 15 символов");
                }
                foreach (var item in telephone)
                {
                    if (!char.IsDigit(item))
                    {
                        throw new Exception("Номер телефона должен содержать только цифры");
                    }
                }

                string passportSeries = tbPassportSeries.Text.Trim();
                if (passportSeries.Length != 4)
                {
                    throw new Exception("Серия паспорта должна содержать 4 цифры");
                }
                string passportNumber = tbPassportNumber.Text.Trim();
                if (passportNumber.Length != 6)
                {
                    throw new Exception("Номер паспорта должен содержать 6 цифр");
                }
                string passport = passportSeries + passportNumber;
                foreach (var item in passport)
                {
                    if (!char.IsDigit(item))
                    {
                        throw new Exception("Серия и номер паспорта должны содержать только цифры");
                    }
                }


                Policyholder policyholder = new Policyholder(searchPolicyholder.ID, fullName, searchPolicyholder.Birthday, telephone, passport);

                Database.ChangePolicyholder(policyholder);

                MessageBox.Show("Страхователь успешно изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbFullName.Clear();
                tbBirthday.Clear();
                tbTelephone.Clear();
                tbPassportSeries.Clear();
                tbPassportNumber.Clear();
                
                tbException.Visibility = Visibility.Hidden;
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnDeletePolicyholder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database.DeletePolicyholder(searchPolicyholder.ID);

                MessageBox.Show("Страхователь успешно удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbFullName.Clear();
                tbBirthday.Clear();
                tbTelephone.Clear();
                tbPassportSeries.Clear();
                tbPassportNumber.Clear();
                
                tbException.Visibility = Visibility.Hidden;
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }
    }
}
