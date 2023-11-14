using Business.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Maui.Skia.Core;
using OxyPlot.Series;
using PlotCommands = OxyPlot.Maui.Skia.PlotCommands;

namespace ProcessingImages;

public partial class ResultPage
{
    private List<RenyiData> datas;
    private string preparationName;
    private string increasePicker;
    private PlotModel plot;

    public ResultPage(List<RenyiData> datas, string preparationName, string increasePicker)
    {
        InitializeComponent();

        this.datas = datas;
        this.preparationName = preparationName;
        this.increasePicker = increasePicker;

        this.Loaded += PanModePage_Loaded;
    }

    private void PanModePage_Loaded(object sender, EventArgs e)
    {
        plotView1.Model = CreateNormalDistributionModel();
        plotView1.Model = CreateNormalDistributionModel();
    }

    public PlotModel CreateNormalDistributionModel()
    {
        // http://en.wikipedia.org/wiki/Normal_distribution

        plot = new PlotModel
        {
            Title = $"Результат исследования препара {preparationName} методом \"Cпектр обобщенных размерностей Реньи\"",
        };

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
            TickStyle = TickStyle.Inside
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
            TickStyle = TickStyle.Inside
        });

        plot.Series.Add(AddLine(datas));

        return plot;
    }

    public DataPointSeries AddLine(List<RenyiData> datas)
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

    private void PanMode_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
        {
            return;
        }            

        var rb = sender as RadioButton;
        if (rb?.Value == null)
            return;

        var viewCmd = rb.Value switch
        {
            "2" => PlotCommands.PanZoomByTouchTwoFinger,
            "3" => PlotCommands.PanZoomByTouchAxisOnly,
            _ => PlotCommands.PanZoomByTouch
        };

        var cmd = new CompositeDelegateViewCommand<OxyTouchEventArgs>(
            PlotCommands.SnapTrackTouch,
            viewCmd
        );

        //PlotView.Controller.BindTouchDown(cmd);
    }

    private async void DownloadClicked(object sender, EventArgs e)
    {
        //string path = FileSystem.Current.AppDataDirectory;

        // ToDo add try-catch

        try
        {
            plot.Title = "Crystal";
            using FileStream stream = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "crystal.pdf"));
            PdfExporter pdfExporter = new PdfExporter { Width = 600, Height = 400 };
            pdfExporter.Export(plot, stream);

            await DisplayAlert("Сохранение изображения", "Данные сохранились успешно", "Хорошо");
        }
        catch
        {
            await DisplayAlert("Сохранение изображения", "Сохранение данных не удалось", "Хорошо");
        }
    }
}