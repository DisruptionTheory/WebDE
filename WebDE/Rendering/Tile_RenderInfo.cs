using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Rendering;

namespace WebDE.GameObjects
{
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public partial class Tile : GameEntity
    {
        /*
        public void Render()
        {
            base.Render();

            Color backgroundColor = this.CalculateLightLevel();
            //putting this here rather than complicating up tile, but it may make better sense to put it in calculatelightlevel instead...
            if (View.GetMainView().GetLightingStyle() == LightingStyle.Tiles)
            {
                //this.GetRenderElement().Style.BackgroundColor = "#" + backgroundColor.GetHex();
                //jQuery.FromElement(this.GetRenderElement()).CSS("background-color", "#" + backgroundColor.GetHex());
            }
            else     //assuming gradients
            {
                Debug.log("Lighting stiles alternative to tile not currently implemented.");
                //Seeing what it looks like with always black tiles
                backgroundColor = Color.Black;

                //boolean system, make it black if it's black, white if it's anything else
                if (backgroundColor.Match(Color.Black) != true)
                {
                    //this.GetRenderElement().Style.BackgroundColor = "#" + Color.White.GetHex();
                    this.GetRenderElement().Style.BackgroundColor = "transparent";
                }
                else
                {
                    this.GetRenderElement().Style.BackgroundColor = "#" + Color.Black.GetHex();
                }
            }
        }
        */
    }
}
