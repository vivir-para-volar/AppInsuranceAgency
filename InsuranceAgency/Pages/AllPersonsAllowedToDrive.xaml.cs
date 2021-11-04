using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class AllPersonsAllowedToDrive : Page
    {
        List<PersonAllowedToDrive> list = Database.AllPersonsAllowedToDrive();

        public AllPersonsAllowedToDrive()
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

                PersonAllowedToDrive searchPersonAllowedToDrive = Database.SearchPersonAllowedToDrive(search);

                DataTable dt = DTColumn();
                AddRow(dt, searchPersonAllowedToDrive);

                DataView view = new DataView(dt);
                dgPersonsAllowedToDrive.ItemsSource = view;

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
            dgPersonsAllowedToDrive.ItemsSource = view;
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
            column.ColumnName = "FullName";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DrivingLicence";
            dt.Columns.Add(column);

            return dt;
        }

        private void AddRow(DataTable dt, PersonAllowedToDrive personAllowedToDrive)
        {
            DataRow row;
            row = dt.NewRow();
            row["ID"] = personAllowedToDrive.ID;
            row["FullName"] = personAllowedToDrive.FullName;
            row["DrivingLicence"] = personAllowedToDrive.DrivingLicence.Insert(4, " ");
            dt.Rows.Add(row);
        }

        private void btnTable_Click(object sender, RoutedEventArgs e)
        {
            AddDataInDG();
        }

        private void PersonsAllowedToDrive_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            int row = dg.Items.IndexOf(dg.CurrentCell.Item);

            dg.SelectedIndex = row;
            var selectedRow = (DataRowView)dg.SelectedItem;

            string ID = selectedRow["ID"].ToString();
            foreach (var item in list)
            {
                if (item.ID == ID)
                {
                    this.NavigationService.Navigate(new Pages.СhangePersonAllowedToDrive(item));
                }
            }
        }
    }
}
