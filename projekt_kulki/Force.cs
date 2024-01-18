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
    public enum InteractionForce
    {
        Electrostatic,
        Linear1,
        Linear2
    }
    internal class Force
    { 
        public static Vector2 Electrostatic(Point position, Point position2, double coefficient)
        {
            double A = 10 * coefficient;
            Vector2 r_i = new((float)position2.X - (float)position.X, (float)position2.Y - (float)position.Y);

            double F_i = A / r_i.LengthSquared();
            F_i /= r_i.Length();
            return Vector2.Multiply(r_i, (float)F_i);
        }

        public static Vector2 Linear1(Point position, Point position2, double coefficient)
        {
            double B = coefficient/100;
            Vector2 r_i = new((float)position2.X - (float)position.X, (float)position2.Y - (float)position.Y);
            double dist = r_i.Length();
            double diameter = 2 * Particle.radius;
            double x_max = 200;
            double x_0 = (x_max - diameter) / 2 + diameter;
            double force_abs;
            if (dist < diameter)
            {
                force_abs = 0;
            }
            else if(dist < x_0)
            {
                force_abs = B / (x_0 - diameter) * (dist - diameter);
            }
            else if(dist < x_max)
            {
                force_abs = -B / (x_max - x_0) * (dist - x_max);
            }
            else{
                force_abs = 0;
            }
            return Vector2.Multiply((float)force_abs / (float)dist, r_i);
        }
        public static Vector2 Linear2(Point position, Point position2, double coefficient)
        {
            double B = -coefficient / 100;
            Vector2 r_i = new((float)position2.X - (float)position.X, (float)position2.Y - (float)position.Y);
            double dist = r_i.Length();
            double diameter = 2 * Particle.radius;
            double x_max = 200;
            double dx = x_max - diameter;
            double x1 = diameter + 0.25 * dx;
            double x2 = diameter + 0.75 * dx;
            double force_abs;
            if (dist <= diameter)
            {
                force_abs = 0;
            }
            else if (dist < x1)
            {
                force_abs = B / (x1 - diameter) * (dist - diameter);
            }
            else if (dist < x2)
            {
                force_abs = B / (x1 - x2) * (2 * dist - x2) + B;
            }
            else if (dist < x_max)
            {
                force_abs = B / (x_max - x2) * (dist - x2) - B;
            }
            else
            {
                force_abs = 0;
            }
            return Vector2.Multiply((float)(force_abs/dist), r_i);
        }

        public static Vector2 ParticleCollisionForce(Point pos, Point pos2, double coefficient)
        {
            Vector2 r_i = new((float)pos2.X - (float)pos.X, (float)pos2.Y - (float)pos.Y);
            float rel_dist = r_i.Length() - 2*Particle.radius;
            if (rel_dist > 0)
            {
                return Vector2.Zero;
            }
            else
            {
                float A = (float) 0.0001 * (float) coefficient;
                float F_i = A * (float) Math.Pow(rel_dist, 4);
                
                return Vector2.Multiply(F_i / rel_dist, r_i);
            }
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
