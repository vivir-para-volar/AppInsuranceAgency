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

        }
    }
}
