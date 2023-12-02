using Business.Methods;
using Business.Models;
using ImageProcessing.Enums;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        private string preparationName;
        private BitmapImage image;

        public ObservableCollection<RenyiData> RenyiDatas { get; set; } = new();

        private PlotModel plot { get; set; }

        public ResultPage(BitmapImage image, Methods method, string preparationName)
        {
            this.preparationName = preparationName;
            this.image = image;

            using MemoryStream outStream = new MemoryStream();

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(image));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            InitializeComponent();

            switch (method)
            {
                case Methods.Renyi:
                    CountRenyi countRenyi = new CountRenyi();
                    List<RenyiData> RenyiResult = countRenyi.CountMFSMethod(bitmap, n: 3);
                    CreateRenyiPlot(RenyiResult);
                    RenyiTable.Visibility = Visibility.Visible;
                    RenyiTable.ItemsSource = RenyiResult;
                    break;
                case Methods.Minkowski:
                    CountMinkowski countMinkowski = new CountMinkowski();
                    List<MinkowskiData> MinkowskiResult = countMinkowski.CountFractalsMethod(bitmap);
                    CreateMinkowskiPlot(MinkowskiResult);
                    MinkowskiTable.Visibility = Visibility.Visible;
                    MinkowskiTable.ItemsSource = MinkowskiResult;
                    break;
                case Methods.Density:
                    CountDensity countDensity = new CountDensity();
                    List<DensityData> DensityResult = countDensity.CountDensityMethod(bitmap, r: 10);
                    CreateDensityPlot(DensityResult);
                    DensityTable.Visibility = Visibility.Visible;
                    DensityTable.ItemsSource = DensityResult;
                    break;
            }
        }

        public void CreateRenyiPlot(List<RenyiData> datas)
        {
            plot = new PlotModel { Title = $"Результат исследования препарата {preparationName} методом \"Cпектр обобщенных размерностей Реньи\"" };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 1.8,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 0,
                Maximum = 2.2,
                MajorStep = 0.05,
                MinorStep = 0.01,
                Title = "D",
                TickStyle = TickStyle.Inside,
                FontSize = 17

            });

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 0,
                Minimum = -3,
                Maximum = 6,
                MajorStep = 1,
                MinorStep = 0.25,
                Title = "q",
                TickStyle = TickStyle.Inside,
                FontSize = 17
            });


            plot.Series.Add(AddRenyiLine(datas));
            plotView1.Model = plot;
        }

        public DataPointSeries AddRenyiLine(List<RenyiData> datas)
        {
            var ls = new LineSeries
            {
                //Title = string.Format("q={0}, D={1}")
            };

            for (int i = 0; i < datas.Count; i++)
            {
                ls.Points.Add(new DataPoint(datas[i].Q, datas[i].Renyi));
            }

            return ls;
        }

        public void CreateMinkowskiPlot(List<MinkowskiData> datas)
        {
            plot = new PlotModel { Title = $"Результат исследования препарата {preparationName} методом \"Размерность Минковского\"" };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 1,
                Minimum = 18,
                Maximum = 21,
                MajorStep = 0.5,
                MinorStep = 0.25,
                Title = "ln(A)",
                TickStyle = TickStyle.Inside,
                FontSize = 17
            });

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 0,
                Minimum = 0.5,
                Maximum = 3.5,
                MajorStep = 0.5,
                MinorStep = 0.25,
                Title = "ln(delta)",
                TickStyle = TickStyle.Inside,
                FontSize = 17
            });


            plot.Series.Add(AddMinkowskiLine(datas));
            plotView1.Model = plot;
        }

        public DataPointSeries AddMinkowskiLine(List<MinkowskiData> datas)
        {
            var ls = new LineSeries
            {
                //Title = string.Format("q={0}, D={1}")
            };

            for (int i = 0; i < datas.Count; i++)
            {
                ls.Points.Add(new DataPoint(datas[i].LnI, datas[i].LnA));
            }

            return ls;
        }

        public void CreateDensityPlot(List<DensityData> datas)
        {
            plot = new PlotModel { Title = $"Результат исследования препарата {preparationName} методом \"Размерность Минковского\"" };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 1,
                Minimum = 0.5,
                Maximum = 2.5,
                MajorStep = 0.5,
                MinorStep = 0.1,
                Title = "D",
                TickStyle = TickStyle.Inside,
                FontSize = 17
            });

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 0,
                Minimum = 4,
                Maximum = 5.7,
                MajorStep = 0.3,
                MinorStep = 0.1,
                Title = "q",
                TickStyle = TickStyle.Inside,
                FontSize = 17
            });


            plot.Series.Add(AddDensityLine(datas));
            plotView1.Model = plot;
        }

        public DataPointSeries AddDensityLine(List<DensityData> datas)
        {
            var ls = new StairStepSeries
            {
                StrokeThickness = 3,
                VerticalStrokeThickness = 0,
                MarkerType = MarkerType.None
            };

            for (int i = 0; i < datas.Count; i++)
            {
                ls.Points.Add(new DataPoint(datas[i].q, datas[i].D));
            }

            return ls;
        }

        private void DownloadClicked(object sender, EventArgs e)
        {
            try
            {
                String home = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

                plot.Title = "Crystal";
                using FileStream stream = File.Create(Path.Combine(home, $"Result_{preparationName}.pdf"));
                PdfExporter pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(plot, stream);

                MessageBox.Show($"Данные сохранились успешно\nПуть к файлу: {home}\\Result_{preparationName}.pdf", "Сохранение изображения", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Сохранение данных не удалось", "Сохранение изображения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetInformationClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Версия системы: 1.0.0\nОпубликовано: 2023.12.01", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            MainWindow.Main.Content = new HelpPage();
        }
    }
}
