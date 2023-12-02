using Business.Models;
using System.Drawing;

namespace Business.Methods
{
    public class CountDensity
    {
        private int[,] FillArrayFromImage(Bitmap original, int[,] array)
        {
            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color actualColor = original.GetPixel(i, j);

                    byte red = actualColor.R;
                    byte blue = actualColor.B;
                    byte green = actualColor.G;

                    // Get image brightness
                    int brightness = (int)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    array[i, j] = brightness;
                }
            }

            return array;
        }

        private int CountМ(int[,] array, int r, int i, int j)
        {
            int brightness = 0;
            for (int l = i - r; l <= i + r; l++)
            {
                for (int k = j - r; k <= j + r; k++)
                {
                    brightness += array[l, k];
                }
            }

            return brightness;
        }

        private double[,] CountD(int[,] array, int r)
        {
            double[,] d = new double[array.GetLength(0), array.GetLength(1)];
            for (int i = r; i < array.GetLength(0) - r; i++)
            {
                for (int j = r; j < array.GetLength(1) - r; j++)
                {
                    int m = CountМ(array, r, i, j);
                    d[i, j] = Math.Log(m) / Math.Log(r);
                }
            }

            return d;
        }

        private double CountCapacitive(int[,] newD, int r)
        {
            int count = 0;

            for (int i = 0; i < newD.GetLength(0) - r; i = i + r)
            {
                for (int j = 0; j < newD.GetLength(1) - r; j = j + r)
                {
                    for (int l = i; l <= i + r; l++)
                    {
                        for (int k = j; k <= j + r; k++)
                        {
                            if (newD[l, k] == 0)
                            {
                                count++;
                                l = newD.GetLength(0);
                                break;
                            }
                        }
                    }
                }
            }

            return Math.Log2(count) / -Math.Log2(r / (double)newD.GetLength(0));
        }

        public List<DensityData> CountDensityMethod(Bitmap image, int r)
        {
            int[,] array = new int[image.Width, image.Height];
            array = FillArrayFromImage(image, array);

            double[,] d = CountD(array, r);

            double min = 100;
            double max = 0;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (d[i, j] < min && d[i, j] != 0)
                    {
                        min = d[i, j];
                    }

                    if (d[i, j] > max)
                    {
                        max = d[i, j];
                    }
                }
            }

            List<int[,]> intervals = new();
            double e = (max - min) / 8;

            for (double i = min; i < max; i += e)
            {
                int[,] newD = new int[image.Width, image.Height];

                for (int l = 0; l < array.GetLength(0); l++)
                {
                    for (int k = 0; k < array.GetLength(1); k++)
                    {
                        if (d[l, k] > i && d[l, k] < i + e)
                        {
                            newD[l, k] = 0;
                        }
                        else
                        {
                            newD[l, k] = 255;
                        }
                    }
                }
                intervals.Add(newD);
            }

            List<DensityData> datas = new();

            for (int i = 0; i < intervals.Count; i++)
            {
                double q = min + i * e;
                double D = CountCapacitive(intervals[i], r);

                datas.Add(new DensityData
                {
                    q = q + e / 2,
                    D = D
                });
            }

            return datas;
        }
    }
}
