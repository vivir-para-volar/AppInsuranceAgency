using InsuranceAgency.Struct;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class InsuranceEvents : Page
    {
        private Struct.Policy Policy;
        DataTable dt;

        public InsuranceEvents(Struct.Policy policy)
        {
            InitializeComponent();

            Policy = policy;
            AddDataInDG();

            dpDate.SelectedDate = DateTime.Now;
        }

        private void AddDataInDG()
        {
            var list = Database.SearchInsuranceEvent(Policy.ID);

            dt = DTColumn();
            foreach (var item in list)
            {
                AddRow(dt, item);
            }

            DataView view = new DataView(dt);
            dgInsuranceEvents.ItemsSource = view;
        }

        private void btnAddInsuranceEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = Convert.ToDateTime(dpDate.Text);
                if (date < Policy.DateOfConclusion)
                {
                    throw new Exception("Дата не может быть меньше даты заключения полиса");
                }
                if (date > Policy.ExpirationDate)
                {
                    throw new Exception("Дата не может быть больше даты окончания действия полиса");
                }

                string insurancePayment_temp = tbInsurancePayment.Text.Trim();
                if (insurancePayment_temp == "")
                {
                    throw new Exception("Заполните поле Страховая выплата");
                }
                int insurancePayment = 0;
                try { insurancePayment = Convert.ToInt32(insurancePayment_temp); }
                catch { throw new Exception("Страховая выплата должна быть целым числом"); }
                if (insurancePayment > Policy.InsuranceAmount)
                {
                    throw new Exception("Страховая выплата долна быть меньше Страховой суммы");
                }


                InsuranceEvent insuranceEvent = new InsuranceEvent(date, insurancePayment, Policy.ID);
                Database.AddInsuranceEvent(insuranceEvent);

                DataRow dr = dt.NewRow();
                dr["ID"] = 1;
                dr["Date"] = date.ToString("d");
                dr["InsurancePayment"] = insurancePayment;
                dr["PolicyID"] = Policy.ID;
                dt.Rows.Add(dr);
                DataView view = new DataView(dt);
                view.Sort = "Date";
                dgInsuranceEvents.ItemsSource = view;

                MessageBox.Show("Страховой случай успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                tbException.Visibility = Visibility.Hidden;
            }
            catch(Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.ChangePolicy(Policy.ID));
        }

        private DataTable DTColumn()
        {
            DataTable dt = new DataTable();

            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ID";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Date";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "InsurancePayment";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "PolicyID";
            dt.Columns.Add(column);

            return dt;
        }

        private void AddRow(DataTable dt, InsuranceEvent insuranceEvent)
        {
            DataRow row;
            row = dt.NewRow();
            row["ID"] = insuranceEvent.ID;
            row["Date"] = insuranceEvent.Date.ToString("d");
            row["InsurancePayment"] = insuranceEvent.InsurancePayment;
            row["PolicyID"] = insuranceEvent.PolicyID;
            dt.Rows.Add(row);
        }
    }
}
