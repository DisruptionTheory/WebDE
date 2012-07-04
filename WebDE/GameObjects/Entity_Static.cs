using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

//the static components of the GameEntity class
//(generally?) used for dealing with all of the entities,
//or groups of entities not tied to a particular object such as a stage

//frankly I'm not sure any of this is being called or yet needs to be...

using WebDE.Animation;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    partial class GameEntity
    {
        private static List<GameEntity> cachedEntities = new List<GameEntity>();
        //the last used id
        private static int lastid = 0;

        public static GameEntity GetById(string id)
        {
            foreach (GameEntity ent in cachedEntities)
            {
                if (ent.id == id)
                {
                    return ent;
                }
            }

            return null;
        }

        public static List<GameEntity> GetCachedEntities()
        {
            return GameEntity.cachedEntities;
        }

        public static GameEntity isGameEntityLoaded()
        {
            return null;
        }
    }
}
