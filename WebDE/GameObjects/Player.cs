using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Player
    {
        public static Player LocalPlayer;

        public string Name { get; set; }

        private GameEntity avatar;
        //network information...
        //control / input information...
        public Faction Faction = new Faction("LocalPlayer", Color.Red);

        public Player(string name = "Player")
        {
            this.Name = name;

            // The first player created is the local player ...
            if (LocalPlayer == null)
            {
                LocalPlayer = this;
            }
        }

        public void AddResource(int resourceId, double resourceAmount)
        {
        }

        public void SetResource(int resourceId, double resourceAmount)
        {
        }

        public int GetResource(int resourceId)
        {
            return 0;
        }
    }
}
