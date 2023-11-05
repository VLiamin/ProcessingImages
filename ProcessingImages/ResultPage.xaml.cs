using Business.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Maui.Skia.Core;
using OxyPlot.Series;
using System.Drawing;
using PlotCommands = OxyPlot.Maui.Skia.PlotCommands;

namespace ProcessingImages;

public partial class ResultPage
{
    private List<RenyiData> datas;
    private string preparationName;
    private string increasePicker;

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
        Results.Text = $"Результат исследования препара {preparationName} методом \"Cпектр обобщенных размерностей Реньи\"";
        PlotView.Model = CreateNormalDistributionModel();
    }

    public PlotModel CreateNormalDistributionModel()
    {
        // http://en.wikipedia.org/wiki/Normal_distribution

        var plot = new PlotModel
        {
            Title = "График",
        };

        plot.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Minimum = -0.05,
            Maximum = 1.05,
            MajorStep = 0.2,
            MinorStep = 0.05,
            TickStyle = TickStyle.Inside
        });
        plot.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Minimum = -5.25,
            Maximum = 5.25,
            MajorStep = 1,
            MinorStep = 0.25,
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
            return;

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

        PlotView.Controller.BindTouchDown(cmd);
    }
}