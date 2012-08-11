using System;
using System.Collections.Generic;

using SharpKit.Html4;
using SharpKit.JavaScript;
using SharpKit.jQuery;

using WebDE.Animation;
using WebDE.GameObjects;
using WebDE.GUI;

namespace WebDE.Rendering
{
    [JsType(JsMode.Clr, Filename = "../scripts/Rendering.js")]
    public class DOM_Renderer : IRenderEngine
    {
        private static IRenderEngine gameRenderer;

        public static IRenderEngine GetRenderer()
        {
            return gameRenderer;
        }

        private HtmlDocument document;
        private HtmlWindow window;
        private bool initiallyRendered = false;
        private Dictionary<string, HtmlElement> elementsByGameObjectId = new Dictionary<string, HtmlElement>();
        private Dictionary<string, HtmlElement> elementsByGuiId = new Dictionary<string, HtmlElement>();
        private List<string> gameEntitiesToUpdate = new List<string>();
        private List<string> guiElementsToUpdate = new List<string>();

        //used for resize and reposition functionality...
        private int x;
        private int y;

        public DOM_Renderer()
        {
            gameRenderer = this;
            document = Main.GetDocument();
            window = Main.GetWindow();
            Game.SetRenderer(this);
        }

        public void InitialRender()
        {
            //create the game wrapper taat holds all of the game visual elements
            HtmlElement parentElement = document.createElement("div");
            parentElement.id = "gameWrapper";
            //create the game board
            HtmlElement gameBoard = document.createElement("div");
            gameBoard.id = "gameBoard";
            //add the object to the body (DOM). Note that this has to be called at Window / Document load or later
            document.body.appendChild(parentElement);
            parentElement.appendChild(gameBoard);

            //Rectangle windowArea = new Rectangle(0, 0, document.parentWindow.innerWidth, document.parentWindow.innerHeight);
            Rectangle windowArea = new Rectangle(0, 0, window.innerWidth, window.innerHeight);
            parentElement.style.width = JsMath.round(windowArea.width * .98) + "px";
            parentElement.style.height = JsMath.round(windowArea.height * .98) + "px";

            this.RebuildAnimationFrames();

            //create the GUI elements (now the responsibility of the views)
            //BuildGui();

            foreach (View view in View.GetActiveViews())
            {
                //render any applicable properties of the view
                //should it have its own div with boundaries and so forth?

                //render all gui layers
                foreach (GuiLayer layer in view.GetLayers())
                {
                    this.RenderGUILayer(layer);
                    foreach (GuiElement gelm in layer.GetGuiElements())
                    {
                        this.RenderGUIElement(gelm);
                    }
                }
            }

            //move this to input
            //Window.AddEventListener("resize", delegate(ElementEvent e) { resize(); });

            //call the view's resize event so that things determine their correct size and location
            this.Resize();

            //mark that this view has been rendered
            this.initiallyRendered = true;

            //call the render function
            this.Render();
        }

        /*
        private void buildGui()
        {
            foreach (GuiLayer layerToBuild in GuiLayer.allTheLayers)
            {
                layerToBuild.InitialRender();
            }
        }
        */

        public void Render()
        {
            Debug.Watch("Entities to update", this.gameEntitiesToUpdate.Count.ToString());

            //just in case this gets called before initialrender
            if (this.initiallyRendered == false)
            {
                this.InitialRender();
            }

            foreach (View view in View.GetActiveViews())
            {
                //game entities, including living and tiles
                foreach (GameEntity gent in view.GetVisibleEntities())
                {
                    this.RenderGameEntity(gent);
                }
                //if (this.LightStyle == LightingStyle.Gradients)
                Debug.Watch("Rendering light sources: ", Stage.CurrentStage.GetLights().Count.ToString());
                foreach(LightSource light in Stage.CurrentStage.GetLights())
                {
                    this.RenderLightSource(light);
                }

                //gui
                foreach (GuiLayer layer in view.GetLayers())
                {
                    if (elementsByGuiId[layer.GetId()] == null || guiElementsToUpdate.Contains(layer.GetId())
                        || layer.FollowingCursor() == true)
                    {
                        this.RenderGUILayer(layer);
                    }

                    foreach (GuiElement gelm in layer.GetGuiElements())
                    {
                        this.RenderGUIElement(gelm);
                    }
                }
            }
        }

