using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Timekeeper;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public class GameEntityBatch
    {
        public readonly GameEntity GameEntityType;
        public readonly int GameEntityCount;
        public readonly int spawnDelay;

        public GameEntityBatch(GameEntity GameEntityType, int GameEntityCount, int spawnDelay)
        {
            this.GameEntityType = GameEntityType;
            this.GameEntityCount = GameEntityCount;
            this.spawnDelay = spawnDelay;
        }
    }
}
