﻿using System.Windows;

namespace ImageProcessing.Windows
{
    /// <summary>
    /// Логика взаимодействия для Binary.xaml
    /// </summary>
    public partial class Binary : Window
    {
        public Binary()
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
