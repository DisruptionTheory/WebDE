using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.PathFinding;
using WebDE.GameObjects;

namespace WebDE.AI
{
    [JsType(JsMode.Clr, Filename = "../scripts/AI.js")]
    public partial class MovementPath
    {
        public static MovementPath AStar(Point start, Point end, Stage stage = null)
        {
            MovementPath newPath = new MovementPath(null);
            if (stage == null)
            {
                stage = Stage.CurrentStage;
            }

            // setup grid with walls
            //MyPathNode[,] grid = new MyPathNode[(int)stage.Width, (int)stage.Height];
            MyPathNode[][] grid = new MyPathNode[(int)stage.Width][];

            for (int x = 0; x < (int)stage.Width; x++)
            {
                grid[x] = new MyPathNode[(int) stage.Height];
                for (int y = 0; y < (int)stage.Height; y++)
                {

                    grid[x][y] = new MyPathNode()
                    {
                        IsWall = stage.GetTileAt(x, y).GetWalkable(),
                        X = x,
                        Y = y,
                    };
                }
            }  

            SpatialAStar<MyPathNode, Object> aStar = new SpatialAStar<MyPathNode, Object>(grid);

            LinkedList<MyPathNode> path = aStar.Search(new Point(0, 0),
               new Point(stage.Width - 2, stage.Height - 2), null);  
            return null;
        }

        //resolve the shortest path between two given points
        public static MovementPath ResolvePath(Point start, Point end, Stage stage = null)
        {
            MovementPath newPath = new MovementPath(null);
            if (stage == null)
            {
                stage = Stage.CurrentStage;
            }
            // The last point added
            Point currentPoint = start;

            // A list of all path legs on the horizontal plane of the map, indexed by map row.
            Dictionary<short, List<PathLeg>> horizontalPathLegs = new Dictionary<short, List<PathLeg>>();
            // A list of all path legs on the vertical plane of the map, indexed by map column.
            Dictionary<short, List<PathLeg>> verticalPathLegs = new Dictionary<short, List<PathLeg>>();
            // Resolve the legs in the map
            MovementPath.resolvePathLegs(stage, false, ref horizontalPathLegs);
            MovementPath.resolvePathLegs(stage, true, ref verticalPathLegs);

            // Prioritize the side with more long paths (horizontal or vertical)

            // Determine which paths to attempt to join together, and calculate the cost of traveling between those paths

            // If the travel cost is significant (exceeds the difference with the next-shortest path), try the next path

            return newPath;
        }

        // Resolve the map's path legs for a given dimension
        private static void resolvePathLegs(Stage stage, bool vertical, ref Dictionary<short, List<PathLeg>> returnVals)
        {
            short col = 0;
            short row = 0;
            short legStart = -1;
            short legEnd = -1;

            if (vertical)
            {
                // Loop through all of the rows
                while (col < stage.Width)
                {
                    returnVals[col] = new List<PathLeg>();

                    // Loop through all of the columns in this row ...
                    while (row < stage.Height)
                    {
                        // If the tile is passable by entities.
                        if (stage.GetTileAt(col, row).GetWalkable())
                        {
                            // If we're in a leg, extend it
                            if (legStart != -1)
                            {
                                legEnd = row;
                            }
                            // Start a new leg
                            else
                            {
                                legStart = row;
                            }
                        }
                        else
                        {
                            // If we had a leg going, store it.
                            if (legStart != -1 && legEnd != -1)
                            {
                                returnVals[col].Add(new PathLeg(new Point(col, legStart), new Point(col, legEnd), vertical));
                            }

                            legStart = -1;
                            legEnd = -1;
                        }

                        row++;
                    }

                    row = 0;
                    col++;
                }
            }
            else
            {
                // Loop through all of the rows
                while (row < stage.Height)
                {
                    returnVals[row] = new List<PathLeg>();

                    // Loop through all of the columns in this row ...
                    while (col < stage.Width)
                    {
                        // If the tile is passable by entities.
                        if (stage.GetTileAt(col, row).GetWalkable())
                        {
                            // If we're in a leg, extend it
                            if (legStart != -1)
                            {
                                legEnd = col;
                            }
                            // Start a new leg
                            else
                            {
                                legStart = col;
                            }
                        }
                        else
                        {
                            // If we had a leg going, store it.
                            if (legStart != -1 && legEnd != -1)
                            {
                                returnVals[row].Add(new PathLeg(new Point(legStart, row), new Point(legEnd, row), vertical));
                            }

                            legStart = -1;
                            legEnd = -1;
                        }

                        col++;
                    }

                    col = 0;
                    row++;
                }
            }
        }

