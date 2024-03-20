using System.Drawing;

namespace Business.ImageProcessing;

public class Dilatation
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
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (oldImage.GetPixel(i, j).B > 100)
                    {
                        for (int l = i - r; l <= i + r; l++)
                        {
                            for (int k = j - r; k <= j + r; k++)
                            {
                                if (l >= 0 && l < image.Width && k >= 0 && k < image.Height)
                                {
                                    image.SetPixel(l, k, Color.White);
                                    oldImage.SetPixel(l, k, Color.Black);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (oldImage.GetPixel(i, j).B < 100)
                    {
                        for (int l = i - r; l <= i + r; l++)
                        {
                            for (int k = j - r; k <= j + r; k++)
                            {
                                if (l >= 0 && l < image.Width && k >= 0 && k < image.Height)
                                {
                                    image.SetPixel(l, k, Color.Black);
                                    oldImage.SetPixel(l, k, Color.White);
                                }
                            }
                        }
                    }
                }
            }
        }

        return image;
    }
}
