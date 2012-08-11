using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Color
    {
        private string name = "";
        public int red = 0;
        public int green = 0;
        public int blue = 0;

        public Color(int redVal, int greenVal, int blueVal)
        {
            this.red = redVal;
            this.green = greenVal;
            this.blue = blueVal;
        }

        public string GetHex()
        {
            //I think something might be messed up with the way hex is interpreted
            //(red, blue, green rather than the norm)
            //normally, I'd blame some foreign GameEntity in a show of blatant ignorant patriotism,
            //but this has Micro$oft written all over it

            string returnString = "";

            //divide red by 16 to get the left #
            double redDub = red / 16;
            returnString += ToHex((int)Helpah.Round(redDub));
            //red mod 16 is the right #
            redDub = red % 16;
            returnString += ToHex((int)Helpah.Round(redDub));

            double blueDub = blue / 16;
            returnString += ToHex((int)Helpah.Round(blueDub));
            blueDub = blue % 16;
            returnString += ToHex((int)Helpah.Round(blueDub));

            double greenDub = green / 16;
            returnString += ToHex((int)Helpah.Round(greenDub));
            greenDub = green % 16;
            returnString += ToHex((int)Helpah.Round(greenDub));

            return returnString;
        }

        public bool Match(Color colorTomatch)
        {
            if (colorTomatch == null)
            {
                return false;
            }

            if (this.red != colorTomatch.red ||
                this.green != colorTomatch.green ||
                this.blue != colorTomatch.blue)
            {
                return false;
            }

            return true;
        }

        public static string ToHex(int val)
        {
            string hexstuff = "ABCDEF";

            if (val < 10)
            {
                return val.ToString();
            }
            else
            {
                val -= 10;
                return hexstuff[val].ToString();
            }
        }

        public static Color FromHex(string hexValue)
        {
            //convert the hex to RGB
            return new Color(0, 0, 0);
        }

        public bool IsOpposite(Color otherColor)
        {
            //convert both colors to HSL...

            return true;
        }

        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(255, 255, 255);
    }
}
