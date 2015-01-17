using System;
using System.Collections.Generic;

using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;

using WebDE.Animation;
using WebDE.GUI;
using WebDE.GameObjects;
using WebDE.Clock;
using WebDE.InputManager;

namespace WebDE.Rendering
{
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public partial class View
    {
        #region Variables
        //the stage the view is looking at / attached to
        private Stage stgAttached = null;
        //a list of all gui layers in the view
        private List<GuiLayer> guiLayers = new List<GuiLayer>();

        //visible area
        private Dimension size = new Dimension(800, 600);

        //location within the game world...
        private int x = 0;
        private int y = 0;

        private bool active = false;

        private LightingStyle LightStyle;


        // Not properly implemented yet:

        // Which entity we're following.
        private GameEntity followingGent;
        // offsets
        private int offsetX = 0;
        private int offsetY = 0;
        // How far zoomed in we are on the stage.
        private double zoomLevel = 1.0;
        private double maxZoomLevel = 1.9;
        private double minZoomLevel = 0.1;
        private double zoomIncrement = 0.1;
        // The angle at which the X is rotated in display ...
        private short xAngle = 90;
        // The angle at which the Y coordinate is rotated in display ...
        private short yAngle = 90;
        // Displayed x-coordinates should be rendered as: x += (xAngle / 90) * screen.Height
        // y coordinates should use width ...

        private List<GameEntity> visibleEntities = new List<GameEntity>();

        private Func<object, List<Rectangle>> exclusionMask = null;
        private object exclusionMaskOptions = null;
        private List<Rectangle> exclusionAreas = new List<Rectangle>();

        public int OffsetX { get { return this.offsetX; } }
        public int OffsetY { get { return this.offsetY; } }
        public bool EntitiesVisible { get; set; }
        public bool TilesVisible { get; set; }
        public bool LightsVisible { get; set; }
        #endregion

        public View(LightingStyle lightStyle)
        {
            //this.viewyGUI = new GuiLayer(this, new Rectangle(0, 0, 0, 0));
            //this.viewyGUI.SetGameLayer(true);
            this.LightStyle = lightStyle;

            this.Activate();
            if (View.mainView == null)
            {
                View.mainView = this;
                // The main view's size should match whatever size the renderer is set to.
                this.size = Game.Renderer.GetSize();
            }

            this.EntitiesVisible = true;
            this.TilesVisible = true;
            this.LightsVisible = true;
        }

        public bool IsActive()
        {
            return this.active;
        }

        public void Activate()
        {
            this.active = true;

            if (this.stgAttached == null && Stage.CurrentStage != null)
            {
                this.stgAttached = Stage.CurrentStage;
            }

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

        #region Dimension handling

        public Dimension GetSize()
        {
            return this.size;
        }

        public void SetSize(int newWidth, int newHeight)
        {
            this.size.width = newWidth;
            this.size.height = newHeight;
        }

        public void SetSize(Dimension newSize)
        {
            this.size = newSize;
        }

        public Point GetPosition()
        {
            return new Point(this.x, this.y);
        }

        public void SetPosition(int newX, int newY)
        {
            // We shouldn't really need to check position.
            // The view can be out of bounds.

            this.x = newX;
            this.y = newY;
        }

        public void SetOffsets(int newX, int newY)
        {
            if (Stage.CurrentStage.GetBackgroundSprite(this) != null)
            {
                double zoomedWidth = this.GetSize().width * (1 / this.GetZoomLevel());
                double zoomedHeight = this.GetSize().height * (1 / this.GetZoomLevel());

                // We shouldn't be zooming out beyond the background size.
                double imgWidth = Stage.CurrentStage.GetBackgroundSprite(this).Size.width;
                double imgHeight = Stage.CurrentStage.GetBackgroundSprite(this).Size.height;
                if (newX + zoomedWidth > imgWidth)
                {
                    newX = JsMath.floor(imgWidth - zoomedWidth);
                }
                if (newY + zoomedHeight > imgHeight)
                {
                    newY = JsMath.floor(imgHeight - zoomedHeight);
                }
            }

            // We are going to need to factor in position, as this assumes it's 0,0.

            // Check to make sure the X and Y are valid
            // Probably need to account for zoom level ... ?
            if (newX < 0)
            {
                newX = 0;
            }
            //if (newX > Game.Renderer.GetSize().width - this.size.width)
            if (newX > Stage.CurrentStage.PixelWidth - this.size.width)
            {
                newX = (int)(Stage.CurrentStage.PixelWidth - this.size.width);
            }
            if (newY < 0)
            {
                newY = 0;
            }
            if (newY > Stage.CurrentStage.PixelHeight - this.size.height)
            {
                newY = (int)(Stage.CurrentStage.PixelHeight - this.size.height);
            }

            if (newX != this.offsetX || newY != this.offsetY)
            {
                this.offsetX = newX;
                this.offsetY = newY;

                this.FindVisibleEntities();
            }
            else
            {
                this.offsetX = newX;
                this.offsetY = newY;
            }

            this.FindVisibleEntities();
        }

        public Rectangle GetArea()
        {
            return new Rectangle(this.x, this.y, this.size.width, this.size.height);
        }

        public void SetArea(Rectangle newArea)
        {
            this.x = (int)newArea.x;
            this.y = (int)newArea.y;
            this.size.width = (int)newArea.width;
            this.size.height = (int)newArea.height;
        }

        #endregion

        #region GUI
        public GuiLayer AddLayer(string layerName, Rectangle layerPos)
        {
            //create a new element with this as the parent, and the given text as its caption
            GuiLayer newLayer = new GuiLayer(layerName, this, layerPos);
            //add the element to the list of elements in this layer
            
            //make the view the active view?

            return newLayer;
        }

        public void AddLayer(GuiLayer layer)
        {
            if (!guiLayers.Contains(layer))
            {
                this.guiLayers.Add(layer);
            }
        }

        public void RemoveLayer(GuiLayer layer)
        {
            if (this.guiLayers.Contains(layer))
            {
                this.guiLayers.Remove(layer);
            }
        }

        public List<GuiLayer> GetLayers()
        {
            return this.guiLayers;
        }

        public void GUI_Event(GUIFunction buttonFunction, InputDevice sendingDevice, int clickX, int clickY)
        {
            clickX -= (int)this.GetArea().x;
            clickX -= (int)this.GetArea().y;

            //loop through all of the active gui layers
            //foreach (GuiLayer layer in this.guiLayers)
            for (int i = 0; i < this.guiLayers.Count; i++)
            {
                GuiLayer layer = this.guiLayers[i];
                Point actionLocation = new Point(clickX, clickY);

                //skip this layer if the layer doesn't exist in the clicked space
                //if (clickX > layer.GetArea().Right || clickX < layer.GetArea().x || clickY < layer.GetArea().y || clickY > layer.GetArea().Top)
                if (!layer.GetArea().Contains(actionLocation))
                {
                    continue;
                }

                //translate the coordinates to be relative to the gui layer?

                //fire a new gui event on the layer...
                //tell the GUI layer which action was triggered and (if applicable) where
                layer.GUI_Event(buttonFunction, sendingDevice, actionLocation);
            }
        }
#endregion

        public Rectangle GetVisibleArea()
        {
            // The Y position of the view in game units is the height of the stage - the offsetY - the view's height
            // Then we convert that from pixels
            double yPos = (stgAttached.GetSize().height * stgAttached.GetTileSize().height)
                - this.offsetY - this.GetSize().height;

            return new Rectangle(
                this.offsetX / this.stgAttached.GetTileSize().width,
                //(this.offsetY - this.GetSize().height) / this.stgAttached.GetTileSize().height,
                yPos / stgAttached.GetTileSize().height,
                this.GetSize().width / this.stgAttached.GetTileSize().width,
                this.GetSize().height / this.stgAttached.GetTileSize().height);
        }

        public List<GameEntity> GetVisibleEntities()
        {
            return this.visibleEntities;
        }

        // Fill the list of entities visible by this view.
        public void FindVisibleEntities()
        {
            foreach (GameEntity gent in stgAttached.GetVisibleEntities(this))
            {
                CheckEntityVisibility(gent);
                /*
                // The view should only be accessible to the renderer
                // When it moves, or an entity moves, a list of what entities it renders needs to be updated ...

                // If it's within viewport visibility AND not excluded by the exclusion mask ... 
                // The size of this is given in pixels, so we have to convert to game units
                if (gent.GetPosition().x >= this.x + (offsetX / stage.GetTileSize().width)
                    && gent.GetPosition().x <= this.x + ((offsetX + this.size.width) / stage.GetTileSize().width)
                    && gent.GetPosition().y >= this.x + (offsetY / stage.GetTileSize().height)
                    && gent.GetPosition().y <= this.x + ((offsetY + this.size.height) / stage.GetTileSize().height))
                {
                    returnList.Add(gent);
                }
                 */
            }
        }

        public bool CheckEntityVisibility(GameEntity gent)
        {
            if (this.GetVisibleArea().Contains(gent.GetArea()))
            {
                if (!visibleEntities.Contains(gent))
                {
                    visibleEntities.Add(gent);
                }
                return true;
            }
            else
            {
                if (visibleEntities.Contains(gent))
                {
                    visibleEntities.Remove(gent);
                }
                return false;
            }
        }

        public void old_CheckEntityVisibility(GameEntity gent)
        {
            if (gent.GetPosition().x >= this.x + (offsetX / stgAttached.GetTileSize().width)
                && gent.GetPosition().x <= this.x + ((offsetX + this.size.width) / stgAttached.GetTileSize().width)
                && gent.GetPosition().y >= this.x + (offsetY / stgAttached.GetTileSize().height)
                && gent.GetPosition().y <= this.x + ((offsetY + this.size.height) / stgAttached.GetTileSize().height))
            {
                if (!visibleEntities.Contains(gent))
                {
                    visibleEntities.Add(gent);
                }
            }
            else
            {
                if (visibleEntities.Contains(gent))
                {
                    visibleEntities.Remove(gent);
                }
            }
        }

        public List<Tile> GetVisibleTiles()
        {
            List<Tile> returnList = new List<Tile>();

            // Which stage to deal with. Default to current stage, override if given.
            Stage stage = Stage.CurrentStage;
            if (this.stgAttached != null)
            {
                stage = stgAttached;
            }

            foreach (Tile tile in stage.GetVisibleTiles(this))
            {
                // If it's within viewport visibility AND not excluded by the exclusion mask ... 
                // The size of this is given in pixels, so we have to convert to game units
                if (tile.GetPosition().x >= (this.x / stage.GetTileSize().width) 
                    && tile.GetPosition().x <= (this.x + (this.size.width * ( 1 / this.zoomLevel))) / stage.GetTileSize().width
                    && tile.GetPosition().y >= (this.y / stage.GetTileSize().height)
                    && tile.GetPosition().y <= (this.y + (this.size.height * ( 1 / this.zoomLevel))) / stage.GetTileSize().height)
                {
                    // Check against exclusion mask ... 
                    if (this.exclusionAreas != null)
                    {
                        // If its not IN ANY exclusion area, add it
                        foreach (Rectangle exclusionArea in exclusionAreas)
                        {
                            if(exclusionArea.Contains(tile.GetArea()))
                            {
                                continue;
                            }
                        }
                        returnList.Add(tile);
                    }
                }
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

        public void FollowEntity(GameEntity gentToFollow)
        {
            Debug.log("Warning. Attempting to follow " + gentToFollow.GetName() + " but function not implemented.");
        }

        public void CenterOn(double tileX, double tileY)
        {
            int pixelX = (int)(tileX * Stage.CurrentStage.GetTileSize().width);
            int pixelY = (int)(tileY * Stage.CurrentStage.GetTileSize().height);
            //int wrapperWidth = this.getOuterWidth(document.getElementById("gameWrapper"));
            //int wrapperHeight = this.getOuterHeight(document.getElementById("gameWrapper"));
            int boardWidth = (int)(Stage.CurrentStage.Width * Stage.CurrentStage.GetTileSize().width);
            int boardHeight = (int)(Stage.CurrentStage.Height * Stage.CurrentStage.GetTileSize().height);
            int screenWidth = (int) Game.Renderer.GetSize().width;
            int screenHeight = (int)Game.Renderer.GetSize().height;

            int leftX = this.x;
            int topY = this.y;
            // if we need to scroll horizontally
            if (boardWidth > screenWidth)
            {
                // the left that needs to be set in order to center tileX
                // The position in the map minus half the visible area
                leftX = pixelX - (screenWidth / 2);

                // if we need to center close to the left edge
                if (leftX < 0)
                {
                    leftX = 0;
                }
                // if we need to center on the right edge
                else if (leftX > screenWidth)
                {
                    leftX = boardWidth - screenWidth;
                }
            }
            // if we need to scroll vertically
            if (boardHeight > screenHeight)
            {
                // the top that needs to be set in order to center tileY
                // The position in the map minus half the visible area
                topY = pixelY - (screenHeight / 2);

                // if we need to center close to the top edge
                if (topY < 0)
                {
                    topY = 0;
                }
                // if we need to center on the right edge
                else if (topY > screenHeight)
                {
                    topY = boardHeight - screenHeight;
                }
            }

            this.SetOffsets(leftX, topY);
        }

        public void SetViewAngle(short x, short y)
        {
            xAngle = x;
            yAngle = y;
        }

        public Tuple<short, short> GetViewAngle()
        {
            return new Tuple<short, short>(xAngle, yAngle);
        }

        public double GetZoomLevel()
        {
            return this.zoomLevel;
        }

        public void SetZoomLevel(double newZoomLevel)
        {
            this.zoomLevel = newZoomLevel;
        }

        public void ZoomIn()
        {
            // Check to ensure the view offsets don't put it out of view ...
            if (this.zoomLevel + zoomIncrement <= maxZoomLevel)
            {
                this.zoomLevel += zoomIncrement;
            }
        }

        public void ZoomOut()
        {
            if (this.zoomLevel - zoomIncrement >= minZoomLevel)
            {
                this.zoomLevel -= zoomIncrement;
            }

            // for now...
            this.SetOffsets(this.offsetX, this.offsetY);
        }

        public void ZoomIn(double amount)
        {
            if (this.zoomLevel + amount <= maxZoomLevel)
            {
                this.zoomLevel += amount;
            }

            this.SetOffsets(this.offsetX, this.offsetY);
        }

        public void ZoomOut(double amount)
        {
            if (this.zoomLevel - amount >= minZoomLevel)
            {
                this.zoomLevel -= amount;
            }
        }

        public List<Rectangle> GenerateExclusionMask()
        {
            if (this.exclusionMask != null)
            {
                return this.exclusionMask.Invoke(this.exclusionMaskOptions);
            }

            return null;
        }

        public void SetExclusionMask(Func<object, List<Rectangle>> mask, object options = null)
        {
            this.exclusionMask = mask;
            exclusionMaskOptions = options;
        }
    }
}
