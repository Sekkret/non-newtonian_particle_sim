using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace projekt_kulki
{
    internal class Force
    { 
        public static Vector2 Ionic(Point position, Point position2, double coefficient) //r is a position of particle we calculate force for, ri i the source of the force.
        {
            double A = 1000;
            double B = coefficient;
            double m = 2;
            Vector2 r_i = new((float)position2.X - (float)position.X, (float)position2.Y - (float)position.Y);
            double F_i = (- m * A / Math.Pow(r_i.Length(), m + 1) + B / r_i.LengthSquared());
            F_i /= r_i.Length();
            return Vector2.Multiply(10*r_i, (float) F_i);
            // this 10 in return formula origins from mistake when I mulitplied force by mass. Now mass is divided, but effective acceleration remians the same
        }

        public static Vector2 BorderForce(Point position, Canvas canvas)
        {
            float A = (float) 0.0005;
            Vector2 borderForce = Vector2.Zero;
            if(position.X > 0.95 * canvas.ActualWidth)
            {
                borderForce.X = (float)(- A * Math.Pow(position.X - 0.95*canvas.ActualWidth, 2));
                //borderForce.X = -1;
            }
            else if(position.X < 0.05 * canvas.ActualWidth)
            {
                borderForce.X = (float)( A * (float)Math.Pow(position.X - 0.05 * canvas.ActualWidth, 2));
                //borderForce.X = 1;
            }
            if(position.Y > 0.95 * canvas.ActualHeight)
            {
                borderForce.Y = (float)(- A * (float)Math.Pow(position.Y - 0.95*canvas.ActualHeight, 2));
                //borderForce.Y = -1;
            }
            else if(position.Y < 0.05 * canvas.ActualHeight)
            {
                borderForce.Y = (float)( A * (float)Math.Pow(position.Y  - 0.05 * canvas.ActualHeight, 2));
                //borderForce.Y = 1;
            }
            return borderForce;
        }

        public static Vector2 Resistance(Vector2 velocity)
        {
            float b = (float) -0.01;
            return Vector2.Multiply(b, velocity);
        }
    }
}
