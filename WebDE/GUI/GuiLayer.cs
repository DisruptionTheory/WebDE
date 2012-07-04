using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Animation;
using WebDE.GameObjects;
using WebDE.Rendering;

namespace WebDE.GUI
{
    [JsType(JsMode.Clr, Filename = "../scripts/GUI.js")]
    public partial class GuiLayer
    {
        private static int lastid = 0;

        //all gui elements contained in this layer
        private List<GuiElement> guiElements = new List<GuiElement>();
        //the list of currently selected gui elements
        private List<GuiElement> selectedElements = new List<GuiElement>();
        //whether or not the layer is currently active
        private bool active = true;

        //if this layer is attached to a view, the view that it is attached to
        private View attachedView;
        //the horizontal and vertical position of the GUI layer, as well as its dimensions (width and height)
        private Rectangle area = new Rectangle(0, 0, 0, 0);
        //whether or not the GUI Layer should interperet events for game elements
        private Boolean isGameLayer = false;
        //the element that has focus
        private GuiElement focusedElement = null;
        private bool visible = true;

        //the name of the layer
        private string name;
        private string id;

        //whether or not this GUI Layer is following the user's cursor
        private bool followingCursor = false;

        /// <summary>
        /// Create a new Layer for user interface
        /// </summary>
        /// <param name="attachingView">The game view that the interface layer is attached to.</param>
        /// <param name="rectLayerPos">The position and size of the layer.</param>
        public GuiLayer(String layerName, View attachingView, Rectangle rectLayerPos)
        {
            GuiLayer.lastid++;
            this.id = "GUILayer" + GuiLayer.lastid;
            this.name = layerName;
            this.attachedView = attachingView;
            this.SetArea(rectLayerPos);

            //add this layer to the list of all layers in the game
            GuiLayer.allTheLayers.Add(this);
            //if there isn't currently an active GUILayer, make this one the active one
        }

        public string GetId()
        {
            return this.id;
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string newName)
        {
            this.name = newName;
        }

        public void Activate()
        {
            this.active = true;
        }

        public void Deactivate()
        {
            this.active = false;
            GuiLayer.lastActiveLayer = this;
        }

        public bool IsActive()
        {
            return this.active;
        }

        public Rectangle GetArea()
        {
            return this.area;
        }

        public Point GetPosition()
        {
            Point returnVal = new Point(this.area.x, this.area.y);

            return returnVal;
        }

        public void SetPosition(double newX, double newY)
        {
            if (newX < 0)
            {
                newX = this.GetAttachedView().GetSize().Item1 - Math.Abs(newX) - this.GetSize().Item1;
            }
            if (newY < 0)
            {
                newY = this.GetAttachedView().GetSize().Item2 - Math.Abs(newY) - this.GetSize().Item2;
            }

            this.area.x = newX;
            this.area.y = newY;
        }

        public Tuple<int, int> GetSize()
        {
            return new Tuple<int, int>
                (JsMath.round(this.area.width), JsMath.round(this.area.height));
        }

        public void SetSize(double newWidth, double newHeight)
        {
            this.area.width = newWidth;
            this.area.height = newHeight;
        }

        public void SetArea(Rectangle newArea)
        {
            this.SetSize(newArea.width, newArea.height);
            this.SetPosition(newArea.x, newArea.y);
        }

        /// <summary>
        /// Create a new element and add it to the gui layer
        /// </summary>
        /// <param name="text">The text to appear with the element.</param>
        /// <param name="guiSprite">A sprite which represent's the GUI's graphical icon.</param>
        /// <returns></returns>
        public GuiElement AddGUIElement(string text)
        {
            //create a new element with this as the parent, and the given text as its caption
            GuiElement newElement = new GuiElement(this, text);
            //add the element to the list of elements in this layer
            this.guiElements.Add(newElement);
            //if no element has the focus on this layer, give focus to the new element
            if (this.focusedElement == null)
            {
                this.focusedElement = newElement;
            }

            return newElement;
        }

        public List<GuiElement> GetGuiElements()
        {
            return this.guiElements;
        }

        //select the previous item in the list of gui elements
        public void PreviousItem()
        {
            //this is probably a terrible way to do this

            //the index of the currently focused item in the array of gui elements within this layer
            int focusedInex = this.guiElements.IndexOf(this.focusedElement);
            //if the element at the end of the list is the one that's currently selected, point to before the end of the list, so that it loops
            if (focusedInex <= 0)
            {
                focusedInex = this.guiElements.Count -1;
            }
            this.focusedElement = this.guiElements[focusedInex - 1];
        }

        //select the next item in the list of gui elements
        public void NextItem()
        {
            //the index of the currently focused item in the array of gui elements within this layer
            int focusedInex = this.guiElements.IndexOf(this.focusedElement);
            //if the element at the end of the list is the one that's currently selected, point to before the beginning of the list, so that it loops
            if (focusedInex >= this.guiElements.Count - 1)
            {
                focusedInex = -1;
            }
            this.focusedElement = this.guiElements[focusedInex + 1];
        }

