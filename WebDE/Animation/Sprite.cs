using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;

namespace WebDE.Animation
{
    [JsType(JsMode.Clr, Filename = "../scripts/Animation.js")]
    public partial class Sprite
    {
        public string name = "";
        private List<Animation> animations = new List<Animation>();
        private int width = 40;
        private int height = 40;
        private Animation currentAnimation;
        private Animation defaultAnimation;
        private string currentRenderFrame = "";

        public Sprite(string spriteName)
        {
            this.name = spriteName;
            Sprite.loadedSprites.Add(this);
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string newName)
        {
            this.name = newName;
        }

        //called by the game clock, preforms the mutations necessary to advance the current animation
        public string Animate()
        {
            if (this.getCurrentAnimation() != null)
            {
                return this.getCurrentAnimation().Animate();
            }
            else
            {
                return "";
            }
        }

        //attempts to play an animation on this sprite once
        //returns true if the sprite can play the animation
        //returns false if the sprite cannot play the animation
        public bool playAnimation(string animationName)
        {
            foreach(Animation anim in this.animations) {
                if (anim.GetName() == animationName)
                {
                    //Animation whatever = new Animation();
                    //divSpriteDiv.Style.BackgroundImage = whatever.
                    return true;
                }
            }
            return false;
        }

        //sets the current animation to the provided one
        //returns true if the sprite has that animation
        //false if it doesn't
        public bool setAnimation(string animationName)
        {
            foreach (Animation anim in this.animations)
            {
                if (anim.GetName() == animationName)
                {
                    this.currentAnimation = anim;
                    return true;
                }
            }

            return false;
        }

        public void addAnimation(Animation animToAdd)
        {
            //Animation mayorMcCheese = (Animation)Helpah.Clone(animToAdd);
            //Object anmany = Helpah.Clone(animToAdd);
            //Animation mayorMcCheese = (Animation)anmany;
            animToAdd = Helpah.Clone(animToAdd).As<Animation>();
            //animToAdd = (Animation)Helpah.Clone(animToAdd);
            //this.animations.Add(animToAdd);
            this.animations.Add(animToAdd);
        }

        public Animation getCurrentAnimation()
        {
            if (this.currentAnimation == null)
            {
                if (this.defaultAnimation == null)
                {
                    this.defaultAnimation = this.animations[0];
                }

                this.currentAnimation = this.defaultAnimation;
            }

            return this.currentAnimation;
        }

        public Tuple<int, int> getSize()
        {
            Tuple<int, int> returnValue = new Tuple<int, int>
                (this.width, this.height);

            return returnValue;
        }

        public void setSize(int newWidth, int newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;
        }

        public string GetCurrentRenderFrame()
        {
            return this.currentRenderFrame;
        }

        public void SetCurrentRenderFrame(string newId)
        {
            this.currentRenderFrame = newId;
        }
    }
}