        //resolve the shortest path between two given points
        public static MovementPath ResolvePath2(Point start, Point end, Stage stage = null)
        {
            MovementPath newPath = new MovementPath(null);
            if (stage == null)
            {
                stage = Stage.CurrentStage;
            }
            // The last point added
            Point currentPoint = start;

            //need better pathfinding and collision detection later

            // Add the beginning
            newPath.AddPoint(start);

            // List all of the avenues we've explored
            List<PathLeg> pathLegs = new List<PathLeg>();

            // Start from the start and explore the legs
            // March repeatedly through each path, until reaching the exit or a dead end

            //if there's horizontal distance to cover between the points, cover it first
            while(currentPoint.x != end.x)
            {
                // If we can move to the next tile, do so
                if (stage.GetTileAt((int)currentPoint.x + 1, (int)currentPoint.y).GetWalkable())
                {
                    currentPoint = new Point(currentPoint.x + 1, currentPoint.y);
                    newPath.AddPoint(currentPoint);
                }
                // If not, switch to vertical differencing ... 
                else
                {

                }
            }

            return newPath;
        }

        // Resolve a single (horizontal or vertical) leg of a path between two points.
        private static List<Point> resolveLeg(Point start, Point end, Stage stage, bool vertical)
        {
            List<Point> resultPath = new List<Point>();
            Point currentPoint = start;

            resultPath.Add(start);

            if (vertical)
            {
                // Positive movement
                if (end.y > start.y)
                {
                    while (stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y + 1) != null &&
                        stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y + 1).GetWalkable())
                    {
                        currentPoint = new Point(currentPoint.x, currentPoint.y + 1);
                        resultPath.Add(currentPoint);
                    }
                }
                // Negative movement
                else
                {
                    while (stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y - 1) != null &&
                        stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y - 1).GetWalkable())
                    {
                        currentPoint = new Point(currentPoint.x, currentPoint.y - 1);
                        resultPath.Add(currentPoint);
                    }
                }
            }
            // Horizontal
            else
            {
                // Positive movement
                if (end.x > start.x)
                {
                    while (stage.GetTileAt((int)currentPoint.x + 1, (int)currentPoint.y) != null &&
                        stage.GetTileAt((int)currentPoint.x + 1, (int)currentPoint.y).GetWalkable())
                    {
                        currentPoint = new Point(currentPoint.x + 1, currentPoint.y);
                        resultPath.Add(currentPoint);
                    }
                }
                // Negative movement
                else
                {
                    while (stage.GetTileAt((int)currentPoint.x - 1, (int)currentPoint.y) != null &&
                        stage.GetTileAt((int)currentPoint.x - 1, (int)currentPoint.y).GetWalkable())
                    {
                        currentPoint = new Point(currentPoint.x - 1, currentPoint.y);
                        resultPath.Add(currentPoint);
                    }
                }
            }

            // Check the point at the end of the path to make sure it's accessible from two directions
            // Remove points from the end until the last point is accessible from two directions.

            return resultPath;
        }

        // Get the longest passage connected to a point
        private static MovementDirection longestPassage(Point point, Stage stage)
        {
            short longestPassage = 0;
            MovementDirection longestDirection = MovementDirection.None;
            Point currentPoint = point;
            short passageLength = 0;

            // Check Right
            while (stage.GetTileAt((int) currentPoint.x + 1, (int) currentPoint.y) != null && 
                stage.GetTileAt((int) currentPoint.x + 1, (int) currentPoint.y).GetWalkable())
            {
                currentPoint = new Point(currentPoint.x + 1, currentPoint.y);
                passageLength++;
            }
            if (passageLength > longestPassage)
            {
                longestDirection = MovementDirection.Right;
            }
            passageLength = 0;
            currentPoint = point;

            // Check Left
            while (stage.GetTileAt((int)currentPoint.x - 1, (int)currentPoint.y) != null && 
                stage.GetTileAt((int)currentPoint.x - 1, (int)currentPoint.y).GetWalkable())
            {
                currentPoint = new Point(currentPoint.x - 1, currentPoint.y);
                passageLength++;
            }
            if (passageLength > longestPassage)
            {
                longestDirection = MovementDirection.Left;
            }
            passageLength = 0;
            currentPoint = point;

            // Check Up
            while (stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y + 1) != null && 
                stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y + 1).GetWalkable())
            {
                currentPoint = new Point(currentPoint.x, currentPoint.y + 1);
                passageLength++;
            }
            if (passageLength > longestPassage)
            {
                longestDirection = MovementDirection.Up;
            }
            passageLength = 0;
            currentPoint = point;

            // Check Down
            while (stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y - 1) != null && 
                stage.GetTileAt((int)currentPoint.x, (int)currentPoint.y - 1).GetWalkable())
            {
                currentPoint = new Point(currentPoint.x, currentPoint.y - 1);
                passageLength++;
            }
            if (passageLength > longestPassage)
            {
                longestDirection = MovementDirection.Down;
            }

            return longestDirection;
        }

        public static MovementPath Patrol(Point origin, Point destination)
        {
            MovementPath newPath = new MovementPath(null);

            newPath.AddPoint(origin);
            newPath.AddPoint(destination);
            newPath.SetLooping(true);

            return newPath;
        }

        /// <summary>
        /// Randomize a path from start to finish. Assume no obstacles.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static MovementPath old_DrunkWalk(Point start, Point end, Rectangle acceptableArea)
        {
            MovementPath newPath = new MovementPath(null);
            //the number of steps taken in this direction so far
            int stepsTaken = 0;
            //the maximum number of 'steps' the 'drunk' can take in one direction before stumbling / changing direction
            int maxSteps = 5;
            //the current position of the path
            Point currentPos = start;
            //the next position
            Point nextPos;
            //the last cardinal direction the character has walked
            Direction lastDirection;

            //walk until we get to the end
            while (currentPos != end)
            {
                /*
                //if we have moved previously, and haven't walked so far as to have to stumble
                if (lastDirection != "" && stepsTaken < maxSteps)
                {
                    //throw a weighted random to select if we move on this direction again
                }
                */

                //pick a random direction
                lastDirection = MovementPath.randomDirection();
                //make sure it's a valid (non negative) point
                nextPos = currentPos.Shift(lastDirection);
                if(newPath.Contains(nextPos) == false && acceptableArea.Contains(nextPos))
                {
                    newPath.AddPoint(nextPos);
                    currentPos = nextPos;
                }
                //otherwise, we'll just try again next time!
            }

            return newPath;
        }

        public static MovementPath DrunkWalk(Point start, Point end, Rectangle acceptableArea)
        {
            MovementPath newPath = new MovementPath(null);
            newPath.AddPoint(start);
            Point currentPoint = start;
            Point midPoint = end;

            Point point1 = end;
            Point point2 = null;

            for (int deviations = 0; deviations < 7; deviations++)
            {
                point1 = Point.MidPoint(start, point1).Shift(MovementPath.randomDirection(), 3);
                if (acceptableArea.Contains(point1))
                {
                    newPath.AddPoint(point1);
                }

                if (point2 != null)
                {
                    point2 = Point.MidPoint(point2, end).Shift(MovementPath.randomDirection(), 3);
                    if (acceptableArea.Contains(point2))
                    {
                        newPath.AddPoint(point2);
                    }
                }
                else
                {
                    point2 = point1;
                }
            }

            newPath.AddPoint(end);

            return newPath;
        }

        //deviate a line from start to end...
        private static Point deviateLine(Point start, Point end)
        {
            Point returnPoint = Point.MidPoint(start, end);
            return returnPoint.Shift(MovementPath.randomDirection(), 3);
        }

        public static MovementPath DirectWithObstacles(Point start, Point end, Rectangle acceptableArea)
        {
            MovementPath returnPath = new MovementPath(null);
            returnPath.AddPoint(start);
            Point currentPoint = start;
            MovementDirection lastMovement = MovementDirection.None;

            int obstacleProbability = 5;
            List<Point> obstacles = new List<Point>();

            //work toward the 'finish'
            while (currentPoint.x != end.x && currentPoint.y != end.y)
            {
                //try to continue in a straight line
                //if(lastMovement != MovementDirection.None && currentPoint.x 
            }

            returnPath.AddPoint(end);
            return returnPath;
        }

        private static Direction randomDirection()
        {
            int random = Helpah.Rand(1, 4);
            return random.As<Direction>();
            //return (Direction)random;
            //ain't no enum.parse :(
            //return (Direction)Enum.Parse(typeof(Direction), randStr);
        }
    }
}