        public List<GuiElement> GetSelectedItems()
        {
            List<GuiElement> elementList = this.GetGuiElements();

            foreach (GuiElement gelm in elementList)
            {
                if (gelm.Selected() == false)
                {
                    elementList.Remove(gelm);
                }
            }

            return elementList;
        }

        /// <summary>
        /// Give focus to a guielement within this layer, either by its name or by its index
        /// </summary>
        /// <param name="itemName">The name of the item. If this is not specified, the index is used.</param>
        /// <param name="itemIndex">The index of the item, used only if the name is null or empty.</param>
        public void SelectItem(string itemName, int itemIndex)
        {
            //deselect existing items
            foreach (GuiElement gelm in this.GetSelectedItems())
            {
                gelm.Select(false);
            }

            //select a specific gui element by name
            GuiElement elementToSelect = null;

            //if name is specified, select by name
            if (itemName != "" && itemName != null)
            {
                List<GuiElement> elementList = this.GetGuiElements();
                foreach (GuiElement gelm in elementList)
                {
                    if (gelm.GetText() == itemName)
                    {
                        elementToSelect = gelm;
                        break;
                    }
                }
            }
            else   //select by index
            {
                elementToSelect = this.GetGuiElements()[itemIndex];
            }

            if (elementToSelect != null)
            {
                elementToSelect.Select(true);
            }
        }

        public View GetAttachedView()
        {
            return this.attachedView;
        }

        //get a GUI element at a specific position
        //xpos and ypos are given in game coordinates, not screen coordinates...probably...hopefully?
        public GuiElement GetElementAt(double xpos, double ypos)
        {
            //correct for the position of the layer itself
            //need to figure out screen width and whatnot and correct for that...
            if (xpos > this.GetPosition().x)
            {
                xpos -= this.GetPosition().x;
                ypos -= this.GetPosition().y;
            }

            //Debug.log("Layer " + this.GetName() + " position is " + this.GetPosition().x + "," + this.GetPosition().y + ". " + "The click is at " + xpos + "," + ypos);

            //this is a game layer, so check for game objects at this location
            if (this.IsGameLayer() == true)
            {
                //get the list of objects
                List<GameEntity> tileList = this.GetAttachedView().GetAttachedStage().GetVisibleTiles(this.GetAttachedView());
                //go through each of them
                //match their position
            }
            else    //this is a regular guilayer
            {
                //Script.Eval("console.log('GuiLayer/GetElementAt/214: Click is at " + xpos + ", " + ypos + "')");
                //check all of the elements
                foreach (GuiElement gelm in this.guiElements)
                {
                    //Debug.log(gelm.GetId() + " is at " + gelm.GetPosition().x.ToString() + ", " + gelm.GetPosition().y.ToString());
                    //match position
                    if ( (gelm.GetPosition().x <= xpos && gelm.GetPosition().x + gelm.GetSize().Item1 >= xpos) &&
                         (gelm.GetPosition().y <= ypos && gelm.GetPosition().y + gelm.GetSize().Item2 >= ypos) )
                    {
                        return gelm;
                    }
                }
            }

            return null;
        }

        public List<GameEntity> GetEntitiesAt(double xpos, double ypos)
        {
            List<GameEntity> returnList = new List<GameEntity>();

            //get all of the entities in the applicable area
            List<GameEntity> GameEntityList = this.GetAttachedView().GetAttachedStage().GetVisibleEntities(this.GetAttachedView());
            //loop through each and check if they exist at that click location
            foreach (GameEntity ent in GameEntityList)
            {
                if ((ent.GetPosition().x <= xpos && ent.GetPosition().x + ent.GetSize().Item1 >= xpos) &&
                     (ent.GetPosition().y <= ypos && ent.GetPosition().y + ent.GetSize().Item2 >= ypos))
                {
                    //if they do, add them to the list of entities to return
                    returnList.Add(ent);
                }
            }

            return returnList;
        }

