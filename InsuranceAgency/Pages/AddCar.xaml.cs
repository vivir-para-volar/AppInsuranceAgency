using InsuranceAgency.Struct;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InsuranceAgency.Pages
{
    public partial class AddCar : Page
    {
        List<BitmapImage> listPhotos = new List<BitmapImage>();
        List<string> listEncodedPhotos = new List<string>();
        int currentIndex = 0;

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
            openFileDialog.Filter = "All Embroidery Files | *.bmp; *.gif; *.jpeg; *.jpg; " +
                                    "*.fif;*.fiff;*.png;*.wmf;*.emf" +
                                    "|Windows Bitmap (*.bmp)|*.bmp" +
                                    "|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg" +
                                    "|Graphics Interchange Format (*.gif)|*.gif" +
                                    "|Portable Network Graphics (*.png)|*.png" +
                                    "|Tag Embroidery File Format (*.tif)|*.tif;*.tiff";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bi = new BitmapImage(new Uri(openFileDialog.FileName));
                imgCar.Source = bi;
                listPhotos.Add(bi);
                currentIndex = listPhotos.Count - 1;
                if(currentIndex == 1)
                {
                    btnLeft.Visibility = Visibility.Visible;
                    btnRight.Visibility = Visibility.Visible;
                }

                string image = DBImage.Encode(bi);
                listEncodedPhotos.Add(image);
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

                if (listEncodedPhotos.Count == 0)
                {
                    throw new Exception("Добавьте фотографию автомобиля");
                }

                Car car = new Car(model, vin, registrationPlate, vehiclePassport);

                Database.AddCarWithPhotos(car, listEncodedPhotos);

                MessageBox.Show("Автомобиль успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbModel.Clear();
                tbVIN.Clear();
                tbRegistrationPlate.Clear();
                tbVehiclePassportSeries.Clear();
                tbVehiclePassportNumber.Clear();

                listPhotos.Clear();
                listEncodedPhotos.Clear();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/InsuranceAgency;component/Assets/Car.jpg", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                imgCar.Source = bi;
                btnLeft.Visibility = Visibility.Hidden;
                btnRight.Visibility = Visibility.Hidden;

                tbException.Visibility = Visibility.Hidden;
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            if(currentIndex == 0)
            {
                currentIndex = listPhotos.Count - 1;
            }
            else
            {
                currentIndex--;
            }

            imgCar.Source = listPhotos[currentIndex];
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex == listPhotos.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            imgCar.Source = listPhotos[currentIndex];
        }

        private void btnDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            listPhotos.RemoveAt(currentIndex);
            listEncodedPhotos.RemoveAt(currentIndex);

            currentIndex = 0;

            if (listPhotos.Count == 1)
            {
                currentIndex = 0;
                btnLeft.Visibility = Visibility.Hidden;
                btnRight.Visibility = Visibility.Hidden;
            }

            if (listPhotos.Count == 0)
            {
                currentIndex = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/InsuranceAgency;component/Assets/Car.jpg", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                imgCar.Source = bi;
            }
            else
            {
                imgCar.Source = listPhotos[currentIndex];
            }
        }
    }
}
