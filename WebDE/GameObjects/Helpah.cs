using System;

using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace WebDE.GameObjects
{
    //used to subdivide a stage for easier calculations
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Helpah : JsContextBase
    {
        public static object Clone(object o)
        {
            if(o == null) {
                return null;
            }
            return jQuery.extend(true, new object(), o);
        }

        public static void Destroy(object o)
        {
            o = null;
        }

        public static int Round(double number)
        {
            string numString = number.ToString();
            if (numString.IndexOf(".") > -1)
            {
                numString = numString.Substring(0, numString.IndexOf("."));
            }

            return int.Parse(numString);
        }

        public static double Round(double number, int decimalPlaces)
        {
            string numString = number.ToString();
            if (numString.IndexOf(".") > -1)
            {
                //get everything before the "."
                string numString1 = numString.Substring(0, numString.IndexOf("."));
                //get two spots after the "." (include the dot)
                string numString2 = numString.Substring(numString.IndexOf("."), decimalPlaces + 1);

                //add 'em up
                numString = numString1 + numString2;

                /*
                int beforeDec = int.Parse(numString.Substring(0, numString.IndexOf(".")));
                int afterDec = int.Parse(numString.Substring(numString.IndexOf("."), decimalPlaces + 1));
                double returnVal = beforeDec;
                returnVal += (afterDec / 10);
                */
            }

            return parseFloat(numString);
            //return Double.Parse(numString);
        }

        public static int Parse(string s)
        {
            int returnVal = 0;
            int i = 0;
            while (i < s.Length)
            {
                if (Char.IsDigit(s[i]))
                {
                    returnVal = returnVal * 10;
                    returnVal += s[i];
                }
                //should we break if it's not a digit?
                i++;
            }

            return returnVal;
        }

        public static int Rand(int min, int max)
        {
            double bit = min + (int)(JsMath.random() * ((max - min) + 1));
            return (int) Math.Floor(bit);
        }

        /// <summary>
        /// Double to int
        /// </summary>
        /// <param name="dubbs"></param>
        /// <returns></returns>
        public static int d2i(double dubbs)
        {
            return int.Parse(dubbs.ToString());
        }
    }
}
