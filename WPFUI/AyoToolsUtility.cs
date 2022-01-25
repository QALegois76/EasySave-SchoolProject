using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Controls;

namespace AyoControlLibrary
{
    public enum ESideResize
    {
        All,
        X,
        Y,
    }
    public static class AyoToolsUtility
    {
        #region VARRIBALES
        private static Color _ayoYellow = Color.FromArgb(252, 192, 0);
        private static Color _ayoLightGray = Color.FromArgb(180, 166, 148);
        private static Color _ayoDarkGray = Color.FromArgb(25, 29, 31);
        private static Color _ayoGray = Color.FromArgb(75, 87, 95);


        #region public 
        public static Color AyoYellow { get => _ayoYellow; } 
        public static Color AyoLightGray { get => _ayoLightGray; }
        public static Color AyoDarkGray { get => _ayoDarkGray; }
        public static Color AyoGray { get => _ayoGray; }


        public const float Deg2Rad = 360f / (2f * (float)Math.PI);

        #endregion

        #endregion


        #region METHODS

        /// <summary>
        /// return the rectangle resize and allign in the middle
        /// </summary>
        /// <param name="ctrl">center in this control</param>
        /// <param name="rect">the rect to resize</param>
        /// <param name="zoom">the coefficient of resizing</param>
        /// <param name="eSide">the side to rezoom</param>
        /// <returns></returns>
        public static RectangleF ReZoom(this ContentControl ctrl, RectangleF rect, float zoom, ESideResize eSide = ESideResize.All)
        {
            RectangleF output = new RectangleF();

            if (eSide == ESideResize.All || eSide == ESideResize.X)
                output.Width = rect.Width * zoom;
            else
                output.Width = rect.Width;

            if (eSide == ESideResize.All || eSide == ESideResize.Y)
                output.Height = rect.Height * zoom;
            else
                output.Height = rect.Height;

            if (eSide == ESideResize.All || eSide == ESideResize.X)
                output.X = ((float)ctrl.Width -(float) output.Width) / 2f;
            else
                output.X = rect.X;

            if (eSide == ESideResize.All || eSide == ESideResize.Y)
                output.Y = ((float)ctrl.Height - (float)output.Height) / 2f;
            else
                output.Y = rect.Y;

            return output;
        }


        public static float Clamp(this float value, float min, float max)
        {
            if (value <= min)
                return min;
            else if (value >= max)
                return max;
            else
                return value;
        }
        
        public static int Clamp(this int value, int min, int max)
        {
            if (value <= min)
                return min;
            else if (value >= max)
                return max;
            else
                return value;
        }


        // return a point who center the control at the point given
        public static Point CenterControlAt(this Control ctrl, Point pt)
        {
            return new Point(pt.X - (int)ctrl.Width / 2, pt.Y - (int)ctrl.Height / 2);
        }

        public static Point GetCenterI(this Control ctrl)
        {
            return new Point((int)ctrl.ActualWidth / 2, (int)ctrl.ActualHeight / 2);
        }
        
        public static System.Windows.Point GetCenterD(this Control ctrl)
        {
            return new System.Windows.Point((double)ctrl.ActualWidth / 2d, (double)ctrl.ActualHeight / 2d);
        }



        public static Vector2 GetVectorFromPt(this Control ctrl, Point pt, Vector2 middle, bool norm = false, float fact = 1)
        {
            Vector2 output = new Vector2((float)pt.X - (float)middle.X, (float)(ctrl.Height - pt.Y) - (float)middle.Y);

            if (norm && !(output.X == 0 && output.Y == 0))
                output = Vector2.Normalize(output);
            output *= fact;

            return output;
        }


        public static Point GetPointFromVect(this Control ctrl,Vector2 vect, Vector2 middle)
        {
            return new Point((int)vect.X + (int)middle.X, (int)ctrl.Height - (int)vect.Y - (int)middle.Y);
        }


        #region convert region
        public static Rectangle ToRectangle(this RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static Point ToPoint(this PointF pt)
        {
            return new Point((int)pt.X, (int)pt.Y);
        }

        public static Point ToPoint(this Vector2 vect)
        {
            return new Point((int)vect.X, (int)vect.Y);
        }

        public static Vector2 ToVector2(this Point pt)
        {
            return new Vector2((float) pt.X,(float) pt.Y);
        }

        public static Vector2 ToVector2(this PointF pt)
        {
            return new Vector2((float)pt.X, (float)pt.Y);
        }



        public static int ToDecimal(this List<bool> boolean)
        {
            if (boolean == null || boolean.Count ==0)
                return -1;

            int result = 0;
            for (int i = 0; i < boolean.Count; i++)
            {
                result += (int)Math.Pow(2, i) * ((boolean[i]) ? 1 : 0);
            }
            return result;
        }

        public static int ToDecimal(this bool[] boolean) => boolean.ToList().ToDecimal();

        public static List<bool> ToBool(this List<int> list , int nBit = 0)
        {
            List<bool> output = new List<bool>();


            while (list.First() == 0 && list.Count > nBit)
            {
                list.RemoveAt(0);
            }
            foreach (int i in list)
            {
                if (i == 0)
                    output.Add(false);
                else
                    output.Add(true);
            }
            return output;
        }

        public static List<bool> ToBool(this int value, int nbBit = 0)
        {
            // NBA : corrigé
            List<bool> output = new List<bool>();
            int p = 1;
            for (int i = 0; i < nbBit; i++)
            {
                if ((value & p) == p)
                {
                    output.Add(true);
                }
                else
                    output.Add(false);

                p *= 2;
            }
            return output;
        }



        #endregion

        #endregion
    }
}
