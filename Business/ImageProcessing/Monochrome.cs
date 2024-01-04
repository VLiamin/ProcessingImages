using System.Drawing;

namespace Business.ImageProcessing
{
    public class Monochrome
    {
        public Bitmap MakeMonochrome(Bitmap oldImage)
        {
            if (oldImage is null)
            {
                return null;
            }

            Bitmap image = new Bitmap(oldImage);

            Color actualColor;

            double[,] brightnesses = new double[image.Width, image.Height];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    actualColor = image.GetPixel(i, j);

                    int average = (actualColor.R + actualColor.B + actualColor.G) / 3;

                    byte newRed = (byte)average;

                    byte newBlue = (byte)average;

                    byte newGreen = (byte)average;

                    image.SetPixel(i, j, Color.FromArgb(newRed, newGreen, newBlue));
                }
            }

            return image;
        }
    }
}
