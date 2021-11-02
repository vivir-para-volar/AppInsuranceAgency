using InsuranceAgency.Struct;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InsuranceAgency.Pages
{
    public partial class AddCar : Page
    {
        public AddCar()
        {
            InitializeComponent();
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

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string model = tbModel.Text.Trim();
                if (model == "")
                {
                    throw new Exception("Заполните поле Модель");
                }

                string vin = tbVIN.Text.Trim();
                if (vin.Length != 17)
                {
                    throw new Exception("VIN номер должен содержать 17 знаков");
                }
                foreach (var item in vin)
                {
                    if (!char.IsDigit(item) && !(Convert.ToInt32(item) >= 65 && Convert.ToInt32(item) <= 90))
                    {
                        throw new Exception("VIN номер должен состоять из цифр и заглавных латинских букв");
                    }
                }

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
                for(var i = 0; i < vehiclePassportSeries.Length; i++)
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

                Car car = new Car(model, vin, registrationPlate, vehiclePassport, image);

                Database.AddCar(car);

                MessageBox.Show("Автомобиль успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);

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
