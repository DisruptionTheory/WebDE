using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.AI
{
    //http://en.wikipedia.org/wiki/A%2a
    [JsType(JsMode.Clr, Filename = "../scripts/AI.js")]
    public partial class MovementPath
    {
        //each of the points in the path
        private List<Point> nodes = new List<Point>();
        //whether or not this is a (looping) patrol path
        private bool looping = false;
        private int currentNode = -1;

        /// <summary>
        /// Create a new movement path.
        /// </summary>
        /// <param name="movementNodes_nullable">The list of nodes in the path. If this parameter is null, the list is empty.</param>
        public MovementPath(List<Point> movementNodes_nullable)
        {
            if (movementNodes_nullable != null)
            {
                this.nodes = movementNodes_nullable;
            }
        }

        //return the given node
        public Point GetNode(int nodeIndex)
        {
            //if we have reached the end of the node list
            if (nodeIndex > this.nodes.Count - 1 || nodeIndex < 0)
            {
                //if the path loops, 
                return null;
            }
            return this.nodes[nodeIndex];
        }

        //return the node after the given node, or after the current node
        public Point GetNextNode()
        {
            if (this.nodes.Count == 0)
            {
                return null;
            }

            this.currentNode++;
            //the end of the list
            if (this.currentNode >= this.nodes.Count)
            {
                if (this.looping == true)
                {
                    this.currentNode = 0;
                }
                else
                {
                    return null;
                }
            }

            return this.nodes[this.currentNode];
        }

        public void AddPoint(Point pointToAdd)
        {
            //need to figure out if the point lies between two existing points
            this.nodes.Add(pointToAdd);
        }

        public int GetNodeCount()
        {
            return this.nodes.Count;
        }

        public bool GetLooping()
        {
            return this.looping;
        }

        public void SetLooping(bool doesLoop)
        {
            this.looping = doesLoop;
        }
    }
}
