using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.AI;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Projectile : LivingGameEntity
    {
        //different projectiles will have different properties, such as:
        //(simple) bullets, arcing projectiles (arrows, etc.), lasers, explosives...
        private double damage = 0;
        private Action impactEvent;
        private Point targetPoint;

        public Projectile(string projectileName, Point targetPoint)
            : base(projectileName, 10)
        {
            this.targetPoint = targetPoint;
            //target the target
            ArtificialIntelligence ai = new ArtificialIntelligence();
            //MovementPath newPath = new MovementPath(this.GetPosition(), 
            //ai.SetMovementPath(
        }

        public void SetPosition(double newX, double newY)
        {
            base.SetPosition(newX, newY);
            //make a path directly from where we are to where we want to be
            MovementPath newPath = new MovementPath(new List<Point>{ this.GetPosition(), this.targetPoint});
            ArtificialIntelligence newAi = new ArtificialIntelligence();
            newAi.SetMovementPath(newPath);
            this.SetAI(newAi);
        }

        public void SetDamage(double newDamage)
        {
            this.damage = newDamage;
        }

        public double GetDamage()
        {
            return this.damage;
        }

        /// <summary>
        /// This projectile has hit something.
        /// </summary>
        public void Collision()
        {
            if (this.impactEvent != null)
            {
                this.impactEvent();
            }
        }
    }
}
