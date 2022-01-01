using Optimus.Core;
using Optimus.Domain;

namespace Optimus.MultiSwarm
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
        private readonly IObjectiveFunction Aux;

        public ParticleSwarm[] Swarms;
        //public double[] BestGlobalPosition;
        //public double BestGlobalValue;
        private readonly Solution _best;

        public MultiSwarmOptimizer(IObjectiveFunction aux, int dim, double minX, double maxX,
            int numberParticles, int numberSwarms, long maxLoop) : base(aux)
        {
            Aux = aux;
            Dim = dim;
            MinX = minX;
            MaxX = maxX;
            NumberParticles = numberParticles;
            NumberSwarms = numberSwarms;
            MaxLoop = maxLoop;
            _best = new Solution(aux, dim, minX, maxX);

            //? MinVelocity = -1.0 * maxX;
            //? MaxVelocity = maxX;

            // Kick Hives
            Swarms = new ParticleSwarm[NumberSwarms];

            for (int i = 0; i < NumberSwarms; i++)
            {
                Swarms[i] = new ParticleSwarm(ObjectiveFunction, NumberParticles, Dim, MinX, MaxX);
                if (Swarms[i].Best.Value < Best.Value)
                {
                    _best.Move(Swarms[i].Best.Position());
                }
            }
        }

        public ISolution Best => _best;

        private void Immigration(int i, int j)
        {
            // Swap particle j in swarm i
            // with a random particle in a random swarm
            int otheri = Randomizer.Next(0, Swarms.Length);
            int otherj = Randomizer.Next(0, Swarms[0].Particles.Length);
            Particle tmp = Swarms[i].Particles[j];
            Swarms[i].Particles[j] = Swarms[otheri].Particles[otherj];
            Swarms[otheri].Particles[otherj] = tmp;
        }

        public override ISolution Solve()
        {
            int iteration = 0;
            double wMin = 0;
            double wMax = 1;
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
                            Swarms[i].Particles[j] = new Particle(Aux, Dim, MinX, MaxX);
                        }

                        double q = Randomizer.NextDouble();
                        if (q < immigrate)
                        {
                            Immigration(i, j); // swap curr particle with a random particle in diff swarm
                        }

                        //! automatically scale inertia weight
                        //? w = wMax - (wMax - wMin) * (iteration + i * NumberParticles + j) / (MaxLoop + NumberSwarms * NumberParticles);
                        
                        for (int k = 0; k < Dim; ++k) // update velocity. each x position component
                        {
                            double r1 = Randomizer.NextDouble();
                            double r2 = Randomizer.NextDouble();
                            double r3 = Randomizer.NextDouble();

                            Swarms[i].Particles[j].Velocity[k] = (w * Swarms[i].Particles[j].Velocity[k]) +
                              (c1 * r1 * (Swarms[i].Particles[j].Best[k] - Swarms[i].Particles[j][k])) +
                              (c2 * r2 * (Swarms[i].Best[k] - Swarms[i].Particles[j][k])) +
                              (c3 * r3 * (Best[k] - Swarms[i].Particles[j][k]));

                            if (Swarms[i].Particles[j].Velocity[k] < MinX)
                            {
                                Swarms[i].Particles[j].Velocity[k] = MinX;
                            }
                            else if (Swarms[i].Particles[j].Velocity[k] > MaxX)
                            {
                                Swarms[i].Particles[j].Velocity[k] = MaxX;
                            }
                        }

                        for (int k = 0; k < Dim; ++k) // update position
                        {
                            Swarms[i].Particles[j][k] += Swarms[i].Particles[j].Velocity[k];
                        }

                        // check for new best cost
                        if (Swarms[i].Particles[j].Value < Swarms[i].Particles[j].Best.Value)
                        {
                            Swarms[i].Particles[j].Best.Move(Swarms[i].Particles[j].Position());
                        }
                        if (Swarms[i].Particles[j].Value < Swarms[i].Best.Value)
                        {
                            Swarms[i].Best.Move(Swarms[i].Particles[j].Position());
                        }
                        if (Swarms[i].Particles[j].Value < Best.Value)
                        {
                            Best.Move(Swarms[i].Particles[j].Position());
                        }
                    }
                }
            }
            return Best.Clone();
        }
    }
}