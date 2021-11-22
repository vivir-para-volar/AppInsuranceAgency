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

        public СhangeEmployee(Employee employee)
        {
            InitializeComponent();

            searchEmployee = employee;
            AddInfoInTb(employee);

            flagSearchEmployee = true;
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

        bool flagSearchEmployee = false;
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

                AddInfoInTb(searchEmployee);

                tbSearch.Text = "";

                flagSearchEmployee = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddInfoInTb(Employee employee)
        {
            tbFullName.Text = employee.FullName;
            tbBirthday.Text = employee.Birthday.ToString("d");
            tbTelephone.Text = employee.Telephone;
            tbPassportSeries.Text = "";
            tbPassportNumber.Text = "";
            for (var i = 0; i < 4; i++) tbPassportSeries.Text += employee.Passport[i];
            for (var i = 4; i < 10; i++) tbPassportNumber.Text += employee.Passport[i];
            tbLogin.Text = employee.Login;
            tbPassword.Text = "";
            if (employee.Admin == false)
            {
                cbAdmin.SelectedIndex = 0;
            }
            else
            {
                cbAdmin.SelectedIndex = 1;
            }
            if (employee.Works == true)
            {
                cbWorks.SelectedIndex = 0;
            }
            else
            {
                cbWorks.SelectedIndex = 1;
            }
        }

        private void btnChangeEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!flagSearchEmployee) throw new Exception("Выберите сотрудника");

                string fullName = tbFullName.Text.Trim();
                if (fullName.Trim() == "")
                {
                    throw new Exception("Заполните поле ФИО");
                }

                string telephone = tbTelephone.Text.Trim();
                if (telephone.Trim() == "")
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
                bool changePassword = false;
                if (tbPassword.Text.Trim() == "")
                {
                    password = searchEmployee.Password;
                }
                else
                {
                    changePassword = true;
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

                bool works;
                if (cbWorks.Text == "")
                {
                    throw new Exception("Заполните поле Работает ли");
                }
                else if (cbWorks.Text == "Не работает")
                {
                    works = false;
                }
                else
                {
                    works = true;
                }

                Employee employee = new Employee(searchEmployee.ID, fullName, searchEmployee.Birthday, telephone, passport, login, password, admin, works);

                Database.ChangeEmployee(employee, changePassword);

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
                flagSearchEmployee = false;

                if(searchEmployee.Login == Database.Login)
                {
                    MessageBox.Show("Перезагрузите программу", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
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
                if (!flagSearchEmployee) throw new Exception("Выберите сотрудника");

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите удалить данного сотрудника", "Удаление",
                                                                                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                                                                                System.Windows.Forms.MessageBoxIcon.Information,
                                                                                                System.Windows.Forms.MessageBoxDefaultButton.Button1,
                                                                                                System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                if (result == System.Windows.Forms.DialogResult.Yes)
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
                    flagSearchEmployee = false;

                    if (searchEmployee.Login == Database.Login)
                    {
                        MessageBox.Show("Перезагрузите программу", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }
    }
}
