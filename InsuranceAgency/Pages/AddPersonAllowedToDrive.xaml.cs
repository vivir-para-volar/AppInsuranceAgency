using InsuranceAgency.Struct;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class AddPersonAllowedToDrive : Page
    {
        public AddPersonAllowedToDrive()
        {
            InitializeComponent();
        }
        private void tbDrivingLicenceSeries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDrivingLicenceSeries.Text.Length == 0)
            {
                tbDrivingLicenceSeriesHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbDrivingLicenceSeriesHint.Visibility = Visibility.Hidden;
            }
        }

        private void tbDrivingLicenceNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDrivingLicenceNumber.Text.Length == 0)
            {
                tbDrivingLicenceNumberHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbDrivingLicenceNumberHint.Visibility = Visibility.Hidden;
            }
        }

        private void btnAddPersonAllowedToDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullName = tbFullName.Text.Trim();
                if (fullName == "")
                {
                    throw new Exception("Заполните поле ФИО");
                }

                string drivingLicenceSeries = tbDrivingLicenceSeries.Text.Trim();
                if (drivingLicenceSeries.Length != 4)
                {
                    throw new Exception("Серия водительского удостоверения должна содержать 4 цифры");
                }
                string drivingLicenceNumber = tbDrivingLicenceNumber.Text.Trim();
                if (drivingLicenceNumber.Length != 6)
                {
                    throw new Exception("Номер водительского удостоверения должен содержать 6 цифр");
                }
                string drivingLicence = drivingLicenceSeries + drivingLicenceNumber;
                foreach (var item in drivingLicence)
                {
                    if (!char.IsDigit(item))
                    {
                        throw new Exception("Серия и номер водительского удостоверения должны содержать только цифры");
                    }
                }


                PersonAllowedToDrive personAllowedToDrive = new PersonAllowedToDrive(fullName, drivingLicence);

                Database.AddPersonAllowedToDrive(personAllowedToDrive);

                MessageBox.Show("Водитель успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbFullName.Clear();
                tbDrivingLicenceSeries.Clear();
                tbDrivingLicenceNumber.Clear();
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
