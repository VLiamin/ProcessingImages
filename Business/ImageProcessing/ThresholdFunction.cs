using System.Drawing;

namespace RemoveBackGround
{
    public class ThresholdFunction
    {
        public Bitmap MakeBinaryImage(Bitmap oldImage, double backgroundPart)
        {
            if (oldImage is null)
            {
                return null;
            }

            Bitmap image = new Bitmap(oldImage);

            Color actualColor;

            byte red = 0;
            byte blue = 0;
            byte green = 0;
            int brightness = 0;

            int[,] brightnesses = new int[image.Width, image.Height];
            int[] range = new int[255];

            int surroundings = 5;
            int count = 0;

            for (int i = surroundings / 2; i < image.Width - surroundings / 2; i++)
            {
                for (int j = surroundings / 2; j < image.Height - surroundings / 2; j++)
                {
                    count = 0;
                    brightness = 0;
                    for (int l = i - surroundings / 2; l <= i + surroundings / 2; l++)
                    {
                        for (int k = j - surroundings / 2; k <= j + surroundings / 2; k++)
                        {
                            count++;

                            actualColor = oldImage.GetPixel(l, k);

                            red = actualColor.R;
                            blue = actualColor.B;
                            green = actualColor.G;

                            brightness += (int)((0.2989 * red) + (0.5870 * green) + (0.1141 * blue));
                        }
                    }

                    brightness = brightness / 25;

                    brightnesses[i, j] = brightness;
                    range[brightness]++;
                }
            }

            int value = 1;
            count = range[0];
            while (count < backgroundPart * image.Width * image.Height && value < range.Length)
            {
                count += range[value];
                value++;
            }

            for (int i = surroundings / 2; i < image.Width - surroundings / 2; i++)
            {
                for (int j = surroundings / 2; j < image.Height - surroundings / 2; j++)
                {
                    if (brightnesses[i, j] < value)
                    {
                        image.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        image.SetPixel(i, j, Color.White);
                    }
                }
            }

            return image;
        }
    }
}
