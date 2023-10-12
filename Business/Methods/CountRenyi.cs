using Business.Models;
using CsvHelper;
using System.Drawing;
using System.Globalization;

namespace Business.Methods
{
    public class CountRenyi
    {
        private int imageSize = 0;
        private List<double> pi = new();

        private void CountPi(List<int[,]> parts, int[,] image)
        {
            long allBrightness = 0;

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    allBrightness += image[i, j];
                }
            }

            foreach (int[,] part in parts)
            {
                int brightness = 0;
                for (int i = 0; i < part.GetLength(0); i++)
                {
                    for (int j = 0; j < part.GetLength(1); j++)
                    {
                        brightness += part[i, j];
                    }
                }

                double p = brightness / (double)allBrightness;

                pi.Add(p);
            }

            double sum = 0;

            for (int i = 0; i < pi.Count; i++)
            {
                sum += pi[i];
            }

            for (int i = 0; i < pi.Count; i++)
            {
                pi[i] /= sum;
            }
        }

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

        private double Counts(double q)
        {
            double S = 0;

            foreach (double p in pi)
            {

                S += Math.Pow(p, q);
            }

            return S;
        }

        private double CountD(double S, double q, int n)
        {
            return Math.Log(S) / ((q - 1) * Math.Log(n / (double)imageSize));
        }

        private double CountD1(int n)
        {
            double sum = 0;
            foreach (double p in pi)
            {
                sum += p * Math.Log(p);
            }

            double result = sum / Math.Log(n / (double)imageSize);

            return result;
        }

        private List<int[,]> GetArraysFromImage(int[,] image, int n)
        {
            List<int[,]> arrays = new();
            imageSize = image.GetLength(0);

            int i = 0;
            int j = 0;
            for (i = 0; i < image.GetLength(0) - n; i += n)
            {
                for (j = 0; j < image.GetLength(1) - n; j += n)
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
            int n = Int32.Parse(Console.ReadLine());

            int[,] array = new int[image.Width, image.Height];
            array = FillArrayFromImage(image, array);

            List<int[,]> arrays = GetArraysFromImage(array, n);

            CountPi(arrays, array);

            List<RenyiData> datas = new();

            for (double q = -2; q < 6; q++)
            {
                double D = 0;

                if (q == 1)
                {
                    D = CountD1(n);
                }
                else
                {
                    double S = Counts(q);
                    D = CountD(S, q, n);
                }

                datas.Add(new RenyiData
                {
                    Q = q,
                    Renyi = D
                }); ;

                Console.Write("q = " + q);
                Console.WriteLine(" Renyi = " + D);
            }

            using (StreamWriter streamReader = new StreamWriter("АцФосфор30RotObrBlTreble1,5ImageData.csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(streamReader, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<RenyiData>();
                    csvWriter.NextRecord(); // adds new line after header

                    // записываем данные в csv файл
                    csvWriter.WriteRecords(datas);
                }
            }
        }
    }
}
