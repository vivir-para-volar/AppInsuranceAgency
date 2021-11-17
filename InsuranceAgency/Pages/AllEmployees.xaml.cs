using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InsuranceAgency.Pages
{
    public partial class AllEmployees : Page
    {
        List<Employee> list = Database.AllEmployees();

        public AllEmployees()
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
            tbSortHint.Visibility = Visibility.Visible;
            cbSort.Text = "";

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

                Employee searchEmployee = Database.SearchEmployee(search);

                DataTable dt = DTColumn();
                AddRow(dt, searchEmployee);

                DataView view = new DataView(dt);
                dgEmployees.ItemsSource = view;

                tbSearch.Text = "";
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbSortHint.Visibility = Visibility.Hidden;

            DataTable dt = DTColumn();
            switch ((sender as ComboBox).SelectedIndex)
            {
                //Работает и администратор
                case 0:
                    foreach (var item in list)
                    {
                        if (item.Admin && item.Works)
                        {
                            AddRow(dt, item);
                        }
                    }
                    break;
                //Работает и не администратор
                case 1:
                    foreach (var item in list)
                    {
                        if (!item.Admin && item.Works)
                        {
                            AddRow(dt, item);
                        }
                    }
                    break;
                //Работает
                case 2:
                    foreach (var item in list)
                    {
                        if (item.Works)
                        {
                            AddRow(dt, item);
                        }
                    }
                    break;
                //Не работает
                case 3:
                    foreach (var item in list)
                    {
                        if (!item.Works)
                        {
                            AddRow(dt, item);
                        }
                    }
                    break;
                //Администратор
                case 4:
                    foreach (var item in list)
                    {
                        if (item.Admin)
                        {
                            AddRow(dt, item);
                        }
                    }
                    break;
                //Не администратор
                case 5:
                    foreach (var item in list)
                    {
                        if (!item.Admin)
                        {
                            AddRow(dt, item);
                        }
                    }
                    break;
            }

            DataView view = new DataView(dt);
            dgEmployees.ItemsSource = view;
        }

        private void AddDataInDG()
        {
            DataTable dt = DTColumn();
            foreach (var item in list)
            {
                AddRow(dt, item);
            }

            DataView view = new DataView(dt);
            dgEmployees.ItemsSource = view;
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

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Login";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Password";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Admin";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Works";
            dt.Columns.Add(column);

            return dt;
        }

        private void AddRow(DataTable dt, Employee employee)
        {
            DataRow row;
            row = dt.NewRow();
            row["ID"] = employee.ID;
            row["FullName"] = employee.FullName;
            row["Birthday"] = employee.Birthday.ToString("d");
            row["Telephone"] = employee.Telephone;
            row["Passport"] = employee.Passport.Insert(4, " ");
            row["Login"] = employee.Login;
            row["Password"] = employee.Password;
            row["Admin"] = employee.Admin;
            row["Works"] = employee.Works;
            dt.Rows.Add(row);
        }

        private void btnTable_Click(object sender, RoutedEventArgs e)
        {
            tbSortHint.Visibility = Visibility.Visible;
            cbSort.Text = "";

            AddDataInDG();
        }

        private void dgEmployees_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            int row = dg.Items.IndexOf(dg.CurrentCell.Item);

            dg.SelectedIndex = row;
            var selectedRow = (DataRowView)dg.SelectedItem;

            int ID = Convert.ToInt32(selectedRow["ID"].ToString());
            foreach (var item in list)
            {
                if (item.ID == ID)
                {
                    this.NavigationService.Navigate(new Pages.СhangeEmployee(item));
                }
            }
        }
    }
}
