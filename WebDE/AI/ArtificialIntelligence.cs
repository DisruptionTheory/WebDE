﻿using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE;
using WebDE.GameObjects;
using WebDE.Rendering;
using WebDE.Misc;

namespace WebDE.AI
{
    [JsType(JsMode.Clr, Filename = "../scripts/AI.js")]
    public class ArtificialIntelligence
    {
        /// <summary>
        /// the entity this AI belongs to
        /// </summary>
        private LivingGameEntity body;
        /// <summary>
        /// the current movement path the AI is assigned to
        /// </summary>
        private MovementPath currentPath;
        /// <summary>
        /// the index of the node the AI is moving toward in its current movement path
        /// </summary>
        private int currentNode = 0;
        // Where the entity expects to wind up if it keeps moving along as normal.
        private Point expectedLocation = new Point(0, 0);

        //I'm not sure that I want this varaible. There's probably a better way to do this
        /// <summary>
        /// temporarily store the old speed if it needs to be changed
        /// </summary>
        private Vector oldSpeed;
        private Debug aiDebug;
        private Point destination;

        public Point Destination { 
            get { return destination; }
            set { currentNode = 0; destination = value; } }

        // The function that will be executed when the entity reaches the end of the movement path.
        public Action<ArtificialIntelligence> DestinationArrival = null;

        /// <summary>
        /// Get the MovementPath object attached to this ArtificialIntelligence.
        /// </summary>
        /// <returns></returns>
        public MovementPath GetMovementPath()
        {
            return this.currentPath;
        }
        /// <summary>
        /// Set the MovementPath object attached to this ArtificialIntelligence.
        /// </summary>
        /// <param name="newPath">The new path.</param>
        public void SetMovementPath(MovementPath newPath)
        {
            //I don't think that we need ot clone the path, because they can all share one instance, as the path itself doesn't have any dynamic variables
            //this.currentPath = (MovementPath)Helpah.Clone(newPath);
            this.currentPath = newPath;
        }

        /// <summary>
        /// Get the entity that this AI is attached to (the "body" to this "mind")
        /// </summary>
        /// <returns></returns>
        public LivingGameEntity GetBody()
        {
            return this.body;
        }

        /// <summary>
        /// Set the entity that this AI is attached to (the "body" to this "mind")
        /// </summary>
        /// <param name="newBody">The Entity to attach.</param>
        public void SetBody(LivingGameEntity newBody)
        {
            this.body = newBody;
        }

        /// <summary>
        /// Figure out what actions to preform the next time the body is able to.
        /// Currently determines where along the MovementPath the entity is, and sets the desired direction toward the next node.
        /// </summary>
        public void Think()
        {
            //technically, this function doesn't need to be called multiple times between movements
            //the actual repositioning of the Entity or alterating of related properties could trigger a necessity to think
            //but thinking doesn't need to happen more often than movement or any of the other actions that it controls

            //decision tree

            //once a decision is reached:

            //fire should definitely be done before movement
            this.ThinkAboutDoingViolence();

            this.ThinkAboutWhereToGo();
        }

        private Action<ArtificialIntelligence> alternateMovementProcess;

