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
    /// Interaction logic for CircleButton.xaml
    /// </summary>
    public partial class CircleButton : UserControl , IClickable, IActivable
    { 

        private bool _isActiv = false;
        private bool _isOver = false;
        private bool _isDown = false;
        private bool _isAutoCheck = false;

        private double _radiusPourcent = 0.3;
        private double _thicknessBorderPourcent = 0.02;
        private double _thicknessRingPourcent = 0.1;
        private double _gapBorderPourcent = 0.025;
        private double _gapRingPourcent = 1;

        public event EventHandler OnActivStateChanged;
        public event EventHandler OnClick;

        public CircleButton()
        {
            InitializeComponent();
        }



                
        public bool IsActiv { get => _isActiv; set { bool trigger = _isActiv != value; _isActiv = value; InvalidateVisual(); if (trigger) OnActivStateChanged?.Invoke(this, EventArgs.Empty); } }
        public bool IsAutoCheck { get => _isAutoCheck; set => _isAutoCheck = value; }

        public double RadiusCenter { get => _radiusPourcent; set { _radiusPourcent = value; InvalidateVisual(); } }
        public double ThicknessRing { get => _thicknessRingPourcent; set { _thicknessRingPourcent = value; InvalidateVisual(); } }
        public double ThicknessBorder { get => _thicknessBorderPourcent; set { _thicknessBorderPourcent = value; InvalidateVisual(); } }
        public double GapBorder { get => _gapBorderPourcent; set { _gapBorderPourcent = value; InvalidateVisual();} }
        public double GapRing { get => _gapRingPourcent; set { _gapRingPourcent = value; InvalidateVisual();} }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // base.OnRender(drawingContext);
            Brush bYellow = new SolidColorBrush(AyoToolsUtility.AyoYellow);
            Brush bLight = new SolidColorBrush(AyoToolsUtility.AyoLightGray);
            Brush bDark = new SolidColorBrush((IsEnabled) ? AyoToolsUtility.AyoDarkGray: AyoToolsUtility.AyoMiddleGray);
            Brush bOver = new RadialGradientBrush(AyoToolsUtility.AyoGray, AyoToolsUtility.AyoLightGray);
            bOver.Opacity = 0.15;

            Pen penExt = new Pen(_isActiv ? bYellow :   bLight, _thicknessBorderPourcent * Width);
            Pen penRing = new Pen(_isActiv ? bYellow : bLight, _thicknessRingPourcent * Width);
            Pen penCenter = new Pen(bYellow, 1);

            Point center = this.GetCenter();
            Size fullSize = new Size(RenderSize.Width / 2, RenderSize.Height / 2);
            Size ringSize1 = new Size(fullSize.Width -(Width* _thicknessRingPourcent/2)- (_gapBorderPourcent * Width), fullSize.Height -(Height * _thicknessRingPourcent / 2) - (_gapBorderPourcent*Height));
            Size centerSize = new Size(RadiusCenter*Width, RadiusCenter*Height);


            EllipseGeometry ellipseExt = new EllipseGeometry(center,fullSize.Width,fullSize.Height);
            EllipseGeometry ellipseRing1 = new EllipseGeometry(center,ringSize1.Width,ringSize1.Height);
            

            EllipseGeometry ellipseCenter = new EllipseGeometry(center,centerSize.Width,centerSize.Height);

            drawingContext.DrawGeometry(bDark, penExt, ellipseExt);
            if (_thicknessRingPourcent >0)
                drawingContext.DrawGeometry(null, penRing, ellipseRing1.GetOutlinedPathGeometry());


            if (_isActiv)
            {
                drawingContext.DrawGeometry(bYellow, penCenter, ellipseCenter);
            }

            if(IsEnabled && _isOver && !_isDown)
                drawingContext.DrawGeometry(bOver, null, ellipseRing1);

        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (sizeInfo.NewSize.Width != sizeInfo.NewSize.Height)
            {
                double newSize = (sizeInfo.NewSize.Width + sizeInfo.NewSize.Height) / 2;
                this.RenderSize = new Size(newSize, newSize);
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (IsEnabled)
            {
                _isOver = true;
                InvalidateVisual();
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            _isOver = false;
            _isDown = false;
            InvalidateVisual();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (IsEnabled && _isOver)
            {
                _isDown = true;
                InvalidateVisual();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (_isDown && _isAutoCheck && IsEnabled)
            {
                _isActiv = !_isActiv;
                OnClick?.Invoke(this, EventArgs.Empty);
                OnActivStateChanged?.Invoke(this, EventArgs.Empty);
            }
            _isDown = false;
            InvalidateVisual();
            base.OnMouseUp(e);
        }
    }
}
