/*Generated by SharpKit 5 v4.27.4000*/
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var WebDE$AI$ArtificialIntelligence=
{
    fullname:"WebDE.AI.ArtificialIntelligence",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            this.body = null;
            this.currentPath = null;
            this.currentNode = 0;
            this.oldSpeed = null;
            this.aiDebug = null;
            System.Object.ctor.call(this);
        },
        GetMovementPath:function()
        {
            return this.currentPath;
        },
        SetMovementPath:function(newPath)
        {
            this.currentPath = newPath;
        },
        GetBody:function()
        {
            return this.body;
        },
        SetBody:function(newBody)
        {
            this.body = newBody;
        },
        Think:function()
        {
            this.ThinkAboutDoingViolence();
            this.ThinkAboutWhereToGo();
        },
        ThinkAboutWhereToGo:function()
        {
            this.body.SetDirection(0);
            if(this.oldSpeed != null)
            {
                this.body.SetSpeed(this.oldSpeed);
                this.oldSpeed = null;
            }
            if(this.currentPath != null)
            {
                var curNode=this.currentPath.GetNode(this.currentNode);
                if(curNode == null)
                {
                    this.currentPath = null;
                    this.body.SetSpeed(new WebDE.Vector.ctor(0,0));
                }
                else
                {
                    var hOffset=this.body.GetPosition().x - curNode.x;
                    var vOffset=this.body.GetPosition().y - curNode.y;
                    WebDE.Debug.Watch("Current Position",WebDE.Objects.Helpah.Round(this.body.GetPosition().x,2) + "," + WebDE.Objects.Helpah.Round(this.body.GetPosition().y,2));
                    WebDE.Debug.Watch("Desired Position",WebDE.Objects.Helpah.Round(curNode.x,2) + "," + WebDE.Objects.Helpah.Round(curNode.y,2));
                    WebDE.Debug.Watch("hOffset",WebDE.Objects.Helpah.Round(hOffset,2).toString());
                    WebDE.Debug.Watch("vOffset",WebDE.Objects.Helpah.Round(vOffset,2).toString());
                    WebDE.Debug.Watch("xSpeed",WebDE.Objects.Helpah.Round(this.body.GetSpeed().x,2).toString());
                    WebDE.Debug.Watch("ySpeed",WebDE.Objects.Helpah.Round(this.body.GetSpeed().y,2).toString());
                    hOffset = System.Math.Round$$Double(hOffset);
                    vOffset = System.Math.Round$$Double(vOffset);
                    if(System.Math.Abs$$Double(hOffset) > System.Math.Abs$$Double(vOffset))
                    {
                        if(hOffset > 0)
                        {
                            this.body.SetDirection(-1);
                        }
                        else
                        {
                            this.body.SetDirection(1);
                        }
                    }
                    else
                    {
                        if(vOffset > 0)
                        {
                            this.body.SetDirection(-2);
                        }
                        else
                        {
                            this.body.SetDirection(2);
                        }
                    }
                    if(hOffset != 0 && System.Math.Abs$$Double(hOffset) <= this.body.GetSpeed().x / this.body.GetAcceleration())
                    {
                        this.body.SetDirection(0);
                        if(System.Math.Abs$$Double(hOffset) < this.body.GetAcceleration())
                        {
                            this.body.SetSpeed(new WebDE.Vector.ctor(hOffset,this.body.GetSpeed().y));
                        }
                    }
                    if(vOffset != 0 && System.Math.Abs$$Double(vOffset) <= this.body.GetSpeed().y / this.body.GetAcceleration())
                    {
                        this.body.SetDirection(0);
                        if(System.Math.Abs$$Double(vOffset) < this.body.GetAcceleration())
                        {
                            this.body.SetSpeed(new WebDE.Vector.ctor(this.body.GetSpeed().x,vOffset));
                        }
                    }
                    if(System.Math.Round$$Double(hOffset) == 0 && System.Math.Round$$Double(vOffset) == 0)
                    {
                        this.body.SetDirection(0);
                        this.currentNode++;
                    }
                    WebDE.Debug.Watch("Desired direction:",this.body.GetDirection().toString());
                }
            }
        },
        ThinkAboutDoingViolence:function()
        {
            try
            {
                var listoguns=this.GetBody().GetWeapons();
                var i=0;
                while(listoguns.get_Count() > i)
                {
                    var theWeapon=listoguns.get_Item$$Int32(i);
                    if(theWeapon.GetTarget() != null && this.body.GetPosition().Distance(theWeapon.GetTarget().GetPosition()) < theWeapon.GetRange())
                    {
                        theWeapon.Fire();
                    }
                    else
                    {
                        var $it1=WebDE.Objects.Stage.CurrentStage.GetVisibleEntities(WebDE.Rendering.View.GetMainView()).GetEnumerator();
                        while($it1.MoveNext())
                        {
                            var ent=$it1.get_Current();
                            if(ent.GetPosition().Distance(this.body.GetPosition()) < theWeapon.GetRange())
                            {
                                theWeapon.SetTarget(ent);
                                theWeapon.Fire();
                                break;
                            }
                        }
                    }
                    i++;
                }
            }
            catch(ex)
            {
            }
        },
        updateDebug:function(newMsg)
        {
            if(this.aiDebug == null)
            {
                this.aiDebug = WebDE.Debug.Watch("hOffset","World");
            }
            this.aiDebug.UpdateValue(newMsg);
        }
    }
};
JsTypes.push(WebDE$AI$ArtificialIntelligence);
var WebDE$AI$MovementPath=
{
    fullname:"WebDE.AI.MovementPath",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        ResolvePath:function(start,end)
        {
            var newPath=new WebDE.AI.MovementPath.ctor(null);
            newPath.AddPoint(start);
            if(start.x != end.x)
            {
                var newPoint=new WebDE.Point.ctor(end.x,start.y);
                newPath.AddPoint(newPoint);
            }
            if(start.y != end.y)
            {
                newPath.AddPoint(end);
            }
            return newPath;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(movementNodes_nullable)
        {
            this.nodes = new System.Collections.Generic.List$1.ctor(WebDE.Point);
            this.looping = false;
            System.Object.ctor.call(this);
            if(movementNodes_nullable != null)
            {
                this.nodes = movementNodes_nullable;
            }
        },
        GetNode:function(nodeIndex)
        {
            return this.nodes.get_Item$$Int32(nodeIndex);
        },
        GetNextNode:function(nodeIndex)
        {
            return this.nodes.get_Item$$Int32(nodeIndex + 1);
        },
        AddPoint:function(pointToAdd)
        {
            this.nodes.Add(pointToAdd);
        },
        GetLooping:function()
        {
            return this.looping;
        },
        SetLooping:function(doesLoop)
        {
            this.looping = doesLoop;
        }
    }
};
JsTypes.push(WebDE$AI$MovementPath);
var WebDE$AI$Objective=
{
    fullname:"WebDE.AI.Objective",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            this.target = null;
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(WebDE$AI$Objective);
