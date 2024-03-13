using System.Drawing;

namespace Dilatation
{
    public class HalftoneDilatation
    {
        public Bitmap MakeDilatation(Bitmap oldImage, int r, bool isDilatation = true)
        {
            if (oldImage is null)
            {
                return null;
            }

            Bitmap image = new Bitmap(oldImage);

            if (isDilatation)
            {
                for (int i = r; i < image.Width - r; i++)
                {
                    for (int j = r; j < image.Height - r; j++)
                    {
                        int min = 256;
                        for (int l = i - r; l <= i + r; l++)
                        {
                            for (int k = j - r; k <= j + r; k++)
                            {
                                if (oldImage.GetPixel(l, k).R < min)
                                {
                                    min = oldImage.GetPixel(l, k).R;
                                }
                            }
                        }

                        image.SetPixel(i, j, Color.FromArgb(min, min, min));
                    }
                }
            }
            else
            {
                for (int i = r; i < image.Width - r; i++)
                {
                    for (int j = r; j < image.Height - r; j++)
                    {
                        int max = 0;
                        for (int l = i - r; l <= i + r; l++)
                        {
                            for (int k = j - r; k <= j + r; k++)
                            {
                                if (oldImage.GetPixel(l, k).R > max)
                                {
                                    max = oldImage.GetPixel(l, k).R;
                                }
                            }
                        }

                        image.SetPixel(i, j, Color.FromArgb(max, max, max));
                    }
                }
            }

            return image;
        }
    }
}
