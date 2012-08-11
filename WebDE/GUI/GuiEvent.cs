﻿using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.Animation;
using WebDE.Timekeeper;
using WebDE.GameObjects;

namespace WebDE.GUI
{
    //what happens when an area of the screen gets activated or something
    //can only be created / generated by like a factory or whatever
    [JsType(JsMode.Clr, Filename = "../scripts/GUI.js")]
    public partial class GuiEvent
    {
        public List<Tile> clickedTiles = new List<Tile>();
        public List<GameEntity> clickedEntities = new List<GameEntity>();
        //the GameEntity that is on top for the given location, and is most likely to have been clicked
        public GameEntity topGameEntity;

        //the screen coordinate location of the event
        public Point eventPos = null;
        //the position of the event, in pixels
        public Point eventPixelPos = null;
        public GuiElement clickedElement;

        public GuiEvent(int xPos, int yPos)
        {
            Dimension TileSize = Stage.CurrentStage.GetTileSize();

            this.eventPixelPos = new Point(xPos, yPos);
            this.eventPos = new Point(this.eventPixelPos.x / TileSize.width, this.eventPixelPos.y / TileSize.height);
        }

        //public static GuiEvent FromClickData(GuiLayer gLayer, jQueryEvent jqClickData)
        public static GuiEvent FromClickData(GuiLayer gLayer, Point clickPos)
        {
            GuiEvent returnEvent = new GuiEvent(int.Parse(clickPos.x.ToString()), int.Parse(clickPos.y.ToString()));
            //Dimension TileSize = Stage.CurrentStage.GetTileSize();

            //returnEvent.eventPixelPos = new Point(clickPos.x, clickPos.y);
            //returnEvent.eventPos = new Point(returnEvent.eventPixelPos.x / TileSize.First, returnEvent.eventPixelPos.y / TileSize.Second);
            //Script.Eval("console.log('Pixelpos is " + returnEvent.eventPixelPos.x + ", " + returnEvent.eventPixelPos.y + ". Pos is " + returnEvent.eventPos.x + ", " + returnEvent.eventPos.y + "');");
            //returnEvent.eventAction = "Click";

            returnEvent.clickedElement = gLayer.GetElementAt(returnEvent.eventPos.x, returnEvent.eventPos.y);
            returnEvent.clickedEntities = gLayer.GetEntitiesAt(returnEvent.eventPos.x, returnEvent.eventPos.y);
            returnEvent.clickedTiles = gLayer.GetTilesAt(returnEvent.eventPos.x, returnEvent.eventPos.y);

            return returnEvent;
        }

        public static GuiEvent FromGuiElement(GuiElement sender)
        {
            GuiEvent returnEvent = new GuiEvent((int)sender.GetPosition().x, (int)sender.GetPosition().y);

            returnEvent.clickedElement = sender;
            returnEvent.clickedTiles = sender.GetParentLayer().GetTilesAt(returnEvent.eventPos.x, returnEvent.eventPos.y);
            //presume no entities and tiles...?

            return returnEvent;
        }

        /// <summary>
        /// Each of the parameters is nullable. The most complete set of data possible will be built from any non-null parameters.
        /// </summary>
        /// <param name="sendingTile"></param>
        /// <param name="sendingGameEntity"></param>
        /// <param name="sendingElement"></param>
        /// <param name="triggeringPosition"></param>
        /// <param name="triggeringScreenPosition"></param>
        /// <returns></returns>
        public static GuiEvent FromPartialData(Tile sendingTile, GameEntity sendingGameEntity, GuiElement sendingElement, Point triggeringPosition, Point triggeringScreenPosition)
        {
            GuiEvent eventToReturn = new GuiEvent(0, 0);

            //check each of the parameters for non-null values and set them in the event to return
            if (sendingTile != null)
            {
                eventToReturn.clickedTiles.Add(sendingTile);
            }
            if (sendingGameEntity != null)
            {
                eventToReturn.clickedEntities.Add(sendingGameEntity);
            }
            if (sendingElement != null)
            {
                eventToReturn.clickedElement = sendingElement;
            }
            if (triggeringPosition != null)
            {
                eventToReturn.eventPos = triggeringPosition;
            }
            if (triggeringScreenPosition != null)
            {
                eventToReturn.eventPixelPos = triggeringScreenPosition;
            }

            //go through the values, and determine values for those that are null
            if (eventToReturn.eventPos == null)
            {
                //get the position from one of the properties that we have...
            }

            //gotta do this for the other stuff too
            //eventToReturn.clickedElement = gLayer.GetElementAt(returnEvent.eventPos.x, returnEvent.eventPos.y);
            //eventToReturn.clickedEntities = gLayer.GetEntitiesAt(returnEvent.eventPos.x, returnEvent.eventPos.y);
            //eventToReturn.clickedTiles = gLayer.GetTilesAt(returnEvent.eventPos.x, returnEvent.eventPos.y);

            return eventToReturn;
        }
    }
}
