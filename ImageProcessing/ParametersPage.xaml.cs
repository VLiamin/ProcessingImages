using ImageProcessing.Constants;
using ImageProcessing.Enums;
using ImageProcessing.Windows;
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
        private readonly BitmapImage image1;
        private readonly BitmapImage image2;
        private readonly Methods method;

        public ParametersPage(BitmapImage image1, Methods method, string drugName, BitmapImage image2 = null, string drugName2 = null)
        {
            this.image1 = image1;
            this.image2 = image2;
            this.method = method;

            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromRgb(179, 255, 255));

            imageCrystal.Source = this.image1;
            imageCrystal.Height = 200;

            imageCrystal2.Source = this.image2;
            imageCrystal2.Height = 200;

            Entry_DrugName.Text = drugName;
            Entry_DrugName2.Text = drugName2;
            //  IncreasePicker.Text = "40x";

            if (image2 is not null)
            {
                TextBox2.Visibility = Visibility.Visible;
                imageCrystal2.Visibility = Visibility.Visible;
                ResultSeveralWindowsButton.Visibility = Visibility.Visible;
            }
        }

        private void ResultOneWindowClicked(object sender, EventArgs e)
        {
            if (Entry_DrugName.Text is null)
            {
                MessageBox.Show("Вам необходимо заполнить все поля", "Недостаточно данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MainWindow.Main.Content = new ResultPage(image1, method, Entry_DrugName.Text, image2, Entry_DrugName2.Text);
            //, IncreasePicker.SelectedItem.ToString());
        }

        private void ResultSeveralWindowsClicked(object sender, EventArgs e)
        {
            if (Entry_DrugName.Text is null)
            {
                MessageBox.Show("Вам необходимо заполнить все поля", "Недостаточно данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SupportingWindow supportingWindow = new SupportingWindow();
            supportingWindow.Content = new ResultPage(image2, method, Entry_DrugName2.Text);
            supportingWindow.Show();

            MainWindow.Main.Content = new ResultPage(image1, method, Entry_DrugName.Text);
            //, IncreasePicker.SelectedItem.ToString());
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
