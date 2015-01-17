using System;
using System.Collections.Generic;

using SharpKit.JavaScript;

namespace WebDE.Misc
{
    [JsType(JsMode.Clr, Filename = "../scripts/Misc.js")]
    //a direction and magnitude
    public class Vector
    {
        //the horizontal magnitude
        //positive is to the right of the Y-axis, negative is to the left
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector(double xMagnitude, double yMagnitude)
        {
            this.X = xMagnitude;
            this.Y = yMagnitude;
            this.Z = 1;
        }

        public Vector(double xMagnitude, double yMagnitude, double zMagnitude)
        {
            this.X = xMagnitude;
            this.Y = yMagnitude;
            this.Z = zMagnitude;
        }

        public static double Distance(Vector vector1, Vector vector2)
        {
            return Math.Abs(vector2.Y - vector1.Y) + Math.Abs(vector2.X - vector1.X);
        }

        public double GetGreatest()
        {
            if (Math.Abs(this.X) > Math.Abs(this.Y))
            {
                return this.X;
            }
            else
            {
                return this.Y;
            }
        }

        /// <summary>
        /// Perform an Isometric projection of the vector.
        /// </summary>
        /// <param name="alpha">The first angle of projection.</param>
        /// <param name="beta">The second angle of projection.</param>
        /// <returns></returns>
        public Point Project(double alpha, double beta)
        {
            Matrix3D alphaMat = new Matrix3D();
            Matrix3D betaMat = new Matrix3D();

            alphaMat.X1 = 1; alphaMat.X2 = 0; alphaMat.X3 = 0;
            alphaMat.Y1 = 0; alphaMat.Y2 = Math.Cos(alpha); alphaMat.Y3 = Math.Sin(alpha);
            alphaMat.Z1 = 0; alphaMat.Z2 = Math.Sin(alpha) * -1; alphaMat.Z3 = Math.Cos(alpha);

            betaMat.X1 = Math.Cos(beta); betaMat.X2 = 0; betaMat.X3 = Math.Sin(beta) * -1;
            betaMat.Y1 = 0; betaMat.Y2 = 1; betaMat.Y3 = 0;
            betaMat.Z1 = Math.Sin(beta); betaMat.Z2 = 0; betaMat.Z3 = Math.Cos(beta);

            Matrix3D projectionMat = alphaMat.Multiply3D(betaMat);
            Vector projectionVector = projectionMat.MultiplyVector3(this);
            return new Point(projectionVector.X, projectionVector.Y);
        }
    }
}
