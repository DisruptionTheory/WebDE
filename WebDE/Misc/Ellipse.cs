using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Ellipse
    {
        private double x;
        private double y;
        private double height;
        private double width;

        public Point Center { get { return new Point(x + (width / 2), y + (height / 2)); } }

        public double Right { get { return x + width; } }
        public double Top { get { return y + height; } }

        public Ellipse(double left, double top, double width, double height)
        {
            this.x = left;
            this.y = top;
            this.height = height;
            this.width = width;
        }

        public bool Contains(Point point)
        {
            return (Math.Pow(point.x - Center.x, 2) / Math.Pow(this.width, 2)) +
                (Math.Pow(point.y - Center.y, 2) / Math.Pow(this.height, 2)) <= 1;
        }
    }
}
