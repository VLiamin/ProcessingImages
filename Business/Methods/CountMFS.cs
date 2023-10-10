using System.Drawing;

namespace Business.Methods
{
    public class CountMFS
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
            if (volumeAfter != 50)
            {
                int t = 1;
            }
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

        private List<int[,]> GetArraysFromImage(int[,] image, int n)
        {
            List<int[,]> arrays = new();

            for (int i = 0; i < image.GetLength(0) - n; i += n)
            {
                for (int j = 0; j < image.GetLength(1) - n; j += n)
                {
                    int[,] array = new int[n, n];

                    for (int k = i; k < i + n; k++)
                    {
                        for (int l = j; l < j + n; l++)
                        {
                            array[k - i, l - j] = image[k, l];
                        }
                    }

                    arrays.Add(array);
                }
            }

            return arrays;
        }

        public void CountMFSMethod(Bitmap image)
        {
            Console.Write("n: ");
            int n = int.Parse(Console.ReadLine());

            int[,] array = new int[image.Width, image.Height];

            array = FillArrayFromImage(image, array);

            List<int[,]> arrays = GetArraysFromImage(array, n);
            int[] volumes = new int[arrays.Count];
            double[] a = new double[arrays.Count];

            for (int i = 0; i < arrays.Count; i++)
            {
                int[,] u = arrays[i];
                int[,] b = arrays[i];
                for (int j = 0; j < 2; j++)
                {
                    int[,] newU = new int[n, n];
                    int[,] newB = new int[n, n];

                    for (int l = 0; l < n; l++)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            newU[l, k] = CountU(u, l, k, j + 1);
                            newB[l, k] = CountB(b, l, k, j + 1);
                        }
                    }

                    if (volumes[i] == 0)
                    {
                        volumes[i] = CountVolume(newU, newB);
                    }
                    else
                    {
                        int newVolume = CountVolume(newU, newB);
                        a[i] = CountA(newVolume, volumes[i]);

                        volumes[i] = newVolume;
                    }
                }
            }

            double MFS = 0;
            for (int i = 0; i < a.Length; i++)
            {
                MFS += a[i];
            }

            Console.WriteLine("MFS = " + MFS);
            Console.WriteLine("Average MFS = " + (double)MFS / a.Length);
        }
    }
}
