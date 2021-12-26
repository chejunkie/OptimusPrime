using Optimization.Infrastructure;

namespace ParticleSwarm
{
    internal class Swarm
    {
        private static Random Randomizer = new Random(1);

        public ParticleSolution[] Particles;
        public double[] BestSwarmPosition;
        public double BestSwarmValue;

        public Swarm(IObjectiveFunction aux, int numberParticles, int dim, double minX, double maxX)
        {
            Particles = new ParticleSolution[numberParticles];

            //TODO: simplify - best particle?
            BestSwarmPosition = new double[dim];
            BestSwarmValue = double.MaxValue;

            for (int i = 0; i < numberParticles; ++i) // initialize each Particle in the swarm
            {
                // Random position (starting parameter values)
                double[] randomPosition = new double[dim];
                for (int j = 0; j < randomPosition.Length; ++j)
                {
                    double lo = minX;
                    double hi = maxX;
                    randomPosition[j] = (hi - lo) * Randomizer.NextDouble() + lo;
                }
                double fitness = aux.EvaluateAt(randomPosition);

                /* Velocity indicates where particle will move next,
                /* and is added to the position. */
                double[] randomVelocity = new double[dim];
                for (int j = 0; j < randomVelocity.Length; ++j)
                {
                    double lo = -1.0 * Math.Abs(maxX - minX);
                    double hi = Math.Abs(maxX - minX);
                    randomVelocity[j] = (hi - lo) * Randomizer.NextDouble() + lo;
                }
                Particles[i] = new ParticleSolution(aux, randomPosition, randomVelocity);

                // does current Particle have global best position/solution?
                if (Particles[i].Value < BestSwarmValue)
                {
                    BestSwarmValue = Particles[i].Value;
                    Particles[i].Vector.CopyTo(BestSwarmPosition, 0);
                }
            }
        }

        public ParticleSolution this[int index]
        {
            get => Particles[index];
        }

        public int Length
        {
            get { return Particles.Length; }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Particles.Length; ++i)
                s += "[" + i + "] " + Particles[i].ToString() + "\n";
            s += "BestSwarPosition [ ";
            for (int i = 0; i < BestSwarmPosition.Length; ++i)
                s += BestSwarmPosition[i].ToString("F2") + " ";
            s += "] ";
            s += "BestSwarmValue = " + BestSwarmValue.ToString("F3");
            s += "\n";
            return s;
        }
    }
}