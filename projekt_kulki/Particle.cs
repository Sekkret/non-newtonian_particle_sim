using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace projekt_kulki
{
    public enum ParticleType
    {
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
        Gray
    }

    class Particle : FrameworkElement
    {
        private Ellipse ellipse;
        public Vector2 Velocity { get; set; }
        static public int radius = 20;
        public ParticleType particleType { get; set; }

        public Particle(Point position, Vector2 velocity, ParticleType particleType)
        {
            this.particleType = particleType;

            // Initialize Ellipse
            ellipse = new Ellipse
            {
                Width = 2*radius,
                Height = 2*radius,
                Stroke = Brushes.Black,
                Fill = new RadialGradientBrush
                {
                    GradientStops =
                {
                    new GradientStop(Colors.White, 0.058),
                    new GradientStop(Colors.Black, 1),
                    new GradientStop(chooseEllipseColor(particleType), 0.467)
                }
                }
            };
            Canvas.SetLeft(this, position.X);
            Canvas.SetTop(this, position.Y);

            // initialize velocity
            this.Velocity = velocity;

            // Add the ellipse (Particle) to the visual tree
            this.AddVisualChild(ellipse);
            this.AddLogicalChild(ellipse);

        }

        //Those functions are necessary, so Ellipse rendering can work
        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return ellipse;
            }
            throw new ArgumentOutOfRangeException();
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            // Measure the size needed for the Ellipse
            ellipse.Measure(availableSize);
            return ellipse.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // Arrange the Ellipse within the available space
            ellipse.Arrange(new Rect(finalSize));
            return finalSize;
        }
        //private functions
        private Color chooseEllipseColor(ParticleType particleColor)
        {
            switch (particleColor)
            {
                case ParticleType.Red:
                    return Colors.Red;
                case ParticleType.Blue:
                    return Colors.Blue;
                case ParticleType.Green:
                    return Colors.Green;
                case ParticleType.Yellow:
                    return Colors.Yellow;
                case ParticleType.Purple:
                    return Colors.Purple;
                case ParticleType.Gray:
                    return Colors.Gray;
                default:
                    return Colors.Red;
            }
        }

        // public functions
        public double mass()
        {
            return UniverseProperties.getMass(particleType);
        }
        public void Move(float dt)
        {
            Point currentPos = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            double newXPos = currentPos.X + this.Velocity.X;
            double newYPos = currentPos.Y + this.Velocity.Y;
            try
            {
                this.SetPosition(new Point(newXPos, newYPos));
            }
            catch (System.ArgumentException ex)
            {
                throw ex;
            }
        }

        public void SetPosition(Point position)
        {
            try
            {
                double posX = position.X;
                double posY = position.Y;
                Canvas.SetLeft(this, posX);
                Canvas.SetTop(this, posY);
            }
            catch (System.ArgumentException ex)
            {
                throw ex;
            } 
        }

        public Point GetPosition()
        {
            return new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
        }

        public void ApplyForce(Vector2 Force, float dt)
        {
            Vector2 a = Vector2.Divide(Force, (float) UniverseProperties.getMass(particleType) );
            Velocity += a * dt;
        }
    }

}
