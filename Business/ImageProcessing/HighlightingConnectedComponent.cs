﻿using System.Drawing;

namespace RemoveBackGround;

public class HighlightingConnectedComponent
{
    public void HighlightComponent(Bitmap oldImage, int i, int j)
    {
        if (oldImage is null)
        {
            return;
        }

        List<(int i, int j)> values = new();

        int number = 0;
        int i0 = 0;
        int j0 = 0;
        while (true)
        {
            if (i + number < oldImage.Width && oldImage.GetPixel(i + number, j).R == 0)
            {
                i0 = i + number;
                j0 = j;
                break;
            }

            if (i - number > 0 && oldImage.GetPixel(i - number, j).R == 0)
            {
                i0 = i - number;
                j0 = j;
                break;
            }

            if (j + number < oldImage.Height && oldImage.GetPixel(i, j + number).R == 0)
            {
                i0 = i;
                j0 = j + number;
                break;
            }

            if (j - number > 0 && oldImage.GetPixel(i, j - number).R == 0)
            {
                i0 = i;
                j0 = j - number;
                break;
            }

            number++;
        }

        oldImage.SetPixel(i, j, Color.White);
        values.Add((i0, j0));

        number = 0;
        while (number < values.Count)
        {
            (int i, int j) indexes = values[number];

            if (indexes.j < oldImage.Height - 1 && oldImage.GetPixel(indexes.i, indexes.j + 1).R == 0)
            {
                values.Add((indexes.i, indexes.j + 1));
                oldImage.SetPixel(indexes.i, indexes.j + 1, Color.White);
            }

            if (indexes.j > 0 && oldImage.GetPixel(indexes.i, indexes.j - 1).R == 0)
            {
                values.Add((indexes.i, indexes.j - 1));
                oldImage.SetPixel(indexes.i, indexes.j - 1, Color.White);
            }

            if (indexes.j < oldImage.Height - 1 && indexes.i < oldImage.Width - 1 && oldImage.GetPixel(indexes.i + 1, indexes.j + 1).R == 0)
            {
                values.Add((indexes.i + 1, indexes.j + 1));
                oldImage.SetPixel(indexes.i + 1, indexes.j + 1, Color.White);
            }

            if (indexes.j < oldImage.Height - 1 && indexes.i > 0 && oldImage.GetPixel(indexes.i - 1, indexes.j + 1).R == 0)
            {
                values.Add((indexes.i - 1, indexes.j + 1));
                oldImage.SetPixel(indexes.i - 1, indexes.j + 1, Color.White);
            }

            if (indexes.i < oldImage.Width - 1 && indexes.j > 0 && oldImage.GetPixel(indexes.i + 1, indexes.j - 1).R == 0)
            {
                values.Add((indexes.i + 1, indexes.j - 1));
                oldImage.SetPixel(indexes.i + 1, indexes.j - 1, Color.White);
            }

            if (indexes.i > 0 && indexes.j > 0 && oldImage.GetPixel(indexes.i - 1, indexes.j - 1).R == 0)
            {
                values.Add((indexes.i - 1, indexes.j - 1));
                oldImage.SetPixel(indexes.i - 1, indexes.j - 1, Color.White);
            }

            if (indexes.i > 0 && oldImage.GetPixel(indexes.i - 1, indexes.j).R == 0)
            {
                values.Add((indexes.i - 1, indexes.j));
                oldImage.SetPixel(indexes.i - 1, indexes.j, Color.White);
            }

            if (indexes.i < oldImage.Width - 1 && oldImage.GetPixel(indexes.i + 1, indexes.j).R == 0)
            {
                values.Add((indexes.i + 1, indexes.j));
                oldImage.SetPixel(indexes.i + 1, indexes.j, Color.White);
            }

            number++;
        }

        for (i = 0; i < oldImage.Width; i++)
        {
            for (j = 0; j < oldImage.Height; j++)
            {
                oldImage.SetPixel(i, j, Color.White);
            }
        }

        for (i = 0; i < values.Count; i++)
        {
            oldImage.SetPixel(values[i].i, values[i].j, Color.Black);
        }
    }
}
