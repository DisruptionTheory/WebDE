/*Generated by SharpKit 5 v4.28.1000*/
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var WebDE$GUI$GuiElement=
{
    fullname:"WebDE.GUI.GuiElement",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.GUI.GuiElement.lastid = 0;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(owningLayer,elementText)
        {
            this.id = null;
            this.text = null;
            this.sprIcon = null;
            this.position = new WebDE.Point.ctor(0,0);
            this.width = 0;
            this.height = 0;
            this.parentLayer = null;
            this.selected = false;
            this.customValue = "";
            this.styleClasses = new System.Collections.Generic.List$1.ctor(System.String);
            this.customStyles = new System.Collections.Generic.Dictionary$2.ctor(System.String,System.String);
            this.elementFunctions = new System.Collections.Generic.Dictionary$2.ctor(WebDE.GUI.GUIFunction,System.Action$1);
            System.Object.ctor.call(this);
            WebDE.GUI.GuiElement.lastid++;
            this.id = "GuiElement_" + WebDE.GUI.GuiElement.lastid;
            this.parentLayer = owningLayer;
            this.text = elementText;
        },
        GetId:function()
        {
            return this.id;
        },
        GetParentLayer:function()
        {
            return this.parentLayer;
        },
        SetGUIFunction:function(func,newEvent)
        {
            this.elementFunctions.set_Item$$TKey(func,newEvent);
        },
        DoGUIFunction:function(func)
        {
            if(this.elementFunctions.get_Item$$TKey(func) != null)
            {
                var eventToTrigger=WebDE.GUI.GuiEvent.FromGuiElement(this);
                this.elementFunctions.get_Item$$TKey(func)(eventToTrigger);
            }
            else
            {
                var $it11=this.elementFunctions.get_Keys().GetEnumerator();
                while($it11.MoveNext())
                {
                    var gf=$it11.get_Current();
                }
            }
        },
        AttachToGameEntity:function(entToAttach)
        {
        },
        GetPosition:function()
        {
            return this.position;
        },
        SetPosition:function(xPos,yPos)
        {
            this.position.x = xPos;
            this.position.y = yPos;
            this.SetNeedsUpdate();
        },
        SetSize:function(newWidth,newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;
            this.SetNeedsUpdate();
        },
        GetText:function()
        {
            return this.text;
        },
        SetText:function(newText)
        {
            this.text = newText;
            this.SetNeedsUpdate();
        },
        Hide:function()
        {
            this.SetNeedsUpdate();
            throw new System.NotImplementedException.ctor();
        },
        Show:function()
        {
            this.SetNeedsUpdate();
            throw new System.NotImplementedException.ctor();
        },
        Minimize:function()
        {
            this.SetNeedsUpdate();
            throw new System.NotImplementedException.ctor();
        },
        GetSprite:function()
        {
            return this.sprIcon;
        },
        SetSprite:function(newSprite)
        {
            this.sprIcon = newSprite;
            this.SetNeedsUpdate();
        },
        GetCustomValue:function()
        {
            return this.customValue;
        },
        SetCustomValue:function(newVal)
        {
            this.customValue = newVal;
        },
        Select:function(toApply)
        {
            if(toApply == true)
            {
                this.selected = true;
            }
            else
            {
                this.selected = false;
            }
            this.SetNeedsUpdate();
        },
        Selected:function()
        {
            return this.selected;
        },
        GetSize:function()
        {
            return new System.Tuple$2.ctor(System.Int32,System.Int32,this.width,this.height);
        },
        SetNeedsUpdate:function()
        {
            WebDE.Rendering.DOM_Renderer.GetRenderer().SetNeedsUpdate$$GuiElement(this);
        },
        AddStyle:function(styleToAdd)
        {
            if(!this.styleClasses.Contains(styleToAdd))
            {
                this.styleClasses.Add(styleToAdd);
            }
        },
        RemoveStyle:function(styleToAdd)
        {
            if(this.styleClasses.Contains(styleToAdd))
            {
                this.styleClasses.Remove(styleToAdd);
            }
        },
        GetStyles:function()
        {
            return this.styleClasses;
        },
        GetStyle:function(styleName)
        {
            return this.customStyles.get_Item$$TKey(styleName);
        },
        SetStyle:function(styleName,styleValue)
        {
            this.customStyles.set_Item$$TKey(styleName,styleValue);
            this.SetNeedsUpdate();
        }
    }
};
JsTypes.push(WebDE$GUI$GuiElement);
var WebDE$GUI$GuiEvent=
{
    fullname:"WebDE.GUI.GuiEvent",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        FromClickData:function(gLayer,clickPos)
        {
            var returnEvent=new WebDE.GUI.GuiEvent.ctor(System.Int32.Parse$$String(clickPos.x.toString()),System.Int32.Parse$$String(clickPos.y.toString()));
            returnEvent.clickedElement = gLayer.GetElementAt(returnEvent.eventPos.x,returnEvent.eventPos.y);
            returnEvent.clickedEntities = gLayer.GetEntitiesAt(returnEvent.eventPos.x,returnEvent.eventPos.y);
            returnEvent.clickedTiles = gLayer.GetTilesAt(returnEvent.eventPos.x,returnEvent.eventPos.y);
            return returnEvent;
        },
        FromGuiElement:function(sender)
        {
            var returnEvent=new WebDE.GUI.GuiEvent.ctor(Cast(sender.GetPosition().x,System.Int32),Cast(sender.GetPosition().y,System.Int32));
            returnEvent.clickedElement = sender;
            returnEvent.clickedTiles = sender.GetParentLayer().GetTilesAt(returnEvent.eventPixelPos.x,returnEvent.eventPixelPos.y);
            return returnEvent;
        },
        FromPartialData:function(sendingTile,sendingGameEntity,sendingElement,triggeringPosition,triggeringScreenPosition)
        {
            var eventToReturn=new WebDE.GUI.GuiEvent.ctor(0,0);
            if(sendingTile != null)
            {
                eventToReturn.clickedTiles.Add(sendingTile);
            }
            if(sendingGameEntity != null)
            {
                eventToReturn.clickedEntities.Add(sendingGameEntity);
            }
            if(sendingElement != null)
            {
                eventToReturn.clickedElement = sendingElement;
            }
            if(triggeringPosition != null)
            {
                eventToReturn.eventPos = triggeringPosition;
            }
            if(triggeringScreenPosition != null)
            {
                eventToReturn.eventPixelPos = triggeringScreenPosition;
            }
            if(eventToReturn.eventPos == null)
            {
            }
            return eventToReturn;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(xPos,yPos)
        {
            this.clickedTiles = new System.Collections.Generic.List$1.ctor(WebDE.GameObjects.Tile);
            this.clickedEntities = new System.Collections.Generic.List$1.ctor(WebDE.GameObjects.GameEntity);
            this.topGameEntity = null;
            this.eventPos = null;
            this.eventPixelPos = null;
            this.clickedElement = null;
            System.Object.ctor.call(this);
            var TileSize=WebDE.GameObjects.Stage.CurrentStage.GetTileSize();
            this.eventPixelPos = new WebDE.Point.ctor(xPos,yPos);
            this.eventPos = new WebDE.Point.ctor(this.eventPixelPos.x / TileSize.get_Item1(),this.eventPixelPos.y / TileSize.get_Item2());
        }
    }
};
JsTypes.push(WebDE$GUI$GuiEvent);
var WebDE$GUI$GUIFunction=
{
    fullname:"WebDE.GUI.GUIFunction",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.GUI.GUIFunction.guiFunctions = new System.Collections.Generic.List$1.ctor(WebDE.GUI.GUIFunction);
            WebDE.GUI.GUIFunction.defaultFunction = null;
        },
        GetByName:function(functionName)
        {
            var $it12=WebDE.GUI.GUIFunction.guiFunctions.GetEnumerator();
            while($it12.MoveNext())
            {
                var gf=$it12.get_Current();
                if(gf.GetName() == functionName)
                {
                    return gf;
                }
            }
            return null;
        },
        GetDefaultFunction:function()
        {
            return WebDE.GUI.GUIFunction.defaultFunction;
        },
        SetDefaultFunction:function(newFunc)
        {
            WebDE.GUI.GUIFunction.defaultFunction = newFunc;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor$$String$$InputDevice$$String$$ButtonCommand:function(name,bindingDevice,defaultButtonName,buttonCommand)
        {
            this.eventName = "";
            this.firingDelay = 250;
            this.lastFire = 0;
            this.boundButtons = new System.Collections.Generic.List$1.ctor(System.String);
            System.Object.ctor.call(this);
            this.eventName = name;
            bindingDevice.Bind$$String$$Int32$$ButtonCommand$$GUIFunction(defaultButtonName,0,buttonCommand,this);
            if(WebDE.GUI.GUIFunction.defaultFunction == null)
            {
                WebDE.GUI.GUIFunction.defaultFunction = this;
            }
            WebDE.GUI.GUIFunction.guiFunctions.Add(this);
        },
        ctor$$String$$InputDevice$$String:function(name,bindingDevice,defaultButtonName)
        {
            this.eventName = "";
            this.firingDelay = 250;
            this.lastFire = 0;
            this.boundButtons = new System.Collections.Generic.List$1.ctor(System.String);
            System.Object.ctor.call(this);
            this.eventName = name;
            bindingDevice.Bind$$String$$Int32$$ButtonCommand$$GUIFunction(defaultButtonName,0,0,this);
            if(WebDE.GUI.GUIFunction.defaultFunction == null)
            {
                WebDE.GUI.GUIFunction.defaultFunction = this;
            }
            WebDE.GUI.GUIFunction.guiFunctions.Add(this);
        },
        GetName:function()
        {
            return this.eventName;
        }
    }
};
JsTypes.push(WebDE$GUI$GUIFunction);
var WebDE$GUI$GuiLayer=
{
    fullname:"WebDE.GUI.GuiLayer",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.GUI.GuiLayer.lastid = 0;
            WebDE.GUI.GuiLayer.allTheLayers = new System.Collections.Generic.List$1.ctor(WebDE.GUI.GuiLayer);
            WebDE.GUI.GuiLayer.lastActiveLayer = null;
            WebDE.GUI.GuiLayer.defaultLayer = null;
        },
        GetLayerByName:function(layerName)
        {
            var $it19=WebDE.GUI.GuiLayer.allTheLayers.GetEnumerator();
            while($it19.MoveNext())
            {
                var currentLayer=$it19.get_Current();
                if(currentLayer.GetName() == layerName)
                {
                    return currentLayer;
                }
            }
            return null;
        },
        GetActiveLayers:function()
        {
            var resultList=new System.Collections.Generic.List$1.ctor(WebDE.GUI.GuiLayer);
            var $it20=WebDE.GUI.GuiLayer.allTheLayers.GetEnumerator();
            while($it20.MoveNext())
            {
                var layer=$it20.get_Current();
                if(layer.IsActive())
                {
                    resultList.Add(layer);
                }
            }
            return resultList;
        },
        AsCollisionMap:function(sourceStage)
        {
            var collisionLayer=WebDE.Rendering.View.GetMainView().AddLayer("CollisionLayer",WebDE.Rendering.DOM_Renderer.GetRenderer().BoardArea());
            var tileSize=WebDE.GameObjects.Stage.CurrentStage.GetTileSize();
            var $it21=sourceStage.GetVisibleTiles(collisionLayer.GetAttachedView()).GetEnumerator();
            while($it21.MoveNext())
            {
                var tile=$it21.get_Current();
                var gelm=collisionLayer.AddGUIElement("");
                gelm.SetPosition(WebDE.GameObjects.Helpah.Round$$Double(tile.GetPosition().x * tileSize.get_Item1()),WebDE.GameObjects.Helpah.Round$$Double(tile.GetPosition().y * tileSize.get_Item2()));
                gelm.AddStyle("collisionBlock");
                if(tile.GetBuildable())
                {
                    gelm.AddStyle("buildable");
                }
            }
            return collisionLayer;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(layerName,attachingView,rectLayerPos)
        {
            this.guiElements = new System.Collections.Generic.List$1.ctor(WebDE.GUI.GuiElement);
            this.selectedElements = new System.Collections.Generic.List$1.ctor(WebDE.GUI.GuiElement);
            this.active = true;
            this.attachedView = null;
            this.area = new WebDE.Rectangle.ctor(0,0,0,0);
            this.isGameLayer = false;
            this.focusedElement = null;
            this.visible = true;
            this.name = null;
            this.id = null;
            this.followingCursor = false;
            this.layerFunctions = new System.Collections.Generic.Dictionary$2.ctor(WebDE.GUI.GUIFunction,System.Action$1);
            System.Object.ctor.call(this);
            WebDE.GUI.GuiLayer.lastid++;
            this.id = "GUILayer" + WebDE.GUI.GuiLayer.lastid;
            this.name = layerName;
            this.attachedView = attachingView;
            this.SetArea(rectLayerPos);
            WebDE.GUI.GuiLayer.allTheLayers.Add(this);
        },
        GetId:function()
        {
            return this.id;
        },
        GetName:function()
        {
            return this.name;
        },
        SetName:function(newName)
        {
            this.name = newName;
        },
        Activate:function()
        {
            this.active = true;
        },
        Deactivate:function()
        {
            this.active = false;
            WebDE.GUI.GuiLayer.lastActiveLayer = this;
        },
        IsActive:function()
        {
            return this.active;
        },
        GetArea:function()
        {
            return this.area;
        },
        GetPosition:function()
        {
            var returnVal=new WebDE.Point.ctor(this.area.x,this.area.y);
            return returnVal;
        },
        SetPosition:function(newX,newY)
        {
            if(newX < 0)
            {
                newX = this.GetAttachedView().GetSize().get_Item1() - System.Math.Abs$$Double(newX) - this.GetSize().get_Item1();
            }
            if(newY < 0)
            {
                newY = this.GetAttachedView().GetSize().get_Item2() - System.Math.Abs$$Double(newY) - this.GetSize().get_Item2();
            }
            this.area.x = newX;
            this.area.y = newY;
        },
        GetSize:function()
        {
            return new System.Tuple$2.ctor(System.Int32,System.Int32,Math.round(this.area.width),Math.round(this.area.height));
        },
        SetSize:function(newWidth,newHeight)
        {
            this.area.width = newWidth;
            this.area.height = newHeight;
        },
        SetArea:function(newArea)
        {
            this.SetSize(newArea.width,newArea.height);
            this.SetPosition(newArea.x,newArea.y);
        },
        AddGUIElement:function(text)
        {
            var newElement=new WebDE.GUI.GuiElement.ctor(this,text);
            this.guiElements.Add(newElement);
            if(this.focusedElement == null)
            {
                this.focusedElement = newElement;
            }
            return newElement;
        },
        GetGuiElements:function()
        {
            return this.guiElements;
        },
        PreviousItem:function()
        {
            var focusedInex=this.guiElements.IndexOf$$T(this.focusedElement);
            if(focusedInex <= 0)
            {
                focusedInex = this.guiElements.get_Count() - 1;
            }
            this.focusedElement = this.guiElements.get_Item$$Int32(focusedInex - 1);
        },
        NextItem:function()
        {
            var focusedInex=this.guiElements.IndexOf$$T(this.focusedElement);
            if(focusedInex >= this.guiElements.get_Count() - 1)
            {
                focusedInex = -1;
            }
            this.focusedElement = this.guiElements.get_Item$$Int32(focusedInex + 1);
        },
        GetSelectedItems:function()
        {
            var elementList=this.GetGuiElements();
            var $it13=elementList.GetEnumerator();
            while($it13.MoveNext())
            {
                var gelm=$it13.get_Current();
                if(gelm.Selected() == false)
                {
                    elementList.Remove(gelm);
                }
            }
            return elementList;
        },
        SelectItem:function(itemName,itemIndex)
        {
            var $it14=this.GetSelectedItems().GetEnumerator();
            while($it14.MoveNext())
            {
                var gelm=$it14.get_Current();
                gelm.Select(false);
            }
            var elementToSelect=null;
            if(itemName != "" && itemName != null)
            {
                var elementList=this.GetGuiElements();
                var $it15=elementList.GetEnumerator();
                while($it15.MoveNext())
                {
                    var gelm=$it15.get_Current();
                    if(gelm.GetText() == itemName)
                    {
                        elementToSelect = gelm;
                        break;
                    }
                }
            }
            else
            {
                elementToSelect = this.GetGuiElements().get_Item$$Int32(itemIndex);
            }
            if(elementToSelect != null)
            {
                elementToSelect.Select(true);
            }
        },
        GetAttachedView:function()
        {
            return this.attachedView;
        },
        GetElementAt:function(xpos,ypos)
        {
            if(xpos > this.GetPosition().x)
            {
                xpos -= this.GetPosition().x;
                ypos -= this.GetPosition().y;
            }
            if(this.IsGameLayer() == true)
            {
                var tileList=this.GetAttachedView().GetAttachedStage().GetVisibleTiles(this.GetAttachedView());
            }
            else
            {
                var $it16=this.guiElements.GetEnumerator();
                while($it16.MoveNext())
                {
                    var gelm=$it16.get_Current();
                    if((gelm.GetPosition().x <= xpos && gelm.GetPosition().x + gelm.GetSize().get_Item1() >= xpos) && (gelm.GetPosition().y <= ypos && gelm.GetPosition().y + gelm.GetSize().get_Item2() >= ypos))
                    {
                        return gelm;
                    }
                }
            }
            return null;
        },
        GetEntitiesAt:function(xpos,ypos)
        {
            var returnList=new System.Collections.Generic.List$1.ctor(WebDE.GameObjects.GameEntity);
            var GameEntityList=this.GetAttachedView().GetAttachedStage().GetVisibleEntities(this.GetAttachedView());
            var $it17=GameEntityList.GetEnumerator();
            while($it17.MoveNext())
            {
                var ent=$it17.get_Current();
                if((ent.GetPosition().x <= xpos && ent.GetPosition().x + ent.GetSize().get_Item1() >= xpos) && (ent.GetPosition().y <= ypos && ent.GetPosition().y + ent.GetSize().get_Item2() >= ypos))
                {
                    returnList.Add(ent);
                }
            }
            return returnList;
        },
        GetTilesAt:function(xpos,ypos)
        {
            var tileSize=WebDE.GameObjects.Stage.CurrentStage.GetTileSize();
            xpos = xpos / tileSize.get_Item1();
            ypos = ypos / tileSize.get_Item2();
            var returnList=new System.Collections.Generic.List$1.ctor(WebDE.GameObjects.Tile);
            var tileList=this.GetAttachedView().GetAttachedStage().GetVisibleTiles(this.GetAttachedView());
            xpos = WebDE.GameObjects.Helpah.Round$$Double(xpos);
            ypos = WebDE.GameObjects.Helpah.Round$$Double(ypos);
            var $it18=tileList.GetEnumerator();
            while($it18.MoveNext())
            {
                var ent=$it18.get_Current();
                if(ent.GetPosition().x == xpos && ent.GetPosition().y == ypos)
                {
                    var til=Cast(ent,WebDE.GameObjects.Tile);
                    returnList.Add(til);
                }
            }
            return returnList;
        },
        SetGameLayer:function(newTruth_nullable)
        {
            if(newTruth_nullable == null)
            {
                this.isGameLayer = !this.isGameLayer;
            }
            else
            {
                this.isGameLayer = newTruth_nullable;
            }
        },
        IsGameLayer:function()
        {
            return this.isGameLayer;
        },
        FollowingCursor:function()
        {
            return this.followingCursor;
        },
        FollowCursor:function(toFollow)
        {
            this.followingCursor = toFollow;
        },
        GUI_Event:function(buttonFunction,eventPos)
        {
            this.DoGUIFunction(buttonFunction,eventPos);
            var elementToNotify;
            if(eventPos == null)
            {
                if(this.focusedElement == null)
                {
                    return;
                }
                elementToNotify = this.focusedElement;
            }
            else
            {
                elementToNotify = this.GetElementAt(eventPos.x,eventPos.y);
            }
            if(elementToNotify == null)
            {
                return;
            }
            elementToNotify.DoGUIFunction(buttonFunction);
        },
        SetGUIFunction:function(func,newEvent)
        {
            this.layerFunctions.set_Item$$TKey(func,newEvent);
        },
        DoGUIFunction:function(func,eventPos)
        {
            eventPos.x -= this.GetPosition().x;
            eventPos.y -= this.GetPosition().y;
            if(this.layerFunctions.get_Item$$TKey(func) != null)
            {
                var eventToTrigger=WebDE.GUI.GuiEvent.FromClickData(this,eventPos);
                this.layerFunctions.get_Item$$TKey(func)(eventToTrigger);
            }
            else
            {
            }
        },
        Show:function()
        {
            this.visible = true;
            this.SetNeedsUpdate();
        },
        Hide:function()
        {
            this.visible = false;
            this.SetNeedsUpdate();
        },
        Visible:function()
        {
            return this.visible;
        },
        SetNeedsUpdate:function()
        {
            WebDE.Rendering.DOM_Renderer.GetRenderer().SetNeedsUpdate$$GuiLayer(this);
        }
    }
};
JsTypes.push(WebDE$GUI$GuiLayer);
var WebDE$GUI$Hint=
{
    fullname:"WebDE.GUI.Hint",
    baseTypeName:"WebDE.GUI.GuiElement",
    staticDefinition:
    {
        cctor:function()
        {
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(owningLayer,elementText)
        {
            this.title = null;
            this.hasAction = false;
            WebDE.GUI.GuiElement.ctor.call(this,owningLayer,elementText);
        }
    }
};
JsTypes.push(WebDE$GUI$Hint);
