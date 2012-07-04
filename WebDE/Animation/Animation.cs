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
    public partial class Animation
    {
        private List<AnimationFrame> frames = new List<AnimationFrame>();
        private string name = "New Animation";
        //the current frame number (out of how many numbers to display the current animation frame)
        private int currentFrameNum = 0;
        private AnimationFrame currentFrame = null;

        public Animation()
        {
        }

        public string Animate()
        {
            //this could be the first time this gets called, so make sure there's an animation loaded
            if (this.currentFrame == null)
            {
                this.currentFrameNum = 0;
                this.currentFrame = this.frames[0];
                return this.currentFrame.getId();
            }

            //if the animation frame is done being displayed, move on to the next frame in the animation
            if (currentFrameNum >= this.currentFrame.getDisplayLength())
            {
                this.nextFrame();
            }
            else
            {
                //mark the frame as having been shown for another render cycle
                currentFrameNum++;
            }

            //return the id of the current frame being displayed
            return this.currentFrame.getId();
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string newName)
        {
            this.name = newName;
        }

        public bool AddFrame(AnimationFrame sourceFrame)
        {
            this.frames.Add(sourceFrame);
            return true;
        }

        public bool RemoveFrame()
        {
            return true;
        }

        public AnimationFrame GetCurrentFrame()
        {
            return this.currentFrame;
        }

        //change the animation to display the next frame
        private void nextFrame()
        {
            //whether or not we found a new frame yet
            bool foundFrame = false;

            //if there's another frame after this one, go to that
            for (int i = 0; i < frames.Count; i++)
            {
                //if this is the current frame and there's another after
                if (frames[i] == this.currentFrame && i < frames.Count - 1)
                {
                    this.currentFrame = this.frames[i + 1];
                    foundFrame = true;
                }
            }

            //there's no frame after the current one, so start back at the first frame
            if (foundFrame == false)
            {
                this.currentFrame = this.frames[0];
            }

            //reset the number of times the frame has been displayed
            this.currentFrameNum = 0;
        }

        /*
         * CSS Animations
         */

        public Animation cssAnimation()
        {
            return null;
        }
    }
}
