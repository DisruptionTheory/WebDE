using System;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Dimension
    {
        public double width = 0;
        public double height = 0;
        public double depth = 0;

        public Dimension(double myWidth, double myHeight, double myDepth)
        {
            this.width = myWidth;
            this.height = myHeight;
            this.depth = myDepth;
        }

        public Tuple<double, double> ToTuple()
        {
            Tuple<double, double> returnVal = new Tuple<double, double>
                (this.width, this.height);

            return returnVal;
        }
    }
}
