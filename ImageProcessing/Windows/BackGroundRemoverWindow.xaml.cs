using System.Windows;

namespace ImageProcessing.Windows
{
    /// <summary>
    /// Interaction logic for BackGroundRemoverWindow.xaml
    /// </summary>
    public partial class BackGroundRemoverWindow : Window
    {
        public BackGroundRemoverWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string BackGroundPart
        {
            get { return backgroundPart.Text; }
        }
    }
}
