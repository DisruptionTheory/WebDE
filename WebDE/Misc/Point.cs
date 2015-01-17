using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

using WebDE.GameObjects;

namespace WebDE
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public class Point
    {
        public static Point MidPoint(Point start, Point end)
        {
            int resultX = Helpah.d2i((start.x + end.x) / 2);
            int resultY = Helpah.d2i((start.y + end.y) / 2);
            return new Point(resultX, resultY);
            //return new Point(end.x - start.x, end.y - start.y);
        }

        public double x = 0;
        public double y = 0;

        public Point(double theX, double theY)
        {
            this.x = theX;
            this.y = theY;
        }

        public double Distance(Point point2)
        {
            //return Math.Abs(point2.x - this.x) + Math.Abs(point2.y - this.y);
            return Math.Sqrt( Math.Pow(point2.x - this.x, 2) + Math.Pow(point2.y - this.y, 2) );
        }

        public Point Shift(Direction dir)
        {
            if (dir == Direction.NORTH)
            {
                return new Point(this.x, this.y - 1);
            }
            if (dir == Direction.EAST)
            {
                return new Point(this.x + 1, this.y);
            }
            if (dir == Direction.WEST)
            {
                return new Point(this.x - 1, this.y);
            }
            else
            {
                return new Point(this.x, this.y + 1);
            }
        }

        public Point Shift(Direction dir, int amount)
        {
            if (dir == Direction.NORTH)
            {
                return new Point(this.x, this.y - amount);
            }
            if (dir == Direction.EAST)
            {
                return new Point(this.x + amount, this.y);
            }
            if (dir == Direction.WEST)
            {
                return new Point(this.x - amount, this.y);
            }
            else
            {
                return new Point(this.x, this.y + amount);
            }
        }

        /// <summary>
        /// Checks if this point is between two points.
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <returns></returns>
        public bool Between(Point pointA, Point pointB)
        {
            if (this.x > pointA.x && this.x < pointB.x &&
                this.y > pointA.y && this.y < pointB.y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a point is within a given distance of a rectangle's edge.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="gent"></param>
        /// <returns></returns>
        public bool Within(double distance, Rectangle rect)
        {
            Ellipse distCheck = new Ellipse(rect.x - (distance / 2), rect.y - (distance / 2),
                rect.width + distance, rect.height + distance);
            //Circle distCheck = new Circle(rect.x, rect.y, (rect.Size.GetGreatest() + distance) / 2);
            return distCheck.Contains(this);
        }
    }

    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    public enum Direction
    {
        NORTH = 1,
        EAST = 2,
        WEST = 3,
        SOUTH = 4
    }
}
