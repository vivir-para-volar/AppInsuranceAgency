using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class ChangePolicy : Page
    {
        int PolicyhilderID;
        List<PersonAllowedToDrive> listPerson = new List<PersonAllowedToDrive>();

        public ChangePolicy(int policyhilderID, int policyID)
        {
            InitializeComponent();

            PolicyhilderID = policyhilderID;

            Struct.Policy policy = Database.SearchPolicyID(policyID);
            AddInfoInTb(policy);
        }

        private void AddInfoInTb(Struct.Policy policy)
        {
            tbInsuranceType.Text = policy.InsuranceType;
            tbInsurancePremium.Text = policy.InsurancePremium.ToString();
            tbInsuranceAmount.Text = policy.InsuranceAmount.ToString();
            dpDateOfConclusion.SelectedDate = policy.DateOfConclusion;
            dpExpirationDate.SelectedDate = policy.ExpirationDate;
            tbVIN.Text = Database.SearchCarID(policy.CarID).VIN;
            tbEmployee.Text = Database.SearchEmployeeID(policy.EmployeeID).FullName;

            List<Connection> connections = Database.SearchConnection(policy.ID);
            foreach (var item in connections)
            {
                PersonAllowedToDrive personAllowedToDrive = Database.SearchPersonAllowedToDriveID(item.PersonAllowedToDriveID);
                listPerson.Add(personAllowedToDrive);
                cbPersonsAllowedToDrive.Items.Add(personAllowedToDrive.FullName);
            }
            cbPersonsAllowedToDrive.Text = "";
            tbPersonsAllowedToDriveHint.Visibility = Visibility.Visible;
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

        private void btnChangePolicy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string insuranceType = tbInsuranceType.Text;

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

                DateTime expirationDate = Convert.ToDateTime(dpExpirationDate.Text); 

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
                int carID = car.ID;

                int employeeID = Database.SearchEmployeeLogin(Database.Login).ID;

                Struct.Policy policy = new Struct.Policy(insuranceType, insurancePremium, insuranceAmount, dateOfConclusion, expirationDate, PolicyhilderID, carID, employeeID);


                //Database.AddPolicyWithConnections(policy, listPerson);

                MessageBox.Show("Полис успешно изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);

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
                if (listPerson.Contains(personAllowedToDrive))
                {
                    throw new Exception("Данный водитель уже добавлен");
                }
                listPerson.Add(personAllowedToDrive);
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

        private void btnDeletePersonAllowedToDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = cbPersonsAllowedToDrive.SelectedIndex;
                try
                {
                    if (cbPersonsAllowedToDrive.Text == listPerson[index].FullName)
                    {
                        listPerson.RemoveAt(index);
                    }
                    else
                    {
                        throw new Exception("Данный водитель не существует в списке добавленных водителей");
                    }
                }
                catch { throw new Exception("Данный водитель не существует в списке добавленных водителей"); }
                cbPersonsAllowedToDrive.Items.Clear();
                foreach (var item in listPerson)
                {
                    cbPersonsAllowedToDrive.Items.Add(item.FullName);
                }

                MessageBox.Show("Водитель удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                cbPersonsAllowedToDrive.Text = "";
                tbPersonsAllowedToDriveHint.Visibility = Visibility.Visible;
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.Policy(PolicyhilderID));
        }
    }
}
