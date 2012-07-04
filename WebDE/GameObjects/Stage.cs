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

        //the number of tiles or units wide the level is
        private int width = 20;
        private int height = 16;

        //the default dimensions of the tiles 
        private int tileWidth = 40;
        private int tileHeight = 40;

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

        public void RemoveGameEntity(GameEntity GameEntityToRemove)
        {
            if (GameEntityToRemove is LightSource)
            {
                LightSource lightToRemove = (LightSource)GameEntityToRemove;
                if (this.stageLights.Contains(lightToRemove))
                {
                    this.stageLights.Remove(lightToRemove);
                }
            }
            else if (GameEntityToRemove is Tile)
            {
                Tile tileToRemove = (Tile)GameEntityToRemove;
                if (this.stageTiles.Contains(tileToRemove))
                {
                    this.stageTiles.Remove(tileToRemove);
                }
            }
            else
            {
                if (this.stageEntities.Contains(GameEntityToRemove))
                {
                    this.stageEntities.Remove(GameEntityToRemove);
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
                //need to check if the GameEntity is visible
                resultList.Add(tile);
            }

            return resultList;
        }

        public Tuple<int, int> GetSize()
        {
            return new Tuple<int, int>
                (this.width, this.height);
        }

        public void SetSize(int newWidth, int newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;

            //this.stageGui.SetSize(this.width * this.GetTileSize().First, this.height * this.GetTileSize().Second);
        }

        public void CreateRandomTiles()
        {
            for (int h = 0; h < this.height; h++)
            {
                for (int w = 0; w < this.width; w++)
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
            }

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

            //if the GameEntity is of type projectile and is outside of the bounds of the stage, remove it
            foreach (GameEntity ent in this.livingEntities)
            {
                if (ent is Projectile)
                {
                    if (!this.GetBounds().Contains(ent.GetPosition()))
                    {
                        Debug.log(ent.GetPosition().x + "," + ent.GetPosition().y + " isn\'t in " +
                            this.GetBounds().x + "," + this.GetBounds().width + "," + this.GetBounds().y + "," + this.GetBounds().height);
                        ent.Destroy();
                    }
                }
            }
        }

        //for now, just move entities around
        public void CalculateGameEntityPhysics()
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

        public Tuple<int, int> GetTileSize()
        {
            return new Tuple<int, int>
                (this.tileWidth, this.tileHeight);
        }

        public void SetTileSize(int newWidth, int newHeight)
        {
            this.tileWidth = newWidth;
            this.tileHeight = newHeight;
        }

        public List<LightSource> GetLights()
        {
            return this.stageLights;
        }

        public void AddLight(LightSource newLight)
        {
            this.stageLights.Add(newLight);
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
            return new Rectangle(0, 0, this.width, this.height);
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
    }
}
