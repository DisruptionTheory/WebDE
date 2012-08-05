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
function Initialize()
{
    new MemeDefense.MemeDefense.ctor();
};
if(typeof(JsTypes) == "undefined")
    JsTypes = [];
var MemeDefense$MemeDefense=
{
    fullname:"MemeDefense.MemeDefense",
    baseTypeName:"System.Object",
    staticDefinition:
    {
        prepSprites:function()
        {
            MemeDefense.MemeDefense.PrepPastryLevelSprites();
            MemeDefense.MemeDefense.PrepSpaceLevelSprites();
            MemeDefense.MemeDefense.PrepAquaLevelSprites();
            var grassFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/grass.png",0,0);
            var grassIcon=new WebDE.Animation.Animation.ctor();
            grassIcon.SetName("GrassIcon");
            grassIcon.AddFrame(grassFrame);
            var Grass=new WebDE.Animation.Sprite.ctor("Grass");
            Grass.addAnimation(grassIcon);
            var frmRageOne=new WebDE.Animation.AnimationFrame.ctor("images/ragefaces/7.png",0,0);
            var animRageOne=new WebDE.Animation.Animation.ctor();
            animRageOne.AddFrame(frmRageOne);
            var sprRageOne=new WebDE.Animation.Sprite.ctor("RageOne");
            sprRageOne.addAnimation(animRageOne);
            var arrowFrame=new WebDE.Animation.AnimationFrame.ctor("images/arrow.png",0,0);
            var ArrowAnim=new WebDE.Animation.Animation.ctor();
            ArrowAnim.SetName("Bullet");
            ArrowAnim.AddFrame(arrowFrame);
            var Bullet=new WebDE.Animation.Sprite.ctor("Bullet");
            Bullet.addAnimation(ArrowAnim);
            Bullet.setSize(10,14);
            var gigaFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/gigapuddi.png",0,0);
            var GigaPudiIcon=new WebDE.Animation.Animation.ctor();
            GigaPudiIcon.SetName("GigaPudiIcon");
            GigaPudiIcon.AddFrame(gigaFrame);
            var GigaTower=new WebDE.Animation.Sprite.ctor("GigaPuddiTower");
            GigaTower.addAnimation(GigaPudiIcon);
            var nyamFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/nyancat.png",0,0);
            var NyanIcon=new WebDE.Animation.Animation.ctor();
            NyanIcon.SetName("NyanIcon");
            NyanIcon.AddFrame(nyamFrame);
            var NyanTower=new WebDE.Animation.Sprite.ctor("NyanTower");
            NyanTower.addAnimation(NyanIcon);
            var CeilingCatFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/ceilingcat.png",0,0);
            var CeilingCatIcon=new WebDE.Animation.Animation.ctor();
            CeilingCatIcon.SetName("CeilingCatIcon");
            CeilingCatIcon.AddFrame(CeilingCatFrame);
            var CeilingCatTower=new WebDE.Animation.Sprite.ctor("CeilingCatTower");
            CeilingCatTower.addAnimation(CeilingCatIcon);
            var StareDadFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/staredad.png",0,0);
            var StareDadIcon=new WebDE.Animation.Animation.ctor();
            StareDadIcon.SetName("GigaPudiIcon");
            StareDadIcon.AddFrame(StareDadFrame);
            var StareDadTower=new WebDE.Animation.Sprite.ctor("StareDadTower");
            StareDadTower.addAnimation(StareDadIcon);
            var PepperCopFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/pepperspray.png",0,0);
            var PepperCopIcon=new WebDE.Animation.Animation.ctor();
            PepperCopIcon.SetName("PepperCopIcon");
            PepperCopIcon.AddFrame(PepperCopFrame);
            var PepperCopTower=new WebDE.Animation.Sprite.ctor("PepperCopTower");
            PepperCopTower.addAnimation(PepperCopIcon);
            var XTheYFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/xthey.png",0,0);
            var XTheYIcon=new WebDE.Animation.Animation.ctor();
            XTheYIcon.SetName("XTheYIcon");
            XTheYIcon.AddFrame(XTheYFrame);
            var XTheYTower=new WebDE.Animation.Sprite.ctor("XTheYTower");
            XTheYTower.addAnimation(XTheYIcon);
            var TableFlipperFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/tableflipper.png",0,0);
            var TableFlipperIcon=new WebDE.Animation.Animation.ctor();
            TableFlipperIcon.SetName("TableFlipperIcon");
            TableFlipperIcon.AddFrame(TableFlipperFrame);
            var TableFlipperTower=new WebDE.Animation.Sprite.ctor("TableFlipperTower");
            TableFlipperTower.addAnimation(TableFlipperIcon);
            var GggFrame=new WebDE.Animation.AnimationFrame.ctor("images/ragefaces/goodguygreg.jpg",0,0);
            var GggIcon=new WebDE.Animation.Animation.ctor();
            GggIcon.SetName("GoodGuyGregIcon");
            GggIcon.AddFrame(GggFrame);
            var GoodGuyGreg=new WebDE.Animation.Sprite.ctor("GoodGuyGreg");
            GoodGuyGreg.addAnimation(GggIcon);
            var ScumbagFrame=new WebDE.Animation.AnimationFrame.ctor("images/scumbagsteve.jpg",0,0);
            var ScumbagIcon=new WebDE.Animation.Animation.ctor();
            ScumbagIcon.SetName("ScumbagSteveIocn");
            ScumbagIcon.AddFrame(ScumbagFrame);
            var ScumbagSteve=new WebDE.Animation.Sprite.ctor("ScumbagSteve");
            ScumbagSteve.addAnimation(ScumbagIcon);
        },
        PrepPastryLevelSprites:function()
        {
            var CherryFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/cherry.png",0,0);
            var CherryIcon=new WebDE.Animation.Animation.ctor();
            CherryIcon.SetName("CherryPastryIcon");
            CherryIcon.AddFrame(CherryFrame);
            var CherryPastry=new WebDE.Animation.Sprite.ctor("CherryPastry");
            CherryPastry.addAnimation(CherryIcon);
        },
        PrepSpaceLevelSprites:function()
        {
            var SaturnFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/saturn.png",0,0);
            var SaturnIcon=new WebDE.Animation.Animation.ctor();
            SaturnIcon.SetName("SaturnIcon");
            SaturnIcon.AddFrame(SaturnFrame);
            var Saturn=new WebDE.Animation.Sprite.ctor("Saturn");
            Saturn.addAnimation(SaturnIcon);
            var AsteroidFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/asteroid-field.png",0,0);
            var AsteroidFieldIcon=new WebDE.Animation.Animation.ctor();
            AsteroidFieldIcon.SetName("AsteroidFieldIcon");
            AsteroidFieldIcon.AddFrame(AsteroidFrame);
            var AsteroidField=new WebDE.Animation.Sprite.ctor("AsteroidField");
            AsteroidField.addAnimation(AsteroidFieldIcon);
            var RedDwarfFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/red-dwarf.png",0,0);
            var RedDwarfIcon=new WebDE.Animation.Animation.ctor();
            RedDwarfIcon.SetName("RedDwarfIcon");
            RedDwarfIcon.AddFrame(RedDwarfFrame);
            var RedDwarf=new WebDE.Animation.Sprite.ctor("RedDwarf");
            RedDwarf.addAnimation(RedDwarfIcon);
        },
        PrepAquaLevelSprites:function()
        {
            var WaterFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/water.png",0,0);
            var WaterIcon=new WebDE.Animation.Animation.ctor();
            WaterIcon.SetName("WaterIcon");
            WaterIcon.AddFrame(WaterFrame);
            var Water=new WebDE.Animation.Sprite.ctor("Water");
            Water.addAnimation(WaterIcon);
            var PlanksFrame=new WebDE.Animation.AnimationFrame.ctor("images/tiles/planks.png",0,0);
            var PlanksIcon=new WebDE.Animation.Animation.ctor();
            PlanksIcon.SetName("PlanksIcon");
            PlanksIcon.AddFrame(PlanksFrame);
            var Planks=new WebDE.Animation.Sprite.ctor("Planks");
            Planks.addAnimation(PlanksIcon);
        }
    },
    assemblyName:"MemeDefense",
    Kind:"Class",
    definition:
    {
        ctor:function()
        {
            this.towerToPlace = null;
            this.desiredTowerName = null;
            System.Object.ctor.call(this);
            WebDE.DefaultClient.Initialize();
            MemeDefense.MemeDefense.prepSprites();
            this.prepTowers();
            this.createTowerMenu();
            this.CreateTestArea();
        },
        CreateTestArea:function()
        {
            var testArea=WebDE.Game.CreateStage("Test Area",0);
            testArea.CreateRandomTiles();
            var $it1=testArea.GetVisibleTiles(WebDE.Rendering.View.GetMainView()).GetEnumerator();
            while($it1.MoveNext())
            {
                var tile=$it1.get_Current();
                tile.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("Grass"));
            }
            this.prepEnemies(testArea);
            return testArea;
        },
        CreatePastryLevel:function()
        {
            var pastryLevel=WebDE.Game.CreateStage("FrostedPastries",0);
            pastryLevel.SetSize(16,14);
            var cherryPastry=WebDE.Animation.Sprite.GetSpriteByName("CherryPastry");
            for(var h=0;h < pastryLevel.GetSize().get_Item2();h++)
            {
                for(var w=0;w < pastryLevel.GetSize().get_Item1();w++)
                {
                    var buildable=false;
                    var rand=Math.round(Math.random());
                    if(rand == 1)
                    {
                        buildable = true;
                    }
                    var aTile=pastryLevel.AddTile("",true,buildable);
                    aTile.SetParentStage(pastryLevel);
                    aTile.SetPosition(w,h);
                    if(buildable == false)
                    {
                        aTile.SetSprite(cherryPastry);
                    }
                }
            }
            return pastryLevel;
        },
        CreateAquaLevel:function()
        {
            var aquaLevel=WebDE.Game.CreateStage("AquaticAdventure",0);
            aquaLevel.SetSize(16,15);
            var water=WebDE.Animation.Sprite.GetSpriteByName("Water");
            var planks=WebDE.Animation.Sprite.GetSpriteByName("Planks");
            var waterTile=new WebDE.GameObjects.Tile.ctor("Water",false,false);
            var plankTile=new WebDE.GameObjects.Tile.ctor("Planks",true,true);
            for(var h=0;h < aquaLevel.GetSize().get_Item2();h++)
            {
                for(var w=0;w < aquaLevel.GetSize().get_Item1();w++)
                {
                    var buildable=false;
                    var rand=Math.round(Math.random());
                    if(rand == 1)
                    {
                        buildable = true;
                    }
                    var aTile;
                    if(buildable == true)
                    {
                        aTile = WebDE.GameObjects.Tile.GetByName("Planks");
                        aTile.SetSprite(planks);
                    }
                    else
                    {
                        aTile = WebDE.GameObjects.Tile.GetByName("Water");
                        aTile.SetSprite(water);
                    }
                    aTile.SetParentStage(aquaLevel);
                    aTile.SetPosition(w,h);
                    aquaLevel.AppendTile(aTile);
                }
            }
            return aquaLevel;
        },
        CreateSpaceLevel:function()
        {
            var spaceLevel=WebDE.Game.CreateStage("Space",0);
            spaceLevel.SetSize(16,14);
            var saturnTile=new WebDE.GameObjects.Tile.ctor("Saturn",false,false);
            var asteroidTile=new WebDE.GameObjects.Tile.ctor("AsteroidField",false,false);
            var redDwarfTile=new WebDE.GameObjects.Tile.ctor("RedDwarf",false,false);
            for(var h=0;h < spaceLevel.GetSize().get_Item2();h++)
            {
                for(var w=0;w < spaceLevel.GetSize().get_Item1();w++)
                {
                    var spaceTile;
                    var randomTileType=WebDE.GameObjects.Helpah.Rand(1,4);
                    if(randomTileType != 1)
                    {
                        spaceTile = WebDE.GameObjects.Tile.GetByName("Saturn");
                        if(randomTileType == 3)
                        {
                            spaceTile = WebDE.GameObjects.Tile.GetByName("AsteroidField");
                        }
                        else if(randomTileType == 4)
                        {
                            spaceTile = WebDE.GameObjects.Tile.GetByName("RedDwarf");
                        }
                        spaceTile.SetParentStage(spaceLevel);
                        spaceTile.SetPosition(w,h);
                        spaceTile.SetSprite(WebDE.Animation.Sprite.GetSpriteByName(spaceTile.GetName()));
                        spaceLevel.AppendTile(spaceTile);
                    }
                }
            }
            return spaceLevel;
        },
        prepTowers:function()
        {
            var GigaPuddiTower=new WebDE.GameObjects.LivingGameEntity.ctor("GigaPuddiTower",100);
            var NyanTower=new WebDE.GameObjects.LivingGameEntity.ctor("NyanTower",100);
            var StaredadTower=new WebDE.GameObjects.LivingGameEntity.ctor("StaredadTower",100);
            var CeilingCatTower=new WebDE.GameObjects.LivingGameEntity.ctor("CeilingCatTower",100);
            var PepperCopTower=new WebDE.GameObjects.LivingGameEntity.ctor("PepperCopTower",100);
            var XTheYTower=new WebDE.GameObjects.LivingGameEntity.ctor("XTheYTower",100);
            var TableFlipperTower=new WebDE.GameObjects.LivingGameEntity.ctor("TableFlipperTower",100);
        },
        createTowerMenu:function()
        {
            var gfAction=new WebDE.GUI.GUIFunction.ctor("Action",WebDE.InputManager.InputDevice.Mouse,"mouse0");
            var gfContext=new WebDE.GUI.GUIFunction.ctor("Context",WebDE.InputManager.InputDevice.Mouse,"mouse1");
            var towerMenu=WebDE.Rendering.View.GetMainView().AddLayer("TowerMenu",new WebDE.Rectangle.ctor(-100,40,146,300));
            towerMenu.Activate();
            var label=towerMenu.AddGUIElement("Towers");
            label.SetPosition(0,0);
            var gigaTower=towerMenu.AddGUIElement("Giga Puddi Tower");
            gigaTower.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("GigaPuddiTower"));
            gigaTower.SetPosition(20,32);
            gigaTower.GetSprite().Animate();
            gigaTower.SetCustomValue("GigaPuddiTower");
            var nyanTower=towerMenu.AddGUIElement("Nyan Nest");
            nyanTower.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("NyanTower"));
            nyanTower.SetPosition(70,32);
            nyanTower.GetSprite().Animate();
            nyanTower.SetCustomValue("NyanTower");
            var staredadTower=towerMenu.AddGUIElement("Staredad Seat");
            staredadTower.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("StareDadTower"));
            staredadTower.SetPosition(20,82);
            staredadTower.GetSprite().Animate();
            staredadTower.SetCustomValue("StaredadTower");
            var ceilingCatTower=towerMenu.AddGUIElement("Celing Cat Hole");
            ceilingCatTower.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("CeilingCatTower"));
            ceilingCatTower.SetPosition(70,82);
            ceilingCatTower.GetSprite().Animate();
            ceilingCatTower.SetCustomValue("CeilingCatTower");
            var pepperCopTower=towerMenu.AddGUIElement("Pepper Cop Tower");
            pepperCopTower.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("PepperCopTower"));
            pepperCopTower.SetPosition(20,132);
            pepperCopTower.GetSprite().Animate();
            pepperCopTower.SetCustomValue("PepperCopTower");
            var xTheYTower=towerMenu.AddGUIElement("All The Things");
            xTheYTower.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("XTheYTower"));
            xTheYTower.SetPosition(70,132);
            xTheYTower.GetSprite().Animate();
            xTheYTower.SetCustomValue("XTheYTower");
            var tableFlipperTower=towerMenu.AddGUIElement("Table Flipper");
            tableFlipperTower.SetSprite(WebDE.Animation.Sprite.GetSpriteByName("TableFlipperTower"));
            tableFlipperTower.SetPosition(20,182);
            tableFlipperTower.GetSprite().Animate();
            tableFlipperTower.SetCustomValue("TableFlipperTower");
            var $it2=towerMenu.GetGuiElements().GetEnumerator();
            while($it2.MoveNext())
            {
                var towerIcon=$it2.get_Current();
                towerIcon.SetGUIFunction(gfAction,$CreateDelegate(this,this.towerMenu_Click));
            }
        },
        towerMenu_Click:function(gEvent)
        {
            this.desiredTowerName = gEvent.clickedElement.GetCustomValue();
            if(this.desiredTowerName == "" || this.desiredTowerName == null)
            {
                return;
            }
            this.towerToPlace = WebDE.GameObjects.LivingGameEntity.CloneByName(this.desiredTowerName);
            var gfAction=WebDE.GUI.GUIFunction.GetByName("Action");
            var $it3=WebDE.GameObjects.Stage.CurrentStage.GetCollisionMap().GetGuiElements().GetEnumerator();
            while($it3.MoveNext())
            {
                var colTile=$it3.get_Current();
                colTile.SetGUIFunction(gfAction,$CreateDelegate(this,this.placeATower));
            }
            WebDE.GameObjects.Stage.CurrentStage.ShowCollisionMap();
            gEvent.clickedElement.Select(true);
            var ggg=WebDE.Animation.Sprite.GetSpriteByName("GoodGuyGreg");
            WebDE.Game.Notification(ggg,"Good Guy Greg","Click where you would like the tower to be placed.",10);
        },
        placeATower:function(gEvent)
        {
            WebDE.GameObjects.Stage.CurrentStage.HideCollisionMap();
            var clickedTile=gEvent.clickedTiles.get_Item$$Int32(0);
            if(clickedTile == null)
            {
                return;
            }
            var newTower=new WebDE.GameObjects.LivingGameEntity.ctor(this.desiredTowerName,100);
            newTower.SetParentStage(WebDE.GameObjects.Stage.CurrentStage);
            newTower.SetPosition(clickedTile.GetPosition().x,clickedTile.GetPosition().y);
            var dummyWeapon=new WebDE.GameObjects.Weapon.ctor(newTower,10,1000,0.5,180,40);
            dummyWeapon.SetRange(50);
            newTower.AddWeapon(dummyWeapon);
            WebDE.GameObjects.Stage.CurrentStage.AddLivingGameEntity(newTower);
            WebDE.GameObjects.Stage.CurrentStage.HideCollisionMap();
            var towerLayer=WebDE.GUI.GuiLayer.GetLayerByName("TowerMenu");
        },
        prepEnemies:function(currentStage)
        {
            var sprRageOne=WebDE.Animation.Sprite.GetSpriteByName("RageOne");
            var rageOne=new WebDE.GameObjects.LivingGameEntity.ctor("RageOne",100);
            rageOne.SetSprite(sprRageOne);
            rageOne.SetAcceleration(0.1);
            var ragePath=WebDE.AI.MovementPath.ResolvePath(new WebDE.Point.ctor(6,0),new WebDE.Point.ctor(8,6));
            var aiRage=new WebDE.AI.ArtificialIntelligence.ctor();
            aiRage.SetMovementPath(ragePath);
            rageOne.SetAI(aiRage);
            var rageSpawner=new WebDE.GameObjects.GameEntitySpawner.ctor("RageSpawner",1);
            rageSpawner.AddGameEntityBatch(rageOne,1,2);
            rageSpawner.SetPosition(6,0);
            currentStage.AddGameEntity(rageSpawner);
            rageSpawner.Activate();
        }
    }
};
JsTypes.push(MemeDefense$MemeDefense);
