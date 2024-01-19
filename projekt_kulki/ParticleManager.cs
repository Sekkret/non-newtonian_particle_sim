using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Numerics;
using System.Windows.Controls;
using System.Security;

namespace projekt_kulki
{
    class ParticleManager
    {
        public List<Particle> Particles { get; private set; }
        private Canvas playgroundCanvas;

        public ParticleManager(Canvas playgroundCanvas)
        {
            Particles = new List<Particle>();
            this.playgroundCanvas = playgroundCanvas;
        }

        public void AddParticle(Particle particle)
        {
            Particles.Add(particle);
        }


        public void MoveParticles()
        {
            List<Particle> particlesToRemove = new List<Particle>(); //necessary to handle exception
            foreach (var particle in Particles)
            {
                // avoid errors of molecular dynamics -- The Great Slow Down
                if(particle.Velocity.Length() > 50)
                {
                    particle.Velocity = new Vector2(particle.Velocity.X / 1000, particle.Velocity.Y / 1000);
                }
                Vector2 totalForce = Vector2.Zero;
                foreach(var particle2 in Particles)
                {
                    if (particle != particle2)
                    {
                        double coeff = UniverseProperties.getCoefficient(particle.particleType, particle2.particleType);
                        totalForce = Vector2.Add(  totalForce, UniverseProperties.interactionForce( particle.GetPosition(), particle2.GetPosition(), coeff )  );
                    }
                }
                totalForce = Vector2.Add(totalForce, Force.BorderForce(particle.GetPosition(), playgroundCanvas));
                totalForce = Vector2.Add(totalForce, Force.Resistance(particle.Velocity));
                particle.ApplyForce(totalForce, (float) UniverseProperties.timeStep);
            }
            foreach (var particle in Particles)
            {
                try
                {
                    particle.Move((float) UniverseProperties.timeStep);
                }
                catch (System.ArgumentException)
                {
                    particle.SetPosition(new Point(0, 0));
                    particle.Velocity = Vector2.Zero;
                    particlesToRemove.Add(particle);
                }

            }

            //finishing of handling exception
            foreach (var particleToRemove in particlesToRemove)
            {
                Particles.Remove(particleToRemove);
                playgroundCanvas.Children.Remove(particleToRemove);
            }
            particlesToRemove.Clear();
        }


        public void RemoveParticle(Particle clickedParticle)
        {
            Particles.Remove(clickedParticle);
        }

        public void RemoveAllParticles()
        {
            foreach (var particle in Particles) { 
                playgroundCanvas.Children.Remove(particle);
            }
            Particles.Clear();
        }
    }
}
