using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../../Lights/scripts/Lightstone.js")]
    public class Lightstone : LightSource
    {
        public Lightstone(double x, double y, double lightness, double distance)
            : base(x, y, lightness, distance)
        {
        }
    }
}