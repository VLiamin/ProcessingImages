using System.Drawing;

namespace Business.ImageProcessing
{
    public class Rotation
    {
        public Bitmap Rotate(Bitmap oldImage, double angle)
        {
            if (oldImage is null)
            {
                return null;
            }

            angle = Math.PI * angle / 180;

            Color actualColor;

            int x0 = oldImage.Width / 2;
            int y0 = oldImage.Height / 2;

            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            int newWidth = (int)(oldImage.Width * Math.Abs(cos) + oldImage.Height * Math.Abs(sin));
            int newHeight = (int)(oldImage.Width * Math.Abs(sin) + oldImage.Height * Math.Abs(cos));

            Bitmap image = new Bitmap(newWidth, newHeight);

            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    int x1 = (int)(x0 + (i - newWidth / 2) * cos - (j - newHeight / 2) * sin);
                    int y1 = (int)(y0 + (i - newWidth / 2) * sin + (j - newHeight / 2) * cos);

                    if (x1 >= 0 && x1 < oldImage.Width && y1 >= 0 && y1 < oldImage.Height)
                    {
                        image.SetPixel(i, j, oldImage.GetPixel(x1, y1));
                    }
                }
            }

            return image;
        }
    }
}
