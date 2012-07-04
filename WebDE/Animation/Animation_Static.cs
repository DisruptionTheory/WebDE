using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

//this is something to take note of:
//https://developer.mozilla.org/en/CSS/CSS_animations
//namely, the different reasons why CSS transformations are optimal when applicable
//(the question then being their performance over statically animated images such as gifs)

namespace WebDE.Animation
{
    [JsType(JsMode.Clr, Filename = "../scripts/Animation.js")]
    partial class Animation
    {
        //create a new animation from a single sprite sheet with each x and y location being a new frame
        //public static Animation fromSpriteSheet(string imgSheetLocation, List<int, int> framePositions)
        //public static Animation fromSpriteSheet(string imgSheetLocation, int frameWidth, int frameHeight, int[,] framePositions)
        public static Animation FromSpriteSheet(string imgSheetLocation, int frameWidth, int frameHeight, List<Point> framePositions)
        {
            Animation resultAnim = new Animation();

            foreach (Point point in framePositions)
            {
                AnimationFrame animFrame = new AnimationFrame(
                    imgSheetLocation, 
                    (int) Math.Round(point.x), 
                    (int) Math.Round(point.y));
                resultAnim.AddFrame(animFrame);
            }

            return resultAnim;
        }

        public static Animation SingleFrame(AnimationFrame frame)
        {
            Animation returnAnim = new Animation();
            returnAnim.AddFrame(frame);
            return returnAnim;
        }
    }
}
