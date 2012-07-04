using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

using WebDE.Animation;
using WebDE.GUI;
using WebDE.GameObjects;
using WebDE.Timekeeper;

namespace WebDE.Rendering
{
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public class View
    {
        private static List<View> activeViews = new List<View>();
        //the main output screen for the local player
        private static View mainView;

        public static List<View> GetActiveViews()
        {
            return activeViews;
        }

        public static void SetMainView(View newView)
        {
            View.mainView = newView;
        }

        /// <summary>
        /// Reuturns the primary output View for the local player
        /// </summary>
        /// <returns></returns>
        public static View GetMainView()
        {
            return View.mainView;
        }

        //the stage the view is looking at / attached to
        private Stage stgAttached = null;
        //a list of all gui layers in the view
        private List<GuiLayer> guiLayers = new List<GuiLayer>();

        //visible area
        private int width = 800;
        private int height = 600;

        //location within the game world...
        private int x = 0;
        private int y = 0;

        private bool active = false;

        private LightingStyle LightStyle;

        public View(LightingStyle lightStyle)
        {
            //this.viewyGUI = new GuiLayer(this, new Rectangle(0, 0, 0, 0));
            //this.viewyGUI.SetGameLayer(true);
            this.LightStyle = lightStyle;

            this.Activate();
            if (View.mainView == null)
            {
                View.mainView = this;
            }
        }

        public bool IsActive()
        {
            return this.active;
        }

        public void Activate() {
            this.active = true;

            if (View.activeViews.Contains(this) == false)
            {
                View.activeViews.Add(this);
            }
        }

        public void Deactivate()
        {
            this.active = false;
            if (View.activeViews.Contains(this) == true)
            {
                View.activeViews.Remove(this);
            }
        }

        public void AttachStage(Stage newStage)
        {
            this.stgAttached = newStage;
        }

        public Stage GetAttachedStage()
        {
            return this.stgAttached;
        }

        public Rectangle GetViewArea()
        {
            return new Rectangle(this.x, this.y, this.width, this.height);
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
        }

        public void SetPosition(int newX, int newY)
        {
            this.x = newX;
            this.y = newY;
        }

        public Rectangle GetArea()
        {
            Rectangle returnVal = new Rectangle(this.x, this.y, this.width, this.height);
            return returnVal;
        }

        public void SetArea(Rectangle newArea)
        {
            this.x = (int)newArea.x;
            this.y = (int)newArea.y;
            this.width = (int)newArea.width;
            this.height = (int)newArea.height;
        }

        public GuiLayer AddLayer(string layerName, Rectangle layerPos)
        {
            //create a new element with this as the parent, and the given text as its caption
            GuiLayer newLayer = new GuiLayer(layerName, this, layerPos);
            //add the element to the list of elements in this layer
            this.guiLayers.Add(newLayer);
            
            //make the view the active view?

            return newLayer;
        }

        public List<GuiLayer> GetLayers()
        {
            return this.guiLayers;
        }

        public List<GameEntity> GetVisibleEntities()
        {
            List<GameEntity> returnList = new List<GameEntity>();

            if (this.stgAttached != null)
            {
                returnList.AddRange(this.stgAttached.GetVisibleEntities(this));
                returnList.AddRange(this.stgAttached.GetVisibleTiles(this));
            }

            return returnList;
        }

        public void SetLightingStyle(LightingStyle newStyle)
        {
            this.LightStyle = newStyle;
        }

        public LightingStyle GetLightingStyle()
        {
            return this.LightStyle;
        }

        //when the viewable area of the view changes
        private void resize()
        {
        }

        public void RenderLightSources()
        {
            //this function will technically handle initial render...need to do something else for dynamic render...

            //get all of the entities in the stage and call their individual render functions
            foreach (LightSource light in this.stgAttached.GetLights())
            {
                //ent.getSprite().Animate();
                //light.Render();
            }
        }

        public void GUI_Event(GUIFunction buttonFunction, int clickX, int clickY)
        {
            clickX -= (int)this.GetArea().x;
            clickX -= (int)this.GetArea().y;

            //loop through all of the active gui layers
            foreach (GuiLayer layer in this.guiLayers)
            {
                //skip this layer if the layer doesn't exist in the clicked space
                if (clickX > layer.GetArea().Right() || clickX < layer.GetArea().x || clickY > layer.GetArea().Bottom() || clickY < layer.GetArea().y)
                {
                    continue;
                }
                //translate the coordinates to be relative to the gui layer?
                Point actionLocation = new Point(clickX, clickY);
                //fire a new gui event on the layer...
                //tell the GUI layer which action was triggered and (if applicable) where
                layer.GUI_Event(buttonFunction, actionLocation);
            }
        }
    }
}
