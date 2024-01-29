using System.Drawing;

namespace RemoveBackGround;

public class BackGroundRemover
{
    public Bitmap RemoveBackGround(Bitmap binary, Bitmap original)
    {
        Bitmap image = new Bitmap(original);

        for (int i = 0; i < original.Width; i++)
        {
            for (int j = 0; j < original.Height; j++)
            {
                Color actualColor = original.GetPixel(i, j);

                image.SetPixel(i, j, actualColor);
            }
        }


        for (int j = 0; j < original.Height; j++)
        {
            bool f = true;
            int i = 0;
            while (f && i < original.Width)
            {
                Color actualColor = binary.GetPixel(i, j);

                if (actualColor.B < 10)
                {
                    f = false;
                }
                else
                {
                    image.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
                i++;
            }

            i = original.Width - 1;
            f = true;
            while (f && i >= 0)
            {
                Color actualColor = binary.GetPixel(i, j);

                if (actualColor.B < 10)
                {
                    f = false;
                }
                else
                {
                    image.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
                i--;
            }
        }

        for (int i = 0; i < original.Width; i++)
        {
            bool f = true;
            int j = 0;

            while (f && j < original.Height)
            {
                Color actualColor = binary.GetPixel(i, j);

                if (actualColor.B < 10)
                {
                    f = false;
                }
                else
                {
                    image.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
                j++;
            }

            j = original.Height - 1;
            f = true;
            while (f && j >= 0)
            {
                Color actualColor = binary.GetPixel(i, j);

                if (actualColor.B < 10)
                {
                    f = false;
                }
                else
                {
                    image.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
                j--;
            }
        }

        return image;
    }
}
