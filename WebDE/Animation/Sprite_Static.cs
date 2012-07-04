using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;

namespace WebDE.Animation
{
    [JsType(JsMode.Clr, Filename = "../scripts/Animation.js")]
    public partial class Sprite
    {
        private static List<Sprite> loadedSprites = new List<Sprite>();

        public static Sprite GetSpriteByName(string spriteName)
        {
            foreach (Sprite spr in loadedSprites)
            {
                if (spr.GetName() == spriteName)
                {
                    //we have to return a copy of the sprite, as each sprite running needs to be a unique instance
                    //return (Sprite)Helpah.Clone(spr);
                    return Helpah.Clone(spr).As<Sprite>();
                }
            }

            return null;
        }

        //create a new sprite intended for a single frame animation
        public static Sprite singleFrame(string name, int width, int height, string frameLocation, int xOffset, int yOffset)
        {
            AnimationFrame animfrm = new AnimationFrame(frameLocation, xOffset, yOffset);
            Animation annie = new Animation();
            annie.AddFrame(animfrm);

            Sprite newSprite = new Sprite(name);
            newSprite.setSize(width, height);
            newSprite.addAnimation(annie);

            return newSprite;
        }
    }
}