        //!Need to add better support for custom styles
        public void RenderGameEntity(GameEntity gent)
        {
            HtmlElement gentlement = elementsByGameObjectId[gent.GetId()];
            //if it's not rendered, render it
            if (gentlement == null)
            {
                gentlement = document.createElement("div");
                gentlement.id = gent.GetId();
                //later, we may want to add iterating through base types to get their type name and add it...
                this.AddClass(gentlement, "Entity");
                this.AddClass(gentlement, gent.GetType().Name);
                document.getElementById("gameBoard").appendChild(gentlement);
                foreach (string style in gent.GetCustomStyles())
                {
                    this.AddClass(gentlement, style);
                }

                elementsByGameObjectId[gent.GetId()] = gentlement;
            }

            if(this.gameEntitiesToUpdate.Contains(gent.GetId()))
            {
                //reposition the element based on game position
                gentlement.style.left = (gent.GetPosition().x * Stage.CurrentStage.GetTileSize().width) + "px";
                gentlement.style.top = (gent.GetPosition().y * Stage.CurrentStage.GetTileSize().height) + "px";
                gentlement.style.opacity = gent.GetOpacity();

                this.gameEntitiesToUpdate.Remove(gent.GetId());
            }

            //if the entity doesn't have a sprite, we're done with all rendering for now
            if (gent.GetSprite() == null)
            {
                return;
            }

            //call the sprite's animate function
            //it will return the id of the frame that the sprite and its animation are currently on
            string frameId = gent.GetSprite().Animate();

            //if that result is different from the value we have stored as this GameEntity's current display frame,
            if (frameId != gent.GetSprite().GetCurrentRenderFrame())
            {
                //update this thing's CSS to the new frame
                //jQueryObject GameEntityDiv = jQuery.FromElement(GameEntityDiv);
                this.RemoveClass(gentlement, gent.GetSprite().GetCurrentRenderFrame());
                this.AddClass(gentlement, frameId);
                //and update our current frameid
                gent.GetSprite().SetCurrentRenderFrame(frameId);

                gentlement.style.width = gent.GetSprite().GetSize().width + "px";
                gentlement.style.height = gent.GetSprite().GetSize().height + "px";
            }
        }

        public void DestroyGameEntity(GameEntity gent)
        {
            HtmlElement gentlement = elementsByGameObjectId[gent.GetId()];
            //if it's not rendered, render it
            if (gentlement != null)
            {
                try
                {
                    gentlement.parentElement.removeChild(gentlement);
                    elementsByGameObjectId.Remove(gent.GetId());
                }
                catch (Exception ex)
                {
                    Debug.log("Failed to destroy " + gent.GetId() + " ( " + gent.GetName() + " ) :");
                    Debug.log(ex.Message);
                }
            }
        }

        public void RenderGUILayer(GuiLayer glayer)
        {
            HtmlElement layerElem = elementsByGuiId[glayer.GetId()];
            //if it's not rendered, render it
            if (layerElem == null)
            {
                layerElem = document.createElement("div");
                layerElem.id = glayer.GetName();
                this.AddClass(layerElem, "GUILayer");
                document.getElementById("gameWrapper").appendChild(layerElem);
                //this.elementsByGuiId[gelm.GetParentLayer().GetName()].appendChild(gentlement);

                elementsByGuiId[glayer.GetId()] = layerElem;
            }

            if (guiElementsToUpdate.Contains(glayer.GetId()))
            {
                guiElementsToUpdate.Remove(glayer.GetId());
            }

            if (glayer.Visible())
            {
                layerElem.style.display = "inline";
            }
            else
            {
                layerElem.style.display = "none";
            }

            if (glayer.FollowingCursor() == true)
            {
                //set the position to the cursor position
                glayer.SetPosition(
                    InputManager.InputDevice.Mouse.GetAxisPosition(0) - (glayer.GetSize().width / 2),
                    InputManager.InputDevice.Mouse.GetAxisPosition(1) - (glayer.GetSize().height / 2));
                layerElem.style.position = "absolute";
            }

            //reposition the element based on game position
            if (glayer.GetPosition().x > 0)
            {
                layerElem.style.left = glayer.GetPosition().x + "px";
            }
            else
            {
                layerElem.style.right = JsMath.abs(glayer.GetPosition().x) + "px";
            }
            if (glayer.GetPosition().y > 0)
            {
                layerElem.style.top = glayer.GetPosition().y + "px";
            }
            else
            {
                layerElem.style.bottom = JsMath.abs(glayer.GetPosition().y) + "px";
            }
            layerElem.style.width = glayer.GetSize().width + "px";
            layerElem.style.height = glayer.GetSize().height + "px";
        }

