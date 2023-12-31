﻿using Business.ImageProcessing;
using ImageProcessing.Constants;
using ImageProcessing.Enums;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private static BitmapImage image;
        private static Methods method;
        private static string drugName;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnFilePickerClicked(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";

                if (op.ShowDialog() == true)
                {
                    image = new BitmapImage(new Uri(op.FileName));
                    imageCrystal.Source = image;

                    string[] path = image.UriSource.LocalPath.Split('\\');
                    drugName = path[path.Length - 1].Split('.')[0];
                }

                imageCrystal.Height = 400;
                imageCrystal.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Изображение не найдено", "Загрузка изображения не удалась", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnFileRemovePickerClicked(object sender, EventArgs e)
        {
            image = null;
            imageCrystal.Source = null;
            imageCrystal.Visibility = Visibility.Collapsed;
        }

        private void OnFileSavePickerClicked(object sender, EventArgs e)
        {
            if (image is null)
            {
                MessageBox.Show("Изображение не найдено", "Сохранение изображения не удалась", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            string home = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

            using MemoryStream outStream = new MemoryStream();

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(image));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);
            bitmap.Save(Path.Combine(home, "crystal.jpg"));

            MessageBox.Show($"Данные сохранились успешно\nПуть к файлу: {home}\\crystal.jpg", "Сохранение изображения", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnMakeMonochromeClicked(object sender, EventArgs e)
        {
            if (image is null)
            {
                MessageBox.Show("Изображение не найдено", "Необходимо загрузить изображение", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            using MemoryStream outStream = new MemoryStream();

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(image));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            Monochrome monochrome = new Monochrome();
            bitmap = monochrome.MakeMonochrome(bitmap);

            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            using var memory = new MemoryStream();

            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            image = bitmapImage;
            imageCrystal.Source = bitmapImage;
        }

        private void OnMethodClicked(object sender, EventArgs e)
        {
            if (image is null)
            {
                MessageBox.Show("Вы не выбрали изображения для исследования", "Изображение не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MainWindow.Main.Content = new ParametersPage(image, method, drugName);
        }

        private void OnMinkowskiMethodCheckedChanged(object sender, EventArgs e)
        {
            method = Methods.Minkowski;
        }

        private void OnDensityMethodCheckedChanged(object sender, EventArgs e)
        {
            method = Methods.Density;
        }

        private void OnRenyiMethodCheckedChanged(object sender, EventArgs e)
        {
            method = Methods.Renyi;
        }

        private void GetInformationClicked(object sender, EventArgs e)
        {
            MessageBox.Show(ApplicationConstants.Version, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnHelpClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.Content = new HelpPage();
        }
    }
}
