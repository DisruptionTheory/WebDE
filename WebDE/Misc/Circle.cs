using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Circle
    {
        private double x;
        private double y;
        public double Radius;


        public Point Center { get { return new Point(x + (Width / 2), y + (Width / 2)); } }

        public double Width { get { return Radius * 2; } }
        public double Height { get { return Radius * 2; } }
        public double Right { get { return x + Width; } }
        public double Top { get { return y + Width; } }

        public Circle(double left, double top, double radius)
        {
            this.x = left;
            this.y = top;
            this.Radius = radius;
        }

        public bool Contains(Point point)
        {
            //return Math.Pow(point.x - Center.x, 2) + Math.Pow(y - Center.y, 2) <= (Radius * Radius);
            double square_dist = Math.Pow(Center.x - x, 2) + Math.Pow(Center.y - y, 2);
            return square_dist <= Math.Pow(Radius, 2);
        }
    }
}
