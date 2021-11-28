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
    public partial class Reports : Page
    {
        public Reports()
        {
            InitializeComponent();

            dpDateStart.SelectedDate = DateTime.Now.AddYears(-1);
            dpDateEnd.SelectedDate = DateTime.Now;
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string insuranceType = cbInsuranceType.Text;
                if (insuranceType == "")
                {
                    throw new Exception("Заполните поле Вид страхования");
                }

                DateTime dateStart = Convert.ToDateTime(dpDateStart.Text);
                DateTime dateEnd = Convert.ToDateTime(dpDateEnd.Text);
                if (dateStart >= dateEnd)
                {
                    throw new Exception("Дата начала не может быть больше даты окончания");
                }

                (int CountContracts, int SumContracts, int SumInsuranceEvents) tuple = Database.Reports(insuranceType, dateStart, dateEnd);

                tbCountContracts.Text = tuple.CountContracts.ToString();
                tbSumContracts.Text = tuple.SumContracts.ToString();
                tbSumInsuranceEvents.Text = tuple.SumInsuranceEvents.ToString();

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
