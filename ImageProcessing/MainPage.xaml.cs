﻿using Business.ImageProcessing;
using ImageProcessing.Constants;
using ImageProcessing.Enums;
using ImageProcessing.Windows;
using Microsoft.Win32;
using RemoveBackGround;
using Sharpness;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private static BitmapImage image;
        private static BitmapImage image0;
        private static BitmapImage previousImage;
        private static Methods method;
        private static string drugName;
        private static string drugName0;

        public MainPage()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(179, 255, 255));
        }

        private void OnFilePickerClicked(object sender, EventArgs e)
        {
            if (image0 != null)
            {
                MessageBox.Show("Ошибка при добавлении изображения", "Достигнуто максимальное количество изображений", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (image is not null)
            {
                const string message =
                    "Добавить второе изображение";
                const string caption = "Добавление  изображения";
                MessageBoxResult result = MessageBox.Show(message, caption,
                                             MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    image0 = image;
                    drugName0 = drugName;

                    imageCrystal0.Source = image0;
                    imageCrystal0.Height = 400;
                    imageCrystal0.Visibility = Visibility.Visible;
                    imageCrystal.Visibility = Visibility.Collapsed;
                }
            }
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
            if (image0 is not null)
            {
                image = image0;
                imageCrystal.Source = image0;
                drugName = drugName0;

                drugName0 = null;
                image0 = null;
                imageCrystal0.Source = null;
                imageCrystal0.Visibility = Visibility.Collapsed;
            }
            else
            {
                image = null;
                imageCrystal.Source = null;
                imageCrystal.Visibility = Visibility.Collapsed;
            }

            previousImage = null;
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

            previousImage = image;
            image = bitmapImage;
            imageCrystal.Source = bitmapImage;
        }

        private void OnRemoveBackGroundClicked(object sender, EventArgs e)
        {
            if (image is null)
            {
                MessageBox.Show("Изображение не найдено", "Необходимо загрузить изображение", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            BackGroundRemoverWindow backgroundemoverWindow = new BackGroundRemoverWindow();

            if (!backgroundemoverWindow.ShowDialog().Value)
            {
                return;
            }

            using MemoryStream outStream = new MemoryStream();

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(image));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            Monochrome monochrome = new Monochrome();
            bitmap = monochrome.MakeMonochrome(bitmap);

            ThresholdFunction threshold = new();

            using Bitmap binary = threshold.MakeBinaryImage(bitmap, double.Parse(backgroundemoverWindow.BackGroundPart));

            HighlightingConnectedComponent highlightingConnectedComponent = new();

            highlightingConnectedComponent.HighlightComponent(binary);

            BackGroundRemover function = new();
            ImageResizer imageResizer = new();

            bitmap = function.RemoveBackGround(binary, bitmap);
            bitmap = imageResizer.ResizeImage(bitmap);

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

            previousImage = image;
            image = bitmapImage;
            imageCrystal.Source = bitmapImage;
        }

        private void OnBinaryClicked(object sender, EventArgs e)
        {
            if (image is null)
            {
                MessageBox.Show("Изображение не найдено", "Необходимо загрузить изображение", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            BackGroundRemoverWindow backgroundRemoverWindow = new BackGroundRemoverWindow();

            if (!backgroundRemoverWindow.ShowDialog().Value)
            {
                return;
            }

            using MemoryStream outStream = new MemoryStream();

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(image));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            Monochrome monochrome = new Monochrome();
            bitmap = monochrome.MakeMonochrome(bitmap);

            ThresholdFunction threshold = new();

            using Bitmap binary = threshold.MakeBinaryImage(bitmap, double.Parse(backgroundRemoverWindow.BackGroundPart));

/*            IntPtr hBitmap = binary.GetHbitmap();
            BitmapImage retval;*/

            using var memory = new MemoryStream();

            binary.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            previousImage = image;
            image = bitmapImage;
            imageCrystal.Source = bitmapImage;
        }

        private void OnLaplacianMethod(object sender, EventArgs e)
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

            SharpnessByLaplacian sharpnessByLaplacian = new SharpnessByLaplacian();
            Bitmap laplacian = sharpnessByLaplacian.MakeLaplacian(bitmap);
            bitmap = sharpnessByLaplacian.MakeSharpnessByLapalacian(bitmap, laplacian);

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

            previousImage = image;
            image = bitmapImage;
            imageCrystal.Source = bitmapImage;
        }

        private void OnIncreaseTrebleMethod(object sender, EventArgs e)
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

            IncreaseTreble increaseTreble = new IncreaseTreble();
            bitmap = increaseTreble.MakeIncreaseTreble(bitmap, 1.5);

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

            previousImage = image;
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

            if (image0 is not null)
            {
                MainWindow.Main.Content = new ParametersPage(image0, method, drugName0, image, drugName);
            }
            else
            {
                MainWindow.Main.Content = new ParametersPage(image, method, drugName);
            }
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

        private void CancelAction(object sender, RoutedEventArgs e)
        {
            image = previousImage;
            imageCrystal.Source = previousImage;
        }
    }
}
