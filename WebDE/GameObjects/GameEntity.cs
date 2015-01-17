using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE;
using WebDE.Animation;
using WebDE.Rendering;
using WebDE.Misc;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
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
        // Properties
        public int Width { get { return (int)this.size.width; } }
        public int Height { get { return (int)this.size.height; } }
        public int PixelWidth { get { return (int)(this.Width * Stage.CurrentStage.GetTileSize().width); } }
        public int PixelHeight { get { return (int)(this.Height * Stage.CurrentStage.GetTileSize().height); } }

        private string id;

        //game variables
        private Faction faction;
        public Faction Faction
        {
            get { return faction; }
            set
            {
                if (this.faction != null)
                    this.faction.RemoveEntity(this);
                value.AddEntity(this);
            }
        }
        // Size in game units (tiles)
        private Dimension size = new Dimension(1, 1);
        private string strGameEntityName = "New GameEntity";
        private Stage parentStage = null;
        //private Dictionary<string, int> resources = new Dictionary<string, int>();

        //display variables
        private Sprite sprGameEntitySprite = null;
        private double opacity = 1.00;
        private bool visible = true;
        private List<string> customStyles = new List<string>();

        //physics variables
        private Point position = new Point(0, 0);
        private Vector speed = new Vector(0, 0);
        private Vector minSpeed = new Vector(-.5, -.5);
        private Vector maxSpeed = new Vector(.5, .5);
        //units per second the GameEntity accelerates (up to maxSpeed), in a given direction
        private double acceleration = .5;
        private Action<GameEntity> collisionEvent;
        // Whether or not the entity is touching ground.
        private bool grounded = false;
        public bool ObeysPhysics = true;

        public double Left { get { return this.position.x; } }
        public double Right { get { return this.position.x + this.Width; } }
        public double Top { get { return this.position.y + this.Height; } }
        public double Bottom { get { return this.position.y; } }
        
        //the angle at which the GameEntity moves (0 - 360), if not using movement direction
        //if the number is negative, it means the GameEntity is not moving
        private int movementAngle = -1;
        private MovementDirection desiredDirection = MovementDirection.None;
        private MovementDirection facingDirection = MovementDirection.None;

        //to account for the loss of typing in cloning...
        private string actualTypeName;

        public GameEntity(string entName, string entId = "")
        {
            if (entId != "" && entId != null)
            {
                this.id = entId;
            }
            else
            {
                GameEntity.lastid++;
                this.id = "GameEntity_" + GameEntity.lastid;
            }
            this.strGameEntityName = entName;
            GameEntity.CacheEntity(this);

            //if a sprite exists with the same name, attempt to set it to be this GameEntity's sprite
            Sprite nameSprite = Sprite.GetSpriteByName(this.strGameEntityName);

            if (nameSprite != null)
            {
                this.SetSprite(nameSprite);
            }

            actualTypeName = this.GetType().Name;

            Helpah.LoadDataFromDatabase(this);
        }

        // Check that the id of this object is not in use by another object.
        // Give it a new ID if it is.
        public void ValidateID()
        {
            GameEntity.lastid++;
            this.id = "GameEntity_" + GameEntity.lastid;
        }

        public GameEntity(string entName, bool obeysPhysics)
        {
            GameEntity.lastid++;
            this.id = "GameEntity_" + GameEntity.lastid;
            this.strGameEntityName = entName;
            GameEntity.CacheEntity(this);
            this.ObeysPhysics = obeysPhysics;

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

        public Rectangle GetArea()
        {
            return new Rectangle(this.position.x, this.position.y, this.Width, this.Height);
        }

        public void SetPosition(double newX, double newY)
        {
            if (newX != this.position.x || newY != this.position.y)
            {
                if (this == Game.GetPlayerCharacter())
                {
                    Debug.Watch("PlayerY", newY.ToString());
                }

                this.position.x = newX;
                this.position.y = newY;
                //IsOnGround();
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

        public virtual void SetSize(double newWidth, double newHeight)
        {
            if (newWidth != this.size.width || newHeight != this.size.height)
            {
                this.size.width = newWidth;
                this.size.height = newHeight;
                this.SetNeedsUpdate();
            }
        }

        public void SetSize(Dimension newSize)
        {
            if (newSize.width != size.width && newSize.height != size.height)
            {
                this.size = newSize;
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
            // Set the current animation to match the direction, if the sprite has an animation with that direction name.
            //this.GetSprite().setAnimation(newDirection.ToString());

            if (newDirection != MovementDirection.None)
            {
                this.facingDirection = newDirection;
                this.SetNeedsUpdate();
            }
        }

        public MovementDirection GetFacingDirection()
        {
            return this.facingDirection;
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
            this.speed.X += newSpeed.X;
            this.speed.Y += newSpeed.Y;
        }

        public void Destroy()
        {
            if (this.Faction != null)
            {
                this.Faction.RemoveEntity(this);
            }
            Game.Renderer.DestroyGameEntity(this);
            Stage.CurrentStage.RemoveGameEntity(this);
            Helpah.Destroy(this);
        }

        public void CalculateSpeed()
        {
            //only do projectile temporarily...
            if(this.GetTypeName() == "GameEntitySpawner" || this.GetTypeName() == "Projectile")
            {
                return;
            }

            decelerate();

            //we probably want to make a vector that either uses or can handle doubles, rather than converting to int
            if (this.desiredDirection == MovementDirection.Left)
            {
                this.speed.X -= this.acceleration;
            }
            else if (this.desiredDirection == MovementDirection.Right)
            {
                this.speed.X += this.acceleration;
            }
            else if (this.desiredDirection == MovementDirection.Down)
            {
                this.speed.Y -= this.acceleration;
            }
            else if (this.desiredDirection == MovementDirection.Up)
            {
                this.speed.Y += this.acceleration;
            }
            // Instant braking
            else if (this.desiredDirection == MovementDirection.None)
            {
                // AI controllers decelerate differently for now.
                if (this.GetTypeName().Contains("LivingGameEntity") && ((LivingGameEntity)this).GetAI() == null)
                {
                    if (this.speed.X > 0)
                    {
                        this.speed.X -= this.acceleration;
                    }
                    else if (this.speed.X < 0)
                    {
                        this.speed.X += this.acceleration;
                    }
                    if (this.speed.Y > 0)
                    {
                        this.speed.Y -= this.acceleration;
                    }
                    else if (this.speed.Y < 0)
                    {
                        this.speed.Y += this.acceleration;
                    }
                }
                else
                {
                    this.speed = new Vector(0, 0);
                }
            }

            checkSpeedRates();
        }

        private void decelerate()
        {
            // Reduce speed in a direction we're not traveling...

            if (this.desiredDirection != MovementDirection.Left && this.speed.X < 0)
            {
                this.speed.X += this.acceleration;
            }
            else if (this.desiredDirection != MovementDirection.Right && this.speed.X > 0)
            {
                this.speed.X -= this.acceleration;
            }
            else if (this.desiredDirection != MovementDirection.Down && this.speed.Y < 0)
            {
                this.speed.Y += this.acceleration;
            }
            else if (this.desiredDirection != MovementDirection.Up && this.speed.Y > 0)
            {
                if (Stage.CurrentStage.GetGravity().Y == 0 || this.grounded)
                {
                    this.speed.Y -= this.acceleration;
                }
            }
        }

        private void checkSpeedRates()
        {
            // Adjust speed so that it cannot go over maximum or under minimum.
            if (this.speed.X > this.maxSpeed.X)
            {
                this.speed.X = this.maxSpeed.X;
            }
            if (this.speed.X < this.minSpeed.X)
            {
                this.speed.X = this.minSpeed.X;
            }
            if (this.speed.Y > this.maxSpeed.Y)
            {
                this.speed.Y = this.maxSpeed.Y;
            }
            if (this.speed.Y < this.minSpeed.Y)
            {
                this.speed.Y = this.minSpeed.Y;
            }
        }

        public void CalculatePosition()
        {
            // Need to rewrite all of this so that the stage gives a minimum and maximum Y for a given X.
            // THAT is terrain.

            this.SetPosition(
                this.position.x + this.speed.X,
                this.position.y + this.speed.Y);

            // Check if the object is on the ground. Apply gravity if it isn't.
            this.checkGrounding();

            if (this.speed.X == 0 && this.speed.Y == 0)
            {
                return;
            }

            // Should probably only need to check maxY and X, as minY should be covered in grounding.
            if (!Stage.CurrentStage.Contains(this))
            {
                if (this.GetTypeName() == "Projectile")
                {
                    this.Destroy();
                    return;
                }

                if (this.position.x < Stage.CurrentStage.GetBounds().x)
                {
                    this.SetPosition(Stage.CurrentStage.GetBounds().x, this.position.y);
                }
                if (this.position.y < Stage.CurrentStage.GetBounds().y)
                {
                    this.SetPosition(this.position.x, Stage.CurrentStage.GetBounds().y);
                }
                if (this.position.x > Stage.CurrentStage.GetBounds().Right)
                {
                    this.SetPosition(Stage.CurrentStage.GetBounds().Right - this.size.width, this.position.y);
                }
                if (this.position.y > Stage.CurrentStage.GetBounds().Top)
                {
                    this.SetPosition(this.position.x, Stage.CurrentStage.GetBounds().y - this.size.height);
                }
            }

            //if this object isn't a physics object, we don't need to
            if (this.ObeysPhysics == false)
            {
                return;
            }

            //---Check for collisions---
            this.checkCollisions();

            checkTerrainColliders();
        }

        private void checkCollisions()
        {
            // We want to find anything within the entitiy's "reach", which is it's size plus the speed it's moving.
            // For simplicity's sake, we'll use the greatest dimension for size and speed.
            // This won't cause issues if entities aren't particularly fast on one axis, or oblong.
            double distance = this.GetSize().GetGreatest() + Math.Abs(this.speed.GetGreatest());
            //retrieve of all entities in range
            List<GameEntity> entList = Stage.CurrentStage.GetEntitiesNear(this.GetPosition(), distance, true);
            foreach (GameEntity ent in entList)
            {
                //skip checking for collision against itself, and against entities that aren't physics objects
                if (ent == this || ent.ObeysPhysics == false || ent.GetId() == this.GetId())
                {
                    continue;
                }

                if (ent.GetTypeName() == "Tile" && ent.As<Tile>().GetWalkable() == true)
                {
                    continue;
                }

                //don't collide with units of the same faction (for now...we need a better way to handle this)
                if (this.Faction != null && this.Faction == ent.Faction)
                {
                    continue;
                }

                //don't collide with bullets, they collide with me
                //if (this is Projectile == false && ent is Projectile == true)
                //if (ent.instanceof<Projectile>() == true)
                //I really wanted to avoid using strings here, but I have to get around the typing problem caused in the cloning...for a bit...
                if (ent.GetTypeName() == "Projectile")
                {
                    if (ent.instanceof<Projectile>()) Debug.log("Instanceof works now. Magically. You don't have to use strings here.");

                    continue;
                }

                // If the entities don't overlap, they don't collide.
                // Check for actual overlap at this point (as it's more expensive than checking for distance)
                Rectangle thisRect = new Rectangle(this.position.x, this.position.y, this.size.width, this.size.height);
                if (this.speed.X > 0)
                {
                    thisRect.width += this.speed.X;
                }
                else
                {
                    thisRect.x += this.speed.X;
                }
                if (this.speed.Y > 0)
                {
                    thisRect.height += this.speed.Y;
                }
                else
                {
                    thisRect.y += this.speed.Y;
                }
                Rectangle entRect = new Rectangle(ent.position.x, ent.position.y, ent.size.width, ent.size.height);
                if (ent.speed.X > 0)
                {
                    entRect.width += ent.speed.X;
                }
                else
                {
                    entRect.x += ent.speed.X;
                }
                if (ent.speed.Y > 0)
                {
                    entRect.height += ent.speed.Y;
                }
                else
                {
                    entRect.y += ent.speed.Y;
                }

                if (thisRect.Contains(entRect))
                {
                    if (ent.GetTypeName() == "TerrainCollider")
                    {
                        ent.As<TerrainCollider>().CheckCollision(this);
                    }
                    else
                    {
                        this.Collision(ent);
                    }
                }

                //Debug.Watch("Nearent is ", "Origin: " + ent.GetPosition().x + ", " + ent.GetPosition().y + ". Destination: " + this.GetPosition().x + ", " + this.GetPosition().y + ". Distance: " + this.GetSize().GetGreatest());

                //Debug.log(this.GetId() + " is colliding with " + ent.GetId());
            }
        }

        private void checkTerrainColliders()
        {
            double distance = this.GetSize().GetGreatest() + Math.Abs(this.speed.GetGreatest());
            List<TerrainCollider> tcList = Stage.CurrentStage.GetTerrainCollidersNear(this.GetPosition(), distance);

            foreach (TerrainCollider terCol in tcList)
            {
                terCol.CheckCollision(this);
            }
        }

        // Check whether or not this entity is on the ground.
        public bool IsOnGround()
        {
            // Check tiles for collision.

            int distance = Helpah.d2i(this.GetSize().GetGreatest());// * Stage.CurrentStage.GetTileSize().GetGreatest());
            foreach (TerrainCollider terrainCollider in Stage.CurrentStage.GetTerrainCollidersNear(this.GetPosition(), distance))
            {
                if(terrainCollider.CheckCollision(this)) {
                    return true;
                }
            }
            return false;
        }

        private void checkGrounding()
        {
            if (!this.ObeysPhysics || this.GetTypeName() == "Projectile") return;
            int thisX = (int)(this.position.x + (this.size.width / 2));

            if (this.position.y > Stage.CurrentStage.MinY(thisX))
            {
                // Not on the ground
                this.speed.X -= Stage.CurrentStage.GetGravity().X;
                this.speed.Y -= Stage.CurrentStage.GetGravity().Y;
            }
            else
            {
                // Touching the floor.
                if (this.position.y < Stage.CurrentStage.MinY(thisX))
                {
                    this.SetPosition(this.position.x, Stage.CurrentStage.MinY(thisX));
                }
                this.speed.Y = 0;
            }
        }

        /// <summary>
        /// This entity is colliding into another. This entity is the one doing the colliding. 
        /// </summary>
        /// <param name="collidingEntity"></param>
        public void Collision(GameEntity collidingEntity)
        {
            Vector collisionVector = new Vector(this.position.x - collidingEntity.GetPosition().x + this.speed.X,
                this.position.x - collidingEntity.GetPosition().x + this.speed.Y);            

            string firingGuy = "";

            if(this.GetTypeName() == "Projectile")
            {
                //the projectile should not hit the entity that fired it
                Projectile projEnt = this.As<Projectile>();
                if (collidingEntity == projEnt.GetFiringEntity())
                {
                    return;
                }

                firingGuy = projEnt.GetFiringEntity().GetName();
            }

            //perform custom collision events first
            if (this.collisionEvent != null)
            {
                this.collisionEvent.Invoke(collidingEntity);
            }
            if (collidingEntity.collisionEvent != null)
            {
                collidingEntity.collisionEvent.Invoke(collidingEntity);
            }

            // Determine where the collision happens
            Point collisionPosition = new Point(0, 0);
            if (Math.Abs(this.speed.X) > Math.Abs(collidingEntity.speed.X))
            {
                collisionPosition.x = (Math.Abs(collidingEntity.speed.X) / Math.Abs(this.speed.X))
                    * Math.Abs(this.position.x - collidingEntity.position.x);
            }
            else
            {
                collisionPosition.x = (Math.Abs(this.speed.X) / Math.Abs(collidingEntity.speed.X))
                    * Math.Abs(collidingEntity.position.x - this.position.x);
            }

            // If the colliding entity is neither above nor below, check X collision.
            if (!collidingEntity.GetArea().Above(this.GetArea()) && !collidingEntity.GetArea().Below(this.GetArea()))
            {
                if ((this.position.x + this.speed.X > collidingEntity.position.x + collidingEntity.speed.X &&
                    this.position.x + this.speed.X < collidingEntity.position.x + collidingEntity.speed.X + collidingEntity.Width) ||
                    (this.position.x + this.speed.X < collidingEntity.position.x + collidingEntity.speed.X &&
                    this.position.x + this.speed.X + this.Width > collidingEntity.position.x + collidingEntity.speed.X))
                {
                    //newSpeed.x = 0;
                    //colNewSpeed.x = 0;
                }
            }

            // If colliding entity is neither to the left nor the right, check Y collision
            if (!(collidingEntity.GetArea().Right < this.GetArea().Left) 
                && !(collidingEntity.GetArea().Left > this.GetArea().Right))
            {
                // If colliding from above...
                // "Minimum" speed is entity position minus colliding entity top.
                if (this.position.y + this.speed.Y < collidingEntity.position.y + collidingEntity.speed.Y &&
                    this.position.y + this.speed.Y + this.Height > collidingEntity.position.y + collidingEntity.speed.Y)
                {
                    this.SetPosition(this.GetPosition().x, collidingEntity.GetArea().Top);
                }

                // If colliding from below,
                // "Maximum" speed is colliding entity top minus entity bottom
                if (this.position.y + this.speed.Y > collidingEntity.position.y + collidingEntity.speed.Y &&
                    this.position.y + this.speed.Y < collidingEntity.position.y + collidingEntity.speed.Y + collidingEntity.Height)
                {
                    this.SetPosition(this.GetPosition().x, collidingEntity.GetArea().Bottom - this.GetArea().height);
                }
            }

            // Collision means speed = 0;
            this.SetSpeed(new Vector(0, 0));
            collidingEntity.SetSpeed(new Vector(0, 0));

            // Colliding with a tile.
            if (collidingEntity.GetTypeName() == "Tile")
            {
                // Debug.log(collisionVector.ToString());
            }

            // Well, we don't need to deal damage from a terrain collider.
            if (this.GetTypeName() == "TerrainCollider")
            {
                return;
            }

            int damage = 10;

            //deal damage to the other entity
            if (collidingEntity.GetTypeName() == "LivingGameEntity")
            {
                LivingGameEntity livingTarget = collidingEntity.As<LivingGameEntity>();
                /*
                Debug.log(this.GetName() + " " + this.GetId() + " at " + this.GetPosition().x + ", " + this.GetPosition().y +
                    " dealing " + damage + " damage to " + livingTarget.GetName() + " (" + livingTarget.GetId() + ") at " + livingTarget.GetPosition().x + ", " + livingTarget.GetPosition().y + 
                    ". IsProjectile: " + isProj.ToString() + ", firingGuy is " + firingGuy + ", This type is " + this.GetType().Name.ToString());
                */
                Debug.log("Damaging " + livingTarget.GetName() + " by " + damage);
                livingTarget.Damage(damage);
            }
            //collidingEntity.dam
        }

        protected void SetNeedsUpdate()
        {
            //Debug.Watch("Setneeds update", "Entity: " + this.GetId());

            if (Game.Renderer != null)
            {
                Game.Renderer.SetNeedsUpdate(this);
            }
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

        public virtual bool Visible(View view)
        {
            return this.visible;
        }

        public string GetTypeName()
        {
            return this.actualTypeName;
        }

        public void SetMaxSpeed(double xMagnitude, double yMagnitude)
        {
            this.maxSpeed = new Vector(xMagnitude, yMagnitude);
        }

        public void SetMinSpeed(double xMagnitude, double yMagnitude)
        {
            this.minSpeed = new Vector(xMagnitude, yMagnitude);
        }

        /*
        public void SetResource(string resourceName, int value)
        {
            Resource targetResource = Resource.ByName(resourceName);

            // Make sure the resource exists.
            if(targetResource == null) return;

            // If we already have this resource, update the value
            if (this.resources[resourceName] != null)
            {
                this.resources[resourceName] = value;
            }
            // Otherwise, add the resource
            else
            {
                this.resources.Add(resourceName, value);
            }
        }

        public int GetResource(string resourceName)
        {
            Resource targetResource = Resource.ByName(resourceName);

            if (targetResource == null || this.resources[resourceName] == null)
            {
                return 0;
            }

            return this.resources[resourceName];
        }
        */
    }
}
