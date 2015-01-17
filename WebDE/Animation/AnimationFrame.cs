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
        private int displayLength = 5;  //default to 5

        //constructor
        public AnimationFrame(string imageLocation, int offsetX, int offsetY)
        {
            this.id = "animFrame_" + animFrames.Count.ToString();

            AnimImage = imageLocation;
            imageX = offsetX;
            imageY = offsetY;

            animFrames.Add(this);
        }

        public string Id { get { return this.id; }}
        public string Image { get { return this.AnimImage; } }
        public Point Position { get { return new Point(this.imageX, this.imageY); } }
        //public bool Cached { get; private set; } 
        public bool Cached { get; set; } 
        public int DisplayLength
        { 
            get { return this.displayLength; } 
            set { this.displayLength = value; }
        }
        
        public void Cache()
        {
            if (AnimationFrame.cachedFrames.Contains(this) == false)
            {
                AnimationFrame.cachedFrames.Add(this);
            }            
        }
    }
}
