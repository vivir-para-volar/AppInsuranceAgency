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

                (int CountContracts, int SumContracts, int SumInsuranceEvents) tuple = Database.Reports(insuranceType, dateStart, dateEnd);

                tbCountContracts.Text = tuple.CountContracts.ToString();
                tbSumContracts.Text = tuple.SumContracts.ToString();
                tbSumInsuranceEvents.Text = tuple.SumInsuranceEvents.ToString();
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }
    }
}

//query = "SELECT COUNT(p.ID) as CountContracts, " +
//                "SUM(p.InsurancePremium) as SumContracts, " +
//                "SUM(ie.InsurancePayment) as SumInsuranceEvents " +
//        "FROM Policies as p LEFT JOIN InsuranceEvents as ie ON p.ID = ie.PolicyID " +
//        "WHERE InsuranceType = @insuranceType AND DateOfConclusion >= @dateStart AND DateOfConclusion <= @dateEnd ";

//con.Open();
//int countContract = Convert.ToInt32(command.ExecuteScalar());

//SqlDataReader reader = command.ExecuteReader();

//int countContracts = 0, sumContracts = 0, sumInsuranceEvents = 0;
//while (reader.Read())
//{
//    countContracts = Convert.ToInt32(reader.GetValue(0));
//    sumContracts = Convert.ToInt32(reader.GetValue(1));
//    sumInsuranceEvents = Convert.ToInt32(reader.GetValue(2).ToString() ?? "0");
//}
//reader.Close();
//int i = 0;
//var touple = (countContracts, sumContracts, sumInsuranceEvents);
//con.Close();
//return touple;
