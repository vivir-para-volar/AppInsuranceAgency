using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class AddPolicy : Page
    {
        string PolicyhilderID;
        List<PersonAllowedToDrive> list = new List<PersonAllowedToDrive>();

        public AddPolicy(string policyhilderID)
        {
            InitializeComponent();

            PolicyhilderID = policyhilderID;
            dpDateOfConclusion.SelectedDate = DateTime.Now;
        }
        private void cbPersonsAllowedToDrive_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPersonsAllowedToDriveHint.Visibility = Visibility.Hidden;
        }

        private void cbPersonsAllowedToDrive_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cbPersonsAllowedToDrive.Text.Length == 0)
            {
                tbPersonsAllowedToDriveHint.Visibility = Visibility.Visible;
            }
        }

        private void btnAddPolicy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string insuranceType = cbInsuranceType.Text;
                if (insuranceType == "")
                {
                    throw new Exception("Заполните поле Вид страхования");
                }

                string insuranceAmount_temp = tbInsuranceAmount.Text.Trim();
                if (insuranceAmount_temp == "")
                {
                    throw new Exception("Заполните поле Страховая сумма");
                }
                int insuranceAmount = 0;
                try { insuranceAmount = Convert.ToInt32(insuranceAmount_temp); }
                catch { throw new Exception("Страховая сумма должна быть целым числом"); }

                string insurancePremium_temp = tbInsurancePremium.Text.Trim();
                if (insurancePremium_temp == "")
                {
                    throw new Exception("Заполните поле Страховая премия");
                }
                int insurancePremium = 0;
                try { insurancePremium = Convert.ToInt32(insurancePremium_temp); }
                catch { throw new Exception("Страховая премия должна быть целым числом"); }

                DateTime dateOfConclusion = Convert.ToDateTime(dpDateOfConclusion.Text);

                DateTime expirationDate = dateOfConclusion;
                if (cbExpirationDate.Text == "")
                {
                    throw new Exception("Заполните поле Срок действия");
                }
                else if (cbExpirationDate.Text == "6 месяцев")
                {
                    expirationDate = expirationDate.AddMonths(6);
                }
                else
                {
                    expirationDate = expirationDate.AddYears(1);
                }

                string vin = tbVIN.Text.Trim();
                if (vin.Length != 17)
                {
                    throw new Exception("VIN номер должен содержать 17 знаков");
                }
                foreach (var item in vin)
                {
                    if (!char.IsDigit(item) && !(Convert.ToInt32(item) >= 65 && Convert.ToInt32(item) <= 90))
                    {
                        throw new Exception("VIN номер должен состоять из цифр и заглавных латинских букв");
                    }
                }
                Car car = Database.SearchCar(vin);
                string carID = car.ID;

                string employeeID = Database.SearchEmployeeLogin(Database.Login).ID;

                Struct.Policy policy = new Struct.Policy(insuranceType, insurancePremium, insuranceAmount, dateOfConclusion, expirationDate, PolicyhilderID, carID, employeeID);
                

                Database.AddPolicyWithConnections(policy, list);
                //foreach(var item in list)
                //{
                //    Database.AddConnection(new Connection(policyID, item.ID));
                //}

                MessageBox.Show("Полис успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);

                this.NavigationService.Navigate(new Pages.Policy(PolicyhilderID));
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnAddPersonAllowedToDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PersonAllowedToDrive personAllowedToDrive = Database.SearchPersonAllowedToDrive(cbPersonsAllowedToDrive.Text);
                if (list.Contains(personAllowedToDrive))
                {
                    throw new Exception("Данный водитель уже добавлен");
                }
                list.Add(personAllowedToDrive);
                cbPersonsAllowedToDrive.Items.Add(personAllowedToDrive.FullName);

                MessageBox.Show("Водитель добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                cbPersonsAllowedToDrive.Text = "";
                tbPersonsAllowedToDriveHint.Visibility = Visibility.Visible;
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }
    }
}
