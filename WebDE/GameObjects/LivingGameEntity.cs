using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.AI;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public class LivingGameEntity : GameEntity
    {
        private static List<LivingGameEntity> cachedEntities = new List<LivingGameEntity>();

        public static LivingGameEntity GetById(string id)
        {
            foreach (LivingGameEntity lent in LivingGameEntity.cachedEntities)
            {
                if (lent.GetId() == id)
                {
                    return lent;
                }
            }

            return null;
        }

        public static void CacheLivingEntity(LivingGameEntity lent)
        {
            if (!LivingGameEntity.cachedEntities.Contains(lent))
            {
                LivingGameEntity.cachedEntities.Add(lent);
            }
        }

        public static LivingGameEntity CloneByName(string lentName)
        {
            //foreach (GameEntity gent in GameEntity.GetCachedEntities())
            foreach (LivingGameEntity lent in LivingGameEntity.cachedEntities)
            {
                if (lent.GetName() == lentName)
                {
                    LivingGameEntity newLent = ObjectCloner.CloneObject(lent);
                    newLent.ValidateID();
                    return newLent;
                }
            }

            return null;
        }

        public double ViewRadius { get; set; }

        private int health = 100;
        private ArtificialIntelligence ai;
        private List<Weapon> weapons;// = new List<Weapon>();

        public LivingGameEntity(string entName, string entId = "")
            : base(entName, entId)
        {
            //give it a / the default AI...
            this.ai = new ArtificialIntelligence();
            this.weapons = new List<Weapon>();
            this.ViewRadius = 5;

            //LivingGameEntity.cachedEntities.Add(this);
            CacheLivingEntity(this);
        }

        public ArtificialIntelligence GetAI()
        {
            return this.ai;
        }

        public void SetAI(ArtificialIntelligence newAi)
        {
            if (newAi != null)
            {
                this.ai = Helpah.Clone(newAi).As<ArtificialIntelligence>();
                this.ai.SetBody(this);
            }
            else
            {
                this.ai = null;
            }
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

        public int GetHealth()
        {
            return this.health;
        }

        public void SetHealth(int newHealth)
        {
            this.health = newHealth;
            //Debug.log(this.GetName() + " health is now " + newHealth);

            if (this.health <= 0)
            {
                this.Kill();
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

        public new void Destroy()
        {
            while (this.weapons.Count > 0)
            {
                this.weapons[0].Destroy();
                this.weapons.RemoveAt(0);
            }
            LivingGameEntity.cachedEntities.Remove(this);
            base.Destroy();
        }

        public Weapon AddWeapon(Weapon weaponToAdd)
        {
            if (!this.weapons.Contains(weaponToAdd))
            {
                //Debug.log("Adding a weapon with projectile " + weaponToAdd.GetProjectile().GetName() + " to entitiy " + this.GetName());
                weaponToAdd = Helpah.Clone(weaponToAdd).As<Weapon>();
                weaponToAdd.SetOwner(this);
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
    }
}
