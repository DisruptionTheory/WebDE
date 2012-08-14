using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Rendering;
using WebDE.GUI;

namespace WebDE.GameObjects
{
    public enum StageType
    {
        Tile = 0,
        Sidescrolling = 1,
        Isometric = 2
    }

    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public class Stage
    {
        public static Stage CurrentStage;

        private string strStageName = "New Stage";
        private List<GameEntity> stageEntities = new List<GameEntity>();
        private List<LivingGameEntity> livingEntities = new List<LivingGameEntity>();
        private List<Tile> stageTiles = new List<Tile>();
        private List<LightSource> stageLights = new List<LightSource>();
        private List<Area> stageAreas;
        private GuiLayer collisionMap;
        private GuiLayer stageGui;
        private Color backgroundColor = Color.Black;

        //the number of tiles or units wide the level is
        private Dimension size = new Dimension(20, 16);

        //the default dimensions of the tiles 
        Dimension tileSize = new Dimension(40, 40);

        //sets up all of the variables for the stage
        public Stage(string stageName, StageType stageType)
        {
            this.SetName(stageName);

            if (CurrentStage == null)
            {
                CurrentStage = this;
            }

            //Rectangle viewRect = new Rectangle(this.getpo
            Rectangle viewRect = View.GetMainView().GetViewArea();
            //stageGui = new GuiLayer(View.GetMainView(), viewRect);
        }

        //creates the stage, loads all of its needed materials, and sets it as the active stage
        public void Load()
        {
            foreach(GameEntity ent in this.stageEntities) {
                //load the GameEntity and its sprite into memory...
            }
        }

        public string getName()
        {
            return this.strStageName;
        }

        public void SetName(string newName)
        {
            this.strStageName = newName;
        }

        public void AddGameEntity(GameEntity GameEntityToAdd)
        {
            if (GameEntityToAdd is LivingGameEntity || GameEntityToAdd is Tile)
            {
                Debug.log("You're trying to add a living GameEntity or tile as a regular GameEntity.");
            }

            stageEntities.Add(GameEntityToAdd);
            GameEntityToAdd.SetParentStage(this);
        }

        public void RemoveGameEntity(GameEntity gameEntityToRemove)
        {
            if (gameEntityToRemove is LightSource)
            {
                LightSource lightToRemove = gameEntityToRemove.As<LightSource>();
                if (this.stageLights.Contains(lightToRemove))
                {
                    this.stageLights.Remove(lightToRemove);
                }
            }
            else if (gameEntityToRemove is Tile)
            {
                Tile tileToRemove = gameEntityToRemove.As<Tile>();
                if (this.stageTiles.Contains(tileToRemove))
                {
                    this.stageTiles.Remove(tileToRemove);
                }
            }
            else if (gameEntityToRemove is LivingGameEntity)
            {
                LivingGameEntity livingEntityToRemove = gameEntityToRemove.As<LivingGameEntity>();
                if (this.livingEntities.Contains(livingEntityToRemove))
                {
                    this.livingEntities.Remove(livingEntityToRemove);
                }
            }
            else
            {
                if (this.stageEntities.Contains(gameEntityToRemove))
                {
                    this.stageEntities.Remove(gameEntityToRemove);
                }
            }
        }

        public Tile AddTile(string name, bool walkable, bool buildable)
        {
            Tile newTile = new Tile(name, walkable, buildable);
            this.stageTiles.Add(newTile);
            return newTile;
        }

        public void AppendTile(Tile tileToAdd)
        {
            this.stageTiles.Add(tileToAdd);
        }

        public void AddLivingGameEntity(LivingGameEntity toAdd)
        {
            this.livingEntities.Add(toAdd);
        }

        public List<LivingGameEntity> GetLivingEntities()
        {
            return this.livingEntities;
        }

        /*
        public void addEnvironment(Environment environmentToAdd)
        {
        }
         */

        //return a list of all of the tiles which are visible according to the restrictions of the passed View
        public List<GameEntity> GetVisibleEntities(View viewer)
        {
            List<GameEntity> resultList = new List<GameEntity>();

            foreach (GameEntity ent in this.stageEntities)
            {
                //need to check if the GameEntity is visible
                resultList.Add(ent);
            }
            foreach (LivingGameEntity lent in this.livingEntities)
            {
                resultList.Add(lent);
            }

            return resultList;
        }

        public List<GameEntity> GetVisibleTiles(View viewer)
        {
            List<GameEntity> resultList = new List<GameEntity>();

            foreach (Tile tile in this.stageTiles)
            {
                //if the tile is the same color as the background, we don't need it to be visible
                //if (!tile.GetLightLevel().Match(this.GetBackgroundColor()))// || tile.GetLightLevel() != null)
                if (!this.GetBackgroundColor().Match(tile.GetLightLevel()))// || tile.GetLightLevel() != null)
                {
                    resultList.Add(tile);
                }
            }

            return resultList;
        }

        public Dimension GetSize()
        {
            return this.size;
        }

        public void SetSize(int newWidth, int newHeight)
        {
            this.size.width = newWidth;
            this.size.height = newHeight;

            //this.stageGui.SetSize(this.size.width * this.GetTileSize().First, this.size.height * this.GetTileSize().Second);
        }

        public void CreateRandomTiles()
        {
            for (int h = 0; h < this.size.height; h++)
            {
                for (int w = 0; w < this.size.width; w++)
                {
                    //with the randomization of the tiles, we're going to randomize whether or not this is buildiable
                    int rand = JsMath.round(JsMath.random());
                    //int rand = (new Random()).Next(0, 1);
                    //check whether the random # is 0 or 1, the long way, because apparently boolean.parse won't figure it out
                    bool buildable = false;
                    if (rand == 1)
                    {
                        buildable = true;
                    }
                    Tile aTile = new Tile("", true, buildable);
                    aTile.SetParentStage(this);
                    aTile.SetPosition(w, h);
                    stageTiles.Add(aTile);
                }
            }
        }

        //preform all of the necessary calculation for the stage's entities
        public void CalculateEntities()
        {
            foreach (LivingGameEntity lent in this.livingEntities)
            {
                lent.Think();

                //if the GameEntity is of type projectile and is outside of the bounds of the stage, remove it
                if (lent is Projectile)
                {
                    if (!this.GetBounds().Contains(lent.GetPosition()))
                    {
                        lent.Destroy();
                    }
                }
            }

            /*
            //should we filter the stage's entities?
            foreach (GameEntity ent in this.stageEntities)
            {
                //run the GameEntity's current AI loop
                try
                {
                    //((LivingGameEntity)ent).Think();
                }
                catch
                {
                }
                //calculate the GameEntity's current speed
                //ent.CalculateSpeed();
                //calculate the GameEntity's current position (based on speed)
                //ent.CalculatePosition();
            }
            */
        }

        //for now, just move entities around
        public void CalculateGameEntityPhysics()
        {
            //should we filter the stage's entities?
            for (int i = 0; i < this.stageEntities.Count; i++)
            {
                //calculate the GameEntity's current speed
                this.stageEntities[i].CalculateSpeed();
                //calculate the GameEntity's current position (based on speed)
                this.stageEntities[i].CalculatePosition();
            }
            //do the same for living entities
            for (int i = 0; i < this.livingEntities.Count; i++)
            {
                this.livingEntities[i].CalculateSpeed();
                this.livingEntities[i].CalculatePosition();
            }
        }

        //for now, just move entities around
        public void old_CalculateGameEntityPhysics()
        {
            //should we filter the stage's entities?
            foreach (GameEntity ent in this.stageEntities)
            {
                //calculate the GameEntity's current speed
                ent.CalculateSpeed();
                //calculate the GameEntity's current position (based on speed)
                ent.CalculatePosition();
            }
            foreach (LivingGameEntity lent in this.livingEntities)
            {
                //calculate the GameEntity's current speed
                lent.CalculateSpeed();
                //calculate the GameEntity's current position (based on speed)
                lent.CalculatePosition();
            }

            //check projectiles for collisions
        }

        public void CalculateLights()
        {
            for(int i = 0; i < this.GetLights().Count; i++) 
            //foreach (LightSource light in this.GetLights())
            {
                LightSource light = this.GetLights()[i];
                if (light != null)
                {
                    light.Think();
                }
            }
        }

        public Dimension GetTileSize()
        {
            return this.tileSize;
        }

        public void SetTileSize(int newWidth, int newHeight)
        {
            this.tileSize.width = newWidth;
            this.tileSize.height = newHeight;
        }

        public List<LightSource> GetLights()
        {
            return this.stageLights;
        }

        public void AddLight(LightSource newLight)
        {
            if (!this.stageLights.Contains(newLight))
            {
                this.stageLights.Add(newLight);
            }
        }

        public List<Area> Subdivide()
        {
            this.stageAreas = new List<Area>();
            //add areas to the stage based on the size of the stage with subdivision...
            return this.stageAreas;
        }

        public Boolean IsSubdivided()
        {
            if (this.stageAreas == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public GuiLayer GetCollisionMap()
        {
            if (collisionMap == null)
            {
                this.RenderCollisionMap();
            }

            return this.collisionMap;
        }

        public void RenderCollisionMap()
        {
            collisionMap = GuiLayer.AsCollisionMap(this);
        }

        //show the collision map for this stage to the player
        public void ShowCollisionMap()
        {
            if (collisionMap == null)
            {
                this.RenderCollisionMap();
            }

            collisionMap.Activate();
            collisionMap.Show();
        }

        public void HideCollisionMap()
        {
            collisionMap.Deactivate();
            collisionMap.Hide();
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(0, 0, this.size.width, this.size.height);
        }

        public Tile GetTileAt(int xPos, int yPos)
        {
            foreach (Tile tile in this.stageTiles)
            {
                if (tile.GetPosition().x == xPos && tile.GetPosition().y == yPos)
                {
                    return tile;
                }
            }

            //Debug.log("Couldn't find a tile at " + xPos + ", " + yPos);

            return null;
        }

        public List<GameEntity> GetEntitiesNear(Point p, int distance)
        {
            List<GameEntity> returnVals = new List<GameEntity>();

            foreach (GameEntity gent in this.stageEntities)
            {
                if (gent.GetPosition().Distance(p) <= distance)
                {
                    returnVals.Add(gent);
                }
            }

            foreach (GameEntity gent in this.livingEntities)
            {
                if (gent.GetPosition().Distance(p) <= distance)
                {
                    returnVals.Add(gent);
                }
            }

            return returnVals;
        }

        public List<GameEntity> GetEntitiesNear(Point p, double distance)
        {
            return this.GetEntitiesNear(p, Helpah.d2i(distance));
        }

        public List<GameEntity> GetPhysicsEntitiesNear(Point p, double distance)
        {
            List<GameEntity> entList = this.GetEntitiesNear(p, distance);
            foreach(GameEntity gent in entList)
            {
                //if(gent.obeysphysics
            }

            return entList;
        }

        public List<LightSource> GetLightsNear(Point p, int distance)
        {
            List<LightSource> returnVals = new List<LightSource>();

            foreach (LightSource light in this.stageLights)
            {
                if (light.GetPosition().Distance(p) <= distance)
                {
                    returnVals.Add(light);
                }
            }

            return returnVals;
        }

        public Color GetBackgroundColor()
        {
            return this.backgroundColor;
        }

        public void SetBackgroundColor(Color newColor)
        {
            this.backgroundColor = newColor;
        }
    }
}
