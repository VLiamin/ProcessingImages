using System.Windows;
using System.Windows.Controls;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame Main = new();

        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new MainPage();
            Content = Main;
        }
    }
}