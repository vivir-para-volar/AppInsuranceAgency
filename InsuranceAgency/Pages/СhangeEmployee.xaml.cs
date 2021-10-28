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

        private void btnChangeEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
