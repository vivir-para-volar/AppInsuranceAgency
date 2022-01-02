using InsuranceAgency.Struct;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InsuranceAgency.Pages
{
    public partial class СhangeCar : Page
    {
        List<Photo> listPhotos = new List<Photo>();
        List<string> listEncodedPhotos = new List<string>();

        List<BitmapImage> listNewPhotos = new List<BitmapImage>();
        List<string> listNewEncodedPhotos = new List<string>();

        List<Photo> listDeletePhotos = new List<Photo>();
        List<string> listAddPhotos = new List<string>();
        
        int currentIndex = 0;

        public СhangeCar()
        {
            InitializeComponent();
        }

        public СhangeCar(int id)
        {
            InitializeComponent();

            Car car = Database.SearchCarID(id);
            searchCar = car;

            listPhotos = Database.SearchPhoto(car.ID);
            foreach(var item in listPhotos)
            {
                listNewEncodedPhotos.Add(item.EncodedPhoto);
                listEncodedPhotos.Add(item.EncodedPhoto);
                listNewPhotos.Add(DBImage.Decode(item.EncodedPhoto));
            }

            AddInfoInTb(car);
            flagSearchCar = true;
        }

        bool flagSearchCar = false;
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
            tbException.Visibility = Visibility.Hidden;
            try
            {
                string search = tbSearch.Text.Trim();
                if (search == "")
                {
                    throw new Exception("Строка поиска пуста");
                }

                searchCar = Database.SearchCar(search);

                listPhotos = Database.SearchPhoto(searchCar.ID);
                foreach (var item in listPhotos)
                {
                    listNewEncodedPhotos.Add(item.EncodedPhoto);
                    listEncodedPhotos.Add(item.EncodedPhoto);
                    listNewPhotos.Add(DBImage.Decode(item.EncodedPhoto));
                }

                AddInfoInTb(searchCar);

                tbSearch.Text = "";

                flagSearchCar = true;
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddInfoInTb(Car car)
        {
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbRegistrationPlate.Text = car.RegistrationPlate;
            tbVehiclePassportSeries.Text = "";
            tbVehiclePassportNumber.Text = "";
            for (var i = 0; i < 4; i++) tbVehiclePassportSeries.Text += car.VehiclePassport[i];
            for (var i = 4; i < 10; i++) tbVehiclePassportNumber.Text += car.VehiclePassport[i];

            imgCar.Source = listNewPhotos[0];
            currentIndex = 0;
            if (listNewPhotos.Count - 1 > 0)
            {
                btnLeft.Visibility = Visibility.Visible;
                btnRight.Visibility = Visibility.Visible;
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
            openFileDialog.Filter = "All Embroidery Files | *.bmp; *.gif; *.jpeg; *.jpg; " +
                                    "*.fif;*.fiff;*.png;*.wmf;*.emf" +
                                    "|Windows Bitmap (*.bmp)|*.bmp" +
                                    "|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg" +
                                    "|Graphics Interchange Format (*.gif)|*.gif" +
                                    "|Portable Network Graphics (*.png)|*.png" +
                                    "|Tag Embroidery File Format (*.tif)|*.tif;*.tiff";
            if (openFileDialog.ShowDialog() == true)
            {
                var size = new FileInfo(openFileDialog.FileName).Length;
                if (size / 1024 > 1024)
                {
                    tbException.Visibility = Visibility.Visible;
                    tbException.Text = "Размер фото не может быть больше 1Мб";
                }
                else
                {
                    BitmapImage bi = new BitmapImage(new Uri(openFileDialog.FileName));
                    imgCar.Source = bi;
                    listNewPhotos.Add(bi);
                    currentIndex = listNewPhotos.Count - 1;
                    if (currentIndex == 1)
                    {
                        btnLeft.Visibility = Visibility.Visible;
                        btnRight.Visibility = Visibility.Visible;
                    }

                    string image = DBImage.Encode(bi);
                    listNewEncodedPhotos.Add(image);
                    listAddPhotos.Add(image);
                }
            }
        }

        private void btnChangeCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!flagSearchCar) throw new Exception("Выберите автомобиль");

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

                if (listNewPhotos.Count == 0)
                {
                    throw new Exception("Добавьте фотографию автомобиля");
                }

                Car car = new Car(searchCar.ID, searchCar.Model, searchCar.VIN, registrationPlate, vehiclePassport);

                Database.ChangeCarWithPhotos(car, listDeletePhotos, listAddPhotos);

                MessageBox.Show("Автомобиль успешно изменён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                tbModel.Clear();
                tbVIN.Clear();
                tbRegistrationPlate.Clear();
                tbVehiclePassportSeries.Clear();
                tbVehiclePassportNumber.Clear();

                listPhotos.Clear();
                listEncodedPhotos.Clear();
                listNewPhotos.Clear();
                listNewEncodedPhotos.Clear();
                listDeletePhotos.Clear();
                listAddPhotos.Clear();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/InsuranceAgency;component/Assets/Car.jpg", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                imgCar.Source = bi;
                btnLeft.Visibility = Visibility.Hidden;
                btnRight.Visibility = Visibility.Hidden;

                tbException.Visibility = Visibility.Hidden;
                flagSearchCar = false;
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
                if (!flagSearchCar) throw new Exception("Выберите автомобиль");

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите удалить данный автомобиль", "Удаление",
                                                                                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                                                                                System.Windows.Forms.MessageBoxIcon.Information,
                                                                                                System.Windows.Forms.MessageBoxDefaultButton.Button1,
                                                                                                System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Database.DeleteCar(searchCar.ID);

                    MessageBox.Show("Автомобиль успешно удалён", "", MessageBoxButton.OK, MessageBoxImage.Information);

                    tbModel.Clear();
                    tbVIN.Clear();
                    tbRegistrationPlate.Clear();
                    tbVehiclePassportSeries.Clear();
                    tbVehiclePassportNumber.Clear();

                    listPhotos.Clear();
                    listEncodedPhotos.Clear();
                    listNewPhotos.Clear();
                    listNewEncodedPhotos.Clear();
                    listDeletePhotos.Clear();
                    listAddPhotos.Clear();
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/InsuranceAgency;component/Assets/Car.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();
                    imgCar.Source = bi;
                    btnLeft.Visibility = Visibility.Hidden;
                    btnRight.Visibility = Visibility.Hidden;

                    tbException.Visibility = Visibility.Hidden;
                    flagSearchCar = false;
                }
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex == 0)
            {
                currentIndex = listNewPhotos.Count - 1;
            }
            else
            {
                currentIndex--;
            }

            imgCar.Source = listNewPhotos[currentIndex];
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex == listNewPhotos.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            imgCar.Source = listNewPhotos[currentIndex];
        }

        private void btnDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listNewPhotos.Count == 0)
                {
                    throw new Exception("Добавьте фотографию автомобиля");
                }

                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите удалить данную фотографию", "Удаление",
                                                                                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                                                                                System.Windows.Forms.MessageBoxIcon.Information,
                                                                                                System.Windows.Forms.MessageBoxDefaultButton.Button1,
                                                                                                System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    for (var i = 0; i < listEncodedPhotos.Count; i++)
                    {
                        if (listNewEncodedPhotos[currentIndex] == listEncodedPhotos[i])
                        {
                            listDeletePhotos.Add(listPhotos[i]);
                        }
                    }
                    for (var i = 0; i < listAddPhotos.Count; i++)
                    {
                        if (listNewEncodedPhotos[currentIndex] == listAddPhotos[i])
                        {
                            listAddPhotos.RemoveAt(i);
                        }
                    }
                    listNewPhotos.RemoveAt(currentIndex);
                    listNewEncodedPhotos.RemoveAt(currentIndex);

                    currentIndex = 0;

                    if (listNewPhotos.Count == 1)
                    {
                        currentIndex = 0;
                        btnLeft.Visibility = Visibility.Hidden;
                        btnRight.Visibility = Visibility.Hidden;
                    }

                    if (listNewPhotos.Count == 0)
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
                        imgCar.Source = listNewPhotos[currentIndex];
                    }
                }
            }
            catch (Exception exp)
            {
                tbException.Visibility = Visibility.Visible;
                tbException.Text = exp.Message;
            }
        }
    }
}
