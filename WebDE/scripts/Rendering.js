/*Generated by SharpKit 5 v4.28.1000*/
if (typeof($CreateDelegate)=='undefined'){
    if(typeof($iKey)=='undefined') var $iKey = 0;
    if(typeof($pKey)=='undefined') var $pKey = String.fromCharCode(1);
    $CreateDelegate = function(target, func){
        if (target == null || func == null) 
            return func;
        if(func.target==target && func.func==func)
            return func;
        if (target.$delegateCache == null)
            target.$delegateCache = {};
        if (func.$key == null)
            func.$key = $pKey + String(++$iKey);
        var delegate;
        if(target.$delegateCache!=null)
            delegate = target.$delegateCache[func.$key];
        if (delegate == null){
            delegate = function(){
                return func.apply(target, arguments);
            };
            delegate.func = func;
            delegate.target = target;
            delegate.isDelegate = true;
            if(target.$delegateCache!=null)
                target.$delegateCache[func.$key] = delegate;
        }
        return delegate;
    }
}
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var WebDE$Rendering$DOM_Renderer=
{
    fullname:"WebDE.Rendering.DOM_Renderer",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Rendering.DOM_Renderer.gameRenderer = null;
        },
        GetRenderer:function()
        {
            return WebDE.Rendering.DOM_Renderer.gameRenderer;
        }
    },
    assemblyName:"WebDE",
    interfaceNames:["WebDE.Rendering.IRenderEngine"],
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            this.document = null;
            this.window = null;
            this.initiallyRendered = false;
            this.elementsByGameObjectId = new System.Collections.Generic.Dictionary$2.ctor(System.String,"ELEMENT");
            this.elementsByGuiId = new System.Collections.Generic.Dictionary$2.ctor(System.String,"ELEMENT");
            this.gameEntitiesToUpdate = new System.Collections.Generic.List$1.ctor(System.String);
            this.guiElementsToUpdate = new System.Collections.Generic.List$1.ctor(System.String);
            this.x = 0;
            this.y = 0;
            System.Object.ctor.call(this);
            WebDE.Rendering.DOM_Renderer.gameRenderer = this;
            this.document = WebDE.DefaultClient.GetDocument();
            this.window = WebDE.DefaultClient.GetWindow();
            WebDE.Game.SetRenderer(this);
        },
        InitialRender:function()
        {
            var parentElement=this.document.createElement("div");
            parentElement.id = "gameWrapper";
            var gameBoard=this.document.createElement("div");
            gameBoard.id = "gameBoard";
            this.document.body.appendChild(parentElement);
            parentElement.appendChild(gameBoard);
            var windowArea=new WebDE.Rectangle.ctor(0,0,this.window.innerWidth,this.window.innerHeight);
            parentElement.style.width = Math.round(windowArea.width * 0.98) + "px";
            parentElement.style.height = Math.round(windowArea.height * 0.98) + "px";
            this.RebuildAnimationFrames();
            var $it48=WebDE.Rendering.View.GetActiveViews().GetEnumerator();
            while($it48.MoveNext())
            {
                var view=$it48.get_Current();
                var $it49=view.GetLayers().GetEnumerator();
                while($it49.MoveNext())
                {
                    var layer=$it49.get_Current();
                    this.RenderGUILayer(layer);
                    var $it50=layer.GetGuiElements().GetEnumerator();
                    while($it50.MoveNext())
                    {
                        var gelm=$it50.get_Current();
                        this.RenderGUIElement(gelm);
                    }
                }
            }
            this.Resize();
            this.initiallyRendered = true;
            this.Render();
        },
        Render:function()
        {
            if(this.initiallyRendered == false)
            {
                this.InitialRender();
            }
            var $it51=WebDE.Rendering.View.GetActiveViews().GetEnumerator();
            while($it51.MoveNext())
            {
                var view=$it51.get_Current();
                var $it52=view.GetVisibleEntities().GetEnumerator();
                while($it52.MoveNext())
                {
                    var gent=$it52.get_Current();
                    this.RenderGameEntity(gent);
                }
                WebDE.Debug.Watch("Rendering light sources: ",WebDE.GameObjects.Stage.CurrentStage.GetLights().get_Count().toString());
                var $it53=WebDE.GameObjects.Stage.CurrentStage.GetLights().GetEnumerator();
                while($it53.MoveNext())
                {
                    var light=$it53.get_Current();
                    this.RenderLightSource(light);
                }
                var $it54=view.GetLayers().GetEnumerator();
                while($it54.MoveNext())
                {
                    var layer=$it54.get_Current();
                    if(this.elementsByGuiId.get_Item$$TKey(layer.GetId()) == null || this.guiElementsToUpdate.Contains(layer.GetId()))
                    {
                        this.RenderGUILayer(layer);
                    }
                    var $it55=layer.GetGuiElements().GetEnumerator();
                    while($it55.MoveNext())
                    {
                        var gelm=$it55.get_Current();
                        this.RenderGUIElement(gelm);
                    }
                }
            }
        },
        RenderGameEntity:function(gent)
        {
            var gentlement=this.elementsByGameObjectId.get_Item$$TKey(gent.GetId());
            if(gentlement == null)
            {
                gentlement = this.document.createElement("div");
                this.AddClass(gentlement,"Entity");
                this.AddClass(gentlement,gent.GetType().get_Name());
                this.document.getElementById("gameBoard").appendChild(gentlement);
                var $it56=gent.GetCustomStyles().GetEnumerator();
                while($it56.MoveNext())
                {
                    var style=$it56.get_Current();
                    this.AddClass(gentlement,style);
                }
                this.elementsByGameObjectId.set_Item$$TKey(gent.GetId(),gentlement);
            }
            if(this.gameEntitiesToUpdate.Contains(gent.GetId()))
            {
                gentlement.style.left = (gent.GetPosition().x * WebDE.GameObjects.Stage.CurrentStage.GetTileSize().get_Item1()) + "px";
                gentlement.style.top = (gent.GetPosition().y * WebDE.GameObjects.Stage.CurrentStage.GetTileSize().get_Item2()) + "px";
                gentlement.style.opacity = gent.GetOpacity();
                this.gameEntitiesToUpdate.Remove(gent.GetId());
            }
            if(gent.GetSprite() == null)
            {
                return;
            }
            var frameId=gent.GetSprite().Animate();
            if(frameId != gent.GetSprite().GetCurrentRenderFrame())
            {
                this.RemoveClass(gentlement,gent.GetSprite().GetCurrentRenderFrame());
                this.AddClass(gentlement,frameId);
                gent.GetSprite().SetCurrentRenderFrame(frameId);
                gentlement.style.width = gent.GetSprite().getSize().get_Item1() + "px";
                gentlement.style.height = gent.GetSprite().getSize().get_Item2() + "px";
            }
        },
        DestroyGameEntity:function(gent)
        {
            var gentlement=this.elementsByGameObjectId.get_Item$$TKey(gent.GetId());
            if(gentlement != null)
            {
                try
                {
                    gentlement.parentElement.removeChild(gentlement);
                    this.elementsByGameObjectId.Remove(gent.GetId());
                }
                catch(ex)
                {
                    WebDE.Debug.log("Failed to destroy " + gent.GetId() + " ( " + gent.GetName() + " ) :");
                    WebDE.Debug.log(ex.get_Message());
                }
            }
        },
        RenderGUILayer:function(glayer)
        {
            var layerElem=this.elementsByGuiId.get_Item$$TKey(glayer.GetId());
            if(layerElem == null)
            {
                layerElem = this.document.createElement("div");
                this.AddClass(layerElem,"GUILayer");
                this.document.getElementById("gameWrapper").appendChild(layerElem);
                this.elementsByGuiId.set_Item$$TKey(glayer.GetId(),layerElem);
            }
            if(this.guiElementsToUpdate.Contains(glayer.GetId()))
            {
                this.guiElementsToUpdate.Remove(glayer.GetId());
            }
            if(glayer.Visible())
            {
                layerElem.style.display = "inline";
            }
            else
            {
                layerElem.style.display = "none";
            }
            if(glayer.GetPosition().x > 0)
            {
                layerElem.style.left = glayer.GetPosition().x + "px";
            }
            else
            {
                layerElem.style.right = Math.abs(glayer.GetPosition().x) + "px";
            }
            if(glayer.GetPosition().y > 0)
            {
                layerElem.style.top = glayer.GetPosition().y + "px";
            }
            else
            {
                layerElem.style.bottom = Math.abs(glayer.GetPosition().y) + "px";
            }
            layerElem.style.width = glayer.GetSize().get_Item1() + "px";
            layerElem.style.height = glayer.GetSize().get_Item2() + "px";
        },
        RenderGUIElement:function(gelm)
        {
            var gentlement=this.elementsByGuiId.get_Item$$TKey(gelm.GetId());
            if(gentlement == null)
            {
                gentlement = this.document.createElement("div");
                this.AddClass(gentlement,"GUIElement");
                this.elementsByGuiId.get_Item$$TKey(gelm.GetParentLayer().GetId()).appendChild(gentlement);
                if(gelm.GetText() != "" && gelm.GetSprite() == null)
                {
                    gentlement.style.width = (gelm.GetText().length * 12) + "px";
                    gentlement.style.height = 12 + "px";
                    gentlement.innerText = gelm.GetText();
                }
                var styleString="";
                var $it57=gelm.GetStyles().GetEnumerator();
                while($it57.MoveNext())
                {
                    var style=$it57.get_Current();
                    styleString += style + " ";
                }
                this.AddClass(gentlement,styleString);
                this.elementsByGuiId.set_Item$$TKey(gelm.GetId(),gentlement);
            }
            if(this.guiElementsToUpdate.Contains(gelm.GetId()))
            {
                gentlement.innerText = gelm.GetText();
                gentlement.style.left = gelm.GetPosition().x + "px";
                gentlement.style.top = gelm.GetPosition().y + "px";
                this.guiElementsToUpdate.Remove(gelm.GetId());
            }
            if(gelm.GetSprite() == null)
            {
                return;
            }
            gentlement.innerText = "";
            var frameId=gelm.GetSprite().Animate();
            if(frameId != gelm.GetSprite().GetCurrentRenderFrame())
            {
                this.RemoveClass(gentlement,gelm.GetSprite().GetCurrentRenderFrame());
                this.AddClass(gentlement,frameId);
                gelm.GetSprite().SetCurrentRenderFrame(frameId);
                gentlement.style.width = gelm.GetSprite().getSize().get_Item1() + "px";
                gentlement.style.height = gelm.GetSprite().getSize().get_Item2() + "px";
            }
        },
        RenderLightSource:function(light)
        {
            var gentlement=this.elementsByGameObjectId.get_Item$$TKey(light.GetId());
            if(gentlement == null)
            {
                gentlement = this.document.createElement("div");
                this.AddClass(gentlement,"Entity LightSource");
                this.AddClass(gentlement,light.GetType().get_Name());
                this.document.getElementById("gameBoard").appendChild(gentlement);
                if(light.GetType().get_Name() == "Lightstone")
                {
                    gentlement.style.background = WebDE.Rendering.Gradient.LightStone(light.GetColor());
                }
                else
                {
                    gentlement.style.background = WebDE.Rendering.Gradient.ToString$$Color(light.GetColor());
                }
                this.elementsByGameObjectId.set_Item$$TKey(light.GetId(),gentlement);
            }
            var tileSize=WebDE.GameObjects.Stage.CurrentStage.GetTileSize();
            var posShift=Cast(light.GetRange(),System.Int32) / 2;
            gentlement.style.left = ((light.GetPosition().x - posShift) * tileSize.get_Item1()) + "px";
            gentlement.style.top = ((light.GetPosition().y - posShift) * tileSize.get_Item2()) + "px";
            gentlement.style.width = (light.GetRange() * tileSize.get_Item1()) + "px";
            gentlement.style.height = (light.GetRange() * tileSize.get_Item2()) + "px";
            if(light.Visible() == true)
            {
                gentlement.style.display = "inline";
            }
            else
            {
                gentlement.style.display = "none";
            }
            var lightLeftInGameUnits=Cast(light.GetPosition().x,System.Int32) - posShift;
            var lightRightInGameUnits=Cast((lightLeftInGameUnits + light.GetRange()),System.Int32);
            var lightTopInGameUnits=Cast(light.GetPosition().y,System.Int32) - posShift;
            var lightBottomInGameUnits=Cast((lightTopInGameUnits + light.GetRange()),System.Int32);
        },
        AddClass:function(elem,className)
        {
            var thisClass=elem.className;
            if(!thisClass.Contains(className))
            {
                thisClass += " " + className;
            }
            elem.className = thisClass;
        },
        RemoveClass:function(elem,className)
        {
            var thisClass=elem.className;
            if(thisClass.Contains(className))
            {
                var cni=thisClass.indexOf(className);
                if(cni > 0)
                {
                    thisClass = thisClass.Remove$$Int32$$Int32(cni - 1,className.length);
                }
                else
                {
                    thisClass = thisClass.Remove$$Int32$$Int32(cni,className.length);
                }
            }
            elem.className = thisClass;
        },
        Resize:function()
        {
            var wrapperWidth=this.getOuterWidth(this.document.getElementById("gameWrapper"));
            var wrapperHeight=this.getOuterHeight(this.document.getElementById("gameWrapper"));
            var boardWidth=this.getOuterWidth(this.document.getElementById("gameBoard"));
            var boardHeight=this.getOuterHeight(this.document.getElementById("gameBoard"));
            this.x = (wrapperWidth - boardWidth) / 2;
            this.y = (wrapperHeight - boardHeight) / 2;
            this.document.getElementById("gameBoard").style.position = "absolute";
            this.document.getElementById("gameBoard").style.left = this.x + "px";
            this.document.getElementById("gameBoard").style.top = this.y + "px";
        },
        BoardArea:function()
        {
            var boardWidth=this.getOuterWidth(this.document.getElementById("gameBoard"));
            var boardHeight=this.getOuterHeight(this.document.getElementById("gameBoard"));
            return new WebDE.Rectangle.ctor(this.x,this.y,boardWidth,boardHeight);
        },
        getOuterWidth:function(elem)
        {
            var returnVal=elem.clientWidth;
            returnVal += (WebDE.GameObjects.Helpah.Parse(elem.style.borderWidth) * 2);
            returnVal += WebDE.GameObjects.Helpah.Parse(elem.style.marginLeft);
            returnVal += WebDE.GameObjects.Helpah.Parse(elem.style.marginRight);
            return returnVal;
        },
        getOuterHeight:function(elem)
        {
            return elem.offsetHeight + (WebDE.GameObjects.Helpah.Parse(elem.style.borderWidth) * 2) + WebDE.GameObjects.Helpah.Parse(elem.style.marginTop) + WebDE.GameObjects.Helpah.Parse(elem.style.marginBottom);
        },
        RebuildAnimationFrames:function()
        {
            if(this.document.getElementById("animFrameClasses") != null)
            {
                this.document.body.removeChild(this.document.getElementById("animFrameClasses"));
            }
            var animFrmCssContents="";
            var $it58=WebDE.Animation.AnimationFrame.GetAnimationFrames().GetEnumerator();
            while($it58.MoveNext())
            {
                var animFrame=$it58.get_Current();
                var framePos=animFrame.getPosition();
                animFrmCssContents += "." + animFrame.getId() + " { " + "background-image: url(\'" + animFrame.getImage() + "\'); " + "background-position-x: " + framePos.get_Item1() + "px; " + "background-position-y: " + framePos.get_Item2() + "px; " + " }";
            }
            var framesStyle=this.document.createElement("style");
            framesStyle.innerHTML = animFrmCssContents;
            this.document.body.appendChild(framesStyle);
        },
        SetNeedsUpdate$$GameEntity:function(gent)
        {
            if(!this.gameEntitiesToUpdate.Contains(gent.GetId()))
            {
                this.gameEntitiesToUpdate.Add(gent.GetId());
            }
        },
        SetNeedsUpdate$$GuiElement:function(gelm)
        {
            if(!this.guiElementsToUpdate.Contains(gelm.GetId()))
            {
                this.guiElementsToUpdate.Add(gelm.GetId());
            }
        },
        SetNeedsUpdate$$GuiLayer:function(layer)
        {
            if(!this.guiElementsToUpdate.Contains(layer.GetId()))
            {
                this.guiElementsToUpdate.Add(layer.GetId());
            }
        },
        GetSize:function()
        {
            return new System.Tuple$2.ctor(System.Int32,System.Int32,this.window.innerWidth,this.window.innerHeight);
        }
    }
};
JsTypes.push(WebDE$Rendering$DOM_Renderer);
var WebDE$Rendering$Gradient=
{
    fullname:"WebDE.Rendering.Gradient",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        ToString$$Color:function(gradientColor)
        {
            return "-webkit-radial-gradient(center, ellipse cover, rgba(" + gradientColor.red + "," + gradientColor.green + "," + gradientColor.blue + ",1) 0%, " + "rgba(" + gradientColor.red + "," + gradientColor.green + "," + gradientColor.blue + ",0.99) 1%, rgba(0,0,0,0) 100%)";
        },
        LightStone:function(gradientColor)
        {
            return " -webkit-gradient(radial, center center, 0px, center center, 100%, color-stop(15%,rgba(0,80,200,0.25)), color-stop(26%,rgba(0,80,200,0.34)), color-stop(59%,rgba(0,80,200,0.6)), color-stop(66%,rgba(0,80,200,0.65)), color-stop(85%,rgba(0,80,200,0)), color-stop(100%,rgba(0,80,200,0)));  -webkit-radial-gradient(center, ellipse cover,  rgba(0,80,200,0.25) 15%,rgba(0,80,200,0.34) 26%,rgba(0,80,200,0.6) 59%,rgba(0,80,200,0.65) 66%,rgba(0,80,200,0) 85%,rgba(0,80,200,0) 100%); ";
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(WebDE$Rendering$Gradient);
var WebDE$Rendering$IRenderEngine={fullname:"WebDE.Rendering.IRenderEngine",baseTypeName:"System.Object",assemblyName:"WebDE",Kind:"Interface"};
JsTypes.push(WebDE$Rendering$IRenderEngine);
var WebDE$Rendering$Surface=
{
    fullname:"WebDE.Rendering.Surface",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Rendering.Surface.renderer = null;
        },
        Initialize:function(renderer)
        {
            WebDE.Rendering.Surface.renderer = renderer;
            WebDE.Timekeeper.Clock.AddRender($CreateDelegate(WebDE.Rendering.Surface.renderer,WebDE.Rendering.Surface.renderer.Render));
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(WebDE$Rendering$Surface);
var WebDE$Rendering$View=
{
    fullname:"WebDE.Rendering.View",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Rendering.View.activeViews = new System.Collections.Generic.List$1.ctor(WebDE.Rendering.View);
            WebDE.Rendering.View.mainView = null;
        },
        GetActiveViews:function()
        {
            return WebDE.Rendering.View.activeViews;
        },
        SetMainView:function(newView)
        {
            WebDE.Rendering.View.mainView = newView;
        },
        GetMainView:function()
        {
            return WebDE.Rendering.View.mainView;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(lightStyle)
        {
            this.stgAttached = null;
            this.guiLayers = new System.Collections.Generic.List$1.ctor(WebDE.GUI.GuiLayer);
            this.width = 800;
            this.height = 600;
            this.x = 0;
            this.y = 0;
            this.active = false;
            this.LightStyle = 0;
            System.Object.ctor.call(this);
            this.LightStyle = lightStyle;
            this.Activate();
            if(WebDE.Rendering.View.mainView == null)
            {
                WebDE.Rendering.View.mainView = this;
            }
        },
        IsActive:function()
        {
            return this.active;
        },
        Activate:function()
        {
            this.active = true;
            if(WebDE.Rendering.View.activeViews.Contains(this) == false)
            {
                WebDE.Rendering.View.activeViews.Add(this);
            }
        },
        Deactivate:function()
        {
            this.active = false;
            if(WebDE.Rendering.View.activeViews.Contains(this) == true)
            {
                WebDE.Rendering.View.activeViews.Remove(this);
            }
        },
        AttachStage:function(newStage)
        {
            this.stgAttached = newStage;
        },
        GetAttachedStage:function()
        {
            return this.stgAttached;
        },
        GetViewArea:function()
        {
            return new WebDE.Rectangle.ctor(this.x,this.y,this.width,this.height);
        },
        GetSize:function()
        {
            return new System.Tuple$2.ctor(System.Int32,System.Int32,this.width,this.height);
        },
        SetSize:function(newWidth,newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;
        },
        SetPosition:function(newX,newY)
        {
            this.x = newX;
            this.y = newY;
        },
        GetArea:function()
        {
            var returnVal=new WebDE.Rectangle.ctor(this.x,this.y,this.width,this.height);
            return returnVal;
        },
        SetArea:function(newArea)
        {
            this.x = Cast(newArea.x,System.Int32);
            this.y = Cast(newArea.y,System.Int32);
            this.width = Cast(newArea.width,System.Int32);
            this.height = Cast(newArea.height,System.Int32);
        },
        AddLayer:function(layerName,layerPos)
        {
            var newLayer=new WebDE.GUI.GuiLayer.ctor(layerName,this,layerPos);
            this.guiLayers.Add(newLayer);
            return newLayer;
        },
        GetLayers:function()
        {
            return this.guiLayers;
        },
        GetVisibleEntities:function()
        {
            var returnList=new System.Collections.Generic.List$1.ctor(WebDE.GameObjects.GameEntity);
            if(this.stgAttached != null)
            {
                returnList.AddRange(this.stgAttached.GetVisibleEntities(this));
                returnList.AddRange(this.stgAttached.GetVisibleTiles(this));
            }
            return returnList;
        },
        SetLightingStyle:function(newStyle)
        {
            this.LightStyle = newStyle;
        },
        GetLightingStyle:function()
        {
            return this.LightStyle;
        },
        resize:function()
        {
        },
        RenderLightSources:function()
        {
            var $it59=this.stgAttached.GetLights().GetEnumerator();
            while($it59.MoveNext())
            {
                var light=$it59.get_Current();
            }
        },
        GUI_Event:function(buttonFunction,clickX,clickY)
        {
            clickX -= Cast(this.GetArea().x,System.Int32);
            clickX -= Cast(this.GetArea().y,System.Int32);
            var $it60=this.guiLayers.GetEnumerator();
            while($it60.MoveNext())
            {
                var layer=$it60.get_Current();
                if(clickX > layer.GetArea().Right() || clickX < layer.GetArea().x || clickY > layer.GetArea().Bottom() || clickY < layer.GetArea().y)
                {
                    continue;
                }
                var actionLocation=new WebDE.Point.ctor(clickX,clickY);
                layer.GUI_Event(buttonFunction,actionLocation);
            }
        }
    }
};
JsTypes.push(WebDE$Rendering$View);
