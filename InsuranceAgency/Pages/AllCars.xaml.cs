using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class AllCars : Page
    {
        List<Car> list = Database.AllCarsDG();

        public AllCars()
        {
            InitializeComponent();
            AddDataInDG();
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
            try
            {
                string search = tbSearch.Text.Trim();
                if (search == "")
                {
                    throw new Exception("Строка поиска пуста");
                }

                Car searchCar = Database.SearchCar(search);

                DataTable dt = DTColumn();
                AddRow(dt, searchCar);

                DataView view = new DataView(dt);
                dgCars.ItemsSource = view;

                dgCars.ItemsSource = view;

                tbSearch.Text = "";
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDataInDG()
        {
            DataTable dt = DTColumn();
            foreach (var item in list)
            {
                AddRow(dt, item);
            }

            DataView view = new DataView(dt);
            dgCars.ItemsSource = view;
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
            column.ColumnName = "Model";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "VIN";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "RegistrationPlate";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "VehiclePassport";
            dt.Columns.Add(column);

            return dt;
        }

        private void AddRow(DataTable dt, Car car)
        {
            DataRow row;
            row = dt.NewRow();
            row["ID"] = car.ID;
            row["Model"] = car.Model;
            row["VIN"] = car.VIN;
            row["RegistrationPlate"] = car.RegistrationPlate;
            row["VehiclePassport"] = car.VehiclePassport.Insert(4, " ");
            dt.Rows.Add(row);
        }

        private void btnTable_Click(object sender, RoutedEventArgs e)
        {
            AddDataInDG();
        }

        private void dgCars_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var row = dg.Items.IndexOf(dg.CurrentCell.Item);

            dg.SelectedIndex = row;
            var selectedRow = (DataRowView)dg.SelectedItem;

            int ID = Convert.ToInt32(selectedRow["ID"].ToString());
            this.NavigationService.Navigate(new Pages.СhangeCar(ID));
        }
    }
}
