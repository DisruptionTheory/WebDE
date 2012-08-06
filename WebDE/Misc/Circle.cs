using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Circle
    {
        public readonly double x;
        public readonly double y;
        public double radius;

        public Circle(double left, double top, double radius)
        {
            this.x = left;
            this.y = top;
            this.radius = radius;
        }

        public bool Contains(Point point)
        {
            /*
            if (point.x < this.x ||
                point.y < this.y ||
                point.x > this.size.width ||
                point.y > this.size.height)
            {
                return false;
            }

            return true;
             */

            return false;
        }
    }
}
