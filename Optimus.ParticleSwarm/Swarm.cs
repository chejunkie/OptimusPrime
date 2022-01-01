using Optimization.Infrastructure;

namespace ParticleSwarm
{
    internal class Swarm
    {
        private static Random Randomizer = new Random(1);

        public Particle[] Particles;
        private Particle _best;

        //x public double[] BestSwarmPosition;
        //x public double BestSwarmValue;

        public Swarm(IObjectiveFunction aux, int numberParticles, int dim, double minX, double maxX)
        {
            Particles = new Particle[numberParticles];

            //TODO: simplify - best particle?
            //? BestSwarmPosition = new double[dim];
            //? BestSwarmValue = double.MaxValue;

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
                Particles[i] = new Particle(aux, randomPosition, randomVelocity);

                // does current Particle have global best position/solution?
                if (null == _best)
                {
                    _best = Particles[i].Clone();
                }
                if (Particles[i].Value < Best.Value)
                {
                    Best.Move(randomPosition);
                }
            }
        }

        public Particle this[int index]
        {
            get => Particles[index];
        }

        public Particle Best => _best;

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
            for (int i = 0; i < Best.Length; ++i)
                s += Best[i].ToString("F2") + " ";
            s += "] ";
            s += "BestSwarmValue = " + Best.Value.ToString("F3");
            s += "\n";
            return s;
        }
    }
}