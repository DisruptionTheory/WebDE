using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;

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
            this.nodes.Add(new Point(pointToAdd.x, pointToAdd.y));
        }

        // Add a point to the end of the path in the direction specified.
        public void AddPoint(MovementDirection directionOfPoint)
        {
            Point lastPoint = this.nodes[this.nodes.Count - 1];

            if (directionOfPoint == MovementDirection.Left)
            {
                this.nodes.Add(new Point(lastPoint.x - 1, lastPoint.y));
            }
            else if (directionOfPoint == MovementDirection.Right)
            {
                this.nodes.Add(new Point(lastPoint.x + 1, lastPoint.y));
            }
            else if (directionOfPoint == MovementDirection.Down)
            {
                this.nodes.Add(new Point(lastPoint.x, lastPoint.y - 1));
            }
            else if (directionOfPoint == MovementDirection.Up)
            {
                this.nodes.Add(new Point(lastPoint.x, lastPoint.y + 1));
            }
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

        public bool Contains(Point p)
        {
            foreach (Point node in this.nodes)
            {
                if (node.x == p.x && node.y == p.y)
                {
                    return true;
                }
            }

            //Debug.log("This path from " + this.nodes[0].x + ", " + this.nodes[0].y + " to " + this.nodes[this.nodes.Count - 1].x + ", " + this.nodes[this.nodes.Count -1].y + 
              //  " does not contain point " + p.x + ", " + p.y);

            return false;
        }

        /// <summary>
        /// Return every single node from the start to the finish, the full path of this movementpath.
        /// </summary>
        /// <returns></returns>
        public MovementPath FullPath()
        {
            MovementPath returnPath = new MovementPath(null);
            int currentPos = 0;
            int totalPoints = this.nodes.Count - 1;
            Point currentPoint = this.nodes[0];
            returnPath.AddPoint(this.nodes[0]);

            Point comparePoint;

            //loop through all of the points
            while (currentPos < totalPoints)
            {
                comparePoint = this.nodes[currentPos + 1];

                //walk towards each point
                while (currentPoint.x != comparePoint.x || currentPoint.y != comparePoint.y)
                {
                    if (currentPoint.x < comparePoint.x)
                    {
                        currentPoint.x = currentPoint.x + 1;
                    }
                    else if (currentPoint.x > comparePoint.x)
                    {
                        currentPoint.x = currentPoint.x - 1;
                    }
                    else if (currentPoint.y < comparePoint.y)
                    {
                        currentPoint.y = currentPoint.y + 1;
                    }
                    else
                    {
                        currentPoint.y = currentPoint.y - 1;
                    }
                    returnPath.AddPoint(currentPoint);
                }
                currentPos++;
            }

            return returnPath;
        }
    }
}
