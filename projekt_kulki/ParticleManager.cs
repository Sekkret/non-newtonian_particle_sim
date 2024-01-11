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
                // avoid errors of molecular dynamics
                if(particle.Velocity.Length() > 1000)
                {
                    particle.Velocity = new Vector2(particle.Velocity.X / 100, particle.Velocity.Y / 100);
                }
                Vector2 totalForce = Vector2.Zero;
                foreach(var particle2 in Particles)
                {
                    if (particle != particle2)
                    {
                        totalForce = Vector2.Add(  totalForce, Force.Ionic( particle.GetPosition(), particle2.GetPosition(), UniverseProperties.getCoefficient(particle.particleType, particle2.particleType) )  );
                    }
                }
                totalForce = Vector2.Add(totalForce, Force.BorderForce(particle.GetPosition(), playgroundCanvas));
                totalForce = Vector2.Add(totalForce, Force.Resistance(particle.Velocity));
                particle.ApplyForce(totalForce, (float) 0.1);
            }
            foreach (var particle in Particles)
            {
                try
                {
                    particle.Move((float) 0.1);
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
            }
            particlesToRemove.Clear();
        }

        public void RemoveParticle(Particle clickedParticle)
        {
            Particles.Remove(clickedParticle);
        }
    }
}
