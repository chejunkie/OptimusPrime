using Optimization.Infrastructure;

namespace MultiSwarm
{
    public class MultiSwarmOptimizer : Optimizer
    {
        private static Random Randomizer = new Random(0);

        private readonly int NumberParticles;
        private readonly int NumberSwarms;
        private readonly long MaxLoop;
        private readonly int Dim;
        private readonly double MinX;
        private readonly double MaxX;

        private Swarm[] Swarms;
        public double[] BestGlobalPosition;
        public double BestGlobalValue;

        public MultiSwarmOptimizer(IObjectiveFunction aux, int dim, double minX, double maxX,
            int numberParticles, int numberSwarms, long maxLoop) : base(aux)
        {
            Dim = dim;
            MinX = minX;
            MaxX = maxX;
            NumberParticles = numberParticles;
            NumberSwarms = numberSwarms;
            MaxLoop = maxLoop;

            //MinVelocity = -1.0 * maxX;
            //MaxVelocity = maxX;

            KickHives(); // initialize swarm
        }

        private void Immigration(int i, int j)
        {
            // Swap particle j in swarm i
            // with a random particle in a random swarm
            int otheri = Randomizer.Next(0, Swarms.Length);
            int otherj = Randomizer.Next(0, Swarms[0].Particles.Length);
            ParticleSolution tmp = Swarms[i].Particles[j];
            Swarms[i].Particles[j] = Swarms[otheri].Particles[otherj];
            Swarms[otheri].Particles[otherj] = tmp;
        }

        private void KickHives()
        {
            Swarms = new Swarm[NumberSwarms];
            BestGlobalPosition = new double[Dim];
            BestGlobalValue = double.MaxValue;

            for (int i = 0; i < NumberSwarms; i++)
            {
                Swarms[i] = new Swarm(ObjectiveFunction, NumberParticles, Dim, MinX, MaxX);
                if (Swarms[i].BestSwarmValue < BestGlobalValue)
                {
                    BestGlobalValue = Swarms[i].BestSwarmValue;
                    Array.Copy(Swarms[i].BestSwarmPosition, BestGlobalPosition, Dim);
                }
            }
        }

        public override ISolution Solve()
        {
            int iteration = 0;
            double w = 0.729; // inertia
            double c1 = 1.49445; // particle / cogntive
            double c2 = 1.49445; // swarm / social
            double c3 = 0.3645; // multiswarm / global
            double death = 0.005; ; // prob of particle death
            double immigrate = 0.005;  // prob of particle immigration

            while (iteration < MaxLoop)
            {
                ++iteration;
                for (int i = 0; i < Swarms.Length; ++i) // each swarm
                {
                    for (int j = 0; j < Swarms[i].Particles.Length; ++j) // each particle
                    {
                        double p = Randomizer.NextDouble();
                        if (p < death)
                        {
                            Swarms[i].Particles[j] = new ParticleSolution(ObjectiveFunction, Dim, MinX, MaxX);
                        }

                        double q = Randomizer.NextDouble();
                        if (q < immigrate)
                        {
                            Immigration(i, j); // swap curr particle with a random particle in diff swarm
                        }

                        for (int k = 0; k < Dim; ++k) // update velocity. each x position component
                        {
                            double r1 = Randomizer.NextDouble();
                            double r2 = Randomizer.NextDouble();
                            double r3 = Randomizer.NextDouble();

                            //TODO: velocity not updating value :/
                            Swarms[i].Particles[j].Velocity[k] = (w * Swarms[i].Particles[j].Velocity[k]) +
                              (c1 * r1 * (Swarms[i].Particles[j].BestPosition[k] - Swarms[i].Particles[j].Vector[k])) +
                              (c2 * r2 * (Swarms[i].BestSwarmPosition[k] - Swarms[i].Particles[j].Vector[k])) +
                              (c3 * r3 * (BestGlobalPosition[k] - Swarms[i].Particles[j].Vector[k]));

                            if (Swarms[i].Particles[j].Velocity[k] < MinX)
                                Swarms[i].Particles[j].Velocity[k] = MinX;
                            else if (Swarms[i].Particles[j].Velocity[k] > MaxX)
                                Swarms[i].Particles[j].Velocity[k] = MaxX;

                        }

                        for (int k = 0; k < Dim; ++k) // update position
                        {
                            //! Swarms[i].Particles[j].Vector[k] += Swarms[i].Particles[j].Velocity[k];
                            Swarms[i].Particles[j][k] += Swarms[i].Particles[j].Velocity[k];
                        }

                        // update cost
                        //x Swarms[i].Particles[j].Value = MultiSwarmProgram.Value(Swarms[i].Particles[j].Vector);

                        // check if new best cost
                        if (Swarms[i].Particles[j].Value < Swarms[i].Particles[j].BestValue)
                        {
                            Swarms[i].Particles[j].BestValue = Swarms[i].Particles[j].Value;
                            Array.Copy(Swarms[i].Particles[j].Vector, Swarms[i].Particles[j].BestPosition, Dim);
                        }

                        if (Swarms[i].Particles[j].Value < Swarms[i].BestSwarmValue)
                        {
                            Swarms[i].BestSwarmValue = Swarms[i].Particles[j].Value;
                            Array.Copy(Swarms[i].Particles[j].Vector, Swarms[i].BestSwarmPosition, Dim);
                        }

                        if (Swarms[i].Particles[j].Value < BestGlobalValue)
                        {
                            BestGlobalValue = Swarms[i].Particles[j].Value;
                            Array.Copy(Swarms[i].Particles[j].Vector, BestGlobalPosition, Dim);
                        }
                    }
                }
            }
            ParticleSolution solution = new ParticleSolution(ObjectiveFunction, BestGlobalPosition);
            return solution;
        }
    }
}