        public void RenderGUIElement(GuiElement gelm)
        {
            HtmlElement gentlement = elementsByGuiId[gelm.GetId()];
            //if it's not rendered, render it
            if (gentlement == null)
            {
                gentlement = document.createElement("div");
                this.AddClass(gentlement, "GUIElement");
                //document.getElementById("gameBoard").appendChild(gentlement);
                this.elementsByGuiId[gelm.GetParentLayer().GetId()].appendChild(gentlement);
                //!! Basing this on size 12 font right now, and it's going to be totally wrong, and needs to be done up properly
                if (gelm.GetText() != "" && gelm.GetSprite() == null)
                {
                    gentlement.style.width = (gelm.GetText().Length * 12) + "px";
                    gentlement.style.height = 12 + "px";
                    gentlement.innerText = gelm.GetText();
                }
                else
                {
                    gentlement.style.width = gelm.GetSize().width + " px";
                    gentlement.style.height = gelm.GetSize().height + "px";
                }

                //since we're building this for the first time, put all the classes into a long list, to cut down on operations
                string styleString = "";
                foreach (string style in gelm.GetStyles())
                {
                    styleString += style + " ";
                }
                //styleString = styleString.TrimEnd();
                this.AddClass(gentlement, styleString);

                elementsByGuiId[gelm.GetId()] = gentlement;
            }

            //update the element with new position information if need be
            if (this.guiElementsToUpdate.Contains(gelm.GetId()))
            {
                gentlement.innerText = gelm.GetText();

                //reposition the element based on game position
                gentlement.style.left = gelm.GetPosition().x + "px";
                gentlement.style.top = gelm.GetPosition().y + "px";

                if (gelm.GetStyle("Background") != null)
                {
                    gentlement.style.background = gelm.GetStyle("Background");
                }

                this.guiElementsToUpdate.Remove(gelm.GetId());
            }

            if (gelm.GetSprite() == null)
            {
                return;
            }
            gentlement.innerText = "";

            //call the sprite's animate function
            //it will return the id of the frame that the sprite and its animation are currently on
            string frameId = gelm.GetSprite().Animate();

            //if that result is different from the value we have stored as this GameEntity's current display frame,
            if (frameId != gelm.GetSprite().GetCurrentRenderFrame())
            {
                //update this thing's CSS to the new frame
                //jQueryObject GameEntityDiv = jQuery.FromElement(GameEntityDiv);
                this.RemoveClass(gentlement, gelm.GetSprite().GetCurrentRenderFrame());
                this.AddClass(gentlement, frameId);
                //and update our current frameid
                gelm.GetSprite().SetCurrentRenderFrame(frameId);

                gentlement.style.width = gelm.GetSprite().GetSize().width + "px";
                gentlement.style.height = gelm.GetSprite().GetSize().height + "px";
            }
        }

