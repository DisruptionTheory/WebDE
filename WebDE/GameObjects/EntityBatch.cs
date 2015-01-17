using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Clock;
using WebDE.AI;

namespace WebDE.GameObjects
{
    //"hey Eric, why is everything in this class read only?" You may ask
    //Well, it's because I wanted to be able to directly access properties when creating and using the class,
    //But make sure they couldn't be edited post-creation
    //The class is meant to be uchanged once it's declared. Hence, readonly
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public class GameEntityBatch
    {
        public readonly string GameEntityName;
        public readonly GameEntity GameEntityType;
        public readonly int GameEntityCount;
        public readonly int spawnDelay;

        public ArtificialIntelligence templateAI = null;

        public GameEntityBatch(GameEntity GameEntityType, int GameEntityCount, int spawnDelay)
        {
            this.GameEntityType = GameEntityType;
            this.GameEntityCount = GameEntityCount;
            this.spawnDelay = spawnDelay;
        }

        public GameEntityBatch(string entName, int entCount, int spawnDelay, ArtificialIntelligence sourceAI)
        {
            this.GameEntityName = entName;
            this.GameEntityCount = entCount;
            this.spawnDelay = spawnDelay;
            this.templateAI = sourceAI;
        }
    }
}
