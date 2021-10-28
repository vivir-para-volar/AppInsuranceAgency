using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class AddCar : Page
    {
        public AddCar()
        {
            InitializeComponent();
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

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
