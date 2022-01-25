using System;
//using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media;
using System.Windows.Shapes;


namespace AyoControlLibrary
{
    public enum EActiveMarkerPosition
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public enum ERoundedType
    {
        None,
        Left,
        Right,
        Top,
        Bottom,
        All
    }

    public enum EButtonBehaviorMode
    {
        Normal,
        Toggle,
        Grouped
    }

    public enum EActiveMarkerShape
    {
        Linebar,
        Border,
        Dot
    }

    public enum EGroupButtonOrientation
    {
        Horizontal,
        Vertical
    }

    public static class AyoControlHelpers
    {
        //public static void DisplayText(DrawingContext e, Rectangle rect, string text, Font font, Color textColor, ContentAlignment textAlignment = ContentAlignment.MiddleCenter, bool withEndEllipsis = true)
        //{
        //    if (!string.IsNullOrEmpty(text.Trim()))
        //    {
        //        TextFormatFlags flags = TextFormatFlags.Default;
        //        switch (textAlignment)
        //        {
        //            case ContentAlignment.TopLeft:
        //                flags = TextFormatFlags.Top | TextFormatFlags.Left;
        //                break;

        //            case ContentAlignment.TopCenter:
        //                flags = TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
        //                break;

        //            case ContentAlignment.TopRight:
        //                flags = TextFormatFlags.Top | TextFormatFlags.Right;
        //                break;

        //            case ContentAlignment.MiddleLeft:
        //                flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
        //                break;

        //            case ContentAlignment.MiddleCenter:
        //                flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
        //                break;

        //            case ContentAlignment.MiddleRight:
        //                flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
        //                break;

        //            case ContentAlignment.BottomLeft:
        //                flags = TextFormatFlags.Bottom | TextFormatFlags.Left;
        //                break;

        //            case ContentAlignment.BottomCenter:
        //                flags = TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
        //                break;

        //            case ContentAlignment.BottomRight:
        //                flags = TextFormatFlags.Bottom | TextFormatFlags.Right;
        //                break;

        //            default:
        //                break;
        //        }
        //        if (withEndEllipsis)
        //        {
        //            flags |= TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis;
        //        }
        //        TextRenderer.DrawText(e, text, font, rect, textColor, flags);
        //    }
        //}

        public static PathGeometry GenerateBorder(ERoundedType type, int radius, Rectangle rect, int penSize)
        {
            PathGeometry res = new PathGeometry();
            
            switch (type)
            {
                case ERoundedType.None:
                    //if (penSize > 1) rect = new Rectangle()
                    //res.AddRectangle(rect);
                    break;

                case ERoundedType.Left:
                   // res = AyoControlHelpers.GenerateLeftRoundedPath(rect, radius, penSize);
                    break;

                case ERoundedType.Right:
                   // res = AyoControlHelpers.GenerateRightRoundedPath(rect, radius, penSize);
                    break;

                case ERoundedType.Top:
                  //  res = AyoControlHelpers.GenerateTopRoundedPath(rect, radius, penSize);
                    break;

                case ERoundedType.Bottom:
                    res = AyoControlHelpers.GenerateBottomRoundedPath(rect, radius, penSize);
                    break;

                case ERoundedType.All:
                  //  res = AyoControlHelpers.GenerateRoundedBorderPath(rect, radius, penSize);
                    break;

                default:
                    break;
            }

            return res;
        }

        public static PathGeometry GenerateBottomRoundedPath(Rectangle rect, int radius, int penSize = 1)
        {
            if (rect == null) throw new ArgumentNullException(nameof(rect));
            if (radius <= 0) throw new ArgumentException("Radius must be greater than 0.");
            PathGeometry graphPath = new PathGeometry();
            int offset = penSize / 2;

            // line at bottom
            Line line = new Line();
            line.X1 = rect.Margin.Left + offset;
            line.Y1 = rect.Margin.Top + offset;
            line.X2 = rect.Margin.Left + rect.Width - offset;
            line.Y2 = rect.Margin.Top + offset;
            
            BezierSegment bezier1 = new BezierSegment();
            bezier1.Point1 = new System.Windows.Point(rect.Width + rect.Margin.Left - offset, rect.Margin.Top + offset);
            bezier1.Point2 = new System.Windows.Point(rect.Width + rect.Margin.Left - offset, rect.Margin.Top + rect.Height + offset);
            bezier1.Point3 = new System.Windows.Point(rect.Margin.Left + offset, rect.Margin.Top + rect.Height + offset);
            
            
            BezierSegment bezier2 = new BezierSegment();
            bezier2.Point1 = new System.Windows.Point(rect.Width + rect.Margin.Left - offset, rect.Margin.Top + rect.Height + offset);
            bezier2.Point2 = new System.Windows.Point(rect.Margin.Left + offset, rect.Margin.Top + rect.Height + offset);
            bezier2.Point3 = new System.Windows.Point(rect.Margin.Left + offset, rect.Margin.Top + offset);


            //graphPath.AddArc(rect.X + rect.Width - radius - offset, rect.Y + rect.Height - radius - offset, radius, radius, 0, 90);
            //graphPath.AddArc(rect.X + offset, rect.Y + rect.Height - radius - offset, radius, radius, 90, 90);
            //graphPath.CloseFigure();


           // graphPath.
            return graphPath;
        }

