using InsuranceAgency.Struct;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class СhangeEmployee : Page
    {
        public СhangeEmployee()
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

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPassword.Text.Length == 0)
            {
                tbPasswordHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbPasswordHint.Visibility = Visibility.Hidden;
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

        Employee searchEmployee;

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

                searchEmployee = Database.SearchEmployee(search);

                tbFullName.Text = searchEmployee.FullName;
                tbBirthday.Text = searchEmployee.Birthday.ToString("d");
                tbTelephone.Text = searchEmployee.Telephone;
                tbPassportSeries.Text = "";
                tbPassportNumber.Text = "";
                for (var i = 0; i < 4; i++) tbPassportSeries.Text += searchEmployee.Passport[i];
                for (var i = 4; i < 10; i++) tbPassportNumber.Text += searchEmployee.Passport[i];
                tbLogin.Text = searchEmployee.Login;
                tbPassword.Text = "";
                if (searchEmployee.Admin == false)
                {
                    cbAdmin.SelectedIndex = 0;
                }
                else
                {
                    cbAdmin.SelectedIndex = 1;
                }
                if (searchEmployee.Works == true)
                {
                    cbWorks.SelectedIndex = 0;
                }
                else
                {
                    cbWorks.SelectedIndex = 1;
                }

                tbSearch.Text = "";
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnChangeEmployee_Click(object sender, RoutedEventArgs e)
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

                string login = tbLogin.Text.Trim();
                if (login.Length < 4 || login.Length > 32)
                {
                    throw new Exception("Длина логина должна быть от 4 до 32 символов");
                }
                foreach (var item in telephone)
                {
                    if (char.IsWhiteSpace(item))
                    {
                        throw new Exception("Логин не может содержать пробелы");
                    }
                }

                string password = tbPassword.Text.Trim();
                if(tbPassword.Text == "")
                {
                    password = searchEmployee.Password;
                }
                else
                {
                    if (password.Length < 4 || password.Length > 32)
                    {
                        throw new Exception("Длина пароля должна быть от 4 до 32 символов");
                    }
                    foreach (var item in telephone)
                    {
                        if (char.IsWhiteSpace(item))
                        {
                            throw new Exception("Пароль не может содержать пробелы");
                        }
                    }
                }

                bool admin;
                if (cbAdmin.Text == "")
                {
                    throw new Exception("Заполните поле Администратор");
                }
                else if (cbAdmin.Text == "Не администратор")
                {
                    admin = false;
                }
                else
                {
                    admin = true;
                }

                Employee employee = new Employee(searchEmployee.ID, fullName, searchEmployee.Birthday, telephone, passport, login, password, admin, true);

                Database.ChangeEmployee(employee);

                MessageBox.Show("Сотрудник успешно изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbFullName.Clear();
                tbBirthday.Clear();
                tbTelephone.Clear();
                tbPassportSeries.Clear();
                tbPassportNumber.Clear();
                tbLogin.Clear();
                tbPassword.Clear();
                cbAdmin.Text = "";
                cbWorks.Text = "";
                tbException.Visibility = Visibility.Hidden;
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database.DeleteEmployee(searchEmployee.ID);

                MessageBox.Show("Сотрудник успешно удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbFullName.Clear();
                tbBirthday.Clear();
                tbTelephone.Clear();
                tbPassportSeries.Clear();
                tbPassportNumber.Clear();
                tbLogin.Clear();
                tbPassword.Clear();
                cbAdmin.Text = "";
                cbWorks.Text = "";
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
