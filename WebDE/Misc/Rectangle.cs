using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Rectangle
    {
        public double x;
        public double y;
        public double width;
        public double height;

        public Rectangle(double left, double top, double width, double height)
        {
            this.x = left;
            this.y = top;
            this.width = width;
            //remember that in the DOMiverse, lower on the screen = higher "height" value
            this.height = height;
        }

        public double Right()
        {
            return this.x + this.width;
        }

        public double Bottom()
        {
            return this.y + this.height;
        }

        public bool Contains(Point point)
        {
            if (point.x < this.x ||
                point.y < this.y ||
                point.x > this.width ||
                point.y > this.height)
            {
                return false;
            }

            return true;
        }
    }
}
