using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Rendering;
using WebDE.GUI;
using WebDE.Animation;
using WebDE.AI;
using WebDE.Misc;

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

        /// <summary>
        /// The width of the stage (in tiles).
        /// </summary>
        public int Width { get { return (int) size.width; } }
        /// <summary>
        /// The Height of the stage (in tiles).
        /// </summary>
        public int Height { get { return (int) size.height; } }
        public int PixelWidth { get { return (int)(size.width * tileSize.width); } }
        public int PixelHeight { get { return (int)(size.height * tileSize.height); } }
        public Pathfinder Pathfinder;

        private string strStageName = "New Stage";
        private List<GameEntity> stageEntities = new List<GameEntity>();
        private List<LivingGameEntity> livingEntities = new List<LivingGameEntity>();
        private List<Tile> stageTiles = new List<Tile>();
        private List<LightSource> stageLights = new List<LightSource>();
        private List<TerrainCollider> terrainColliders = new List<TerrainCollider>();
        private List<Area> stageAreas;
        private GuiLayer collisionMap;
        private GuiLayer stageGui;
        private Color backgroundColor = Color.Black;
        // The color of the light in the stage. If not full white with 255 alpha, will tint everything in the stage.
        private Color ambientLight = Color.White;
        // Default to disabled gravity.
        private Vector stageGravity = new Vector(0, 0);
        //the number of tiles or units wide the level is
        private Dimension size = new Dimension(20, 15);
        //the default dimensions of the tiles 
        Dimension tileSize = new Dimension(40, 40);
        private Dictionary<View, Sprite> backgroundSprites = new Dictionary<View, Sprite>();

        public Color AmbientLightLevel { get; set; }
        public StageType StageType { get; set; }

        //sets up all of the variables for the stage
        public Stage(string stageName, StageType stageType)
        {
            this.SetName(stageName);
            this.StageType = stageType;
            this.AmbientLightLevel = Color.White;

            if (CurrentStage == null)
            {
                CurrentStage = this;
            }
        }

        public Stage(string stageName, StageType stageType, Dimension stageSize)
        {
            this.SetName(stageName);
            this.StageType = stageType;
            this.AmbientLightLevel = Color.White;

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

        public void AddGameEntity(GameEntity gameEntityToAdd)
        {
            if (gameEntityToAdd == null) return;

            if (gameEntityToAdd.GetTypeName() == "LivingGameEntity" || gameEntityToAdd.GetTypeName() == "Projectile") // || GameEntityToAdd.GetTypeName() == "Tile")
            {
                //LivingGameEntity entityToAdd = gameEntityToAdd.As<LivingGameEntity>();
                LivingGameEntity entityToAdd = (LivingGameEntity) gameEntityToAdd;
                //this.livingEntities.Add(gameEntityToAdd.As<LivingGameEntity>());
                if (this.livingEntities.Contains(entityToAdd)) return;
                this.livingEntities.Add(entityToAdd);
            }
            else if (gameEntityToAdd.GetTypeName() == "TerrainCollider")
            {
                //Debug.log("Here. This. Do not want.");
                if (this.terrainColliders.Contains(gameEntityToAdd.As<TerrainCollider>())) return;
                terrainColliders.Add((TerrainCollider)gameEntityToAdd);
            }
            else
            {
                if (this.stageEntities.Contains(gameEntityToAdd)) return;
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
            else if (gameEntityToRemove .GetTypeName() == "Tile")
            {
                Tile tileToRemove = gameEntityToRemove.As<Tile>();
                if (this.stageTiles.Contains(tileToRemove))
                {
                    this.stageTiles.Remove(tileToRemove);
                }
            }
            else if (gameEntityToRemove .GetTypeName() == "LivingGameEntity" || gameEntityToRemove .GetTypeName() == "Projectile")
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
            foreach(Tile tile in this.stageTiles)
            {
                if(tile.GetPosition().x == tilePos.x && tile.GetPosition().y == tilePos.y)
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

        /// <summary>
        /// Get the area of the stage (in game units) that the renderer should render.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetVisibleArea()
        {
            Debug.log("Not implemented!");
            return null;
        }

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
            if (viewer.EntitiesVisible == false) return resultList;

            foreach (TerrainCollider terCol in this.terrainColliders)
            {
                if (terCol.Visible(viewer) == true)
                {
                    resultList.Add(terCol);
                }
            }
            foreach (GameEntity ent in this.stageEntities)
            {
                //need to check if the GameEntity is visible
                if (ent.Visible(viewer) == true)
                {
                    resultList.Add(ent);
                }
            }
            foreach (LivingGameEntity lent in this.livingEntities)
            {
                if (lent.Visible(viewer) == true)
                {
                    resultList.Add(lent);
                }
            }

            return resultList;
        }

        public List<Tile> GetTiles(bool visible = false, View view = null)
        {
            Debug.Watch("StageTiles", this.stageTiles.Count + "");

            // If we're filtering invisible tiles (visible parameter is true), then we need to process that filter.
            if (visible == true && view != null)
            {
                List<Tile> resultList = new List<Tile>();

                foreach (Tile tile in this.stageTiles)
                {
                    if (tile.Visible(view) == true)
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
            if (viewer.TilesVisible == false) return new List<GameEntity>();

            Debug.Watch("StageTiles", this.stageTiles.Count + "");
            List<GameEntity> resultList = new List<GameEntity>();

            foreach (Tile tile in this.stageTiles)
            {
                if (tile.Visible(viewer) == true)
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
            Game.Renderer.Resize();
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

        /// <summary>
        /// Fill the map with empty visible tiles.
        /// </summary>
        public void FillTiles()
        {
            for (int h = 0; h < this.size.height; h++)
            {
                for (int w = 0; w < this.size.width; w++)
                {
                    Tile aTile = new Tile("", true, true);
                    aTile.SetParentStage(this);
                    aTile.SetPosition(w, h);
                    stageTiles.Add(aTile);
                }
            }
        }

        //preform all of the necessary calculation for the stage's entities
        public void CalculateEntities()
        {
            for(int i = 0; i < this.livingEntities.Count; i++)
            //foreach (LivingGameEntity lent in this.livingEntities)
            {
                LivingGameEntity lent = this.livingEntities[i];

                if (lent.GetAI() != null)
                {
                    lent.Think();
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

        public void CalculateLights(View view)
        {
            for(int i = 0; i < this.GetLights(view).Count; i++) 
            //foreach (LightSource light in this.GetLights())
            {
                LightSource light = this.GetLights(view)[i];
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

        public void SetTileSize(Dimension newSize)
        {
            this.tileSize = newSize;
        }

        public void SetTileSize(int newWidth, int newHeight)
        {
            this.tileSize = new Dimension(newWidth, newHeight);
        }

        public List<LightSource> GetLights(View view)
        {
            if (view.LightsVisible == false) return new List<LightSource>();

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

        public Tile GetTileAt(double xPos, double yPos)
        {
            return GetTileAt((int)xPos, (int)yPos);
        }

        public List<GameEntity> GetEntitiesNear(Point p, double distance, bool tilesToo = false)
        {
            // These should all be changed to an ellipsoid contain check.

            List<GameEntity> returnVals = new List<GameEntity>();

            /*
            foreach (TerrainCollider terCol in this.terrainColliders)
            {
                if(p.Within(distance, terCol.GetArea()))
                {
                    returnVals.Add(terCol);
                }
            }
            */
            foreach (GameEntity gent in this.stageEntities)
            {
                if (gent.GetPosition().Distance(p) <= distance)
                {
                    returnVals.Add(gent);
                }
            }

            foreach (GameEntity lent in this.livingEntities)
            {
                if (lent.GetPosition().Distance(p) <= distance)
                {
                    returnVals.Add(lent);
                }
            }

            if (tilesToo == true)
            {
                foreach (Tile tile in this.stageTiles)
                {
                    if (tile.GetPosition().Distance(p) <= distance)
                    {
                        returnVals.Add(tile);
                    }
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

        public List<TerrainCollider> GetTerrainCollidersNear(Point point, double distance)
        {
            if (distance < 0)
            {
                return terrainColliders;
            }

            List<TerrainCollider> returnColliders = new List<TerrainCollider>();

            foreach (TerrainCollider terrainCollider in terrainColliders)
            {
                //if (terrainCollider.GetPosition().Distance(point) < distance)
                if (point.Within(distance, terrainCollider.GetArea()))
                {
                    returnColliders.Add(terrainCollider);
                }
                /* else {
                    Rectangle recy = new Rectangle(terrainCollider.GetPosition().x, terrainCollider.GetPosition().y, 
                        terrainCollider.GetSize().width, terrainCollider.GetSize().height);
                    if (recy.Contains(point))
                    {
                        returnColliders.Add(terrainCollider);
                    }
                } */
            }

            return returnColliders;
        }

        public void ToggleGravity(bool state)
        {
            throw new NotImplementedException();
        }

        public Vector GetGravity()
        {
            return this.stageGravity;
        }

        public void SetGravity(Vector newGravity)
        {
            this.stageGravity = newGravity;
        }

        // Check whether an entity is physically in the stage.
        public bool Contains(GameEntity gent)
        {
            // If it's not in the absolute bounds of the stage, it won't be within the more constricting bounds.
            if (!this.GetBounds().Contains(gent.GetPosition()))
            {
                return false;
            }

            // Should maybe check if it's closer to the top or bottom... ?

            // Check the more constricting bounds
            if (gent.GetPosition().y > MaxY(gent.GetPosition().x) ||
                gent.GetPosition().y < MinY(gent.GetPosition().x))
            {
                return false;
            }

            return true;
        }

        public double MaxY(double givenX)
        {
            double maxY = this.size.height;

            foreach (TerrainCollider terrainCollider in this.terrainColliders)
            {
                // If the collider touches the top of this place.
                if (terrainCollider.GetPosition().y + terrainCollider.Height >= maxY)
                {
                    if (terrainCollider.GetPosition().x + terrainCollider.Width >= givenX && givenX >= terrainCollider.GetPosition().x)
                    {
                        maxY = maxY - terrainCollider.YatX(givenX);
                    }
                }
            }

            return maxY;
        }

        public double MinY(double givenX)
        {
            double minY = 0;

            foreach (TerrainCollider terrainCollider in this.terrainColliders)
            {
                // If the collider touches the top of this place.
                if (terrainCollider.GetPosition().y <= 0)
                {
                    if (terrainCollider.GetPosition().x + terrainCollider.Width >= givenX && givenX >= terrainCollider.GetPosition().x)
                    {
                        // Need to do the specific height for the terrain collider
                        minY = minY + terrainCollider.GetPosition().y + terrainCollider.YatX(givenX);
                    }
                }
            }

            Debug.Watch("MinY", minY.ToString());
            return minY;
        }
        
        public Sprite GetBackgroundSprite(View view)
        {
            if (this.backgroundSprites.ContainsKey(view))
            {
                return this.backgroundSprites[view];
            }

            return null;
        }

        public void SetBackgroundSprite(Sprite sprite, View view)
        {
            this.backgroundSprites[view] = sprite;
        }
    }
}
