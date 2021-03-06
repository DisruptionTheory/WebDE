﻿using System;

using SharpKit.JavaScript;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Dimension
    {
        public double width = 0;
        public double height = 0;
        public double depth = 0;

        public Dimension(double myWidth, double myHeight)
        {
            this.width = myWidth;
            this.height = myHeight;
        }

        public Dimension(double myWidth, double myHeight, double myDepth)
        {
            this.width = myWidth;
            this.height = myHeight;
            this.depth = myDepth;
        }

        /// <summary>
        /// Returns the largest dimension.
        /// </summary>
        /// <returns></returns>
        public double GetGreatest()
        {
            double returnVal = width;

            if (height > returnVal)
            {
                returnVal = height;
            }

            if (depth > returnVal)
            {
                returnVal = depth;
            }

            return returnVal;
        }
    }
}
