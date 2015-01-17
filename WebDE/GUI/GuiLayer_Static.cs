using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;
using WebDE.Rendering;

namespace WebDE.GUI
{
    [JsType(JsMode.Clr, Filename = "../scripts/GUI.js")]
    public partial class GuiLayer
    {
        //all of the Gui layers. these shouldn't be public, but I'm lazy right now
        public static List<GuiLayer> allTheLayers = new List<GuiLayer>();
        //the layer that was active before this one was active
        private static GuiLayer lastActiveLayer;
        //the default GUI Layer that is switched to when all others deactivate / close and have nowhere to go
        //defaults to "none" or "null" ... ?
        private static GuiLayer defaultLayer;

        //get a gui layer based on its name
        public static GuiLayer GetLayerByName(string layerName)
        {
            //we can append this to check tolower versions, and return a case insensitive match if a case sensitive match is not found

            //loop through all of the layers
            foreach (GuiLayer currentLayer in allTheLayers)
            {
                //check the name of the layer against the desired one
                if (currentLayer.GetName() == layerName)
                {
                    //if it's a match, return that layer as the result
                    return currentLayer;
                }
            }

            //if none could be found, return null
            return null;
        }

        public static List<GuiLayer> GetActiveLayers()
        {
            List<GuiLayer> resultList = new List<GuiLayer>();

            foreach (GuiLayer layer in allTheLayers)
            {
                if (layer.IsActive())
                {
                    resultList.Add(layer);
                }
            }

            return resultList;
        }

        public static GuiLayer AsCollisionMap(Stage sourceStage)
        {
            Debug.log("Rendering collision map...");

            //So we need to reposition this so that it isn't at 0,0,
            //but rather at the position of the game board
            //technically, I think the view should be at that position, and this should be 0,0 within the view...

            //the size should be equal to the stage or stage's gui layer
            ///!/// so, fixing the size of this. Not an ideal situation. Wll have to address.
            GuiLayer collisionLayer = View.GetMainView().AddLayer("CollisionLayer", DOM_Renderer.GetRenderer().As<DOM_Renderer>().BoardArea());
            //GuiLayer collisionLayer = View.GetMainView().AddLayer("CollisionLayer", View.GetMainView().GetViewArea());
            //put the layer on top /  make it visible
            //collisionLayer.GetRenderElement().Style.ZIndex = 10;

            //get the size of the tiles in the stage so that we can offset the collision overlay
            Dimension TileSize = Stage.CurrentStage.GetTileSize();

            //loop through each of the tiles, and draw whether or not it collides to the layer
            foreach (Tile tile in sourceStage.GetVisibleTiles(collisionLayer.GetAttachedView()))
            {
                GuiElement gelm = collisionLayer.AddGUIElement("");
                //gelm.InitialRender();
                gelm.SetPosition(
                    (int)Helpah.Round(tile.GetPosition().x * TileSize.width),
                    (int)Helpah.Round(tile.GetPosition().y * TileSize.height));
                // Set the size to 0, 0 so that the renderer ignores the element.
                // The CSS stylesheet will give it size.
                gelm.SetSize(0, 0);
                gelm.AddStyle("collisionBlock");
                //gelm.GetRenderElement().ClassName = "collisionBlock";

                if (tile.GetBuildable())
                {
                    gelm.AddStyle("buildable");
                    //gelm.GetRenderElement().ClassName += " buildable";
                }
            }

            return collisionLayer;
        }
    }
}
