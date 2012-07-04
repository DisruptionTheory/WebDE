using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Timekeeper;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class GameEntitySpawner: GameEntity
    {
        private List<GameEntityBatch> spawnBatches = new List<GameEntityBatch>(); 
        //the amount of time until the next spawn
        private int currentSpawnDelay = 0;
        //if the spawner is activated, the id of the event firing
        private string clockId = null;

        public GameEntitySpawner(string itemName, int initialDelay)
            : base(itemName)
        {
            this.currentSpawnDelay = initialDelay;

            //add think to a calculate clock
            //since calculation will happen as fast as possible, we need to count using regular time
            Clock.IntervalExecute(this.Think, 1);
        }        

        /// <summary>
        /// Add a batch of entities to be spawned by the spawner.
        /// Entities will be spawned in the order that they are added to the spawner,
        /// with the first entries being spawned first.
        /// </summary>
        /// <param name="GameEntityType">The type of GameEntity to spawn.</param>
        /// <param name="count">The number of entities to spawn.</param>
        /// <param name="spawnDelay">How long to wait until spawning the next batch.</param>
        public void AddGameEntityBatch(GameEntity GameEntityType, int GameEntityCount, int spawnDelay)
        {
            GameEntityBatch newBatch = new GameEntityBatch(GameEntityType, GameEntityCount, spawnDelay);
            this.spawnBatches.Add(newBatch);
        }

        //as above, but duplacte to X batches
        public void AddGameEntityBatches(GameEntity GameEntityType, int GameEntityCount, int batchCount, int spawnDelay)
        {
            while (batchCount > 0)
            {
                AddGameEntityBatch(GameEntityType, GameEntityCount, spawnDelay);
                batchCount--;
            }
        }

        public void Activate()
        {
            this.clockId = Clock.AddCalculation(this.Think);
        }

        public void Deactivate()
        {
            if (this.clockId != null)
            {
                Clock.RemoveCalculation(this.clockId);
            }
        }

        public void Think()
        {
            //need to return if this.GetParentStage() == null

            //if we still have batches to spawn...
            if (this.spawnBatches.Count > 0)
            {
                //if the spawner is activated, delay until the next spawn
                if (this.currentSpawnDelay > 0)
                {
                    this.currentSpawnDelay--;
                    return;
                }

                //create the given number of entities
                GameEntityBatch batchToSpawn = spawnBatches[0];
                int entitiesToSpawn = spawnBatches[0].GameEntityCount;
                while (entitiesToSpawn > 0)
                {
                    LivingGameEntity entSrc = (LivingGameEntity) spawnBatches[0].GameEntityType;
                    //LivingGameEntity newEnt = (LivingGameEntity) Helpah.Clone(spawnBatches[0].GameEntityType);
                    LivingGameEntity newEnt = Helpah.Clone(spawnBatches[0].GameEntityType).As<LivingGameEntity>();
                    newEnt.SetPosition(this.GetPosition().x, this.GetPosition().y);
                    newEnt.SetAI(entSrc.GetAI());
                    newEnt.SetParentStage(this.GetParentStage());
                    this.GetParentStage().AddLivingGameEntity(newEnt);
                    entitiesToSpawn--;
                }

                //the batch is done spawning. Set our current delay, and remove the batch from the list
                this.currentSpawnDelay = spawnBatches[0].spawnDelay;
                spawnBatches.RemoveAt(0);
            }
            else
            {
                //we're out of things to spawn. remove the calculation from the clock
                this.Deactivate();
            }
        }
    }
}
