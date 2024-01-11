using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace projekt_kulki
{
    class UniverseProperties
    {
        public static double[] mass = new double[6];
        // first index is particle affected, the second one is affecting
        public static double[,] coefficients_matrix = new double[6, 6];

        public UniverseProperties()
        {
        }

        public static void Load()
        {
            //masses declarations
            mass[(int) ParticleType.Red] = 1;
            mass[(int)ParticleType.Blue] = 1;
            mass[(int)ParticleType.Green] = 1;
            mass[(int)ParticleType.Yellow] = 10;
            mass[(int)ParticleType.Purple] = 10;
            mass[(int)ParticleType.Gray] = 10;

            //coefficients declarations
            coefficients_matrix[(int)ParticleType.Red, (int)ParticleType.Red] = 50;
            coefficients_matrix[(int)ParticleType.Red, (int)ParticleType.Blue] = 50;
            coefficients_matrix[(int)ParticleType.Red, (int)ParticleType.Green] = 100;
            coefficients_matrix[(int)ParticleType.Red, (int)ParticleType.Yellow] = 0;
            coefficients_matrix[(int)ParticleType.Red, (int)ParticleType.Purple] = -50;
            coefficients_matrix[(int)ParticleType.Red, (int)ParticleType.Gray] = 50;

            coefficients_matrix[(int)ParticleType.Blue, (int)ParticleType.Red] = -50;
            coefficients_matrix[(int)ParticleType.Blue, (int)ParticleType.Blue] = 50;
            coefficients_matrix[(int)ParticleType.Blue, (int)ParticleType.Green] = -50;
            coefficients_matrix[(int)ParticleType.Blue, (int)ParticleType.Yellow] = -50;
            coefficients_matrix[(int)ParticleType.Blue, (int)ParticleType.Purple] = -50;
            coefficients_matrix[(int)ParticleType.Blue, (int)ParticleType.Gray] = -50;

            coefficients_matrix[(int)ParticleType.Green, (int)ParticleType.Red] = 50;
            coefficients_matrix[(int)ParticleType.Green, (int)ParticleType.Blue] = 50;
            coefficients_matrix[(int)ParticleType.Green, (int)ParticleType.Green] = 100;
            coefficients_matrix[(int)ParticleType.Green, (int)ParticleType.Yellow] = -50;
            coefficients_matrix[(int)ParticleType.Green, (int)ParticleType.Purple] = 100;
            coefficients_matrix[(int)ParticleType.Green, (int)ParticleType.Gray] = 50;

            coefficients_matrix[(int)ParticleType.Yellow, (int)ParticleType.Red] = 0;
            coefficients_matrix[(int)ParticleType.Yellow, (int)ParticleType.Blue] = 0;
            coefficients_matrix[(int)ParticleType.Yellow, (int)ParticleType.Green] = 0;
            coefficients_matrix[(int)ParticleType.Yellow, (int)ParticleType.Yellow] = 0;
            coefficients_matrix[(int)ParticleType.Yellow, (int)ParticleType.Purple] = 0;
            coefficients_matrix[(int)ParticleType.Yellow, (int)ParticleType.Gray] = 0;

            coefficients_matrix[(int)ParticleType.Purple, (int)ParticleType.Red] = -50;
            coefficients_matrix[(int)ParticleType.Purple, (int)ParticleType.Blue] = 100;
            coefficients_matrix[(int)ParticleType.Purple, (int)ParticleType.Green] = -100;
            coefficients_matrix[(int)ParticleType.Purple, (int)ParticleType.Yellow] = -100;
            coefficients_matrix[(int)ParticleType.Purple, (int)ParticleType.Purple] = -50;
            coefficients_matrix[(int)ParticleType.Purple, (int)ParticleType.Gray] = 200;

            coefficients_matrix[(int)ParticleType.Gray, (int)ParticleType.Red] = 0;
            coefficients_matrix[(int)ParticleType.Gray, (int)ParticleType.Blue] = -50;
            coefficients_matrix[(int)ParticleType.Gray, (int)ParticleType.Green] = 50;
            coefficients_matrix[(int)ParticleType.Gray, (int)ParticleType.Yellow] = -100;
            coefficients_matrix[(int)ParticleType.Gray, (int)ParticleType.Purple] = 200;
            coefficients_matrix[(int)ParticleType.Gray, (int)ParticleType.Gray] = 100;

        }

        public static double getMass(ParticleType particleType)
        {
            return mass[(int) particleType];
        }

        public static double getCoefficient(ParticleType affected, ParticleType affecting)
        {
            return coefficients_matrix[(int)affected, (int)affecting];
        }
    }
}
