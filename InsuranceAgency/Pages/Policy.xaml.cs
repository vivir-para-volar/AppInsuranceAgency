using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class Policy : Page
    {
        public Policy()
        {
            InitializeComponent();
        }

        public Policy(int policyhilderID)
        {
            InitializeComponent();

            searchPolicyholder = Database.SearchPolicyholderID(policyhilderID);
            AddDataInDG();
            btnAddPolicy.Visibility = Visibility.Visible;
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

        Policyholder searchPolicyholder;

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string search_temp = tbSearch.Text.Trim();
                string search = "";
                if (search_temp == "")
                {
                    throw new Exception("Строка поиска пуста");
                }
                foreach (var item in search_temp)
                {
                    if (char.IsDigit(item))
                    {
                        search += item;
                    }
                }

                searchPolicyholder = Database.SearchPolicyholder(search);

                AddDataInDG();

                tbSearch.Text = "";
                btnAddPolicy.Visibility = Visibility.Visible;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDataInDG()
        {
            DataTable dt1 = DTColumnPolicyholder();
            AddRow(dt1, searchPolicyholder);
            DataView view1 = new DataView(dt1);
            dgPolicyholders.ItemsSource = view1;

            List<Struct.Policy> listPolicy = Database.SearchPolicy(searchPolicyholder.ID);


            DataTable dt = DTColumn();
            foreach (var item in listPolicy)
            {
                AddRow(dt, item);
            }

            DataView view = new DataView(dt);
            dgPolicies.ItemsSource = view;
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
            column.ColumnName = "InsuranceType";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "InsurancePremium";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "InsuranceAmount";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DateOfConclusion";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ExpirationDate";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "VIN";
            dt.Columns.Add(column);

            return dt;
        }

        private void AddRow(DataTable dt, Struct.Policy policy)
        {
            DataRow row;
            row = dt.NewRow();
            row["ID"] = policy.ID;
            row["InsuranceType"] = policy.InsuranceType;
            row["InsurancePremium"] = policy.InsurancePremium.ToString();
            row["InsuranceAmount"] = policy.InsuranceAmount.ToString();
            row["DateOfConclusion"] = policy.DateOfConclusion.ToString("d");
            row["ExpirationDate"] = policy.ExpirationDate.ToString("d");
            Car car = Database.SearchCarID(policy.CarID);
            row["VIN"] = car.VIN;
            dt.Rows.Add(row);
        }

        private DataTable DTColumnPolicyholder()
        {
            DataTable dt = new DataTable();

            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ID";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "FullName";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Birthday";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Telephone";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Passport";
            dt.Columns.Add(column);

            return dt;
        }

        private void AddRow(DataTable dt, Policyholder policyholder)
        {
            DataRow row;
            row = dt.NewRow();
            row["ID"] = policyholder.ID;
            row["FullName"] = policyholder.FullName;
            row["Birthday"] = policyholder.Birthday.ToString("d");
            row["Telephone"] = policyholder.Telephone;
            row["Passport"] = policyholder.Passport.Insert(4, " ");
            dt.Rows.Add(row);
        }

        private void dgPolicies_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            int row = dg.Items.IndexOf(dg.CurrentCell.Item);

            dg.SelectedIndex = row;
            var selectedRow = (DataRowView)dg.SelectedItem;

            int ID = Convert.ToInt32(selectedRow["ID"].ToString());
            this.NavigationService.Navigate(new Pages.ChangePolicy(ID));
        }

        private void btnAddPolicy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.AddPolicy(searchPolicyholder.ID));
        }

        private void dgPolicyholders_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            int row = dg.Items.IndexOf(dg.CurrentCell.Item);

            dg.SelectedIndex = row;
            var selectedRow = (DataRowView)dg.SelectedItem;

            this.NavigationService.Navigate(new Pages.СhangePolicyholder(searchPolicyholder));
        }
    }
}
