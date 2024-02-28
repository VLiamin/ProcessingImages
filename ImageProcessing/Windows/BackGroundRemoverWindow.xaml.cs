using System.Windows;

namespace ImageProcessing.Windows
{
    /// <summary>
    /// Interaction logic for BackGroundRemoverWindow.xaml
    /// </summary>
    public partial class BackGroundRemoverWindow : Window
    {
        public BackGroundRemoverWindow(int x, int y)
        {
            InitializeComponent();

            X.Text = x.ToString();
            Y.Text = y.ToString();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string BackGroundPart
        {
            get { return backgroundPart.Text; }
        }

        public string BackX
        {
            get { return X.Text; }
        }

        public string BackY
        {
            get { return Y.Text; }
        }
    }
}
