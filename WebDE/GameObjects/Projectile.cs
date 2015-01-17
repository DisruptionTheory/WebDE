using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.AI;
using WebDE.Misc;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Projectile : LivingGameEntity
    {
        //different projectiles will have different properties, such as:
        //(simple) bullets, arcing projectiles (arrows, etc.), lasers, explosives...
        private double damage = 0;
        private Point targetPoint;
        private DamageType damageType = DamageType.Physical;
        private GameEntity firingEntity;
        private Vector projectileSpeedMultiplier = new Vector(1, 1);

        public Projectile(string projectileName, GameEntity firingEnt, Point targetPoint)
            : base(projectileName)
        {
            this.targetPoint = targetPoint;
            this.firingEntity = firingEnt;
            //target the target
            //this.SetAI(null);
            this.SetAI(new ArtificialIntelligence());
            //ArtificialIntelligence ai = new ArtificialIntelligence();
            //MovementPath newPath = new MovementPath(this.GetPosition(), 
            //ai.SetMovementPath(
        }

        public Projectile(string projectileName)
            : base(projectileName)
        {
            this.SetAI(new ArtificialIntelligence());
        }

        public void SetFiringEntity(GameEntity firingEnt)
        {
            this.firingEntity = firingEnt;
        }

        public void SetPosition(double newX, double newY)
        {
            base.SetPosition(newX, newY);
            
            //make a path directly from where we are to where we want to be
            //MovementPath newPath = new MovementPath(new List<Point>{ this.GetPosition(), this.targetPoint});
            //this.GetAI().SetMovementPath(newPath);
        }

        public void SetDamage(double newDamage)
        {
            this.damage = newDamage;
        }

        public double GetDamage()
        {
            return this.damage;
        }

        public GameEntity GetFiringEntity()
        {
            return this.firingEntity;
        }

        public void SetTargetPoint(Point newPoint)
        {
            this.targetPoint = newPoint;
        }
    }
}