        public void RenderLightSource(LightSource light)
        {
            HtmlElement gentlement = elementsByGameObjectId[light.GetId()];
            //if it's not rendered, render it
            if (gentlement == null)
            {
                Debug.log("Rendering light " + light.GetId());
                gentlement = document.createElement("div");
                this.AddClass(gentlement, "Entity LightSource");
                this.AddClass(gentlement, light.GetType().Name);
                foreach (string style in light.GetCustomStyles())
                {
                    this.AddClass(gentlement, style);
                }
                document.getElementById("gameBoard").appendChild(gentlement);
                if (light.GetType().Name == "Lightstone")
                {
                    gentlement.style.background = Gradient.LightStone(light.GetColor());
                    Debug.log(Gradient.LightStone(light.GetColor()));
                }
                else
                {
                    gentlement.style.background = Gradient.ToString(light.GetColor());
                }

                elementsByGameObjectId[light.GetId()] = gentlement;
            }

            //if (this.gameEntitiesToUpdate.Contains(light.GetId()))
            //this.gameEntitiesToUpdate.Remove(light.GetId());
            
            Dimension tileSize = Stage.CurrentStage.GetTileSize();

            //the light's x and y are its center
            //the size of the div should be the diameter of the light, or twice its range
            //the visual element needs to shift -x and -y with each -range to remain 'centered' on the light's actual x and y

            //the amount to shift the position by
            int posShift = (int) light.GetRange() / 2;

            gentlement.style.left = ((light.GetPosition().x - posShift) * tileSize.width) + "px";
            gentlement.style.top = ((light.GetPosition().y - posShift) * tileSize.height) + "px";

            gentlement.style.width = (light.GetRange() * tileSize.width) + "px";
            gentlement.style.height = (light.GetRange() * tileSize.height) + "px";

            if (light.Visible() == true)
            {
                gentlement.style.display = "inline";
            }
            else
            {
                gentlement.style.display = "none";
            }

            int lightLeftInGameUnits = (int)light.GetPosition().x - posShift;
            int lightRightInGameUnits = (int)(lightLeftInGameUnits + light.GetRange());
            int lightTopInGameUnits = (int)light.GetPosition().y - posShift;
            int lightBottomInGameUnits = (int)(lightTopInGameUnits + light.GetRange());

            /*
            //make the tiles below the light partially transparent. because. just go with me on this.
            for (int tileX = lightLeftInGameUnits; tileX < lightRightInGameUnits; tileX++)
            {
                //Debug.Watch("TileX at ", tileX.ToString());
                for (int tileY = lightTopInGameUnits; tileY < lightBottomInGameUnits; tileY++)
                {
                    tileX = JsMath.ceil(tileX);
                    tileY = JsMath.ceil(tileY);

                    //only get tiles within the radius of the light, don't catch edges
                    //if (new Point(tileX, tileY).Distance(lightMid) > light.GetRange() / 2)
                    Point positionOfTheTileWeAreLookingAt = new Point(tileX, tileY);
                    Point centerOfTheLight = light.GetPosition();
                    //double lightRadius = light.GetRange() / 2;
                    int lightRadius = JsMath.floor(light.GetRange() / 2);
                    int distance = (int) positionOfTheTileWeAreLookingAt.Distance(centerOfTheLight);
                    //if(distance <= lightRadius)
                    //if (positionOfTheTileWeAreLookingAt.Distance(centerOfTheLight) <= lightRadius)
                    if (new Point(tileX, tileY).Distance(light.GetPosition()) > light.GetRange() / 2)
                    {
                        Tile affectedTile = Stage.CurrentStage.GetTileAt(tileX, tileY);
                        if (affectedTile != null)
                        {
                            affectedTile.SetOpcaity(.5);
                        }
                    }
                }
            }
            */
        }

        public void AddClass(HtmlElement elem, string className)
        {
            string thisClass = elem.className;
            if (!thisClass.Contains(className))
            {
                thisClass += " " + className;
            }
            elem.className = thisClass;
        }

        public void RemoveClass(HtmlElement elem, string className)
        {
            string thisClass = elem.className;
            if (thisClass.Contains(className))
            {
                int cni = thisClass.IndexOf(className);
                //if there's string data before this substring
                if (cni > 0)
                {
                    //remove the substring as well as one white space before
                    thisClass = thisClass.Remove(cni - 1, className.Length);
                }
                else
                {
                    //remove the substring as well as one white space after
                    thisClass = thisClass.Remove(cni, className.Length);
                }
            }
            elem.className = thisClass;
        }

