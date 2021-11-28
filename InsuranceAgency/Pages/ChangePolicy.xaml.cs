using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class ChangePolicy : Page
    {
        Struct.Policy Policy;

        List<PersonAllowedToDrive> listPersons = new List<PersonAllowedToDrive>();
        List<PersonAllowedToDrive> listNewPersons = new List<PersonAllowedToDrive>();
        List<PersonAllowedToDrive> listDeletePersons = new List<PersonAllowedToDrive>();
        List<PersonAllowedToDrive> listAddPersons = new List<PersonAllowedToDrive>();

        public ChangePolicy(int policyID)
        {
            InitializeComponent();

            Policy = Database.SearchPolicyID(policyID);
            AddInfoInTb(Policy);
        }

        private void AddInfoInTb(Struct.Policy policy)
        {
            tbInsuranceType.Text = policy.InsuranceType;
            tbInsurancePremium.Text = policy.InsurancePremium.ToString();
            tbInsuranceAmount.Text = policy.InsuranceAmount.ToString();
            tbDateOfConclusion.Text = policy.DateOfConclusion.ToString("d");
            dpExpirationDate.SelectedDate = policy.ExpirationDate;
            tbVIN.Text = Database.SearchCarID(policy.CarID).VIN;
            tbEmployee.Text = Database.SearchEmployeeID(policy.EmployeeID).FullName;

            List<Connection> connections = Database.SearchConnection(policy.ID);
            foreach (var item in connections)
            {
                PersonAllowedToDrive personAllowedToDrive = Database.SearchPersonAllowedToDriveID(item.PersonAllowedToDriveID);
                listPersons.Add(personAllowedToDrive);
                listNewPersons.Add(personAllowedToDrive);
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
                string insurancePremium_temp = tbInsurancePremium.Text.Trim();
                if (insurancePremium_temp == "")
                {
                    throw new Exception("Заполните поле Страховая премия");
                }
                int insurancePremium = 0;
                try { insurancePremium = Convert.ToInt32(insurancePremium_temp); }
                catch { throw new Exception("Страховая премия должна быть целым числом"); }

                DateTime expirationDate = Convert.ToDateTime(dpExpirationDate.Text); 
                if(expirationDate < Policy.DateOfConclusion)
                {
                    throw new Exception("Дата окончания действия не может быть меньше даты заключения");
                }
                DateTime maxDate = Database.SearchInsuranceEventMaxDate(Policy.ID);
                if (maxDate != DateTime.MinValue)
                {
                    if(expirationDate < maxDate)
                    {
                        throw new Exception("Дата окончания действия не может быть меньше даты последнего страхового случая");
                    }
                }

                Struct.Policy policyChange = new Struct.Policy(Policy.ID, Policy.InsuranceType, insurancePremium, Policy.InsuranceAmount, Policy.DateOfConclusion, expirationDate, Policy.PolicyholderID, Policy.CarID, Policy.EmployeeID);

                if (listNewPersons.Count == 0)
                {
                    throw new Exception("Список лиц, допущенных к управлению пуст");
                }

                Database.ChangePolicyWithConnections(policyChange, listDeletePersons, listAddPersons);
                MessageBox.Show("Полис успешно изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                this.NavigationService.Navigate(new Pages.Policy(Policy.PolicyholderID));
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
                PersonAllowedToDrive personAllowedToDrive = Database.SearchPersonAllowedToDrive(cbPersonsAllowedToDrive.Text.Trim());
                foreach (var item in listNewPersons)
                {
                    if (item.ID == personAllowedToDrive.ID)
                    {
                        throw new Exception("Данный водитель уже добавлен");
                    }
                }
                listNewPersons.Add(personAllowedToDrive);
                listAddPersons.Add(personAllowedToDrive);
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
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите удалить данного водителя", "Удаление",
                                                                                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                                                                                System.Windows.Forms.MessageBoxIcon.Information,
                                                                                                System.Windows.Forms.MessageBoxDefaultButton.Button1,
                                                                                                System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    int index = cbPersonsAllowedToDrive.SelectedIndex;

                    try
                    {
                        if (cbPersonsAllowedToDrive.Text == listNewPersons[index].FullName)
                        {
                            for (var i = 0; i < listPersons.Count; i++)
                            {
                                if (listNewPersons[index] == listPersons[i])
                                {
                                    listDeletePersons.Add(listPersons[i]);
                                }
                            }
                            for (var i = 0; i < listAddPersons.Count; i++)
                            {
                                if (listNewPersons[index] == listAddPersons[i])
                                {
                                    listAddPersons.RemoveAt(i);
                                }
                            }

                            listNewPersons.RemoveAt(index);
                        }
                        else
                        {
                            throw new Exception("Данный водитель не существует в списке добавленных водителей");
                        }
                    }
                    catch { throw new Exception("Данный водитель не существует в списке добавленных водителей"); }

                    cbPersonsAllowedToDrive.Items.Clear();
                    foreach (var item in listNewPersons)
                    {
                        cbPersonsAllowedToDrive.Items.Add(item.FullName);
                    }

                    MessageBox.Show("Водитель удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    cbPersonsAllowedToDrive.Text = "";
                    tbPersonsAllowedToDriveHint.Visibility = Visibility.Visible;
                }
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.Policy(Policy.PolicyholderID));
        }

        private void btnInsuranceEvents_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.InsuranceEvents(Policy));
        }

        private void btnExtendPolicy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.AddPolicy(Policy));
        }
    }
}
