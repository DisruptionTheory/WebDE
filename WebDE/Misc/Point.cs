using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Point
    {
        public double x = 0;
        public double y = 0;

        public Point(double theX, double theY)
        {
            this.x = theX;
            this.y = theY;
        }

        public double Distance(Point point2)
        {
            //return Math.Abs(point2.x - this.x) + Math.Abs(point2.y - this.y);
            return Math.Sqrt( Math.Pow(point2.x - this.x, 2) + Math.Pow(point2.y - this.y, 2) );
        }
    }
}
