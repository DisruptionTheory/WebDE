using System;
using System.Collections.Generic;

namespace DisruptionEngine
{
    public class Stage
    {
        public static Stage CurrentStage;
        /// <summary>
        /// The width of the stage (in tiles).
        /// </summary>
        public double Width { get { return size.width; } }
        /// <summary>
        /// The Height of the stage (in tiles).
        /// </summary>
        public double Height { get { return size.height; } }
        public int PixelWidth { get { return (int)(size.width * tileSize.width); } }
        public int PixelHeight { get { return (int)(size.height * tileSize.height); } }

        private string strStageName = "New Stage";
        //private List<GameEntity> stageEntities = new List<GameEntity>();
        //private List<LivingGameEntity> livingEntities = new List<LivingGameEntity>();
        //private List<Tile> stageTiles = new List<Tile>();
        private Color backgroundColor = Color.Black;

        //the number of tiles or units wide the Stage is
        private Dimension size = new Dimension(20, 15);

        //the default dimensions of the tiles (pixels to tile)
        Dimension tileSize = new Dimension(40, 40);

        //sets up all of the variables for the stage
        public Stage(string stageName)
        {
            this.SetName(stageName);

            if (CurrentStage == null)
            {
                CurrentStage = this;
            }
        }

        public Stage(string stageName, Dimension stageSize)
        {
            this.SetName(stageName);

            if (CurrentStage == null)
            {
                CurrentStage = this;
            }

            this.size = stageSize;

            //return;

            // Create a set of empty tiles.
            for (double w = 0; w < this.size.width; w++)
            {
                for (double h = 0; h < this.size.height; h++)
                {
                    this.AddTile("", true, true, new Point(w, h));
                }
            }
        }

