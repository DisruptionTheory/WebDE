using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Animation;
using WebDE.Clock;
using WebDE.GameObjects;

namespace WebDE.GUI
{
    [JsType(JsMode.Clr, Filename = "../scripts/GUI.js")]
    public partial class GuiElement
    {
        private static int lastid = 0;

        private string id;
        private string text;
        private short fontSize = 14;
        private Sprite sprIcon;
        private Point position = new Point(0, 0);
        private Dimension size = new Dimension(0, 0);
        private GuiLayer parentLayer;
        //whether or not thus gui element is selected
        private bool selected = false;
        //a custom value for the scripter to use
        private string customValue = "";
        private List<string> styleClasses = new List<string>();
        private Dictionary<string, string> customStyles = new Dictionary<string, string>();

        public Color Color = Color.Black;

        public short FontSize { 
            get { return this.fontSize; } 
            set { this.fontSize = value; }
        }

        public GuiElement(GuiLayer owningLayer, string elementText)
        {
            GuiElement.lastid++;
            this.id = "GuiElement_" + GuiElement.lastid;
            this.parentLayer = owningLayer;
            owningLayer.AddGUIElement(this);
            this.text = elementText;
        }

        public string GetId()
        {
            return this.id;
        }

        public GuiLayer GetParentLayer()
        {
            return this.parentLayer;
        }

        private Dictionary<GUIFunction, Action<GuiEvent>> elementFunctions = new Dictionary<GUIFunction, Action<GuiEvent>>();

        public void SetGUIFunction(GUIFunction func, Action<GuiEvent> newEvent)
        {
            elementFunctions[func] = newEvent;
        }

        public void DoGUIFunction(GUIFunction func)
        {
            if(this.elementFunctions.ContainsKey(func))
            {
                //GuiEvent eventToTrigger = GuiEvent.FromClickData(this.parentLayer, this.GetPosition());
                /*
                Point thisPixelPos = new Point(
                    this.GetPosition().x * Stage.CurrentStage.GetTileSize().Item1,
                    this.GetPosition().y * Stage.CurrentStage.GetTileSize().Item2);
                */
                //GuiEvent eventToTrigger = GuiEvent.FromClickData(this.parentLayer, thisPixelPos, this);
                GuiEvent eventToTrigger = GuiEvent.FromGuiElement(this);

                this.elementFunctions[func].Invoke(eventToTrigger);
            }
            else
            {
                //Script.Eval("console.log('No function " + func.GetName() + " on that element. It has these " + this.elementFunctions.Count + " functions:');");
                foreach (GUIFunction gf in this.elementFunctions.Keys)
                {
                    //Script.Eval("console.log('" + gf.ToString() + "');");   
                    //Script.Eval("console.log('" + gf.GetName() + "');");   
                }       
            }
        }

        //attach the gui element to a game GameEntity, so that it follows it...
        public void AttachToGameEntity(GameEntity entToAttach)
        {

        }

        public Point GetPosition()
        {
            return this.position;
        }

        public void SetPosition(int xPos, int yPos)
        {
            this.position.x = xPos;
            this.position.y = yPos;

            this.SetNeedsUpdate();
        }

        /*
        public Dimension GetSize()
        {
            Tuple<int, int> returnVal = new Tuple<int, int>();

            returnVal.Item1 = this.size.width;
            returnVal.Item2 = this.size.height;

            return returnVal;
        }
        */

        public void SetSize(int newWidth, int newHeight)
        {
            this.size.width = newWidth;
            this.size.height = newHeight;
            this.SetNeedsUpdate();
        }

        public string GetText()
        {
            return this.text;
        }

        public void SetText(string newText)
        {
            this.text = newText;
            this.SetNeedsUpdate();
        }

        //hide the gui element and all of its children
        public void Hide()
        {
            this.SetNeedsUpdate();
            throw new NotImplementedException();
        }

        //show the gui element and all of its children
        public void Show()
        {
            this.SetNeedsUpdate();
            throw new NotImplementedException();
        }

        //reduce the gui element to a box or button that restores it to its full state
        public void Minimize()
        {
            this.SetNeedsUpdate();
            throw new NotImplementedException();
        }

        public Sprite GetSprite()
        {
            return this.sprIcon;
        }

        public void SetSprite(Sprite newSprite)
        {
            this.sprIcon = newSprite;
            this.SetNeedsUpdate();
        }

        public string GetCustomValue()
        {
            return this.customValue;
        }

        public void SetCustomValue(string newVal)
        {
            this.customValue = newVal;
        }

        public void Select(bool toApply)
        {
            if (toApply == true)
            {
                //renderer.Addclass("selected", this)
                this.selected = true;
            }
            else
            {
                //renderer.removeclass
                this.selected = false;
            }
            this.SetNeedsUpdate();
        }

        public bool Selected()
        {
            return this.selected;
        }

        public Dimension GetSize()
        {
            //if it has a sprite, return the size of the sprite
            if (this.sprIcon != null)
            {
                return this.sprIcon.Size;
            }

            //otherwise, return the size of the icon
            return this.size;
        }

        private void SetNeedsUpdate()
        {
            Game.Renderer.SetNeedsUpdate(this);
        }

        public void AddStyle(string styleToAdd)
        {
            if (!this.styleClasses.Contains(styleToAdd))
            {
                this.styleClasses.Add(styleToAdd);
            }
        }

        public void RemoveStyle(string styleToAdd)
        {
            if (this.styleClasses.Contains(styleToAdd))
            {
                this.styleClasses.Remove(styleToAdd);
            }
        }

        public List<string> GetStyles()
        {
            return this.styleClasses;
        }

        public string GetStyle(string styleName)
        {
            if (this.customStyles.ContainsKey(styleName))
            {
                return this.customStyles[styleName];
            }
            else return "";
        }

        public void SetStyle(string styleName, string styleValue)
        {
            this.customStyles[styleName] = styleValue;
            this.SetNeedsUpdate();
        }

        public void Destroy()
        {
            this.parentLayer.RemoveGUIElement(this);
        }
    }
}
