using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Animation;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class Tile : GameEntity
    {
        private static List<Tile> loadedTiles = new List<Tile>();

        public static Tile GetByName(string tileName)
        {
            Tile newTile = null;

            foreach (Tile tile in Tile.loadedTiles)
            {
                if (tile.GetName() == tileName)
                {
                    //return (Tile)Helpah.Clone(tile);
                    newTile = new Tile(tileName, tile.GetWalkable(), tile.GetBuildable());
                    return newTile;
                }
            }

            if (newTile == null)
            {
                newTile = new Tile(tileName, true, true);
            }

            return newTile;
        }

        public static Tile oldGetByName(string tileName)
        {
            foreach (Tile tile in Tile.loadedTiles)
            {
                if (tile.GetName() == tileName)
                {
                    return Helpah.Clone(tile).As<Tile>();
                }
            }

            return null;
        }

        private Color lightLevel = null;
        private bool isWalkable = false;
        private bool isBuildable = false;

        public Tile(string tileName, bool canWalk, bool canBuild)
            : base(tileName)
        {
            this.isWalkable = canWalk;
            this.isBuildable = canBuild;

            Tile.loadedTiles.Add(this);
        }

        public Color GetLightLevel()
        {
            return this.lightLevel;
        }

        public void SetLightLevel(Color newLevel)
        {
            this.lightLevel = new Color(newLevel.red, newLevel.green, newLevel.blue);
        }

        //calculate the light level of this tile
        //rather than the below, each light influences the color of the tile toward something
        public Color CalculateLightLevel()
        {
            this.SetLightLevel(Color.Black);

            List<LightSource> localLights = LightSource.GetLocalLightSources(this.GetPosition().x, this.GetPosition().y);

            //no light sources, return with black
            if (localLights.Count == 0)
            {
                return this.lightLevel;
            }

            //all of the red, green, and blue values that will each be applied to this list
            List<int> reds = new List<int>();
            List<int> blues = new List<int>();
            List<int> greens = new List<int>();
            foreach (LightSource currentLight in localLights)
            {
                //get the distance between here and the other light
                double dist = this.GetPosition().Distance(currentLight.GetPosition());
                //the diminish amount is how powerfully the light affects the tile,
                //and is a percentage from 10% to 100% (0.1 to 1.0)
                double diminishAmount = dist * .1;

                int newRed = (int)(currentLight.GetColor().red - Helpah.Round(currentLight.GetColor().red * diminishAmount));
                int newGreen = (int)(currentLight.GetColor().green - Helpah.Round(currentLight.GetColor().green * diminishAmount));
                int newBlue = (int)(currentLight.GetColor().blue - Helpah.Round(currentLight.GetColor().blue * diminishAmount));
                reds.Add(newRed);
                greens.Add(newGreen);
                blues.Add(newBlue);
            }

            int avgRed = 0, avgBlue = 0, avgGreen = 0;
            foreach (int curRed in reds)
            {
                avgRed += curRed;
            }
            foreach (int curBlue in blues)
            {
                avgBlue += curBlue;
            }
            foreach (int curGreen in greens)
            {
                avgGreen += curGreen;
            }
            this.lightLevel.red = avgRed = avgRed / reds.Count;
            this.lightLevel.blue = avgBlue = avgBlue / blues.Count;
            this.lightLevel.green = avgGreen = avgGreen / greens.Count;

            return this.lightLevel;
        }

        //calculate the light level of this tile
        //this takes the brightest value and applies it
        public Color old_CalculateLightLevel()
        {
            this.SetLightLevel(Color.Black);

            List<LightSource> localLights = LightSource.GetLocalLightSources(this.GetPosition().x, this.GetPosition().y);
            foreach (LightSource currentLight in localLights)
            {
                //get the distance between here and the other light
                double dist = this.GetPosition().Distance(currentLight.GetPosition());
                double diminishAmount = dist * .1;
                Color calculatedColor = new Color(
                    currentLight.GetColor().red - (int) Helpah.Round(currentLight.GetColor().red * diminishAmount),        //red
                    currentLight.GetColor().green - (int) Helpah.Round(currentLight.GetColor().green * diminishAmount),    //green
                    currentLight.GetColor().blue - (int) Helpah.Round(currentLight.GetColor().blue * diminishAmount));     //blue

                if (calculatedColor.red > this.lightLevel.red)
                {
                    this.lightLevel.red = calculatedColor.red;
                }
                if (calculatedColor.green > this.lightLevel.green)
                {
                    this.lightLevel.green = calculatedColor.green;
                }
                if (calculatedColor.blue > this.lightLevel.blue)
                {
                    this.lightLevel.blue = calculatedColor.blue;
                }
            }

            return this.lightLevel;
        }

        public bool GetBuildable()
        {
            return this.isBuildable;
        }

        public void SetBuildable(bool buildable)
        {
            this.isBuildable = buildable;
        }

        public bool GetWalkable()
        {
            return this.isWalkable;
        }

        public void SetWalkable(bool walkable)
        {
            this.isWalkable = walkable;
        }
    }
}
