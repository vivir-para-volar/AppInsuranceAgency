using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class СhangePersonAllowedToDrive : Page
    {
        public СhangePersonAllowedToDrive()
        {
            InitializeComponent();
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

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


        private void btnChangePersonAllowedToDrive_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeletePersonAllowedToDrive_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