        //creates the stage, loads all of its needed materials, and sets it as the active stage
        public void Load()
        {
            foreach (GameEntity ent in this.stageEntities)
            {
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

        public void AddGameEntity(GameEntity gameEntityToAdd)
        {
            if (gameEntityToAdd.GetTypeName() == "LivingGameEntity" || gameEntityToAdd.GetTypeName() == "Projectile") // || GameEntityToAdd.GetTypeName() == "Tile")
            {
                this.livingEntities.Add(gameEntityToAdd.As<LivingGameEntity>());
            }
            else
            {
                stageEntities.Add(gameEntityToAdd);
            }

            gameEntityToAdd.SetParentStage(this);
        }

        public void RemoveGameEntity(GameEntity gameEntityToRemove)
        {
            if (gameEntityToRemove.GetTypeName() == "LightSource")
            {
                LightSource lightToRemove = gameEntityToRemove.As<LightSource>();
                if (this.stageLights.Contains(lightToRemove))
                {
                    this.stageLights.Remove(lightToRemove);
                }
            }
            else if (gameEntityToRemove.GetTypeName() == "Tile")
            {
                Tile tileToRemove = gameEntityToRemove.As<Tile>();
                if (this.stageTiles.Contains(tileToRemove))
                {
                    this.stageTiles.Remove(tileToRemove);
                }
            }
            else if (gameEntityToRemove.GetTypeName() == "LivingGameEntity" || gameEntityToRemove.GetTypeName() == "Projectile")
            {
                LivingGameEntity livingEntityToRemove = gameEntityToRemove.As<LivingGameEntity>();
                if (this.livingEntities.Contains(livingEntityToRemove))
                {
                    this.livingEntities.Remove(livingEntityToRemove);
                }
                else
                {
                    Debug.log("Warning: Living entity list does not contain " + livingEntityToRemove.GetId() + " ( " + livingEntityToRemove.GetName() + " )");
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

        public GameEntity CreateGameEntity(string gameEntName, string spriteName)
        {
            GameEntity newEnt = new GameEntity(gameEntName);
            newEnt.SetSprite(Sprite.GetSpriteByName(spriteName));
            this.AddGameEntity(newEnt);

            return newEnt;
        }

        public Tile AddTile(string name, bool walkable, bool buildable, Point tilePos)
        {
            // If a tile already exists at this position, destroy it (for now)...
            foreach (Tile tile in this.stageTiles)
            {
                if (tile.GetPosition().x == tilePos.x && tile.GetPosition().y == tilePos.y)
                {
                    //this.stageTiles.Remove(tile);
                    //tile.Destroy();
                    tile.SetBuildable(buildable);
                    tile.SetWalkable(walkable);
                    tile.SetSprite(Sprite.GetSpriteByName(name));

                    return tile;
                }
            }

            // (else)
            Tile newTile = new Tile(name, walkable, buildable);
            newTile.SetPosition(tilePos.x, tilePos.y);
            this.stageTiles.Add(newTile);
            newTile.SetParentStage(this);

            return newTile;
        }

        public void SetBackgroundTile(Tile tile)
        {
            for (short row = 0; row < this.size.height; row++)
            {
                for (short col = 0; col < this.size.width; col++)
                {
                    Tile newTile = new Tile(tile.GetName(), tile.GetWalkable(), tile.GetBuildable());
                    newTile.SetPosition(col, row);
                    this.stageTiles.Add(newTile);
                    newTile.SetParentStage(this);
                }
            }
        }

        public void AppendTile(Tile tileToAdd)
        {
            this.stageTiles.Add(tileToAdd);
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

        public List<GameEntity> GetVisibleEntities()
        {
            //use current view...
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

        //return a list of all of the tiles which are visible according to the restrictions of the passed View
        public List<GameEntity> GetVisibleEntities(View viewer)
        {
            List<GameEntity> resultList = new List<GameEntity>();

            foreach (GameEntity ent in this.stageEntities)
            {
                //need to check if the GameEntity is visible
                if (ent.Visible() == true)
                {
                    resultList.Add(ent);
                }
            }
            foreach (LivingGameEntity lent in this.livingEntities)
            {
                if (lent.Visible() == true)
                {
                    resultList.Add(lent);
                }
            }

            return resultList;
        }

        public List<Tile> GetTiles(bool visible = false)
        {
            Debug.Watch("StageTiles", this.stageTiles.Count + "");

            // If we're filtering invisible tiles (visible parameter is true), then we need to process that filter.
            if (visible == true)
            {
                List<Tile> resultList = new List<Tile>();

                foreach (Tile tile in this.stageTiles)
                {
                    if (tile.Visible() == true)
                    {
                        resultList.Add(tile);
                    }
                }

                return resultList;
            }
            // Otherwise, we can just return the list as-is.
            else
            {
                return this.stageTiles;
            }
        }

        public List<GameEntity> GetVisibleTiles(View viewer)
        {
            Debug.Watch("StageTiles", this.stageTiles.Count + "");
            List<GameEntity> resultList = new List<GameEntity>();

            foreach (Tile tile in this.stageTiles)
            {
                if (tile.Visible() == true)
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
            for (int i = 0; i < this.livingEntities.Count; i++)
            //foreach (LivingGameEntity lent in this.livingEntities)
            {
                LivingGameEntity lent = this.livingEntities[i];

                if (lent.GetAI() != null)
                {
                    lent.Think();
                }

                //if the GameEntity is of type projectile and is outside of the bounds of the stage, remove it
                if (lent.GetTypeName() == "Projectile")
                {
                    if (!this.GetBounds().Contains(lent.GetPosition()))
                    {
                        lent.Destroy();
                    }
                }
            }
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
        
        public Dimension GetTileSize()
        {
            return this.tileSize;
        }

        public void SetTileSize(int newWidth, int newHeight)
        {
            this.tileSize.width = newWidth;
            this.tileSize.height = newHeight;
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

        public Tile GetTileAt(double xPos, double yPos)
        {
            return GetTileAt((int)xPos, (int)yPos);
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
            foreach (GameEntity gent in entList)
            {
                //if(gent.obeysphysics
            }

            return entList;
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