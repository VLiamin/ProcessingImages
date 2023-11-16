using Business.Methods;
using Business.Models;
using System.Drawing;

namespace ProcessingImages;

public partial class RenyiPage : ContentPage
{
    private readonly Bitmap image;
    private readonly string imagePath;

    public RenyiPage(Bitmap image, string imagePath)
    {
        this.image = image;
        this.imagePath = imagePath;

        InitializeComponent();

        label2.Source = imagePath;
        label2.HeightRequest = 200;
    }

    private async void ResultsClicked(object sender, EventArgs e)
    {
        if (Entry_DrugName.Text is null || IncreasePicker.SelectedItem is null)
        {
            await DisplayAlert("Not enough data", "You must complete all fields", "ok");
            return;
        }

        CountRenyi countRenyi = new CountRenyi();
        List<RenyiData> result = countRenyi.CountMFSMethod(image, n: 3);

        await Navigation.PushAsync(new ResultPage(result, Entry_DrugName.Text, IncreasePicker.SelectedItem.ToString()));
    }

    private async void GetInformationClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Информация", "Версия системы: 1.0.0\nОпубликовано: 2023.11.10", "Хорошо");
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
}