using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE;
using WebDE.Animation;
using WebDE.Rendering;

namespace WebDE.GameObjects
{
    public enum MovementDirection
    {
        None = 0,
        Left = -1,
        Right = 1,
        Up = 2,
        Down = -2
    }

    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class GameEntity
    {
        private string id;

        private Dimension size = new Dimension(40, 40);
        private string strGameEntityName = "New GameEntity";
        private Sprite sprGameEntitySprite = null;
        private Stage parentStage = null;

        private double opacity = 1.00;
        private bool visible = true;
        private List<string> customStyles = new List<string>();

        private Point position = new Point(0, 0);
        private Vector speed = new Vector(0, 0);
        private Vector minSpeed = new Vector(-10, -10);
        private Vector maxSpeed = new Vector(10, 10);
        //units per second the GameEntity accelerates (up to maxSpeed), in a given direction
        private double acceleration = 1;

        //the angle at which the GameEntity moves (0 - 360), if not using movement direction
        //if the number is negative, it means the GameEntity is not moving
        private int movementAngle = -1;
        private MovementDirection desiredDirection = MovementDirection.None;

        public GameEntity(string entName)
        {
            GameEntity.lastid++;
            this.id = "GameEntity_" + GameEntity.lastid;
            this.strGameEntityName = entName;
            GameEntity.cachedEntities.Add(this);

            //if a sprite exists with the same name, attempt to set it to be this GameEntity's sprite
            Sprite nameSprite = Sprite.GetSpriteByName(this.strGameEntityName);

            this.SetSprite(nameSprite);
        }

        public string GetId()
        {
            return this.id;
        }

        public string GetName()
        {
            return this.strGameEntityName;
        }

        public Point GetPosition()
        {
            return this.position;
        }

        public void SetPosition(double newX, double newY)
        {
            if (newX != this.position.x || newY != this.position.y)
            {
                this.position.x = newX;
                this.position.y = newY;
                this.SetNeedsUpdate();
            }
        }

        public Stage GetParentStage()
        {
            return this.parentStage;
        }

        public void SetParentStage(Stage newStage)
        {
            this.parentStage = newStage;
        }

        public double GetAcceleration()
        {
            return this.acceleration;
        }

        public void SetAcceleration(double newAccel)
        {
            this.acceleration = newAccel;
        }

        public Sprite GetSprite()
        {
            return sprGameEntitySprite;
        }

        public bool SetSprite(Sprite newSprite)
        {
            try
            {
                //Debug.log("Setting sprite for " + this.GetName() + " to " + newSprite.GetName());
                this.sprGameEntitySprite = Helpah.Clone(newSprite).As<Sprite>();
                this.SetNeedsUpdate();

                return true;
            }
            catch (Exception ex)
            {
                Debug.log("Exception with SetSprite in GameEntity: " + ex.Message);
                return false;
            }
        }

        public Dimension GetSize()
        {
            return this.size;
        }

        public void SetSize(int newWidth, int newHeight)
        {
            if (newWidth != this.size.width || newHeight != this.size.height)
            {
                this.size.width = newWidth;
                this.size.height = newHeight;
                this.SetNeedsUpdate();
            }
        }

        public int GetMovementAngle()
        {
            return this.movementAngle;
        }

        /// <summary>
        /// Movement angle is used for things like projectiles, which will not move in simple quad-directions
        /// </summary>
        public void SetMovementAngle(int newAngle)
        {
            this.movementAngle = newAngle;
        }

        public MovementDirection GetDirection()
        {
            return this.desiredDirection;
        }

        public void SetDirection(MovementDirection newDirection)
        {
            this.desiredDirection = newDirection;
        }

        public Vector GetSpeed() {
            return this.speed;
        }

        public void SetSpeed(Vector newSpeed)
        {
            if (newSpeed != this.speed)
            {
                this.speed = newSpeed;
                this.SetNeedsUpdate();
            }
        }

        public void AddSpeed(Vector newSpeed)
        {
            this.speed.x += newSpeed.x;
            this.speed.y += newSpeed.y;
        }

        public void Destroy()
        {
            Stage.CurrentStage.RemoveGameEntity(this);
            DOM_Renderer.GetRenderer().DestroyGameEntity(this);
            Helpah.Destroy(this);
        }

        public void CalculateSpeed()
        {
            //only do projectile temporarily...
            if(this is GameEntitySpawner || this is Projectile)
            {
                return;
            }

            //gravity isn't applicable in our current genre, but will be later
            //we will need to get it from the environment, which is something that isn't properly implemented yet
            //Stage.CurrentStage.Gravity
            //Script.Eval("console.log('" + this.desiredDirection.ToString() + "');");

            //we probably want to make a vector that either uses or can handle doubles, rather than converting to int
            if (this.desiredDirection == MovementDirection.Left)
            {
                this.speed.x -= this.acceleration;
            }
            else if (this.desiredDirection == MovementDirection.Right)
            {
                this.speed.x += this.acceleration;
            }
            else if (this.desiredDirection == MovementDirection.Down)
            {
                this.speed.y -= this.acceleration;
            }
            else if (this.desiredDirection == MovementDirection.Up)
            {
                this.speed.y += this.acceleration;
            }
            else if (this.desiredDirection == MovementDirection.None)
            {   //might need a better way of slowing things down
                if (this.speed.x > 0)
                {
                    this.speed.x -= this.acceleration;
                }
                else if (this.speed.x < 0)
                {
                    this.speed.x += this.acceleration;
                }
                if (this.speed.y > 0)
                {
                    this.speed.y -= this.acceleration;
                }
                else if (this.speed.y < 0)
                {
                    this.speed.y += this.acceleration;
                }
            }

            if (this.speed.x > this.maxSpeed.x)
            {
                this.speed.x = this.maxSpeed.x;
            }
            if (this.speed.x < this.minSpeed.x)
            {
                this.speed.x = this.minSpeed.x;
            }
            if (this.speed.y > this.maxSpeed.y)
            {
                this.speed.y = this.maxSpeed.y;
            }
            if (this.speed.y < this.minSpeed.y)
            {
                this.speed.y = this.minSpeed.y;
            }
        }

        public void CalculatePosition()
        {
            this.SetPosition(
                this.position.x + this.speed.x,
                this.position.y + this.speed.y);
            //Script.Eval("console.log('" + this.position.x + ", " + this.position.y + ", " + this.speed.x + ", " + this.speed.y + "');");

            //check if position x or y goes out of bounds

            //check for collisions
        }

        public void CheckCollision()
        {
        }

        protected void SetNeedsUpdate()
        {
            //Debug.Watch("Setneeds update", "Entity: " + this.GetId());

            Rendering.DOM_Renderer.GetRenderer().SetNeedsUpdate(this);
        }        

        /// <summary>
        /// Add custom styling properties, to be applied by the styling engine. 
        /// Essentially, CSS classes
        /// </summary>
        /// <param name="styleName"></param>
        public void AddCustomStyling(string styleName)
        {
            if(!this.customStyles.Contains(styleName)) {
                this.customStyles.Add(styleName);
            }
        }

        public List<string> GetCustomStyles()
        {
            return this.customStyles;
        }

        public void SetOpcaity(double newOpacity)
        {
            this.opacity = newOpacity;
            this.SetNeedsUpdate();
        }

        public double GetOpacity()
        {
            return this.opacity;
        }

        public void Hide()
        {
            if (this.visible == true)
            {
                this.visible = false;
                this.SetNeedsUpdate();
            }
        }

        public void Show()
        {
            if (this.visible == false)
            {
                this.visible = true;
                this.SetNeedsUpdate();
            }
        }

        public bool Visible()
        {
            return this.visible;
        }
    }
}
