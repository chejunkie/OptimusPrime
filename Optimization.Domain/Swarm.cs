using Optimus.Core;
using System.Diagnostics;

namespace Optimus.Domain
{
    public class Swarm
    {
        private static Random Randomizer = new Random(0);

        private readonly Particle[] _particles;
        private readonly Solution _best;

        public Swarm(IObjectiveFunction aux, int numberParticles, int dim, double minX, double maxX)
        {
            _particles = new Particle[numberParticles];
            _best = new Solution(aux, dim, minX, maxX);

            for (int i = 0; i < numberParticles; i++)
            {
                _particles[i] = new Particle(aux, dim, minX, maxX);
                if (_particles[i].Value < Best.Value)
                {
                    Best.Move(_particles[i].Position());
                }
            }

            //for (int i = 0; i < numberParticles; ++i) // initialize each Particle in the swarm
            //{
            //    // Random position (starting parameter values)
            //    double[] randomPosition = new double[dim];
            //    for (int j = 0; j < randomPosition.Length; ++j)
            //    {
            //        double lo = minX;
            //        double hi = maxX;
            //        randomPosition[j] = (hi - lo) * Randomizer.NextDouble() + lo;
            //    }

            //    /* Velocity indicates where particle will move next,
            //    /* and is added to the position (its ~particle momentum). */
            //    double[] randomVelocity = new double[dim];
            //    for (int j = 0; j < randomVelocity.Length; ++j)
            //    {
            //        double lo = -1.0 * Math.Abs(maxX - minX);
            //        double hi = Math.Abs(maxX - minX);
            //        randomVelocity[j] = (hi - lo) * Randomizer.NextDouble() + lo;
            //    }
            //    //Particles[i] = new Particle(aux, randomPosition, randomVelocity);
            //    Particles[i] = new Particle(aux, dim, minX, maxX);

            //    if (Particles[i].Value < Best.Value)
            //    {
            //        Best.Move(randomPosition);
            //    }
            //}
        }

        public Particle this[int index]
        {
            get => _particles[index];
        }

        public Particle[] Particles => _particles;

        public ISolution Best => _best;

        public int Length
        {
            get { return _particles.Length; }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _particles.Length; ++i)
                s += "[" + i + "] " + _particles[i].ToString() + "\n";
            s += "BestSwarPosition [ ";
            for (int i = 0; i < Best.Length; ++i)
                s += Best[i].ToString("F2") + " ";
            s += "] ";
            s += "BestSwarmValue = " + Best.Value.ToString("F3");
            s += "\n";
            return s;
        }
    }
}