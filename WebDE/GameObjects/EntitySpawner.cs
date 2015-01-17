using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Clock;
using WebDE.AI;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class GameEntitySpawner : GameEntity
    {
        private List<GameEntityBatch> spawnBatches = new List<GameEntityBatch>();
        //the amount of time until the next spawn
        private int currentSpawnDelay = 0;
        //if the spawner is activated, the id of the event firing
        private string clockId = null;
        // The action to call when the spawner spawns an entity.
        public Action<GameEntitySpawner, GameEntity> EntitySpawned = null;
        //the default faction for entities spawned from this spawner
        public Faction DefaultFaction { get; set; }

        public GameEntitySpawner(string itemName, int initialDelay)
            : base(itemName, false)
        {
            this.currentSpawnDelay = initialDelay;
            this.ObeysPhysics = false;
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

            this.Activate();
        }

        public void AddGameEntityBatch(string entityName, int GameEntityCount, int spawnDelay, ArtificialIntelligence sourceAI)
        {
            GameEntityBatch newBatch = new GameEntityBatch(entityName, GameEntityCount, spawnDelay, sourceAI);
            this.spawnBatches.Add(newBatch);

            this.Activate();
        }

        //as above, but duplacte to X batches
        public void AddGameEntityBatches(GameEntity GameEntityType, int GameEntityCount, int batchCount, int spawnDelay)
        {
            while (batchCount > 0)
            {
                AddGameEntityBatch(GameEntityType, GameEntityCount, spawnDelay);
                batchCount--;
            }

            this.Activate();
        }

        //add think to a calculate clock
        //since calculation will happen as fast as possible, we need to count using regular time
        public void Activate()
        {
            //change the current spawn batch?

            if (this.clockId == null || this.clockId == "")
            {
                this.clockId = Game.Clock.IntervalExecute(this.Think, 1);
            }
        }

        public void Deactivate()
        {
            if (this.clockId != null && this.clockId != "")
            {
                Game.Clock.CancelIntervalExecute(this.clockId);
                this.clockId = "";
                //Clock.RemoveCalculation(this.clockId);
            }
        }

        private int currentBatch = 0;
        private int currentSpawnItem = 0;

        public void Think()
        {
            //if the spawner isn't in a stage yet, we don't have anywhere to put spawned entities
            if (this.GetParentStage() == null || this.spawnBatches.Count <= this.currentBatch)
            {
                this.Deactivate();
                return;
            }

            //presumably a little sub-optimal, in order to support looping entity spawning

            if (this.currentSpawnDelay > 0)
            {
                this.currentSpawnDelay--;
                return;
            }

            //delay the next spawn for the set amount of time
            this.currentSpawnDelay = spawnBatches[currentBatch].spawnDelay;

            // The batch spwans forever.
            if (spawnBatches[currentBatch].GameEntityCount == -1)
            {
                this.SpawnEntity();
            }
            // Spawn the current entity in the batch.
            else if (spawnBatches[currentBatch].GameEntityCount > currentSpawnItem)
            {
                this.SpawnEntity();

                this.currentSpawnItem++;
            }
            //the batch is done spawning. Move on to the next batch
            else
            {
                this.currentBatch++;
                this.currentSpawnItem = 0;

                //if it loops, check if the next batch is going to set us past our max
            }
        }

        private void SpawnEntity()
        {
            LivingGameEntity newEnt;
            // Cloning isn't working right, so we don't want to do this.
            if (GameEntity.Cached(spawnBatches[currentBatch].GameEntityName) && 1 == 2)
            {
                newEnt = LivingGameEntity.CloneByName(spawnBatches[currentBatch].GameEntityName);
            }
            else
            {
                newEnt = new LivingGameEntity(spawnBatches[currentBatch].GameEntityName);
            }
            newEnt.Faction = this.DefaultFaction;
            newEnt.SetPosition(this.GetPosition().x, this.GetPosition().y);
            newEnt.SetParentStage(this.GetParentStage());
            this.GetParentStage().AddGameEntity(newEnt);
            if (spawnBatches[currentBatch].templateAI != null)
            {
                newEnt.SetAI(spawnBatches[currentBatch].templateAI.Clone());
            }

            if (this.EntitySpawned != null)
            {
                try
                {
                    this.EntitySpawned.Invoke(this, newEnt);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public int GetCurrentSpawnPosition()
        {
            // This needs to be a count starting at 1, not 0, so increment it by 1.
            return this.currentSpawnItem + 1;
        }

        public int GetCurrentSpawnCount()
        {
            return this.spawnBatches[currentBatch].GameEntityCount;
        }

        public int GetCurrentBatch()
        {
            // This needs to be a count starting at 1, not 0, so increment it by 1.
            return this.currentBatch + 1;
        }

        public int GetBatchCount()
        {
            return this.spawnBatches.Count;
        }

        public bool Active{
            get {
            // If we have a clock ID, we are active. If not, not active.
            return this.clockId != "";}
        }
    }
}
