using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.GameObjects
{
    //used to subdivide a stage for easier calculations
    //I really don't like the way this is. need to change it so that the resource allows tracking...somehow...
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Resource
    {
        private static List<Resource> gameResources = new List<Resource>();

        public static int GetIdResourceByName(string resourceName)
        {
            foreach (Resource res in gameResources)
            {
                if (res.name == resourceName)
                {
                    return res.id;
                }
            }

            return 0;
        }

        public static Resource ByName(string resourceName)
        {
            foreach (Resource res in gameResources)
            {
                if (res.name == resourceName)
                {
                    return res;
                }
            }
            return null;
        }

        private int id;
        private string name;
        private double amount;
        //tax amount(s) (and to who?)
        //inflation amount(s)
        //icon

        public Resource(string resourceName)
        {
            this.id = gameResources.Count + 1;
            this.name = resourceName;
            this.amount = 0;
        }

        public void SetName(string newName)
        {
        }

        public void SetAmount(double newAmount)
        {
        }
    }
}
