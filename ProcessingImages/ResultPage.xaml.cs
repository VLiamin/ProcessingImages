using Business.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;

namespace ProcessingImages;

public partial class ResultPage
{
    private List<RenyiData> datas;
    private string preparationName;
    private string increasePicker;
    private PlotModel plot;

    public bool IsRefreshing { get; set; }
    public ObservableCollection<RenyiData> RenyiDatas { get; set; } = new();
    public Command RefreshCommand { get; set; }

    public ResultPage(List<RenyiData> datas, string preparationName, string increasePicker)
    {
        this.datas = datas;
        this.preparationName = preparationName;
        this.increasePicker = increasePicker;

        RefreshCommand = new Command(() =>
        {
            LoadRenyiDatas();

            IsRefreshing = true;
            OnPropertyChanged(nameof(IsRefreshing));
        });

        BindingContext = this;

        InitializeComponent();

        this.Loaded += PanModePage_Loaded;
    }

    private void PanModePage_Loaded(object sender, EventArgs e)
    {
        plotView1.Model = CreateNormalDistributionModel();
        plotView1.Model = CreateNormalDistributionModel();
    }

    public PlotModel CreateNormalDistributionModel()
    {
        plot = new PlotModel
        {
            Title = $"Результат исследования препара {preparationName} методом \"Cпектр обобщенных размерностей Реньи\"",
            DefaultFontSize = 20 
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

    private async void DownloadClicked(object sender, EventArgs e)
    {
        try
        {
            String home = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

            plot.Title = "Crystal";
            using FileStream stream = File.Create(Path.Combine(home, "crystal.pdf"));
            PdfExporter pdfExporter = new PdfExporter { Width = 600, Height = 400 };
            pdfExporter.Export(plot, stream);

            await DisplayAlert("Сохранение изображения", $"Данные сохранились успешно\nПуть к файлу: {home}\\crystal.pdf", "Хорошо");
        }
        catch
        {
            await DisplayAlert("Сохранение изображения", "Сохранение данных не удалось", "Хорошо");
        }
    }

    private async void GetInformationClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Информация", "Версия системы: 1.0.0\nОпубликовано: 2023.11.10", "Хорошо");
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        LoadRenyiDatas();
    }

    private void LoadRenyiDatas()
    {
        RenyiDatas.Clear();

        foreach (RenyiData data in datas)
        {
          RenyiDatas.Add(data);
        }
    }
}