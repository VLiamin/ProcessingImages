using ImageProcessing.Enums;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageProcessing.Windows
{
    /// <summary>
    /// Interaction logic for BackGroundRemoverWindow.xaml
    /// </summary>
    public partial class BackGroundRemoverWindow : Window
    {
        private readonly BitmapImage _image;

        public BackGroundRemoverWindow(int x, int y, BitmapImage image)
        {
            InitializeComponent();

            X.Text = x.ToString();
            Y.Text = y.ToString();
            _image = image;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ImageClicked(object sender, EventArgs e)
        {
            WindowHeight.Height = new GridLength(400);
            imageCrystal.Visibility = Visibility.Visible;

            using MemoryStream outStream = new MemoryStream();
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(_image));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            int x = int.Parse(X.Text);
            int y = int.Parse(Y.Text);

            if (x < 1 || x >= _image.Width - 1)
            {
                return;
            }

            if (y < 1 || y >= _image.Height - 1)
            {
                return;
            }

            for (int i = 0; i < 40; i++)
            {
                if (x + i < bitmap.Width)
                {
                    bitmap.SetPixel(x + i, y - 1, Color.Red);
                    bitmap.SetPixel(x + i, y, Color.Red);
                    bitmap.SetPixel(x + i, y + 1, Color.Red);
                }
                
                if (x - i >= 0)
                {
                    bitmap.SetPixel(x - i, y - 1, Color.Red);
                    bitmap.SetPixel(x - i, y, Color.Red);
                    bitmap.SetPixel(x - i, y + 1, Color.Red);
                }

                if (y + 1 < bitmap.Height)
                {
                    bitmap.SetPixel(x - 1, y + i, Color.Red);
                    bitmap.SetPixel(x, y + i, Color.Red);
                    bitmap.SetPixel(x + 1, y + i, Color.Red);
                }

                if (y - i >= 0)
                {
                    bitmap.SetPixel(x - 1, y - i, Color.Red);
                    bitmap.SetPixel(x, y - i, Color.Red);
                    bitmap.SetPixel(x + 1, y - i, Color.Red);
                }
            }

            using var memory = new MemoryStream();

            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            imageCrystal.Source = bitmapImage;


        }

        public string BackGroundPart
        {
            get { return backgroundPart.Text; }
        }

        public string BackX
        {
            get { return X.Text; }
        }

        public string BackY
        {
            get { return Y.Text; }
        }
    }
}