        private void ThinkAboutWhereToGo()
        {
            // Implement tracking of expected location, and handling of displacement at top of function (here) if not at expected location.

            // If something has slated this entity to move differently, do that instead.
            if (this.alternateMovementProcess != null)
            {
                this.alternateMovementProcess.Invoke(this);
                return;
            }

            // Default to not moving
            body.SetDirection(MovementDirection.None);

            // If we have previously temporarily altered the speed, restore it
            if (oldSpeed != null)
            {
                this.body.SetSpeed(oldSpeed);
                oldSpeed = null;
            }

            // Pick a direction (if any) to move
            // If there is a current movement path, move toward the current node
            if (this.currentPath != null)
            {
                if (this.currentPath.GetNodeCount() == 0)
                {
                    Debug.log("A non null path has 0 nodes for entity " + this.GetBody().GetId() + " (" + this.GetBody().GetName() + ")");
                }

                //the current node that we are targeted at / walking toward
                Point curNode = this.currentPath.GetNode(this.currentNode);
                //Point curNode = this.currentPath.GetNextNode();

                //there are no more nodes, the entity has reached the end of the path
                if (curNode == null)
                {
                    //Script.Eval("console.log('The current node in the current AI path is null for some entity.')");
                    this.currentPath = null;
                    //stop the entity
                    this.body.SetSpeed(new Vector(0, 0));

                    // If we need to do something at the completion of the path, do it.
                    if (this.DestinationArrival != null)
                    {
                        this.DestinationArrival.Invoke(this);
                    }
                }
                else
                {
                    // The difference between the current horizontal position and the target horizontal position
                    double hOffset = body.GetPosition().x - curNode.x;
                    // The difference between the current vertical position and the target vertical position
                    double vOffset = body.GetPosition().y - curNode.y;

                    //I wonder if these should be rounded?
                    //hOffset = Helpah.Round(hOffset);
                    //vOffset = Helpah.Round(vOffset);


                    //if there's more horizontal distance than vertical, we want to go that way
                    if (Math.Abs(hOffset) > Math.Abs(vOffset))
                    {
                        //if we're to the right of the target, move left
                        if (hOffset > 0)
                        {
                            body.SetDirection(MovementDirection.Left);
                        }
                        else
                        {
                            body.SetDirection(MovementDirection.Right);
                        }
                    }
                    else
                    {
                        if (vOffset > 0)
                        {
                            body.SetDirection(MovementDirection.Down);
                        }
                        else
                        {
                            body.SetDirection(MovementDirection.Up);
                        }
                    }

                    //if the horizontal distance is less than or equal to the current speed divided by the acceleration, begin decelerating
                    if (hOffset != 0 && Math.Abs(hOffset) <= body.GetSpeed().X / body.GetAcceleration())
                    {
                        body.SetDirection(MovementDirection.None);

                        //if the body needs to travel a distance less than its normal acceleration
                        if (Math.Abs(hOffset) < body.GetAcceleration())
                        {
                            //this.oldSpeed = body.GetSpeed();
                            //this.oldSpeed.x = 0;
                            body.SetSpeed(new Vector(hOffset, body.GetSpeed().Y));
                        }
                    }

                    //if the vertical distance is less than or equal to the current speed divided by the acceleration, begin decelerating
                    if (vOffset != 0 && Math.Abs(vOffset) <= body.GetSpeed().Y / body.GetAcceleration())
                    {
                        //Debug.log("Setting speed manually (voffset).");
                        body.SetDirection(MovementDirection.None);

                        //if the body needs to travel a distance less than its normal acceleration
                        if (Math.Abs(vOffset) < body.GetAcceleration())
                        {
                            //this.oldSpeed = body.GetSpeed();
                            body.SetSpeed(new Vector(body.GetSpeed().X, vOffset));
                        }
                    }

                    //the entity is at the same position as the node, time to target the next one
                    //if (Helpah.Round(hOffset, 2) == 0 && Helpah.Round(vOffset, 2) == 0)
                    if(hOffset == 0 && vOffset == 0)
                    {
                        body.SetDirection(MovementDirection.None);
                        this.currentNode++;
                    }

                    Debug.Watch("Desired direction:", body.GetDirection().ToString());
                }
            }
        }

        /// <summary>
        /// Fire the entity's weapons and so forth!
        /// </summary>
        public void ThinkAboutDoingViolence()
        {
            try
            {
                List<Weapon> listoguns = this.GetBody().GetWeapons();

                //so apparently the foreach gets compiled all wierd. So I replaced it
                int i = 0;
                while (listoguns.Count > i)
                {
                    Weapon theWeapon = listoguns[i];

                    //if it has a target that is still in range
                    if (theWeapon.GetTarget() != null &&
                        this.body.GetPosition().Distance(theWeapon.GetTarget().GetPosition()) < theWeapon.GetRange())
                    {
                        theWeapon.FireAtTarget();
                    }
                    else
                    {
                        //Debug.Watch("Targetless weapon", this.GetBody().GetName() + ", " + listoguns.Count + ", " + theWeapon.GetRange());

                        //find a new target
                        foreach (GameEntity ent in Stage.CurrentStage.GetLivingEntities())
                        {
                            // Skip itself and anything on the same team.
                            if (ent == this.GetBody()
                                || ent.Faction == this.GetBody().Faction)
                            {
                                continue;
                            }

                            //need to check if the entity is this...will probably always return false with a direct compare, since this is livingent...
                            if (ent.GetPosition().Distance(this.body.GetPosition()) < theWeapon.GetRange())
                            {
                                theWeapon.SetTarget(ent);
                                theWeapon.FireAtTarget();
                                break;
                            }
                        }
                    }
                    i++;
                }
            }   
            catch(Exception ex)
            {
                Debug.log("There was an issue with an AI thinking about doing violence. Issue follows.");
                Debug.log(ex.ToString());
                //Debug.log(ex.Message);
            }
        }

        public void SetMovementThoughtPattern(Action<ArtificialIntelligence> newThoughtProcess)
        {
            this.alternateMovementProcess = newThoughtProcess;
        }

        private void updateDebug(string newMsg)
        {
            if (aiDebug == null)
            {
                aiDebug = Debug.Watch("hOffset", "World");
            }
            aiDebug.UpdateValue(newMsg);
        }

        public void Patrol(Point destination)
        {
            this.SetMovementPath(MovementPath.Patrol(this.GetBody().GetPosition(), destination));
        }

        public ArtificialIntelligence Clone()
        {
            //be sure to expand this to handle other properties as AI grows...

            ArtificialIntelligence newAI = new ArtificialIntelligence();
            newAI.SetMovementPath(this.GetMovementPath());
            newAI.DestinationArrival = this.DestinationArrival;

            return newAI;
        }
    }
}
