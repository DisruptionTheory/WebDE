/*Generated by SharpKit 5 v4.27.4000*/
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
var WebDE$Objects$GameEntity=
{
    fullname:"WebDE.Objects.GameEntity",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Objects.GameEntity.cachedEntities = new System.Collections.Generic.List$1.ctor(WebDE.Objects.GameEntity);
            WebDE.Objects.GameEntity.lastid = 1;
        },
        GetById:function(id)
        {
            var $it25=WebDE.Objects.GameEntity.cachedEntities.GetEnumerator();
            while($it25.MoveNext())
            {
                var ent=$it25.get_Current();
                if(ent.id == id)
                {
                    return ent;
                }
            }
            return null;
        },
        isGameEntityLoaded:function()
        {
            return null;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(entName)
        {
            this.id = null;
            this.width = 40;
            this.height = 40;
            this.strGameEntityName = "New GameEntity";
            this.sprGameEntitySprite = null;
            this.parentStage = null;
            this.position = new WebDE.Point.ctor(0,0);
            this.speed = new WebDE.Vector.ctor(0,0);
            this.minSpeed = new WebDE.Vector.ctor(-10,-10);
            this.maxSpeed = new WebDE.Vector.ctor(10,10);
            this.acceleration = 1;
            this.movementAngle = -1;
            this.desiredDirection = 0;
            System.Object.ctor.call(this);
            WebDE.Objects.GameEntity.lastid++;
            this.id = "GameEntity_" + WebDE.Objects.GameEntity.lastid;
            this.strGameEntityName = entName;
            WebDE.Objects.GameEntity.cachedEntities.Add(this);
            var nameSprite=WebDE.Animation.Sprite.GetSpriteByName(this.strGameEntityName);
            this.SetSprite(nameSprite);
        },
        GetId:function()
        {
            return this.id;
        },
        GetName:function()
        {
            return this.strGameEntityName;
        },
        GetPosition:function()
        {
            return this.position;
        },
        SetPosition:function(newX,newY)
        {
            this.position.x = newX;
            this.position.y = newY;
        },
        GetSprite:function()
        {
            return this.sprGameEntitySprite;
        },
        SetSprite:function(newSprite)
        {
            try
            {
                this.sprGameEntitySprite = Cast(WebDE.Objects.Helpah.Clone(newSprite),WebDE.Animation.Sprite);
                return true;
            }
            catch(ex)
            {
                WebDE.Debug.log("Exception with SetSprite in GameEntity: " + ex.get_Message());
                return false;
            }
        },
        GetSize:function()
        {
            var returnVal=new System.Tuple$2.ctor(System.Int32,System.Int32,this.width,this.height);
            return returnVal;
        },
        SetSize:function(newWidth,newHeight)
        {
            this.width = newWidth;
            this.height = newHeight;
        },
        GetMovementAngle:function()
        {
            return this.movementAngle;
        },
        SetMovementAngle:function(newAngle)
        {
            this.movementAngle = newAngle;
        },
        GetDirection:function()
        {
            return this.desiredDirection;
        },
        SetDirection:function(newDirection)
        {
            this.desiredDirection = newDirection;
        },
        GetSpeed:function()
        {
            return this.speed;
        },
        SetSpeed:function(newSpeed)
        {
            this.speed = newSpeed;
        },
        AddSpeed:function(newSpeed)
        {
            this.speed.x += newSpeed.x;
            this.speed.y += newSpeed.y;
        },
        GetAcceleration:function()
        {
            return this.acceleration;
        },
        SetAcceleration:function(newAccel)
        {
            this.acceleration = newAccel;
        },
        GetParentStage:function()
        {
            return this.parentStage;
        },
        SetParentStage:function(newStage)
        {
            this.parentStage = newStage;
        },
        Destroy:function()
        {
            WebDE.Objects.Stage.CurrentStage.RemoveGameEntity(this);
            WebDE.Objects.Helpah.Destroy(this);
        },
        CalculateSpeed:function()
        {
            if(Is(this,WebDE.Objects.GameEntitySpawner))
            {
                return;
            }
            if(this.desiredDirection == -1)
            {
                this.speed.x -= this.acceleration;
            }
            else if(this.desiredDirection == 1)
            {
                this.speed.x += this.acceleration;
            }
            else if(this.desiredDirection == -2)
            {
                this.speed.y -= this.acceleration;
            }
            else if(this.desiredDirection == 2)
            {
                this.speed.y += this.acceleration;
            }
            else if(this.desiredDirection == 0)
            {
                if(this.speed.x > 0)
                {
                    this.speed.x -= this.acceleration;
                }
                else if(this.speed.x < 0)
                {
                    this.speed.x += this.acceleration;
                }
                if(this.speed.y > 0)
                {
                    this.speed.y -= this.acceleration;
                }
                else if(this.speed.y < 0)
                {
                    this.speed.y += this.acceleration;
                }
            }
            if(this.speed.x > this.maxSpeed.x)
            {
                this.speed.x = this.maxSpeed.x;
            }
            if(this.speed.x < this.minSpeed.x)
            {
                this.speed.x = this.minSpeed.x;
            }
            if(this.speed.y > this.maxSpeed.y)
            {
                this.speed.y = this.maxSpeed.y;
            }
            if(this.speed.y < this.minSpeed.y)
            {
                this.speed.y = this.minSpeed.y;
            }
        },
        CalculatePosition:function()
        {
            this.position.x += this.speed.x;
            this.position.y += this.speed.y;
        },
        CheckCollision:function()
        {
        }
    }
};
JsTypes.push(WebDE$Objects$GameEntity);
var WebDE$Objects$GameEntityBatch=
{
    fullname:"WebDE.Objects.GameEntityBatch",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(GameEntityType,GameEntityCount,spawnDelay)
        {
            this.GameEntityType = null;
            this.GameEntityCount = 0;
            this.spawnDelay = 0;
            System.Object.ctor.call(this);
            this.GameEntityType = GameEntityType;
            this.GameEntityCount = GameEntityCount;
            this.spawnDelay = spawnDelay;
        }
    }
};
JsTypes.push(WebDE$Objects$GameEntityBatch);
var WebDE$Objects$GameEntitySpawner=
{
    fullname:"WebDE.Objects.GameEntitySpawner",
    baseTypeName:"WebDE.Objects.GameEntity",
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
        ctor:function(itemName,initialDelay)
        {
            this.spawnBatches = new System.Collections.Generic.List$1.ctor(WebDE.Objects.GameEntityBatch);
            this.currentSpawnDelay = 0;
            this.clockId = null;
            WebDE.Objects.GameEntity.ctor.call(this,itemName);
            this.currentSpawnDelay = initialDelay;
            WebDE.Timekeeper.Clock.IntervalExecute($CreateDelegate(this,this.Think),1);
        },
        AddGameEntityBatch:function(GameEntityType,GameEntityCount,spawnDelay)
        {
            var newBatch=new WebDE.Objects.GameEntityBatch.ctor(GameEntityType,GameEntityCount,spawnDelay);
            this.spawnBatches.Add(newBatch);
        },
        AddGameEntityBatches:function(GameEntityType,GameEntityCount,batchCount,spawnDelay)
        {
            while(batchCount > 0)
            {
                this.AddGameEntityBatch(GameEntityType,GameEntityCount,spawnDelay);
                batchCount--;
            }
        },
        Activate:function()
        {
            this.clockId = WebDE.Timekeeper.Clock.AddCalculation($CreateDelegate(this,this.Think));
        },
        Deactivate:function()
        {
            if(this.clockId != null)
            {
                WebDE.Timekeeper.Clock.RemoveCalculation(this.clockId);
            }
        },
        Think:function()
        {
            if(this.spawnBatches.get_Count() > 0)
            {
                if(this.currentSpawnDelay > 0)
                {
                    this.currentSpawnDelay--;
                    return;
                }
                var batchToSpawn=this.spawnBatches.get_Item$$Int32(0);
                var entitiesToSpawn=this.spawnBatches.get_Item$$Int32(0).GameEntityCount;
                while(entitiesToSpawn > 0)
                {
                    var entSrc=Cast(this.spawnBatches.get_Item$$Int32(0).GameEntityType,WebDE.Objects.LivingGameEntity);
                    var newEnt=Cast(WebDE.Objects.Helpah.Clone(this.spawnBatches.get_Item$$Int32(0).GameEntityType),WebDE.Objects.LivingGameEntity);
                    newEnt.SetPosition(this.GetPosition().x,this.GetPosition().y);
                    newEnt.SetAI(entSrc.GetAI());
                    newEnt.SetParentStage(this.GetParentStage());
                    this.GetParentStage().AddLivingGameEntity(newEnt);
                    entitiesToSpawn--;
                }
                this.currentSpawnDelay = this.spawnBatches.get_Item$$Int32(0).spawnDelay;
                this.spawnBatches.RemoveAt(0);
            }
            else
            {
                this.Deactivate();
            }
        }
    }
};
JsTypes.push(WebDE$Objects$GameEntitySpawner);
var WebDE$Objects$Helpah=
{
    fullname:"WebDE.Objects.Helpah",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        Clone:function(o)
        {
            if(o == null)
            {
                return null;
            }
            return $.extend(true,new System.Object.ctor(),o);
        },
        Destroy:function(o)
        {
            o = null;
        },
        Round:function(number,decimalPlaces)
        {
            var numString=number.toString();
            if(numString.indexOf(".") > -1)
            {
                var numString1=numString.substr(0,numString.indexOf("."));
                var numString2=numString.substr(numString.indexOf("."),decimalPlaces + 1);
                numString = numString1 + numString2;
            }
            return System.Double.Parse$$String(numString);
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
JsTypes.push(WebDE$Objects$Helpah);
var WebDE$Objects$LightSource=
{
    fullname:"WebDE.Objects.LightSource",
    baseTypeName:"WebDE.Objects.GameEntity",
    staticDefinition:
    {
        cctor:function()
        {
        },
        GetLocalLightSources:function(xPos,yPos)
        {
            var returnLights=new System.Collections.Generic.List$1.ctor(WebDE.Objects.LightSource);
            var $it26=WebDE.Objects.Stage.CurrentStage.GetLights().GetEnumerator();
            while($it26.MoveNext())
            {
                var light=$it26.get_Current();
                if(light.GetPosition().Distance(new WebDE.Point.ctor(xPos,yPos)) <= light.range)
                {
                    returnLights.Add(light);
                }
            }
            return returnLights;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(x,y,lightness,distance)
        {
            this.luminosity = 1;
            this.range = 3;
            this.color = new WebDE.Color.ctor(255,255,255);
            this.needsRenderUpdate = false;
            this.diminishing = false;
            WebDE.Objects.GameEntity.ctor.call(this,"");
            this.SetPosition(x,y);
            this.luminosity = lightness;
            this.range = distance;
            WebDE.Objects.Stage.CurrentStage.AddLight(this);
            if(WebDE.Rendering.View.GetMainView() == null)
            {
                this.CalculateIllumination();
            }
        },
        GetLuminosity:function()
        {
            return this.luminosity;
        },
        SetLuminosity:function(newVal)
        {
            this.luminosity = newVal;
        },
        GetRange:function()
        {
            return this.range;
        },
        SetRange:function(newRange)
        {
            this.range = newRange;
        },
        GetColor:function()
        {
            return this.color;
        },
        SetColor:function(newColor)
        {
            this.color = newColor;
            this.needsRenderUpdate = true;
        },
        SetDiminishing:function(isDiminishing)
        {
            this.diminishing = isDiminishing;
        },
        GetDiminishing:function()
        {
            return this.diminishing;
        },
        CalculateIllumination:function()
        {
            var localTiles=WebDE.Objects.Stage.CurrentStage.GetVisibleTiles(WebDE.Rendering.View.GetMainView());
            var $it27=localTiles.GetEnumerator();
            while($it27.MoveNext())
            {
                var localTile=$it27.get_Current();
                if(localTile.GetPosition().Distance(this.GetPosition()) <= this.range)
                {
                }
            }
        },
        Think:function()
        {
            if(this.GetDiminishing() == true)
            {
                this.SetRange(this.GetRange() - 0.1);
                if(this.GetRange() < 0.1)
                {
                    this.Destroy();
                }
                this.needsRenderUpdate = true;
            }
        }
    }
};
JsTypes.push(WebDE$Objects$LightSource);
var WebDE$Objects$LivingGameEntity=
{
    fullname:"WebDE.Objects.LivingGameEntity",
    baseTypeName:"WebDE.Objects.GameEntity",
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
        ctor:function(entName,health)
        {
            this.health = 0;
            this.ai = null;
            this.weapons = null;
            WebDE.Objects.GameEntity.ctor.call(this,entName);
            this.health = health;
            this.ai = new WebDE.AI.ArtificialIntelligence.ctor();
            this.weapons = new System.Collections.Generic.List$1.ctor(WebDE.Objects.Weapon);
        },
        GetAI:function()
        {
            return this.ai;
        },
        SetAI:function(newAi)
        {
            this.ai = Cast(WebDE.Objects.Helpah.Clone(newAi),WebDE.AI.ArtificialIntelligence);
            this.ai.SetBody(this);
        },
        Think:function()
        {
            if(this.ai != null)
            {
                this.ai.SetBody(this);
                this.ai.Think();
            }
        },
        Damage:function(Amount)
        {
        },
        Kill:function()
        {
        },
        AddWeapon:function(weaponToAdd)
        {
            weaponToAdd = Cast(WebDE.Objects.Helpah.Clone(weaponToAdd),WebDE.Objects.Weapon);
            this.weapons.Add(weaponToAdd);
        },
        GetWeapons:function()
        {
            return this.weapons;
        },
        SetHealth:function(newHealth)
        {
            this.health = newHealth;
        },
        GetHealth:function()
        {
            return this.health;
        }
    }
};
JsTypes.push(WebDE$Objects$LivingGameEntity);
var WebDE$Objects$Projectile=
{
    fullname:"WebDE.Objects.Projectile",
    baseTypeName:"WebDE.Objects.LivingGameEntity",
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
        ctor:function(projectileName,targetPoint)
        {
            this.damage = 0;
            this.impactEvent = null;
            this.targetPoint = null;
            WebDE.Objects.LivingGameEntity.ctor.call(this,projectileName,10);
            this.targetPoint = targetPoint;
            var ai=new WebDE.AI.ArtificialIntelligence.ctor();
        },
        SetPosition:function(newX,newY)
        {
            WebDE.Objects.GameEntity.commonPrototype.SetPosition.call(this,newX,newY);
            var newPath=new WebDE.AI.MovementPath.ctor((function()
            {
                var $v1=new System.Collections.Generic.List$1.ctor(WebDE.Point);
                $v1.Add(this.GetPosition());
                $v1.Add(this.targetPoint);
                return $v1;
            }).call(this));
            var newAi=new WebDE.AI.ArtificialIntelligence.ctor();
            newAi.SetMovementPath(newPath);
            this.SetAI(newAi);
        },
        SetDamage:function(newDamage)
        {
            this.damage = newDamage;
        },
        GetDamage:function()
        {
            return this.damage;
        },
        Collision:function()
        {
            if(this.impactEvent != null)
            {
                this.impactEvent();
            }
        }
    }
};
JsTypes.push(WebDE$Objects$Projectile);
var WebDE$Objects$Resource=
{
    fullname:"WebDE.Objects.Resource",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Objects.Resource.gameResources = new System.Collections.Generic.List$1.ctor(WebDE.Objects.Resource);
        },
        GetIdResourceByName:function(resourceName)
        {
            var $it28=WebDE.Objects.Resource.gameResources.GetEnumerator();
            while($it28.MoveNext())
            {
                var res=$it28.get_Current();
                if(res.name == resourceName)
                {
                    return res.id;
                }
            }
            return 0;
        },
        ByName:function(resourceName)
        {
            var $it29=WebDE.Objects.Resource.gameResources.GetEnumerator();
            while($it29.MoveNext())
            {
                var res=$it29.get_Current();
                if(res.name == resourceName)
                {
                    return res;
                }
            }
            return null;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(resourceName)
        {
            this.id = 0;
            this.name = null;
            this.amount = 0;
            System.Object.ctor.call(this);
            this.id = WebDE.Objects.Resource.gameResources.get_Count() + 1;
            this.name = resourceName;
            this.amount = 0;
        },
        SetName:function(newName)
        {
        },
        SetAmount:function(newAmount)
        {
        }
    }
};
JsTypes.push(WebDE$Objects$Resource);
var WebDE$Objects$Stage=
{
    fullname:"WebDE.Objects.Stage",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Objects.Stage.CurrentStage = null;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(stageName,stageType)
        {
            this.strStageName = "New Stage";
            this.stageEntities = new System.Collections.Generic.List$1.ctor(WebDE.Objects.GameEntity);
            this.livingEntities = new System.Collections.Generic.List$1.ctor(WebDE.Objects.LivingGameEntity);
            this.stageTiles = new System.Collections.Generic.List$1.ctor(WebDE.Objects.Tile);
            this.stageLights = new System.Collections.Generic.List$1.ctor(WebDE.Objects.LightSource);
            this.stageAreas = null;
            this.collisionMap = null;
            this.stageGui = null;
            this.width = 20;
            this.height = 20;
            this.tileWidth = 40;
            this.tileHeight = 40;
            System.Object.ctor.call(this);
            this.SetName(stageName);
            if(WebDE.Objects.Stage.CurrentStage == null)
            {
                WebDE.Objects.Stage.CurrentStage = this;
            }
            var viewRect=WebDE.Rendering.View.GetMainView().GetViewArea();
        },
        Load:function()
        {
            var $it30=this.stageEntities.GetEnumerator();
            while($it30.MoveNext())
            {
                var ent=$it30.get_Current();
            }
        },
        getName:function()
        {
            return this.strStageName;
        },
        SetName:function(newName)
        {
            this.strStageName = newName;
        },
        AddGameEntity:function(GameEntityToAdd)
        {
            if(Is(GameEntityToAdd,WebDE.Objects.LivingGameEntity) || Is(GameEntityToAdd,WebDE.Objects.Tile))
            {
                WebDE.Debug.log("You\'re trying to add a living GameEntity or tile as a regular GameEntity.");
            }
            this.stageEntities.Add(GameEntityToAdd);
            GameEntityToAdd.SetParentStage(this);
        },
        RemoveGameEntity:function(GameEntityToRemove)
        {
            if(Is(GameEntityToRemove,WebDE.Objects.LightSource))
            {
                var lightToRemove=Cast(GameEntityToRemove,WebDE.Objects.LightSource);
                if(this.stageLights.Contains(lightToRemove))
                {
                    this.stageLights.Remove(lightToRemove);
                }
            }
            else if(Is(GameEntityToRemove,WebDE.Objects.Tile))
            {
                var tileToRemove=Cast(GameEntityToRemove,WebDE.Objects.Tile);
                if(this.stageTiles.Contains(tileToRemove))
                {
                    this.stageTiles.Remove(tileToRemove);
                }
            }
            else
            {
                if(this.stageEntities.Contains(GameEntityToRemove))
                {
                    this.stageEntities.Remove(GameEntityToRemove);
                }
            }
        },
        AddTile:function(name,walkable,buildable)
        {
            var newTile=new WebDE.Objects.Tile.ctor(name,walkable,buildable);
            this.stageTiles.Add(newTile);
            return newTile;
        },
        AppendTile:function(tileToAdd)
        {
            this.stageTiles.Add(tileToAdd);
        },
        AddLivingGameEntity:function(toAdd)
        {
            this.livingEntities.Add(toAdd);
        },
        GetVisibleEntities:function(viewer)
        {
            var resultList=new System.Collections.Generic.List$1.ctor(WebDE.Objects.GameEntity);
            var $it31=this.stageEntities.GetEnumerator();
            while($it31.MoveNext())
            {
                var ent=$it31.get_Current();
                resultList.Add(ent);
            }
            var $it32=this.livingEntities.GetEnumerator();
            while($it32.MoveNext())
            {
                var lent=$it32.get_Current();
                resultList.Add(lent);
            }
            return resultList;
        },
        GetVisibleTiles:function(viewer)
        {
            var resultList=new System.Collections.Generic.List$1.ctor(WebDE.Objects.GameEntity);
            var $it33=this.stageTiles.GetEnumerator();
            while($it33.MoveNext())
            {
                var tile=$it33.get_Current();
                resultList.Add(tile);
            }
            return resultList;
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
        CreateRandomTiles:function()
        {
            for(var h=0;h < this.height;h++)
            {
                for(var w=0;w < this.width;w++)
                {
                    var rand=(new System.Random.ctor()).Next$$Int32$$Int32(0,1);
                    var buildable=false;
                    if(rand == 1)
                    {
                        buildable = true;
                    }
                    var aTile=new WebDE.Objects.Tile.ctor("",true,buildable);
                    aTile.SetParentStage(this);
                    aTile.SetPosition(w,h);
                    this.stageTiles.Add(aTile);
                }
            }
        },
        CalculateEntities:function()
        {
            var $it34=this.livingEntities.GetEnumerator();
            while($it34.MoveNext())
            {
                var lent=$it34.get_Current();
                lent.Think();
            }
            var $it35=this.stageEntities.GetEnumerator();
            while($it35.MoveNext())
            {
                var ent=$it35.get_Current();
                try
                {
                }
                catch($$e1)
                {
                }
            }
            var $it36=this.livingEntities.GetEnumerator();
            while($it36.MoveNext())
            {
                var ent=$it36.get_Current();
                if(Is(ent,WebDE.Objects.Projectile))
                {
                    if(!this.GetBounds().Contains(ent.GetPosition()))
                    {
                        WebDE.Debug.log("killing an GameEntity");
                        ent.Destroy();
                    }
                }
            }
        },
        CalculateGameEntityPhysics:function()
        {
            var $it37=this.stageEntities.GetEnumerator();
            while($it37.MoveNext())
            {
                var ent=$it37.get_Current();
                ent.CalculateSpeed();
                ent.CalculatePosition();
            }
            var $it38=this.livingEntities.GetEnumerator();
            while($it38.MoveNext())
            {
                var lent=$it38.get_Current();
                lent.CalculateSpeed();
                lent.CalculatePosition();
            }
        },
        CalculateLights:function()
        {
            var $it39=this.GetLights().GetEnumerator();
            while($it39.MoveNext())
            {
                var light=$it39.get_Current();
                light.Think();
            }
        },
        GetTileSize:function()
        {
            return new System.Tuple$2.ctor(System.Int32,System.Int32,this.tileWidth,this.tileHeight);
        },
        SetTileSize:function(newWidth,newHeight)
        {
            this.tileWidth = newWidth;
            this.tileHeight = newHeight;
        },
        GetLights:function()
        {
            return this.stageLights;
        },
        AddLight:function(newLight)
        {
            this.stageLights.Add(newLight);
        },
        Subdivide:function()
        {
            this.stageAreas = new System.Collections.Generic.List$1.ctor(WebDE.Objects.Area);
            return this.stageAreas;
        },
        IsSubdivided:function()
        {
            if(this.stageAreas == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        },
        GetCollisionMap:function()
        {
            if(this.collisionMap == null)
            {
                this.RenderCollisionMap();
            }
            return this.collisionMap;
        },
        RenderCollisionMap:function()
        {
            this.collisionMap = WebDE.GUI.GuiLayer.AsCollisionMap(this);
        },
        ShowCollisionMap:function()
        {
            if(this.collisionMap == null)
            {
                this.RenderCollisionMap();
            }
            this.collisionMap.Activate();
            this.collisionMap.Show();
        },
        HideCollisionMap:function()
        {
            this.collisionMap.Deactivate();
            this.collisionMap.Hide();
        },
        GetBounds:function()
        {
            return new WebDE.Rectangle.ctor(0,0,this.width,this.height);
        }
    }
};
JsTypes.push(WebDE$Objects$Stage);
var WebDE$Objects$Tile=
{
    fullname:"WebDE.Objects.Tile",
    baseTypeName:"WebDE.Objects.GameEntity",
    staticDefinition:
    {
        cctor:function()
        {
            WebDE.Objects.Tile.loadedTiles = new System.Collections.Generic.List$1.ctor(WebDE.Objects.Tile);
        },
        GetByName:function(tileName)
        {
            var newTile=null;
            var $it40=WebDE.Objects.Tile.loadedTiles.GetEnumerator();
            while($it40.MoveNext())
            {
                var tile=$it40.get_Current();
                if(tile.GetName() == tileName)
                {
                    newTile = new WebDE.Objects.Tile.ctor(tileName,tile.GetWalkable(),tile.GetBuildable());
                    return newTile;
                }
            }
            if(newTile == null)
            {
                newTile = new WebDE.Objects.Tile.ctor(tileName,true,true);
            }
            return newTile;
        },
        oldGetByName:function(tileName)
        {
            var $it41=WebDE.Objects.Tile.loadedTiles.GetEnumerator();
            while($it41.MoveNext())
            {
                var tile=$it41.get_Current();
                if(tile.GetName() == tileName)
                {
                    return Cast(WebDE.Objects.Helpah.Clone(tile),WebDE.Objects.Tile);
                }
            }
            return null;
        }
    },
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(tileName,canWalk,canBuild)
        {
            this.lightLevel = null;
            this.isWalkable = false;
            this.isBuildable = false;
            WebDE.Objects.GameEntity.ctor.call(this,tileName);
            this.isWalkable = canWalk;
            this.isBuildable = canBuild;
            WebDE.Objects.Tile.loadedTiles.Add(this);
        },
        GetLightLevel:function()
        {
            return this.lightLevel;
        },
        SetLightLevel:function(newLevel)
        {
            this.lightLevel = new WebDE.Color.ctor(newLevel.red,newLevel.green,newLevel.blue);
        },
        CalculateLightLevel:function()
        {
            this.SetLightLevel(WebDE.Color.Black);
            var localLights=WebDE.Objects.LightSource.GetLocalLightSources(this.GetPosition().x,this.GetPosition().y);
            if(localLights.get_Count() == 0)
            {
                return this.lightLevel;
            }
            var reds=new System.Collections.Generic.List$1.ctor(System.Int32);
            var blues=new System.Collections.Generic.List$1.ctor(System.Int32);
            var greens=new System.Collections.Generic.List$1.ctor(System.Int32);
            var $it42=localLights.GetEnumerator();
            while($it42.MoveNext())
            {
                var currentLight=$it42.get_Current();
                var dist=this.GetPosition().Distance(currentLight.GetPosition());
                var diminishAmount=dist * 0.1;
                var newRed=Cast((currentLight.GetColor().red - System.Math.Round$$Double(currentLight.GetColor().red * diminishAmount)),System.Int32);
                var newGreen=Cast((currentLight.GetColor().green - System.Math.Round$$Double(currentLight.GetColor().green * diminishAmount)),System.Int32);
                var newBlue=Cast((currentLight.GetColor().blue - System.Math.Round$$Double(currentLight.GetColor().blue * diminishAmount)),System.Int32);
                reds.Add(newRed);
                greens.Add(newGreen);
                blues.Add(newBlue);
            }
            var avgRed=0,avgBlue=0,avgGreen=0;
            var $it43=reds.GetEnumerator();
            while($it43.MoveNext())
            {
                var curRed=$it43.get_Current();
                avgRed += curRed;
            }
            var $it44=blues.GetEnumerator();
            while($it44.MoveNext())
            {
                var curBlue=$it44.get_Current();
                avgBlue += curBlue;
            }
            var $it45=greens.GetEnumerator();
            while($it45.MoveNext())
            {
                var curGreen=$it45.get_Current();
                avgGreen += curGreen;
            }
            this.lightLevel.red = avgRed = avgRed / reds.get_Count();
            this.lightLevel.blue = avgBlue = avgBlue / blues.get_Count();
            this.lightLevel.green = avgGreen = avgGreen / greens.get_Count();
            return this.lightLevel;
        },
        old_CalculateLightLevel:function()
        {
            this.SetLightLevel(WebDE.Color.Black);
            var localLights=WebDE.Objects.LightSource.GetLocalLightSources(this.GetPosition().x,this.GetPosition().y);
            var $it46=localLights.GetEnumerator();
            while($it46.MoveNext())
            {
                var currentLight=$it46.get_Current();
                var dist=this.GetPosition().Distance(currentLight.GetPosition());
                var diminishAmount=dist * 0.1;
                var calculatedColor=new WebDE.Color.ctor(currentLight.GetColor().red - Cast(System.Math.Round$$Double(currentLight.GetColor().red * diminishAmount),System.Int32),currentLight.GetColor().green - Cast(System.Math.Round$$Double(currentLight.GetColor().green * diminishAmount),System.Int32),currentLight.GetColor().blue - Cast(System.Math.Round$$Double(currentLight.GetColor().blue * diminishAmount),System.Int32));
                if(calculatedColor.red > this.lightLevel.red)
                {
                    this.lightLevel.red = calculatedColor.red;
                }
                if(calculatedColor.green > this.lightLevel.green)
                {
                    this.lightLevel.green = calculatedColor.green;
                }
                if(calculatedColor.blue > this.lightLevel.blue)
                {
                    this.lightLevel.blue = calculatedColor.blue;
                }
            }
            return this.lightLevel;
        },
        GetBuildable:function()
        {
            return this.isBuildable;
        },
        SetBuildable:function(buildable)
        {
            this.isBuildable = buildable;
        },
        GetWalkable:function()
        {
            return this.isWalkable;
        },
        SetWalkable:function(walkable)
        {
            this.isWalkable = walkable;
        },
        InitialRender:function()
        {
            var tileSize=WebDE.Objects.Stage.CurrentStage.GetTileSize();
            this.SetSize(tileSize.get_Item1(),tileSize.get_Item2());
        }
    }
};
JsTypes.push(WebDE$Objects$Tile);
var WebDE$Objects$Weapon=
{
    fullname:"WebDE.Objects.Weapon",
    baseTypeName:"System.Object",
    assemblyName:"WebDE",
    Kind:"Class",
    definition:
    {
        ctor:function(owner,damage,firingInterval,projectileSpeed,turningRadius,turnSpeed)
        {
            this.maxAmmo = 100;
            this.currentAmmo = 100;
            this.range = 0;
            this.turningRadius = 0;
            this.turningSpeed = 0;
            this.projectileSpeed = 0;
            this.damage = 0;
            this.projectileType = null;
            this.firingDelay = 0;
            this.lastFiredTime = 0;
            this.owner = null;
            this.target = null;
            this.fireWhileMoving = false;
            System.Object.ctor.call(this);
            this.owner = owner;
            this.damage = damage;
            this.firingDelay = firingInterval * 1000;
            this.projectileSpeed = projectileSpeed;
            this.turningRadius = turningRadius;
            this.turningSpeed = turnSpeed;
            this.lastFiredTime = System.DateTime.get_Now().get_Millisecond() - Cast(System.Math.Round$$Double(this.firingDelay),System.Int32);
        },
        Fire:function()
        {
            if(this.owner.GetParentStage() == null)
            {
                return;
            }
            if(System.DateTime.get_Now().get_Millisecond() > this.lastFiredTime + this.firingDelay)
            {
                WebDE.Debug.log("Ima firin mah lazer");
                var myBullet=new WebDE.Objects.Projectile.ctor("Bullet",this.GetTarget().GetPosition());
                myBullet.SetParentStage(this.owner.GetParentStage());
                myBullet.SetDamage(10);
                myBullet.SetPosition(this.owner.GetPosition().x,this.owner.GetPosition().y);
                myBullet.SetSpeed(new WebDE.Vector.ctor(0,0.5));
                myBullet.SetDirection(1);
                myBullet.SetAcceleration(0.1);
                WebDE.Objects.Stage.CurrentStage.AddLivingGameEntity(myBullet);
                this.SetCurrentAmmo(this.GetCurrentAmmo() - 1);
            }
        },
        SetCurrentAmmo:function(newAmmo)
        {
            this.currentAmmo = newAmmo;
        },
        GetCurrentAmmo:function()
        {
            return this.currentAmmo;
        },
        SetMaxAmmo:function(newAmmo)
        {
            this.maxAmmo = newAmmo;
        },
        GetMaxAmmo:function()
        {
            return this.maxAmmo;
        },
        SetRange:function(newRange)
        {
            this.range = newRange;
        },
        GetRange:function()
        {
            return this.range;
        },
        SetProjectileSpeed:function(newSpeed)
        {
            this.projectileSpeed = newSpeed;
        },
        GetProjectileSpeed:function()
        {
            return this.projectileSpeed;
        },
        SetTurningSpeed:function(newSpeed)
        {
            this.turningSpeed = newSpeed;
        },
        GetTurningSpeed:function()
        {
            return this.turningSpeed;
        },
        SetProjectile:function(newProjectile)
        {
            this.projectileType = newProjectile;
        },
        GetProjectile:function()
        {
            return this.projectileType;
        },
        SetFiringDelay:function(newDelay)
        {
            this.firingDelay = newDelay * 1000;
        },
        GetFiringDealy:function()
        {
            return this.firingDelay / 1000;
        },
        SetTarget:function(newTarget)
        {
            this.target = newTarget;
        },
        GetTarget:function()
        {
            return this.target;
        }
    }
};
JsTypes.push(WebDE$Objects$Weapon);