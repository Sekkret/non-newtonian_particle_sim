using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Transactions;

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

        public static Vector2 Ionic_Safe(Point position, Point position2, double coefficient) //r is a position of particle we calculate force for, ri i the source of the force.
        {
            double A = 1000;
            double B = coefficient;
            double m = 2;
            Vector2 r_i = new((float)position2.X - (float)position.X, (float)position2.Y - (float)position.Y);
            if (r_i.Length() < 0.5 * Particle.radius)
            {
                return Vector2.Zero;
            }
            double F_i = (-m * A / Math.Pow(r_i.Length(), m + 1) + B / r_i.LengthSquared());
            F_i /= r_i.Length();
            return Vector2.Multiply(10 * r_i, (float)F_i);
            // this 10 in return formula origins from mistake when I mulitplied force by mass. Now mass is divided, but effective acceleration remians the same
        }

        public static Vector2 Linear(Point pos, Point pos2, double coefficient)
        {
            double B = coefficient/100;
            double dist = Math.Sqrt( Math.Pow(pos.X - pos2.X, 2) + Math.Pow(pos.Y - pos2.Y, 2) );
            double radius = 2* Particle.radius;
            double x_min = radius;
            double x_max = 200;
            double x_0 = (x_min + x_max) / 2;
            double force_abs;
            if (dist < radius)
            {
                force_abs = B / radius * dist - Math.Abs(B);
            }
            else if (dist <= x_min)
            {
                force_abs = 0;
            }
            else if(dist < x_0)
            {
                force_abs = B / (x_0 - x_min) * (dist - x_min);
            }
            else if(dist < x_max)
            {
                force_abs = -B / (x_max - x_0) * (dist - x_max);
            }
            else{
                force_abs = 0;
            }
            Vector2 force_dir = new Vector2((float)(pos2.X - pos.X), (float)(pos2.Y - pos.Y));
            force_dir = Vector2.Divide(force_dir, force_dir.Length());
            return Vector2.Multiply(force_dir, (float)force_abs);
        }
        public static Vector2 Qudratic(Point pos, Point pos2, double coefficient)
        {
            double B = coefficient / 10 + 5;
            double dist = Math.Sqrt(Math.Pow(pos.X - pos2.X, 2) + Math.Pow(pos.Y - pos2.Y, 2));
            double c = -10;
            double force_abs = -Math.Pow(dist, 2) + B * dist - c;
            Vector2 force_dir = new Vector2((float)(pos2.X - pos.X), (float)(pos2.Y - pos.Y));
            force_dir = Vector2.Divide(force_dir, force_dir.Length());
            return Vector2.Multiply(force_dir, (float)force_abs);
        }
        public static Vector2 BorderForce(Point position, Canvas canvas)
        {
            float A = (float) 0.005;
            Vector2 borderForce = Vector2.Zero;
            if(position.X > 0.9 * canvas.ActualWidth)
            {
                borderForce.X = (float)(- A * Math.Pow(position.X - 0.95*canvas.ActualWidth, 2));
                //borderForce.X = -1;
            }
            else if(position.X < 0.05 * canvas.ActualWidth)
            {
                borderForce.X = (float)( A * (float)Math.Pow(position.X - 0.05 * canvas.ActualWidth, 2));
                //borderForce.X = 1;
            }
            if(position.Y > 0.85 * canvas.ActualHeight)
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
            float b = (float) UniverseProperties.frictionCoefficient;
            return Vector2.Multiply(-b, velocity);
        }
    }
}
