using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Animation;
using WebDE.Timekeeper;
using WebDE.GameObjects;

namespace WebDE.GUI
{
    [JsType(JsMode.Clr, Filename = "../scripts/GUI.js")]
    public partial class GuiElement
    {
        private static int lastid = 0;

        private string id;
        private string text;
        private Sprite sprIcon;
        private Point position = new Point(0, 0);
        private int width;
        private int height;
        private GuiLayer parentLayer;
        //whether or not thus gui element is selected
        private bool selected = false;
        //a custom value for the scripter to use
        private string customValue = "";
        private List<string> styleClasses = new List<string>();

        public GuiElement(GuiLayer owningLayer, string elementText)
        {
            GuiElement.lastid++;
            this.id = "GuiElement_" + GuiElement.lastid;
            this.parentLayer = owningLayer;
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
            if (this.elementFunctions[func] != null)
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
        public Tuple<int, int> GetSize()
        {
            Tuple<int, int> returnVal = new Tuple<int, int>();

            returnVal.Item1 = this.width;
            returnVal.Item2 = this.height;

            return returnVal;
        }
        */

        public void SetSize(int newWidth, int newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;
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

        //return 'default' bcuz im dum lol
        public Tuple<int, int> GetSize()
        {
            return Stage.CurrentStage.GetTileSize();
        }

        private void SetNeedsUpdate()
        {
            Rendering.DOM_Renderer.GetRenderer().SetNeedsUpdate(this);
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
    }
}