        //public static PathGeometry GenerateLeftRoundedPath(Rectangle rect, int radius, int penSize = 1)
        //{
        //    if (rect == null) throw new ArgumentNullException(nameof(rect));
        //    if (radius <= 0) throw new ArgumentException("Radius must be greater than 0.");
        //    PathGeometry graphPath = new PathGeometry();
        //    int offset = penSize / 2;

        //    graphPath.AddArc(rect.X + offset, rect.Y + offset, radius, radius, 180, 90);
        //    graphPath.AddLine(rect.X + rect.Width - offset, rect.Y + offset, rect.X + rect.Width - offset, rect.Y + rect.Height - offset);
        //    graphPath.AddArc(rect.X + offset, rect.Y + rect.Height - radius - offset, radius, radius, 90, 90);
        //    graphPath.CloseFigure();

        //    return graphPath;
        //}

        //public static PathGeometry GenerateRightRoundedPath(Rectangle rect, int radius, int penSize = 1)
        //{
        //    if (rect == null) throw new ArgumentNullException(nameof(rect));
        //    if (radius <= 0) throw new ArgumentException("Radius must be greater than 0.");
        //    PathGeometry graphPath = new PathGeometry();
        //    int offset = penSize / 2;

        //    graphPath.AddLine(rect.X + offset, rect.Y + rect.Height - offset, rect.X + offset, rect.Y + offset);
        //    graphPath.AddArc(rect.X + rect.Width - radius - offset, rect.Y + offset, radius, radius, 270, 90);
        //    graphPath.AddArc(rect.X + rect.Width - radius - offset, rect.Y + rect.Height - radius - offset, radius, radius, 0, 90);
        //    graphPath.CloseFigure();

        //    return graphPath;
        //}

        //public static PathGeometry GenerateRoundedBorderPath(Rectangle rect, int radius, int penSize = 1)
        //{
        //    if (rect == null) throw new ArgumentNullException(nameof(rect));
        //    if (radius <= 0) throw new ArgumentException("Radius must be greater than 0.");
        //    PathGeometry graphPath = new PathGeometry();
        //    int offset = penSize / 2;

        //    graphPath.AddArc(rect.X + offset, rect.Y + offset, radius, radius, 180, 90);
        //    graphPath.AddArc(rect.X + rect.Width - radius - offset, rect.Y + offset, radius, radius, 270, 90);
        //    graphPath.AddArc(rect.X + rect.Width - radius - offset, rect.Y + rect.Height - radius - offset, radius, radius, 0, 90);
        //    graphPath.AddArc(rect.X + offset, rect.Y + rect.Height - radius - offset, radius, radius, 90, 90);
        //    graphPath.CloseFigure();

        //    return graphPath;
        //}

        //public static PathGeometry GenerateTopRoundedPath(Rectangle rect, int radius, int penSize = 1)
        //{
        //    if (rect == null) throw new ArgumentNullException(nameof(rect));
        //    if (radius <= 0) throw new ArgumentException("Radius must be greater than 0.");
        //    PathGeometry graphPath = new PathGeometry();
        //    int offset = penSize / 2;

        //    graphPath.AddArc(rect.X + offset, rect.Y + offset, radius, radius, 180, 90);
        //    graphPath.AddArc(rect.X + rect.Width - radius - offset, rect.Y + offset, radius, radius, 270, 90);
        //    graphPath.AddLine(rect.X + rect.Width - offset, rect.Y + rect.Height - offset, rect.X + offset, rect.Y + rect.Height - offset);
        //    graphPath.CloseFigure();

        //    return graphPath;
        //}
    }
}