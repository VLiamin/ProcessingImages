using System.Windows;

namespace ImageProcessing.Windows
{
    /// <summary>
    /// Логика взаимодействия для RotateWindow.xaml
    /// </summary>
    public partial class RotateWindow : Window
    {
        public RotateWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string AngleToRotate
        {
            get { return Angle.Text; }
        }
    }
}
