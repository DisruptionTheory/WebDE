using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.Animation
{
    [JsType(JsMode.Clr, Filename = "../scripts/Animation.js")]
    public partial class AnimationFrame
    {
        //the id of the frame
        private string id;
        //the image to use for this frame
        private string AnimImage;
        //private ImageElement animImage;
        //the horizontal offset within the 
        private int imageX = 0;
        private int imageY = 0;

        //how many frames to display this frame for
        private int displayLength = 25;  //default to 100

        //constructor
        public AnimationFrame(string imageLocation, int offsetX, int offsetY)
        {
            this.id = "animFrame_" + animFrames.Count.ToString();

            AnimImage = imageLocation;
            imageX = offsetX;
            imageY = offsetY;

            animFrames.Add(this);
        }

        public string getId()
        {
            return this.id;
        }

        public int getDisplayLength()
        {
            return this.displayLength;
        }

        public void setDisplayLength(int newLength)
        {
            this.displayLength = newLength;
        }

        public string getImage()
        {
            return AnimImage;
        }

        public Tuple<int, int> getPosition()
        {
            Tuple<int, int> returnVal = new Tuple<int, int>
                (this.imageX, this.imageY);

            return returnVal;
        }
        
        public void markAsCached()
        {
            if (AnimationFrame.cachedFrames.Contains(this) == false)
            {
                AnimationFrame.cachedFrames.Add(this);
            }            
        }
    }
}
