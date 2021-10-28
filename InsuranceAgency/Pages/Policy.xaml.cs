using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InsuranceAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для Policy.xaml
    /// </summary>
    public partial class Policy : Page
    {
        public Policy()
        {
            InitializeComponent();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbSearch.Text.Length == 0)
            {
                TbSearchHint.Visibility = Visibility.Visible;
            }
            else
            {
                TbSearchHint.Visibility = Visibility.Hidden;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
