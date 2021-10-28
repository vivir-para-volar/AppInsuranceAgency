using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class СhangeCar : Page
    {
        public СhangeCar()
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

        private void tbVehiclePassportSeries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbVehiclePassportSeries.Text.Length == 0)
            {
                tbVehiclePassportSeriesHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbVehiclePassportSeriesHint.Visibility = Visibility.Hidden;
            }
        }

        private void tbVehiclePassportNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbVehiclePassportNumber.Text.Length == 0)
            {
                tbVehiclePassportNumberHint.Visibility = Visibility.Visible;
            }
            else
            {
                tbVehiclePassportNumberHint.Visibility = Visibility.Hidden;
            }
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChangeCar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
