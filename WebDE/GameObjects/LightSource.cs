using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Rendering;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Objects.js")]
    public partial class LightSource : GameEntity
    {
        //for now, lights are sphereical in projection
        //later, need a shape

        //this is how "bright" the light is,
        //or how many tiles / units / pixels it will project to at 100% brightness
        private double luminosity = 1;
        private double range = 3;
        private Color color = new Color(255, 255, 255);

        //if something has changed that needs to be updated in the render cycle
            //this should probably be moved to GameEntity
        private bool needsRenderUpdate = false;
        //whether or not this light shrinks over time
        private bool diminishing = false;

        public static List<LightSource> GetLocalLightSources(double xPos, double yPos)
        {
            return GetLocalLightSources(xPos, yPos, Stage.CurrentStage, View.GetMainView());
        }

        public static List<LightSource> GetLocalLightSources(double xPos, double yPos, Stage stage, View view)
        {
            List<LightSource> returnLights = new List<LightSource>();
            foreach (LightSource light in stage.GetLights(view))
            {
                //if the light is brighter than, or as bright as it is far away
                if (light.GetPosition().Distance(new Point(xPos, yPos)) <= light.range)
                {
                    //it is a "local" light source, add it to the return list
                    returnLights.Add(light);
                }
            }

            return returnLights;
        }

        public LightSource(double x, double y, double lightness, double distance)
            : base("")
        {
            this.SetPosition(x, y);
            this.luminosity = lightness;
            this.range = distance;

            //if things have been initialized, calculate illumination
            if (View.GetMainView() == null)
            {
                this.CalculateIllumination();
            }

            Stage.CurrentStage.AddLight(this);
        }

        public double GetLuminosity()
        {
            return this.luminosity;
        }

        public void SetLuminosity(int newVal)
        {
            this.luminosity = newVal;
        }

        public double GetRange()
        {
            return this.range;
        }

        public void SetRange(double newRange)
        {
            this.range = newRange;
            //this.SetSize(Helpah.Round(this.GetRange() * 2), Helpah.Round(this.GetRange() * 2));
            this.SetNeedsUpdate();
        }

        public Color GetColor()
        {
            return this.color;
        }

        public void SetColor(Color newColor)
        {
            this.color = newColor;
            this.needsRenderUpdate = true;
        }

        public void SetDiminishing(bool isDiminishing)
        {
            this.diminishing = isDiminishing;
        }

        public bool GetDiminishing()
        {
            return this.diminishing;
        }

        //calculate the light at the tiles this light touches
        //should be called at runtime, not init
        public void CalculateIllumination()
        {
            //get all of the tiles that this light touches
            List<GameEntity> localTiles = Stage.CurrentStage.GetVisibleTiles(View.GetMainView());

            foreach (Tile localTile in localTiles)
            {
                if (localTile.GetPosition().Distance(this.GetPosition()) <= this.range)
                {
                    //localTile.CalculateLightLevel();
                    //localTile.Render();
                }
            }
        }

        //that's right, lights think, too!
        public void Think()
        {
            //if this is a player generated light, shrink it once per thought...or ... second?
            if (this.GetDiminishing() == true)
            {
                //this.SetSize(this.GetSize().width - 1, this.GetSize().height - 2);
                //not size, you fool, range!
                this.SetRange(this.GetRange() - .1);
                //if it's too small, kill it!
                if (this.GetRange() < .1)
                {
                    this.Destroy();
                }
                //should the color be dakened, or the luminosity be reduced?
            }
        }
    }

    public enum LightingStyle
    {
        None = 0,
        Tiles = 1,
        Gradients = 2
    }
}
