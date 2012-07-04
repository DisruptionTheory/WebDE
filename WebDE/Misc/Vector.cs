using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    //a direction and magnitude
    public class Vector
    {
        //the horizontal magnitude
        //positive is to the right of the Y-axis, negative is to the left
        public double x = 0;
        //vertical magnitude
        public double y = 0;

        public Vector(double xMagnitude, double yMagnitude)
        {
            this.x = xMagnitude;
            this.y = yMagnitude;
        }

        public static double Distance(Vector vector1, Vector vector2)
        {
            return Math.Abs(vector2.y - vector1.y) + Math.Abs(vector2.x - vector1.x);
        }
    }
}
