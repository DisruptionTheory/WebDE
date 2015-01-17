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

        public Dimension Size { get { return new Dimension(width, height); } }

        public Rectangle(double left, double top, double width, double height)
        {
            this.x = left;
            this.y = top;
            this.width = width;
            //remember that in the DOMiverse, lower on the screen = higher "height" value
            this.height = height;
        }

        public double Right { get { return this.x + this.width; } }
        public double Left { get { return this.x; } }
        public double Top { get { return this.y + this.height; } }
        public double Bottom { get { return this.y; } }

        public bool Contains(Point point)
        {
            if (point.x < this.x ||
                point.y < this.y ||
                point.x > this.x + this.width ||
                point.y > this.y + this.height)
            {
                return false;
            }

            return true;
        }

        public bool Contains(Rectangle rect)
        {
            if (rect.Right < this.x ||
                rect.x > this.Right ||
                rect.y > this.Top ||
                rect.Top < this.y)
            {
                return false;
            }

            return true;
        }

        public bool Above(Rectangle rect)
        {
            return this.Bottom > rect.Top;
        }

        public bool Below(Rectangle rect)
        {
            return this.Top < rect.Bottom;
        }
    }
}
