using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.GameObjects
{
    //used to subdivide a stage for easier calculations
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Area
    {
        private int x;
        private int y;
        private int width;
        private int height;

        public Area(int left, int top, int horizSize, int vertSize)
        {
            this.x = left;
            this.y = top;
            this.width = horizSize;
            this.height = vertSize;
        }

        public Boolean Contains(double left, double top)
        {
            if ((left >= x && left <= (x + width)) &&
                (top >= y && top <= (y + height)))
            {
                return true;
            }
            return false;
        }
    }
}
