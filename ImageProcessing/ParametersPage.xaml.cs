using ImageProcessing.Constants;
using ImageProcessing.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for RenyiPage.xaml
    /// </summary>
    public partial class ParametersPage : Page
    {
        private readonly BitmapImage image;
        private readonly Methods method;

        public ParametersPage(BitmapImage image, Methods method, string drugName)
        {
            this.image = image;
            this.method = method;

            InitializeComponent();
            this.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(179, 255, 255));

            imageCrystal.Source = image;
            imageCrystal.Height = 200;
            Entry_DrugName.Text = drugName;
            IncreasePicker.Text = "40x";
        }

        private void ResultsClicked(object sender, EventArgs e)
        {
            if (Entry_DrugName.Text is null || IncreasePicker.SelectedItem is null)
            {
                MessageBox.Show("Вам необходимо заполнить все поля", "Недостаточно данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MainWindow.Main.Content = new ResultPage(image, method, Entry_DrugName.Text);//, IncreasePicker.SelectedItem.ToString());
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
