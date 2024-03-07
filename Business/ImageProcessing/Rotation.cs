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

            int newWidth = (int)(oldImage.Width * Math.Abs(Math.Cos(angle)) + oldImage.Height * Math.Abs(Math.Sin(angle)));
            int newHeight = (int)(oldImage.Width * Math.Abs(Math.Sin(angle)) + oldImage.Height * Math.Abs(Math.Cos(angle)));

            Bitmap image = new Bitmap(newWidth, newHeight);

            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    int x1 = (int)(x0 + (i - newWidth / 2) * Math.Cos(angle) - (j - newHeight / 2) * Math.Sin(angle));
                    int y1 = (int)(y0 + (i - newWidth / 2) * Math.Sin(angle) + (j - newHeight / 2) * Math.Cos(angle));

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
