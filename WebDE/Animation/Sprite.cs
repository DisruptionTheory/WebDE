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
        private Dimension size = new Dimension(40, 40);
        private Animation currentAnimation;
        private Animation defaultAnimation;
        private string currentRenderFrame = "";
        private bool scaleToSize = false;

        public string Name
        { 
            get { return this.name; }
            set { this.name = value; }
        }
        public Dimension Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
        public string CurrentRenderFrame
        {
            get { return this.currentRenderFrame; }
            set { this.currentRenderFrame = value; }
        }
        public bool ScaleToSize
        {
            get { return this.scaleToSize; }
            set { this.scaleToSize = value; }
        }


        public Sprite(string spriteName)
        {
            this.name = spriteName;
            Sprite.loadedSprites.Add(this);
        }

        public Sprite(string spriteName, string fileName)
        {
            this.name = spriteName;
            AnimationFrame animFrame = new AnimationFrame(fileName, 0, 0);
            Animation anim = new Animation();
            anim.SetName("Idle");
            anim.AddFrame(animFrame);
            this.AddAnimation(anim);

            Sprite.loadedSprites.Add(this);
        }

        //called by the game clock, preforms the mutations necessary to advance the current animation
        public string Animate()
        {
            if (this.GetCurrentAnimation() != null)
            {
                return this.GetCurrentAnimation().Animate();
            }
            else
            {
                return "";
            }
        }

        //attempts to play an animation on this sprite once
        //returns true if the sprite can play the animation
        //returns false if the sprite cannot play the animation
        public bool PlayAnimation(string animationName)
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
        public bool SetAnimation(string animationName)
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

        public void AddAnimation(Animation animToAdd)
        {
            //Animation mayorMcCheese = (Animation)Helpah.Clone(animToAdd);
            //Object anmany = Helpah.Clone(animToAdd);
            //Animation mayorMcCheese = (Animation)anmany;
            animToAdd = Helpah.Clone(animToAdd).As<Animation>();
            //animToAdd = (Animation)Helpah.Clone(animToAdd);
            //this.animations.Add(animToAdd);
            this.animations.Add(animToAdd);
        }

        public Animation GetCurrentAnimation()
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

        public void SetSize(int width, int height)
        {
            this.Size = new Dimension(width, height);
        }
    }
}