        public void Resize()
        {
            int wrapperWidth = this.getOuterWidth(document.getElementById("gameWrapper"));
            int wrapperHeight = this.getOuterHeight(document.getElementById("gameWrapper"));
            int boardWidth = this.getOuterWidth(document.getElementById("gameBoard"));
            int boardHeight = this.getOuterHeight(document.getElementById("gameBoard"));

            this.x = (wrapperWidth - boardWidth) / 2;
            this.y = (wrapperHeight - boardHeight) / 2;

            document.getElementById("gameBoard").style.position = "absolute";
            document.getElementById("gameBoard").style.left = this.x + "px";
            document.getElementById("gameBoard").style.top = this.y + "px";
        }

        public Rectangle BoardArea()
        {
            int boardWidth = this.getOuterWidth(document.getElementById("gameBoard"));
            int boardHeight = this.getOuterHeight(document.getElementById("gameBoard"));

            return new Rectangle(this.x, this.y, boardWidth, boardHeight);
        }

        /// <summary>
        /// Get the width of the object, including border and margins.
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private int getOuterWidth(HtmlElement elem)
        {
            int returnVal = elem.clientWidth;
            returnVal += (Helpah.Parse(elem.style.borderWidth) * 2);
            returnVal += Helpah.Parse(elem.style.marginLeft);
            returnVal += Helpah.Parse(elem.style.marginRight);
            //return elem.clientWidth + (int.Parse(elem.style.borderWidth) * 2) + int.Parse(elem.style.marginLeft) + int.Parse(elem.style.marginRight);

            return returnVal;
        }

        private int getOuterHeight(HtmlElement elem)
        {
            return elem.offsetHeight + (Helpah.Parse(elem.style.borderWidth) * 2) + Helpah.Parse(elem.style.marginTop) + Helpah.Parse(elem.style.marginBottom);
        }

        //rebuild the CSS element for the animation frames
        public void RebuildAnimationFrames()
        {
            //remove the existing anim frame container from the dom
            if (this.document.getElementById("animFrameClasses") != null)
            {
                this.document.body.removeChild(this.document.getElementById("animFrameClasses"));
            }

            string animFrmCssContents = "";
            //create visual components for animationframe objects, and cache them for fast loading
            foreach (AnimationFrame animFrame in AnimationFrame.GetAnimationFrames())
            {
                Tuple<int, int> framePos = animFrame.getPosition();
                //new css class
                animFrmCssContents += "." + animFrame.getId() + " { " +
                    "background-image: url('" + animFrame.getImage() + "'); " +
                    "background-position-x: " + framePos.Item1 + "px; " +
                    "background-position-y: " + framePos.Item2 + "px; " +
                    " }";
                //create image to cache it
                //mark it as cached
            }
            //create a new style tag, fill it with the created CSS classes, and append it to the document's head
            //jQuery.FromHtml("<style id='animFrameClasses' type='text/css'>" + animFrmCssContents + "</style>").AppendTo("head");
            HtmlElement framesStyle = document.createElement("style");
            framesStyle.innerHTML = animFrmCssContents;
            //document.head.appendChild(framesStyle);
            document.body.appendChild(framesStyle);
        }

        public void SetNeedsUpdate(GameEntity gent)
        {
            if (!this.gameEntitiesToUpdate.Contains(gent.GetId()))
            {
                this.gameEntitiesToUpdate.Add(gent.GetId());
            }
        }

        public void SetNeedsUpdate(GuiElement gelm)
        {
            if (!this.guiElementsToUpdate.Contains(gelm.GetId()))
            {
                this.guiElementsToUpdate.Add(gelm.GetId());
            }
        }

        public void SetNeedsUpdate(GuiLayer layer)
        {
            if (!this.guiElementsToUpdate.Contains(layer.GetId()))
            {
                this.guiElementsToUpdate.Add(layer.GetId());
            }
        }

        public Dimension GetSize()
        {
            return new Dimension(window.innerWidth, window.innerHeight);
        }
    }
}