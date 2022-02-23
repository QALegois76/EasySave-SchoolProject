using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for TesteWindow.xaml
    /// </summary>
    public partial class TesteWindow : Window
    {
        private const int MAX = 100000;
        private const int MIN = 0;

        public TesteWindow()
        {
            InitializeComponent();

            Slider.Maximum = MAX;
            Slider.Minimum = MIN;

            Slider.ValueChanged -= Slider_ValueChanged;
            Slider.ValueChanged += Slider_ValueChanged;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RCtrl.ProgressPourcent = (double)Slider.Value / (double)(MAX - MIN);
            RCtrl_Copy.ProgressPourcent = (double)Slider.Value / (double)(MAX - MIN);
            RCtrl_Copy1.ProgressPourcent = (double)Slider.Value / (double)(MAX - MIN);
            RCtrl_Copy2.ProgressPourcent = (double)Slider.Value / (double)(MAX - MIN);
        }
    }
}
