using AyoControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InvalidateVisual();
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            PathGeometry pathGeometry = new PathGeometry();

            double radius = 100;

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(0,radius);
            pathFigure.IsClosed = true;

            BezierSegment bezierSegment = new BezierSegment();

            bezierSegment.Point1 = new Point(0, 0);
            //bezierSegment.Point1 = new Point(0, 0);
            //bezierSegment.Point1 = new Point(Width, Height);

            bezierSegment.Point2 = new Point(0, 0);
            //bezierSegment.Point2 = new Point(0, 0);
            //bezierSegment.Point2 = new Point(Width, Height);

            //bezierSegment.Point3 = new Point(0,Height);
            //bezierSegment.Point3 = new Point(0, 0);
            bezierSegment.Point3 = new Point(radius, 0);

            pathFigure.Segments.Add(bezierSegment);

            // add top segment
            pathFigure.Segments.Add(new LineSegment(new Point(Width - radius, 0),true));

            // create and add right rounded corner
            BezierSegment bezierSegmentRight = new BezierSegment();
            bezierSegmentRight.Point1 = new Point(Width, 0);
            bezierSegmentRight.Point2 = new Point(Width, 0);
            bezierSegmentRight.Point3 = new Point(Width, radius);

            pathFigure.Segments.Add(bezierSegmentRight);


            // add right segment
            pathFigure.Segments.Add(new LineSegment(new Point(Width, Height), true));
            
            
            // add bottom segment
            pathFigure.Segments.Add(new LineSegment(new Point(0, Height), true));
            
            
            // add left segment
            pathFigure.Segments.Add(new LineSegment(new Point(0, radius), true));



            //BezierSegment bezierSegment2 = new BezierSegment();
            //bezierSegment2.Point2 = new Point(0, 0);
            //bezierSegment2.Point1 = new Point(Width, 0);
            //bezierSegment2.Point3 = new Point(Width, Height);
            //pathFigure.Segments.Add(bezierSegment2);


            //Rectangle rectangle = new Rectangle();
            //rectangle.Width = Width;
            //rectangle.Height = Height;
            //rectangle.Margin = Margin;
            pathGeometry.Figures.Add(pathFigure);
            Clip = pathGeometry;

        }



        private void BtnMax_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Maximized;


        private void btn_min_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void btn_quit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();





        private void btn_min_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void btn_min_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void BtnMax_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btn_max_MouseLeave(object sender, MouseEventArgs e)
        {

        }



        private void btn_quit_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btn_quit_MouseLeave(object sender, MouseEventArgs e)
        {

        }

    }
}
