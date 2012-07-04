using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.Animation
{
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public partial class AnimationFrame
    {
        private static List<AnimationFrame> cachedFrames = new List<AnimationFrame>();
        //private static List<ImageElement> cachedImages = new List<ImageElement>();
        private static List<AnimationFrame> animFrames = new List<AnimationFrame>();

        public static List<AnimationFrame> GetAnimationFrames()
        {
            return animFrames;
        }
        
        //public List<AnimationFrame> getCachedFrames

        //checks if a frame is cached based on information passed to it
        //returns the frame if it is cached, returns null if it isn't
        public static AnimationFrame IsFrameCached(string imageLocation, int offsetX, int offsetY)
        {
            foreach (AnimationFrame frame in cachedFrames)
            {
                if (frame.AnimImage == imageLocation && frame.imageX == offsetX && frame.imageY == offsetY)
                {
                    return frame;
                }
            }

            return null;
        }

        /*
        //checks if an image is cached based on the image name passed to it
        //returns the image if it is cached, null if it isn't
        public static ImageElement isImageCached(string imageLocation)
        {
            foreach(ImageElement img in cachedImages) {
                if (img.Src == imageLocation)
                {
                    return img;
                }
            }

            return null;
        }
        */
    }
}
