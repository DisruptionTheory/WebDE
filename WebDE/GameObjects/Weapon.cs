using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Weapon
    {
        private int maxAmmo = 100;
        private int currentAmmo = 100;
        private double range = 1;
        //the maximum radius (in degrees) that the weapon can turn from dead center (zero degrees) in either direction to fire
        private double turningRadius = 0;
        //the speed at which the weapon turns
        private double turningSpeed = 0;
        //this will be the initial speed applied to the projectile, or perhaps should be measured in force?
        private double projectileSpeed = 0;
        //should this be moved to the projectile...? maybe a damage multiplier / modifier?
        //...maybe a LIST of damage modifiers or multipliers? how should those work...?
        private double damage = 0;
        private Projectile projectileType;
        //the interval between shots
        private double firingDelay = 0;
        //the last time the weapon was fired
        private int lastFiredTime;
        //the owner of the weapon
        private LivingGameEntity owner;
        //the GameEntity the weapon is currently targeting
        private GameEntity target;
        //can the weapon be fired while the body is moving?
        private bool fireWhileMoving = false;

        /// <summary>
        /// Creates a new weapon.
        /// </summary>
        /// <param name="owner">The game entity which possesses this weapon.</param>
        /// <param name="damage">The amount of damage the weapon does.</param>
        /// <param name="firingInterval">The time, in seconds, between shots.</param>
        /// <param name="projectileSpeed">The speed, in game units, at which the projectile fired by this weapon travels.</param>
        /// <param name="turningRadius">The arc, in degrees, that this weapon can fire in. Assumes bidirectional.</param>
        /// <param name="turnSpeed">The speed at which the weapon can be turned along the turningRadius.</param>
        public Weapon(double damage, double firingInterval, double projectileSpeed, double turningRadius, double turnSpeed)
        {
            this.damage = damage;
            //firing delay is passed in in terms of seconds, but converted to milliseconds for use
            this.firingDelay = firingInterval * 1000;
            this.projectileSpeed = projectileSpeed;
            this.turningRadius = turningRadius;
            this.turningSpeed = turnSpeed;
            this.maxAmmo = 100;

            this.lastFiredTime = DateTime.Now.Millisecond;// -Helpah.Round(this.firingDelay);

            //set the default projectile to bullet?
        }

        public void SimpleFire()
        {
            //trigger the animation, the loss of ammo, the last fired time, the damage to the enemy
        }

        public void Fire()
        {
            if (owner.GetParentStage() == null || this.GetTarget() == null)
            {
                //Debug.log(owner.GetName() + " has no parent stage :(");
                return;
            }

            //if the weapon has had enough time to "cool down" since last firing
            if (DateTime.Now.Millisecond > this.lastFiredTime + this.firingDelay)
            {
                Debug.log("Weapon owned by " + this.owner.GetId() + " is firing.");
                int deltaX = (int)Math.Round(this.GetTarget().GetPosition().x - this.owner.GetPosition().x);
                int deltaY = (int)Math.Round(this.GetTarget().GetPosition().y - this.owner.GetPosition().y);

                //create the projectile, place it, set it facing a direction...
                Projectile myBullet = new Projectile("Bullet", this.owner, this.GetTarget().GetPosition());
                myBullet.SetParentStage(owner.GetParentStage());
                //myBullet.SetSprite(
                myBullet.SetDamage(10);
                myBullet.SetPosition(this.owner.GetPosition().x, this.owner.GetPosition().y);
                myBullet.SetSpeed(new Vector(deltaX, deltaY));
                //myBullet.SetDirection(MovementDirection.Right);
                //myBullet.SetAcceleration(.1);
                Stage.CurrentStage.AddLivingGameEntity(myBullet);

                //deplete ammo by one unit
                this.SetCurrentAmmo(this.GetCurrentAmmo() - 1);
                //this is the new "last time" that we have fired
                this.lastFiredTime = DateTime.Now.Millisecond;
            }
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

        public void SetProjectile(Projectile newProjectile)
        {
            this.projectileType = newProjectile;
        }

        public Projectile GetProjectile()
        {
            return this.projectileType;
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
            Debug.log("Setting target to " + newTarget.GetName());
            this.target = newTarget;
        }

        public GameEntity GetTarget()
        {
            return this.target;
        }

        public void Destroy()
        {
            Debug.log("Destroying weapon owned by " + this.owner.GetId());
            //DOM_Renderer.GetRenderer().DestroyGameEntity(this);
            Helpah.Destroy(this);
        }
    }
}
