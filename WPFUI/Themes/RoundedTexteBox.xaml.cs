using AyoControlLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUI.Themes
{
    /// <summary>
    /// Interaction logic for RoundedTexteBox.xaml
    /// </summary>
    public partial class RoundedTexteBox : UserControl
    {
        public event TextChangedEventHandler TextChanged { add => textBox.TextChanged += value; remove => textBox.TextChanged -= value; }

        public string Text { get => textBox.Text; set => textBox.Text = value; }


        // accessor
        public double SizeText { get => textBox.FontSize; set { textBox.FontSize = value; InvalidateVisual(); } }


        public int CornerRadius { get => Back.CornerRadius; set { Back.CornerRadius = value; InvalidateVisual(); } }
        public int BorderSize { get => Back.BorderSize; set {Back.BorderSize = value; InvalidateVisual(); } }


        public ERoundedType RoundedType { get => Back.RoundedType; set { Back.RoundedType = value; InvalidateVisual(); } }
        public ERoundedFlag RoundedFlag { get => Back.RoundedFlag; set { Back.RoundedFlag = value; InvalidateVisual(); } }


        public Color? ColorBackEnable { get => Back.ColorBackEnable; set { Back.ColorBackEnable = value; InvalidateVisual(); } }
        public Color? ColorBackDisable { get => Back.ColorBackDisable; set { Back.ColorBackDisable = value; InvalidateVisual(); } }
        public Color? ColorBackOver { get => Back.ColorBackOver; set { Back.ColorBackOver = value; InvalidateVisual(); } }
        public Color? ColorBackDown { get => Back.ColorBackDown; set { Back.ColorBackDown = value; InvalidateVisual(); } }
        public Color? ColorBackActiv { get => Back.ColorBackActiv; set { Back.ColorBackActiv = value; InvalidateVisual(); } }
        public Color? ColorBorderEnable { get => Back.ColorBorderEnable; set { Back.ColorBorderEnable = value; InvalidateVisual(); } }
        public Color? ColorBorderDisable { get => Back.ColorBorderEnable; set { Back.ColorBorderEnable = value; InvalidateVisual(); } }
        public Color? ColorBorderOver { get => Back.ColorBorderEnable; set { Back.ColorBorderEnable = value; InvalidateVisual(); } }
        public Color? ColorBorderDown { get => Back.ColorBorderDown; set { Back.ColorBorderDown = value; InvalidateVisual(); } }
        public Color? ColorBorderActiv { get => Back.ColorBorderActiv; set { Back.ColorBorderActiv = value; InvalidateVisual(); } }


        public RoundedTexteBox()
        {
            InitializeComponent();
        }
    }
}