        public List<Tile> GetTilesAt(double xpos, double ypos)
        {
            Tuple<int, int> tileSize = Stage.CurrentStage.GetTileSize();
            xpos = xpos / tileSize.Item1;
            ypos = ypos / tileSize.Item2;
            //Script.Eval("console.log('GuiLayer/GetTilesAt/247: Getting tiles at " + xpos + ", " + ypos + "')");
            List<Tile> returnList = new List<Tile>();

            //get all of the entities in the applicable area
            List<GameEntity> tileList = this.GetAttachedView().GetAttachedStage().GetVisibleTiles(this.GetAttachedView());

            xpos = Helpah.Round(xpos);
            ypos = Helpah.Round(ypos);

            //loop through each and check if they exist at that click location
            foreach (GameEntity ent in tileList)
            {
                //if the tile is AT that location
                if (ent.GetPosition().x == xpos && ent.GetPosition().y == ypos)
                {
                    Tile til = (Tile)ent;
                    returnList.Add(til);
                }
                /*
                if ((ent.GetPosition().x <= xpos && (ent.GetPosition().x + ent.GetSize().First) >= xpos) &&
                     (ent.GetPosition().y <= ypos && (ent.GetPosition().y + ent.GetSize().Second) >= ypos))
                {
                    //Script.Eval("console.log('" + ent.GetPosition().x + " < " + xpos + " < " + (ent.GetPosition().x + ent.GetSize().First) + "')");
                    //Script.Eval("console.log('" + ent.GetPosition().y + " < " + ypos + " < " + (ent.GetPosition().y + ent.GetSize().Second) + "')");

                    //if they do, add them to the list of entities to return
                    Tile til = (Tile)ent;
                    returnList.Add(til);
                }
                 */
            }

            return returnList;
        }

        /// <summary>
        /// Sets whether or not this GUI Layer is to interperet events on Game objects or not.
        /// </summary>
        /// <param name="newTruth_nullable">If this value is null, it will toggle the current state of the layer.</param>
        public void SetGameLayer(Boolean newTruth_nullable)
        {
            //okay, I wanted it to be nullable...maybe there's a better way, other than creating a separate "toggle" function?
            if (newTruth_nullable == null)
            {
                this.isGameLayer = !this.isGameLayer;
            }
            else
            {
                this.isGameLayer = newTruth_nullable;
            }
        }

        public Boolean IsGameLayer()
        {
            return this.isGameLayer;
        }

        public void FollowCursor(bool toFollow)
        {
            this.followingCursor = toFollow;
        }

        public void GUI_Event(GUIFunction buttonFunction, Point eventPos)
        {
            //perform the gui function attached to the affected layer
            this.DoGUIFunction(buttonFunction);

            //the gui element receiving the event
            GuiElement elementToNotify;

            //if the pos is null, then assume that the item to fire on is the focused item
            if (eventPos == null)
            {
                //there is no focused element, so we don't know what element to send the command to
                if (this.focusedElement == null)
                {
                    return;
                }
                elementToNotify = this.focusedElement;
            }
                //if eventpos is not null, find the element at the position
            else
            {
                elementToNotify = this.GetElementAt(eventPos.x, eventPos.y);
            }

            //check to make sure we have an element, don't proceed if it's null
            if (elementToNotify == null)
            {
                //Script.Eval("console.log('Sadface :(');");
                return;
            }

            //perform the gui function attached to the given gui element
            elementToNotify.DoGUIFunction(buttonFunction);
        }

        private Dictionary<GUIFunction, Action<GuiEvent>> layerFunctions = new Dictionary<GUIFunction, Action<GuiEvent>>();

        public void SetGUIFunction(GUIFunction func, Action<GuiEvent> newEvent)
        {
            //Debug.log("Adding event to " + func.GetName() + " on layer " + this.GetName());
            this.layerFunctions[func] = newEvent;
        }

        public void DoGUIFunction(GUIFunction func)
        {
            if (this.layerFunctions[func] != null)
            {
                //Script.Eval("console.log('Trying to fire " + func.GetName() + "');");
                Point thisPixelPos = new Point(
                    this.GetPosition().x * Stage.CurrentStage.GetTileSize().Item1,
                    this.GetPosition().y * Stage.CurrentStage.GetTileSize().Item2);
                GuiEvent eventToTrigger = GuiEvent.FromClickData(this, thisPixelPos);

                this.layerFunctions[func].Invoke(eventToTrigger);
            }
            else
            {
                //Debug.log("I got " + this.layerFunctions.Count + " GUIFunctions but a " + func.GetName() + " aint one. (P.S. I am " + this.GetName() + ")");
                /*
                Script.Eval("console.log('No function " + func.GetName() + " on that element. It has these " + this.elementFunctions.Count + " functions:');");
                foreach (GUIFunction gf in this.elementFunctions.Keys)
                {
                    Script.Eval("console.log('" + gf.ToString() + "');");
                    //Script.Eval("console.log('" + gf.GetName() + "');");   
                }
                */
            }
        }

        public void Show()
        {
            this.visible = true;
            this.SetNeedsUpdate();
        }

        public void Hide()
        {
            this.visible = false;
            this.SetNeedsUpdate();
        }

        public bool Visible()
        {
            return this.visible;
        }

        private void SetNeedsUpdate()
        {
            Rendering.DOM_Renderer.GetRenderer().SetNeedsUpdate(this);
        }
    }

    /// <summary>
    /// The default layout for this GUI Layer, which will arrange GUI Elements according to layout...
    /// </summary>
    public enum GUI_Layout
    {
        Grid_2Col = 0,
        Grid_3Col = 1,
        Grid_4Col = 2
    }
}
