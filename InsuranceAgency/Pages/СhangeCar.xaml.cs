using InsuranceAgency.Struct;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InsuranceAgency.Pages
{
    public partial class СhangeCar : Page
    {
        public СhangeCar()
        {
            InitializeComponent();
        }

        Car searchCar;

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

                searchCar = Database.SearchCar(search);

                tbModel.Text = searchCar.Model;
                tbVIN.Text = searchCar.VIN;
                tbRegistrationPlate.Text = searchCar.RegistrationPlate;
                tbVehiclePassportSeries.Text = "";
                tbVehiclePassportNumber.Text = "";
                for (var i = 0; i < 4; i++) tbVehiclePassportSeries.Text += searchCar.VehiclePassport[i];
                for (var i = 4; i < 10; i++) tbVehiclePassportNumber.Text += searchCar.VehiclePassport[i];
                imgCar.Source = DBImage.Decode(searchCar.Image);

                tbSearch.Text = "";
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bi = new BitmapImage(new Uri(openFileDialog.FileName));
                imgCar.Source = bi;
            }
        }

        private void btnChangeCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string registrationPlate = tbRegistrationPlate.Text.Trim();
                if (registrationPlate == "")
                {
                    throw new Exception("Заполните поле Регистрационный знак");
                }

                string vehiclePassportSeries = tbVehiclePassportSeries.Text.Trim();
                if (vehiclePassportSeries.Length != 4)
                {
                    throw new Exception("Серия паспорта должна содержать 4 знака");
                }
                for (var i = 0; i < vehiclePassportSeries.Length; i++)
                {
                    if ((i == 0 || i == 1) && !char.IsDigit(vehiclePassportSeries[i]))
                    {
                        throw new Exception("Первые два символа серии паспорта ТС должны быть цифрами");
                    }
                    if ((i == 3 || i == 4) && !char.IsLetter(vehiclePassportSeries[i]))
                    {
                        throw new Exception("Последние два символа серии паспорта ТС должны быть буквами");
                    }
                }
                string vehiclePassportNumber = tbVehiclePassportNumber.Text.Trim();
                if (vehiclePassportNumber.Length != 6)
                {
                    throw new Exception("Номер паспорта ТС должен содержать 6 цифр");
                }
                foreach (var item in vehiclePassportNumber)
                {
                    if (!char.IsDigit(item))
                    {
                        throw new Exception("Номер паспорта ТС должен содержать только цифры");
                    }
                }
                string vehiclePassport = vehiclePassportSeries + vehiclePassportNumber;

                BitmapImage bi_image = imgCar.Source as BitmapImage;
                string image = DBImage.Encode(bi_image);

                Car car = new Car(searchCar.ID, searchCar.Model, searchCar.VIN, registrationPlate, vehiclePassport, image);

                Database.ChangeCar(car);

                MessageBox.Show("Автомобиль успешно изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbModel.Clear();
                tbVIN.Clear();
                tbRegistrationPlate.Clear();
                tbVehiclePassportSeries.Clear();
                tbVehiclePassportNumber.Clear();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/InsuranceAgency;component/Assets/Car.jpg", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                imgCar.Source = bi;
                tbException.Visibility = Visibility.Hidden;
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database.DeleteCar(searchCar.ID);

                MessageBox.Show("Автомобиль успешно удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbModel.Clear();
                tbVIN.Clear();
                tbRegistrationPlate.Clear();
                tbVehiclePassportSeries.Clear();
                tbVehiclePassportNumber.Clear();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/InsuranceAgency;component/Assets/Car.jpg", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                imgCar.Source = bi;
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
