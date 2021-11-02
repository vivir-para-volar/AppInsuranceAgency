using InsuranceAgency.Struct;
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
            try
            {
                string fullName = tbFullName.Text.Trim();
                if(fullName == "")
                {
                    throw new Exception("Заполните поле ФИО");
                }

                DateTime birthday = Convert.ToDateTime(dpBirthday.Text);

                string telephone_temp = tbTelephone.Text.Trim();
                string telephone = "";
                if (telephone_temp == "")
                {
                    throw new Exception("Заполните поле Номер телефона");
                }
                foreach(var item in telephone_temp)
                {
                    if (char.IsDigit(item))
                    {
                        telephone += item;
                    }
                }
                if (telephone.Length > 15)
                {
                    throw new Exception("Номер телефона не может быть больше 15 символов");
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

                Employee employee = new Employee(fullName, birthday, telephone, passport, login, password, admin, true);

                Database.AddEmployee(employee);

                MessageBox.Show("Сотрудник успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbFullName.Clear();
                dpBirthday.Text = "01.01.1990";
                tbTelephone.Clear();
                tbPassportSeries.Clear();
                tbPassportNumber.Clear();
                tbLogin.Clear();
                tbPassword.Clear();
                cbAdmin.Text = "";
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
