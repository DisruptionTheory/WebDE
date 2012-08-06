/*Generated by SharpKit 5 v4.28.1000*/
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var WebDE$Animation$Animation=
{
    fullname:"WebDE.Animation.Animation",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        FromSpriteSheet:function(imgSheetLocation,frameWidth,frameHeight,framePositions)
        {
            var resultAnim=new WebDE.Animation.Animation.ctor();
            var $it2=framePositions.GetEnumerator();
            while($it2.MoveNext())
            {
                var point=$it2.get_Current();
                var animFrame=new WebDE.Animation.AnimationFrame.ctor(imgSheetLocation,Cast(System.Math.Round$$Double(point.x),System.Int32),Cast(System.Math.Round$$Double(point.y),System.Int32));
                resultAnim.AddFrame(animFrame);
            }
            return resultAnim;
        },
        SingleFrame:function(frame)
        {
            var returnAnim=new WebDE.Animation.Animation.ctor();
            returnAnim.AddFrame(frame);
            return returnAnim;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            this.frames = new System.Collections.Generic.List$1.ctor(WebDE.Animation.AnimationFrame);
            this.name = "New Animation";
            this.currentFrameNum = 0;
            this.currentFrame = null;
            System.Object.ctor.call(this);
        },
        Animate:function()
        {
            if(this.currentFrame == null)
            {
                this.currentFrameNum = 0;
                this.currentFrame = this.frames.get_Item$$Int32(0);
                return this.currentFrame.getId();
            }
            if(this.currentFrameNum >= this.currentFrame.getDisplayLength())
            {
                this.nextFrame();
            }
            else
            {
                this.currentFrameNum++;
            }
            return this.currentFrame.getId();
        },
        GetName:function()
        {
            return this.name;
        },
        SetName:function(newName)
        {
            this.name = newName;
        },
        AddFrame:function(sourceFrame)
        {
            this.frames.Add(sourceFrame);
            return true;
        },
        RemoveFrame:function()
        {
            return true;
        },
        GetCurrentFrame:function()
        {
            return this.currentFrame;
        },
        nextFrame:function()
        {
            var foundFrame=false;
            for(var i=0;i < this.frames.get_Count();i++)
            {
                if(this.frames.get_Item$$Int32(i) == this.currentFrame && i < this.frames.get_Count() - 1)
                {
                    this.currentFrame = this.frames.get_Item$$Int32(i + 1);
                    foundFrame = true;
                }
            }
            if(foundFrame == false)
            {
                this.currentFrame = this.frames.get_Item$$Int32(0);
            }
            this.currentFrameNum = 0;
        },
        cssAnimation:function()
        {
            return null;
        }
    }
};
JsTypes.push(WebDE$Animation$Animation);
var WebDE$Animation$AnimationFrame=
{
    fullname:"WebDE.Animation.AnimationFrame",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Animation.AnimationFrame.cachedFrames = new System.Collections.Generic.List$1.ctor(WebDE.Animation.AnimationFrame);
            WebDE.Animation.AnimationFrame.animFrames = new System.Collections.Generic.List$1.ctor(WebDE.Animation.AnimationFrame);
        },
        GetAnimationFrames:function()
        {
            return WebDE.Animation.AnimationFrame.animFrames;
        },
        IsFrameCached:function(imageLocation,offsetX,offsetY)
        {
            var $it3=WebDE.Animation.AnimationFrame.cachedFrames.GetEnumerator();
            while($it3.MoveNext())
            {
                var frame=$it3.get_Current();
                if(frame.AnimImage == imageLocation && frame.imageX == offsetX && frame.imageY == offsetY)
                {
                    return frame;
                }
            }
            return null;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(imageLocation,offsetX,offsetY)
        {
            this.id = null;
            this.AnimImage = null;
            this.imageX = 0;
            this.imageY = 0;
            this.displayLength = 25;
            System.Object.ctor.call(this);
            this.id = "animFrame_" + WebDE.Animation.AnimationFrame.animFrames.get_Count().toString();
            this.AnimImage = imageLocation;
            this.imageX = offsetX;
            this.imageY = offsetY;
            WebDE.Animation.AnimationFrame.animFrames.Add(this);
        },
        getId:function()
        {
            return this.id;
        },
        getDisplayLength:function()
        {
            return this.displayLength;
        },
        setDisplayLength:function(newLength)
        {
            this.displayLength = newLength;
        },
        getImage:function()
        {
            return this.AnimImage;
        },
        getPosition:function()
        {
            var returnVal=new System.Tuple$2.ctor(System.Int32,System.Int32,this.imageX,this.imageY);
            return returnVal;
        },
        markAsCached:function()
        {
            if(WebDE.Animation.AnimationFrame.cachedFrames.Contains(this) == false)
            {
                WebDE.Animation.AnimationFrame.cachedFrames.Add(this);
            }
        }
    }
};
JsTypes.push(WebDE$Animation$AnimationFrame);
var WebDE$Animation$Sprite=
{
    fullname:"WebDE.Animation.Sprite",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Animation.Sprite.loadedSprites = new System.Collections.Generic.List$1.ctor(WebDE.Animation.Sprite);
        },
        GetSpriteByName:function(spriteName)
        {
            var $it6=WebDE.Animation.Sprite.loadedSprites.GetEnumerator();
            while($it6.MoveNext())
            {
                var spr=$it6.get_Current();
                if(spr.GetName() == spriteName)
                {
                    return WebDE.GameObjects.Helpah.Clone(spr);
                }
            }
            return null;
        },
        singleFrame:function(name,width,height,frameLocation,xOffset,yOffset)
        {
            var animfrm=new WebDE.Animation.AnimationFrame.ctor(frameLocation,xOffset,yOffset);
            var annie=new WebDE.Animation.Animation.ctor();
            annie.AddFrame(animfrm);
            var newSprite=new WebDE.Animation.Sprite.ctor(name);
            newSprite.setSize(width,height);
            newSprite.addAnimation(annie);
            return newSprite;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(spriteName)
        {
            this.name = "";
            this.animations = new System.Collections.Generic.List$1.ctor(WebDE.Animation.Animation);
            this.size = new WebDE.Dimension.ctor$$Double$$Double(40,40);
            this.currentAnimation = null;
            this.defaultAnimation = null;
            this.currentRenderFrame = "";
            System.Object.ctor.call(this);
            this.name = spriteName;
            WebDE.Animation.Sprite.loadedSprites.Add(this);
        },
        GetName:function()
        {
            return this.name;
        },
        SetName:function(newName)
        {
            this.name = newName;
        },
        Animate:function()
        {
            if(this.getCurrentAnimation() != null)
            {
                return this.getCurrentAnimation().Animate();
            }
            else
            {
                return "";
            }
        },
        playAnimation:function(animationName)
        {
            var $it4=this.animations.GetEnumerator();
            while($it4.MoveNext())
            {
                var anim=$it4.get_Current();
                if(anim.GetName() == animationName)
                {
                    return true;
                }
            }
            return false;
        },
        setAnimation:function(animationName)
        {
            var $it5=this.animations.GetEnumerator();
            while($it5.MoveNext())
            {
                var anim=$it5.get_Current();
                if(anim.GetName() == animationName)
                {
                    this.currentAnimation = anim;
                    return true;
                }
            }
            return false;
        },
        addAnimation:function(animToAdd)
        {
            animToAdd = WebDE.GameObjects.Helpah.Clone(animToAdd);
            this.animations.Add(animToAdd);
        },
        getCurrentAnimation:function()
        {
            if(this.currentAnimation == null)
            {
                if(this.defaultAnimation == null)
                {
                    this.defaultAnimation = this.animations.get_Item$$Int32(0);
                }
                this.currentAnimation = this.defaultAnimation;
            }
            return this.currentAnimation;
        },
        GetSize:function()
        {
            return this.size;
        },
        setSize:function(newWidth,newHeight)
        {
            this.size.width = newWidth;
            this.size.height = newHeight;
        },
        GetCurrentRenderFrame:function()
        {
            return this.currentRenderFrame;
        },
        SetCurrentRenderFrame:function(newId)
        {
            this.currentRenderFrame = newId;
        }
    }
};
JsTypes.push(WebDE$Animation$Sprite);
