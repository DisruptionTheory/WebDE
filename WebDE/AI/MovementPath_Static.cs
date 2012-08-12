using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.AI
{
    [JsType(JsMode.Clr, Filename = "../scripts/AI.js")]
    public partial class MovementPath
    {
        //resolve the shortest path between two given points
        public static MovementPath ResolvePath(Point start, Point end)
        {
            MovementPath newPath = new MovementPath(null);

            //need better pathfinding and collision detection later

            newPath.AddPoint(start);
            //if there's horizontal distance to cover between the points, cover it first
            if (start.x != end.x)
            {
                Point newPoint = new Point(end.x, start.y);
                newPath.AddPoint(newPoint);
            }
            //if there's vertical distance to cover between the points, cover it
            if (start.y != end.y)
            {
                newPath.AddPoint(end);
            }

            return newPath;
        }

        public static MovementPath Patrol(Point origin, Point destination)
        {
            MovementPath newPath = new MovementPath(null);

            newPath.AddPoint(origin);
            newPath.AddPoint(destination);
            newPath.SetLooping(true);

            return newPath;
        }
    }
}
