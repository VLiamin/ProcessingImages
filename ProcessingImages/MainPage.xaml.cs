using System.Drawing;
using System.Net.Http.Headers;

namespace ProcessingImages
{
    public partial class MainPage : ContentPage
    {
        private static Bitmap image;
        private static string imagePath;
        private static object radioButton = "MFS";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnFilePickerClicked(object sender, EventArgs e)
        {
            try
            {
                PickOptions options = new()
                {
                    PickerTitle = "Select a file to upload"
                };

                FileResult fileResult = await FilePicker.Default.PickAsync(options);

                label.Source = fileResult.FullPath;
                label.HeightRequest = 200;
                image = new Bitmap(fileResult.FullPath);
                imagePath = fileResult.FullPath;

                if (fileResult is not null)
                {
                    bool result = await ProcessFile(fileResult);
                }
            }
            catch { }
        }

        private static async Task<bool> ProcessFile(FileResult fileResult)
        {
            if (fileResult is null)
            {
                return false;
            }

            using var fileStream = File.OpenRead(fileResult.FullPath);

            byte[] bytes;

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            bytes = memoryStream.ToArray();

            using var fileContent = new ByteArrayContent(bytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            using var form = new MultipartFormDataContent
            {
                { fileContent, "fileContent", Path.GetFileName(fileResult.FullPath) }
            };

            return true;
        }

        private async void OnMethodClicked(object sender, EventArgs e)
        {
            if (image is null)
            {
                await DisplayAlert("Image not found", "You haven't selected any images", "ok");

                return;
            }

            switch (radioButton.ToString())
            {
                case "Minkowski":
                    await Navigation.PushAsync(new MFSPage(image));
                    break;
                case "Renyi":
                    await Navigation.PushAsync(new RenyiPage(image, imagePath));
                    break;
            }
        }

        private void OnMethodCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton selectedRadioButton = ((RadioButton)sender);

            if (header is not null)
            {
                header.Text = $"Выбранный метод фрактального анализа изображения: {selectedRadioButton.Content}";
                radioButton = selectedRadioButton.Value;
            }
        }
    }
}