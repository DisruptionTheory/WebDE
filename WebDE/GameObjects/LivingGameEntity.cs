using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.AI;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public class LivingGameEntity : GameEntity
    {
        //private static List<LivingGameEntity> cachedEntities = new List<LivingGameEntity>();

        public static LivingGameEntity CloneByName(string lentName)
        {
            foreach (GameEntity gent in GameEntity.GetCachedEntities())
            {
                if (gent.GetName() == lentName)
                {
                    return gent.As<LivingGameEntity>();
                }
            }

            return null;
        }

        private int health;
        private ArtificialIntelligence ai;
        private List<Weapon> weapons;// = new List<Weapon>();

        public LivingGameEntity(string entName, int health)
            : base(entName)
        {
            this.health = health;

            //give it a / the default AI...
            this.ai = new ArtificialIntelligence();
            this.weapons = new List<Weapon>();
        }

        public ArtificialIntelligence GetAI()
        {
            return this.ai;
        }

        public void SetAI(ArtificialIntelligence newAi)
        {
            //make a copy of the AI and assign it to this guy
            //ObjectHelper<ArtificialIntelligence> aiHelper = new ObjectHelper<ArtificialIntelligence>();
            //this.ai = aiHelper.CloneObject(newAi);
            this.ai = Helpah.Clone(newAi).As<ArtificialIntelligence>();
            this.ai.SetBody(this);
        }

        public void Think()
        {
            //if AI is defined, call its think function
            if (ai != null)
            {
                //make sure that the ai is using the current incarnation of this body
                //apparently pass-by-ref didn't take too well...
                ai.SetBody(this);
                ai.Think();
            }
        }

        //this GameEntity takes damage
        public void Damage(int amount)
        {
            //check defenses and what have you, to reduce damage amount

            this.SetHealth(this.health - amount);
        }

        //kill this GameEntity
        public void Kill()
        {
            //add any relevant score, perform any on death actions (like creating a corpse)

            //remove the entity from the game
            this.Destroy();
        }

        public void Destroy()
        {
            while (this.weapons.Count > 0)
            {
                this.weapons[0].Destroy();
            }
            base.Destroy();
        }

        public Weapon AddWeapon(Weapon weaponToAdd)
        {
            if (!this.weapons.Contains(weaponToAdd))
            {
                //Debug.log("Adding a weapon with projectile " + weaponToAdd.GetProjectile().GetName() + " to entitiy " + this.GetName());
                weaponToAdd = Helpah.Clone(weaponToAdd).As<Weapon>();
                this.weapons.Add(weaponToAdd);
            }

            return weaponToAdd;
        }

        public void RemoveWeapon(Weapon weaponToRemove)
        {
            if (this.weapons.Contains(weaponToRemove))
            {
                this.weapons.Remove(weaponToRemove);
            }
        }

        public List<Weapon> GetWeapons()
        {
            return this.weapons;
        }

        public void SetHealth(int newHealth)
        {
            this.health = newHealth;

            if (this.health <= 0)
            {
                this.Kill();
            }
        }

        public int GetHealth()
        {
            return this.health;
        }
    }
}
