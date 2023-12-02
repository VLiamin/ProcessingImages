using Business.Models;
using System.Drawing;

namespace Business.Methods
{
    public class CountMinkowski
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

        private double CountA(int volumeAfter, int volumeBefore)
        {
            return (double)(volumeAfter - volumeBefore) / 2;
        }

        private int CountVolume(int[,] u, int[,] b)
        {
            int vol = 0;
            for (int i = 0; i < u.GetLength(0); i++)
            {
                for (int j = 0; j < u.GetLength(1); j++)
                {
                    vol += u[i, j] - b[i, j];
                }
            }

            return vol;
        }

        private int CountU(int[,] data, int i, int j, int ro)
        {
            int max = data[i, j] + 1;

            for (int l = i - ro; l <= i + ro; l++)
            {
                for (int k = j - ro; k <= j + ro; k++)
                {
                    if (l >= 0 && k >= 0 && l < data.GetLength(0) && k < data.GetLength(1))
                    {
                        if (max < data[l, k])
                        {
                            max = data[l, k];
                        }
                    }
                }
            }

            return max;
        }

        private int CountB(int[,] data, int i, int j, int ro)
        {
            int min = data[i, j] - 1;

            for (int l = i - ro; l <= i + ro; l++)
            {
                for (int k = j - ro; k <= j + ro; k++)
                {
                    if (l >= 0 && k >= 0 && l < data.GetLength(0) && k < data.GetLength(1))
                    {
                        if (min > data[l, k])
                        {
                            min = data[l, k];
                        }
                    }
                }
            }

            return min;
        }

        public List<MinkowskiData> CountFractalsMethod(Bitmap image)
        {
            int[,] array = new int[image.Width, image.Height];

            array = FillArrayFromImage(image, array);
            int range = 10;

            int[] volumes = new int[range];
            double[] a = new double[range];
            double[] aV = new double[range];

            int[,] u = array;
            int[,] b = array;
            for (int j = 1; j < range; j++)
            {
                int[,] newU = new int[image.Width, image.Height];
                int[,] newB = new int[image.Width, image.Height];

                for (int l = 0; l < image.Width; l++)
                {
                    for (int k = 0; k < image.Height; k++)
                    {
                        newU[l, k] = CountU(u, l, k, j);
                        newB[l, k] = CountB(b, l, k, j);
                    }
                }

                int newVolume = CountVolume(newU, newB);
                aV[j] = CountA(newVolume, volumes[j - 1]);
                a[j] = newVolume / (double)(2 * j);
                volumes[j] = newVolume;
            }

            List<MinkowskiData> datas = new();
            for (int i = 2; i < range; i++)
            {
                datas.Add(new MinkowskiData
                {
                    LnI = Math.Log2(i),
                    LnA = Math.Log2(aV[i])
                });
            }

            return datas;
        }
    }
}
