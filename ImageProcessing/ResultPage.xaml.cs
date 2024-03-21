using Aspose.Pdf;
using Aspose.Pdf.Text;
using Business.Methods;
using Business.Models;
using ImageProcessing.Constants;
using ImageProcessing.Enums;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Page = System.Windows.Controls.Page;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        private string preparationName1;
        private string preparationName2;
        private BitmapImage image;
        private Methods method;

        private List<RenyiData> RenyiResult;
        private List<MinkowskiData> MinkowskiResult;
        private List<DensityData> DensityResult;

        private List<RenyiData> RenyiResult2;
        private List<MinkowskiData> MinkowskiResult2;
        private List<DensityData> DensityResult2;

        public ObservableCollection<RenyiData> RenyiDatas { get; set; } = new();

        private PlotModel plot { get; set; }

        public ResultPage(BitmapImage image, Methods method, string preparationName1, BitmapImage image2 = null, string preparationName2 = null)
        {
            this.preparationName1 = preparationName1;
            this.preparationName2 = preparationName2;
            this.image = image;
            this.method = method;

            using MemoryStream outStream = new MemoryStream();

            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(image));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            Bitmap bitmap2 = null;
            if (image2 is not null)
            {
                using MemoryStream outStream2 = new MemoryStream();

                BitmapEncoder enc2 = new BmpBitmapEncoder();
                enc2.Frames.Add(BitmapFrame.Create(image2));
                enc2.Save(outStream2);
                bitmap2 = new Bitmap(outStream2);
            }

            InitializeComponent();
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(179, 255, 255));

            switch (method)
            {
                case Methods.Renyi:
                    CountRenyi countRenyi = new CountRenyi();
                    List<RenyiData> RenyiResult = countRenyi.CountMFSMethod(bitmap, n: 3);

                    List<RenyiData> RenyiResult2 = null;
                    if (bitmap2 is not null)
                    {
                        RenyiResult2 = countRenyi.CountMFSMethod(bitmap2, n: 3);
                        this.RenyiResult2 = RenyiResult2;

                        RenyiTableBox2.Visibility = Visibility.Visible;
                        RenyiTableTitle2.Content = $"Результат исследования препарата {preparationName2}";
                        RenyiTable2.ItemsSource = RenyiResult2;

                        foreach (RenyiData element in RenyiResult2)
                        {
                            element.Renyi = Math.Round(element.Renyi, 5);
                        }
                    }

                    this.RenyiResult = RenyiResult;

                    CreateRenyiPlot(RenyiResult, RenyiResult2);

                    foreach (RenyiData element in RenyiResult)
                    {
                        element.Renyi = Math.Round(element.Renyi, 5);
                    }

                    RenyiTableBox1.Visibility = Visibility.Visible;
                    RenyiTableTitle1.Content = $"Результат исследования препарата {preparationName1}";
                    RenyiDescription.Visibility = Visibility.Visible;
                    RenyiTable.ItemsSource = RenyiResult;
                    break;
                case Methods.Minkowski:
                    CountMinkowski countMinkowski = new CountMinkowski();
                    List<MinkowskiData> MinkowskiResult = countMinkowski.CountFractalsMethod(bitmap);

                    this.MinkowskiResult = MinkowskiResult;

                    List<MinkowskiData> MinkowskiResult2 = null;
                    if (bitmap2 is not null)
                    {
                        MinkowskiResult2 = countMinkowski.CountFractalsMethod(bitmap2);
                        this.MinkowskiResult2 = MinkowskiResult2;

                        MinkowskiTableBox2.Visibility = Visibility.Visible;
                        MinkowskiTableTitle2.Content = $"Результат исследования препарата {preparationName2}";
                        MinkowskiTable2.ItemsSource = MinkowskiResult2;

                        foreach (MinkowskiData element in MinkowskiResult2)
                        {
                            element.LnI = Math.Round(element.LnI, 5);
                            element.LnA = Math.Round(element.LnA, 5);
                        }
                    }

                    CreateMinkowskiPlot(MinkowskiResult, MinkowskiResult2);

                    foreach (MinkowskiData element in MinkowskiResult)
                    {
                        element.LnI = Math.Round(element.LnI, 5);
                        element.LnA = Math.Round(element.LnA, 5);
                    }

                    MinkowskiTableBox1.Visibility = Visibility.Visible;
                    MinkowskiDescription.Visibility = Visibility.Visible;
                    MinkowskiTableTitle1.Content = $"Результат исследования препарата {preparationName1}";
                    MinkowskiTable1.ItemsSource = MinkowskiResult;
                    break;
                case Methods.Density:
                    CountDensity countDensity = new CountDensity();
                    List<DensityData> DensityResult = countDensity.CountDensityMethod(bitmap, r: 10);

                    this.DensityResult = DensityResult;

                    List<DensityData> DensityResult2 = null;
                    if (bitmap2 is not null)
                    {
                        DensityResult2 = countDensity.CountDensityMethod(bitmap2, r: 10);
                        this.DensityResult2 = DensityResult2;

                        DensityTableBox2.Visibility = Visibility.Visible;
                        DensityTableTitle2.Content = $"Результат исследования препарата {preparationName2}";
                        DensityTable2.ItemsSource = DensityResult2;

                        foreach (DensityData element in DensityResult2)
                        {
                            element.D = Math.Round(element.D, 5);
                            element.q = Math.Round(element.q, 5);
                        }
                    }

                    CreateDensityPlot(DensityResult, DensityResult2);

                    foreach (DensityData element in DensityResult)
                    {
                        element.D = Math.Round(element.D, 5);
                        element.q = Math.Round(element.q, 5);
                    }

                    DensityTableBox1.Visibility = Visibility.Visible;
                    DensityDescription.Visibility = Visibility.Visible;
                    DensityTableTitle1.Content = $"Результат исследования препарата {preparationName1}";
                    DensityTable1.ItemsSource = DensityResult;
                    break;
            }
        }

        public void CreateRenyiPlot(List<RenyiData> datas, List<RenyiData> datas2 = null)
        {
            plot = new PlotModel { Title = $"Результат исследования методом \"Cпектр обобщенных размерностей Реньи\"" };

            double maxQ = datas.Max(x => x.Q);
            double maxD = datas.Max(x => x.Renyi);

            double minQ = datas.Min(x => x.Q);
            double minD = datas.Min(x => x.Renyi);

            if (datas2 is not null)
            {
                double maxQ2 = datas2.Max(x => x.Q);
                double maxD2 = datas2.Max(x => x.Renyi);

                if (maxQ2 > maxQ)
                {
                    maxQ = maxQ2;
                }

                if (maxD2 > maxD)
                {
                    maxD = maxD2;
                }

                double minQ2 = datas2.Min(x => x.Q);
                double minD2 = datas2.Min(x => x.Renyi);

                if (minQ2 < minQ)
                {
                    minQ = minQ2;
                }

                if (minD2 < minD)
                {
                    minD = minD2;
                }
            }

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = minD - 0.05,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 0,
                Maximum = maxD + 0.05,
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
                Minimum = minQ - 0.5,
                Maximum = maxQ + 0.5,
                MajorStep = 1,
                MinorStep = 0.25,
                Title = "q",
                TickStyle = TickStyle.Inside,
                FontSize = 17
            });

            plot.Series.Add(AddRenyiLine(datas, preparationName1));

            if (datas2 is not null)
            {
                plot.Series.Add(AddRenyiLine(datas2, preparationName2));
            }

            plot.Legends.Add(new Legend()
            {
                LegendPosition = LegendPosition.RightTop,
                LegendFontSize = 14
            });

            plotView1.Model = plot;
        }

        public DataPointSeries AddRenyiLine(List<RenyiData> datas, string drugName)
        {
            LineSeries ls = new LineSeries
            {
                Title = drugName
            };

            for (int i = 0; i < datas.Count; i++)
            {
                ls.Points.Add(new DataPoint(datas[i].Q, datas[i].Renyi));
            }

            return ls;
        }

        public void CreateMinkowskiPlot(List<MinkowskiData> datas, List<MinkowskiData> datas2 = null)
        {
            plot = new PlotModel { Title = $"Результат исследования препарата {preparationName1} методом \"Размерность Минковского\"" };

            double maxLnA = datas.Max(x => x.LnA);
            double maxLni = datas.Max(x => x.LnI);

            double minLnA = datas.Min(x => x.LnA);
            double minLni = datas.Min(x => x.LnI);

            if (datas2 is not null)
            {
                double maxLnA2 = datas2.Max(x => x.LnA);
                double maxLni2 = datas2.Max(x => x.LnI);

                if (maxLnA2 > maxLnA)
                {
                    maxLnA = maxLnA2;
                }

                if (maxLni2 > maxLni)
                {
                    maxLni = maxLni2;
                }

                double minLnA2 = datas2.Min(x => x.LnA);
                double minLni2 = datas2.Min(x => x.LnI);

                if (minLnA2 < minLnA)
                {
                    minLnA = minLnA2;
                }

                if (minLni2 < minLni)
                {
                    minLni = minLni2;
                }
            }

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MaximumPadding = 0,
                MinimumPadding = 1,
                Minimum = minLnA - 0.5,
                Maximum = maxLnA + 0.5,
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
                Minimum = minLni - 0.5,
                Maximum = maxLni + 0.5,
                MajorStep = 0.5,
                MinorStep = 0.25,
                Title = "ln(delta)",
                TickStyle = TickStyle.Inside,
                FontSize = 17
            });

            plot.Legends.Add(new Legend()
            {
                LegendPosition = LegendPosition.RightTop,
                LegendFontSize = 14
            });

            if (datas2 is not null)
            {
                plot.Series.Add(AddMinkowskiLine(datas2, preparationName2));
            }

            plot.Series.Add(AddMinkowskiLine(datas, preparationName1));
            plotView1.Model = plot;
        }

        public DataPointSeries AddMinkowskiLine(List<MinkowskiData> datas, string preparationName)
        {
            var ls = new LineSeries
            {
                Title = preparationName
            };

            for (int i = 0; i < datas.Count; i++)
            {
                ls.Points.Add(new DataPoint(datas[i].LnI, datas[i].LnA));
            }

            return ls;
        }

        public void CreateDensityPlot(List<DensityData> datas, List<DensityData> datas2 = null)
        {
            plot = new PlotModel { Title = $"Результат исследования методом \"Размерность Минковского\"" };

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

            plot.Legends.Add(new Legend()
            {
                LegendPosition = LegendPosition.RightTop,
                LegendFontSize = 14
            });

            if (datas2 is not null)
            {
                plot.Series.Add(AddDensityLine(datas2, preparationName2));
            }

            plot.Series.Add(AddDensityLine(datas, preparationName1));
            plotView1.Model = plot;
        }

        public DataPointSeries AddDensityLine(List<DensityData> datas, string preparationName)
        {
            var ls = new StairStepSeries
            {
                StrokeThickness = 3,
                VerticalStrokeThickness = 0,
                MarkerType = MarkerType.None,
                Title = preparationName,
            };

            for (int i = 0; i < datas.Count; i++)
            {
                ls.Points.Add(new DataPoint(datas[i].q, datas[i].D));
            }

            return ls;
        }

        private void InitialClicked(object sender, EventArgs e)
        {
            switch (method)
            {
                case Methods.Renyi:
                    CreateRenyiPlot(RenyiResult, RenyiResult2);
                    break;
                case Methods.Minkowski:
                    CreateMinkowskiPlot(MinkowskiResult, MinkowskiResult2);
                    break;
                case Methods.Density:
                    CreateDensityPlot(DensityResult, DensityResult2);
                    break;
            }
        }

        private async void DownloadClicked(object sender, EventArgs e)
        {
            try
            {
                String home = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

                string path = $"Result{method}_{preparationName1}_{preparationName2}.pdf";

                MemoryStream memory = new();

                PdfExporter pdfExporter = new PdfExporter { Width = 600, Height = 400 };

                string s = plot.Title;
                plot.Title = "   ";
                pdfExporter.Export(plot, memory);
                plot.Title = s;

                MemoryStream stream = new MemoryStream();
                byte[] array = memory.ToArray();
                stream.Write(array, 0, array.Length);

                Document document = new Document(stream);

                Aspose.Pdf.Page page = document.Pages.Add();

                TextFragment textFragment = new TextFragment("Название препарата: " + preparationName1 + "\nДата эксперимента: " + DateTime.Today);
                textFragment.TextState.FontSize = 18;
                page.Paragraphs.Add(textFragment);

                switch (method)
                {
                    case Methods.Renyi:
                        CreateRenyiTable(page, RenyiResult, preparationName1);
                        if (RenyiResult2 is not null)
                        {
                            TextFragment textFragment2 = new TextFragment("Название препарата: " + preparationName2);
                            textFragment2.TextState.FontSize = 18;
                            page.Paragraphs.Add(textFragment2);
                            CreateRenyiTable(page, RenyiResult2, preparationName2);
                        }
                        break;
                    case Methods.Minkowski:
                        CreateMinkowskiTable(page, MinkowskiResult, preparationName1);
                        if (MinkowskiResult2 is not null)
                        {
                            TextFragment textFragment2 = new TextFragment("Название препарата: " + preparationName2);
                            textFragment2.TextState.FontSize = 18;
                            page.Paragraphs.Add(textFragment2);
                            CreateMinkowskiTable(page, MinkowskiResult2, preparationName2);
                        }
                        break;
                    case Methods.Density:
                        CreateDensityTable(page, DensityResult, preparationName1);
                        if (DensityResult2 is not null)
                        {
                            TextFragment textFragment2 = new TextFragment("Название препарата: " + preparationName2);
                            textFragment2.TextState.FontSize = 18;
                            page.Paragraphs.Add(textFragment2);
                            CreateDensityTable(page, DensityResult2, preparationName2);
                        }
                        break;
                }

                document.Save(System.IO.Path.Combine(home, path));

                memory.Close();

                MessageBox.Show($"Данные сохранились успешно\nПуть к файлу: {home}\\Result{method}_{preparationName1}_{preparationName2}.pdf", "Сохранение изображения", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Сохранение данных не удалось", "Сохранение изображения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateRenyiTable(Aspose.Pdf.Page page, List<RenyiData> data, string name)
        {
            Aspose.Pdf.Table table = new Aspose.Pdf.Table
            {
                // Set the table border color as LightGray
                Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.Black),
                // Set the border for table cells
                DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.Black),

            };

            table.DefaultCellTextState = new TextState(18);
            table.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center;

            Aspose.Pdf.Row row0 = table.Rows.Add();
            row0.Cells.Add("q");
            row0.Cells.Add("D(q)");

            for (int i = 0; i < data.Count; i++)
            {
                // Add row to table
                Aspose.Pdf.Row row = table.Rows.Add();
                // Add table cells
                row.Cells.Add(data[i].Q.ToString());
                row.Cells.Add(data[i].Renyi.ToString());
            }

            // Add table object to first page of input document
            page.Paragraphs.Add(table);

            Aspose.Pdf.Text.TextFragment text = new Aspose.Pdf.Text.TextFragment();
            text.TextState.FormattingOptions = new Aspose.Pdf.Text.TextFormattingOptions()
            {
                SubsequentLinesIndent = 20
            };
            page.Paragraphs.Add(text);
        }

        private void CreateMinkowskiTable(Aspose.Pdf.Page page, List<MinkowskiData> data, string name)
        {
            Aspose.Pdf.Table table = new Aspose.Pdf.Table
            {
                // Set the table border color as LightGray
                Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.Black),
                // Set the border for table cells
                DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.Black),

            };

            table.DefaultCellTextState = new TextState(18);
            table.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center;

            Aspose.Pdf.Row row0 = table.Rows.Add();
            row0.Cells.Add("ln(delta)");
            row0.Cells.Add("ln(A)");

            for (int i = 0; i < data.Count; i++)
            {
                // Add row to table
                Aspose.Pdf.Row row = table.Rows.Add();
                // Add table cells
                row.Cells.Add(data[i].LnI.ToString());
                row.Cells.Add(data[i].LnA.ToString());
            }

            // Add table object to first page of input document
            page.Paragraphs.Add(table);

            Aspose.Pdf.Text.TextFragment text = new Aspose.Pdf.Text.TextFragment();
            text.TextState.FormattingOptions = new Aspose.Pdf.Text.TextFormattingOptions()
            {
                SubsequentLinesIndent = 20
            };
            page.Paragraphs.Add(text);
        }

        private void CreateDensityTable(Aspose.Pdf.Page page, List<DensityData> data, string name)
        {
            Aspose.Pdf.Table table = new Aspose.Pdf.Table
            {
                // Set the table border color as LightGray
                Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.Black),
                // Set the border for table cells
                DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.Black),

            };

            table.DefaultCellTextState = new TextState(18);
            table.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center;

            Aspose.Pdf.Row row0 = table.Rows.Add();
            row0.Cells.Add("q");
            row0.Cells.Add("D");

            for (int i = 0; i < data.Count; i++)
            {
                // Add row to table
                Aspose.Pdf.Row row = table.Rows.Add();
                // Add table cells
                row.Cells.Add(data[i].q.ToString());
                row.Cells.Add(data[i].D.ToString());
            }

            // Add table object to first page of input document
            page.Paragraphs.Add(table);

            Aspose.Pdf.Text.TextFragment text = new Aspose.Pdf.Text.TextFragment();
            text.TextState.FormattingOptions = new Aspose.Pdf.Text.TextFormattingOptions()
            {
                SubsequentLinesIndent = 20
            };
            page.Paragraphs.Add(text);
        }

        private void GetInformationClicked(object sender, EventArgs e)
        {
            MessageBox.Show(ApplicationConstants.Version, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            MainWindow.Main.Content = new HelpPage();
        }
    }
}
