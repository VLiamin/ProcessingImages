using System.Drawing;

namespace Business.ImageProcessing;

public class ImageResizer
{
    public Bitmap ResizeImage(Bitmap bitmap)
    {
        int top = 0;

        bool f = true;
        for (top = 0; top < bitmap.Height; top++)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                if (bitmap.GetPixel(j, top).R != 255)
                {
                    f = false;
                    break;
                }
            }

            if (!f)
            {
                break;
            }
        }

        f = true;
        int bottom = 0;
        for (bottom = bitmap.Height - 1; bottom >= 0; bottom--)
        {
            for (int j = 0; j < bitmap.Width; j++)
            {
                if (bitmap.GetPixel(j, bottom).R != 255)
                {
                    f = false;
                    break;
                }
            }

            if (!f)
            {
                break;
            }
        }

        f = true;
        int left = 0;
        for (left = 0; left < bitmap.Width; left++)
        {
            for (int j = 0; j < bitmap.Height; j++)
            {
                if (bitmap.GetPixel(left, j).R != 255)
                {
                    f = false;
                    break;
                }
            }

            if (!f)
            {
                break;
            }
        }

        f = true;
        int right = 0;
        for (right = bitmap.Width - 1; right >= 0; right--)
        {
            for (int j = 0; j < bitmap.Height; j++)
            {
                if (bitmap.GetPixel(right, j).R != 255)
                {
                    f = false;
                    break;
                }
            }

            if (!f)
            {
                break;
            }
        }

        Bitmap newImage = new Bitmap(right - left + 1, bottom - top + 1);

        for (int i = left; i <= right; i++)
        {
            for (int j = top; j <= bottom; j++)
            {
                newImage.SetPixel(i - left, j - top, bitmap.GetPixel(i, j));
            }
        }

        return newImage;
    }
}
