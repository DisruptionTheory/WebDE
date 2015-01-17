using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Misc;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Weapon
    {
        private int maxAmmo = 100;
        private int currentAmmo = 100;
        // A default range of 0 means the weapon has to be "manually" fired (won't work for AI until longer range is set)
        private double range = 0;
        //the maximum radius (in degrees) that the weapon can turn from dead center (zero degrees) in either direction to fire
        private double turningRadius = 0;
        //the speed at which the weapon turns
        private double turningSpeed = 0;
        //this will be the initial speed applied to the projectile, or perhaps should be measured in force?
        private double projectileSpeed = 1.0;
        //should this be moved to the projectile...? maybe a damage multiplier / modifier?
        //...maybe a LIST of damage modifiers or multipliers? how should those work...?
        private double damage = 0;
        //the interval between shots (milliseconds)
        private double firingDelay = 1000;
        //the last time the weapon was fired
        private DateTime lastFiredTime;
        //the owner of the weapon
        private LivingGameEntity owner;
        //the GameEntity the weapon is currently targeting
        private GameEntity target;
        //can the weapon be fired while the body is moving?
        private bool fireWhileMoving = false;

        //projectile
        private Projectile projectileTemplate;

        /// <summary>
        /// Creates a new weapon.
        /// </summary>
        /// <param name="owner">The game entity which possesses this weapon.</param>
        /// <param name="damage">The amount of damage the weapon does.</param>
        /// <param name="firingInterval">The time, in seconds, between shots.</param>
        /// <param name="projectileSpeed">The speed, in game units, at which the projectile fired by this weapon travels.</param>
        /// <param name="turningRadius">The arc, in degrees, that this weapon can fire in. Assumes bidirectional.</param>
        /// <param name="turnSpeed">The speed at which the weapon can be turned along the turningRadius.</param>
        public Weapon(double damage, double turningRadius, double turnSpeed)
        {
            this.damage = damage;
            this.projectileSpeed = projectileSpeed;
            this.turningRadius = turningRadius;
            this.turningSpeed = turnSpeed;
            this.maxAmmo = 100;

            //set the default projectile
            this.projectileTemplate = new Projectile("Bullet", null, null);
            this.projectileTemplate.SetDamage(1);
            this.lastFiredTime = DateTime.Now;

            //set the default projectile to bullet?
        }

        /*
        public void SimpleFire()
        {
            //trigger the animation, the loss of ammo, the last fired time, the damage to the enemy
        }
        */

        public void FireAtTarget()
        {
            DateTime nextFireTime = new DateTime(lastFiredTime.Year, lastFiredTime.Month, lastFiredTime.Day,
                lastFiredTime.Hour, lastFiredTime.Minute, lastFiredTime.Second, lastFiredTime.Millisecond + (int)firingDelay);
            //DateTime nextFireTime = this.lastFiredTime.AddMilliseconds(this.firingDelay);

            if (this.owner == null || this.owner.GetParentStage() == null || this.GetTarget() == null)
            {
                //Debug.log(owner.GetName() + " has no parent stage :(");
                return;
            }

            //if the weapon has had enough time to "cool down" since last firing
            //if (DateTime.Now > nextFireTime)
            if(Helpah.DateIsGreater(DateTime.Now, nextFireTime))
            {
                try
                {
                    int deltaX = (int)Math.Round(this.GetTarget().GetPosition().x - this.owner.GetPosition().x);
                    int deltaY = (int)Math.Round(this.GetTarget().GetPosition().y - this.owner.GetPosition().y);

                    //create the projectile, place it, set it facing a direction...
                    Projectile myBullet = ObjectCloner.CloneObject(this.projectileTemplate).As<Projectile>();
                    myBullet.SetFiringEntity(this.owner);
                    myBullet.SetTargetPoint(this.GetTarget().GetPosition());
                    myBullet.SetParentStage(this.owner.GetParentStage());
                    myBullet.SetPosition(this.owner.GetPosition().x, this.owner.GetPosition().y);
                    myBullet.SetSpeed(new Vector(deltaX, deltaY));
                    //myBullet.SetDirection(MovementDirection.Right);
                    //myBullet.SetAcceleration(.1);
                    Stage.CurrentStage.AddGameEntity(myBullet);

                    //deplete ammo by one unit
                    this.SetCurrentAmmo(this.GetCurrentAmmo() - 1);
                    //this is the new "last time" that we have fired
                    this.lastFiredTime = DateTime.Now;
                    Debug.Watch("Bullet deltas", deltaX.ToString() + ", " + deltaY.ToString());
                }
                catch (Exception ex)
                {
                    Debug.log("Couldn't fire weapon:");
                    Debug.log(ex.ToString());
                    Debug.log(ex.Message);
                }
            }
        }

        public void Fire()
        {
            DateTime nextFireTime = lastFiredTime.AddMilliseconds(firingDelay);

            if (this.owner == null || this.owner.GetParentStage() == null)
            {
                return;
            }

            //if the weapon has had enough time to "cool down" since last firing
            if (Helpah.DateIsGreater(DateTime.Now, nextFireTime))
            {
                try
                {
                    int bulletSpeed = 5;
                    int deltaX = 0;
                    int deltaY = 0;

                    if (this.owner.GetFacingDirection() == MovementDirection.Left)
                    {
                        deltaX = bulletSpeed * -1;
                    }
                    if (this.owner.GetFacingDirection() == MovementDirection.Right)
                    {
                        deltaX = bulletSpeed;
                    }
                    if (this.owner.GetFacingDirection() == MovementDirection.Down)
                    {
                        deltaY = bulletSpeed * -1;
                    }
                    if (this.owner.GetFacingDirection() == MovementDirection.Up)
                    {
                        deltaY = bulletSpeed;
                    }

                    //create the projectile, place it, set it facing a direction...
                    Projectile myBullet = shootProjectile();
                    myBullet.SetSpeed(new Vector(deltaX, deltaY));
                    Debug.Watch("Bullet deltas", deltaX.ToString() + ", " + deltaY.ToString());
                }
                catch (Exception ex)
                {
                    Debug.log("Couldn't fire weapon:");
                    Debug.log(ex.ToString());
                }
            }
        }

        private Projectile shootProjectile()
        {
            //create the projectile, place it, set it facing a direction...
            Projectile myBullet = ObjectCloner.CloneObject(this.projectileTemplate).As<Projectile>();
            myBullet.ValidateID();

            myBullet.SetFiringEntity(this.owner);
            myBullet.SetParentStage(this.owner.GetParentStage());
            myBullet.SetPosition(this.owner.GetPosition().x, this.owner.GetPosition().y);
            Stage.CurrentStage.AddGameEntity(myBullet);

            //deplete ammo by one unit
            this.SetCurrentAmmo(this.GetCurrentAmmo() - 1);
            //this is the new "last time" that we have fired
            this.lastFiredTime = DateTime.Now;

            return myBullet;
        }

        public LivingGameEntity GetOwner()
        {
            return this.owner;
        }

        public void SetOwner(LivingGameEntity newOwner)
        {
            this.owner = newOwner;
        }

        public void SetCurrentAmmo(int newAmmo)
        {
            this.currentAmmo = newAmmo;
        }

        public int GetCurrentAmmo()
        {
            return this.currentAmmo;
        }

        public void SetMaxAmmo(int newAmmo)
        {
            this.maxAmmo = newAmmo;
        }

        public int GetMaxAmmo()
        {
            return this.maxAmmo;
        }

        public void SetRange(double newRange)
        {
            this.range = newRange;
        }

        public double GetRange()
        {
            return this.range;
        }

        /// <summary>
        /// Set the speed at which projectiles initially launched are fired.
        /// </summary>
        /// <param name="newSpeed"></param>
        public void SetProjectileSpeed(int newSpeed)
        {
            this.projectileSpeed = newSpeed;
        }

        public double GetProjectileSpeed()
        {
            return this.projectileSpeed;
        }

        public void SetTurningSpeed(int newSpeed)
        {
            this.turningSpeed = newSpeed;
        }

        public double GetTurningSpeed()
        {
            return this.turningSpeed;
        }

        public void SetProjectile(Projectile projectileTemplate)
        {
            this.projectileTemplate = projectileTemplate;
        }

        public Projectile GetProjectile()
        {
            return this.projectileTemplate;
        }

        public void SetFiringDelay(double newDelay)
        {
            //firing delay is passed in in terms of seconds, but converted to milliseconds for use
            this.firingDelay = newDelay * 1000;
        }

        public double GetFiringDealy()
        {
            //firing delay is passed in in terms of seconds, but converted to milliseconds for use
            return this.firingDelay / 1000;
        }

        public void SetTarget(GameEntity newTarget)
        {
            this.target = newTarget;
        }

        public GameEntity GetTarget()
        {
            if (this.target != null)
            {
                if (!Stage.CurrentStage.GetVisibleEntities().Contains(this.target))
                {
                    this.SetTarget(null);
                }
            }

            return this.target;
        }

        public void Destroy()
        {
            //DOM_Renderer.GetRenderer().DestroyGameEntity(this);
            Helpah.Destroy(this);
        }
    }
}
