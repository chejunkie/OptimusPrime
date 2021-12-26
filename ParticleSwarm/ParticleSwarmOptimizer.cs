using Optimization.Infrastructure;

namespace ParticleSwarm
{
    public class ParticleSwarmOptimizer : Optimizer
    {
        // TODO: kill off non-performers and rebirth at new random location.

        private static Random Randomizer = new Random(1);

        private readonly int NumberParticles;
        private readonly long MaxLoop;
        private readonly int Dim;
        private readonly double MinX;
        private readonly double MaxX;

        //private ParticleSolution[] Swarm; 
        private Swarm Swarm;
        private double[]? BestGlobalPosition;
        private double BestGlobalValue;
        //? private Particle BestParticle;
        private readonly double MinVelocity;
        private readonly double MaxVelocity;

        public ParticleSwarmOptimizer(IObjectiveFunction aux, int dim, double minX, double maxX, 
            int numberParticles, long maxLoop) : base(aux)
        {
            NumberParticles = numberParticles;
            MaxLoop = maxLoop;
            Dim = dim;
            MinX = minX;
            MaxX = maxX;
            MinVelocity = -1.0 * maxX;
            MaxVelocity = maxX;

            KickHive(); // initialize swarm
        }

        private void KickHive()
        {
            Swarm = new Swarm(ObjectiveFunction, NumberParticles, Dim, MinX, MaxX);
            BestGlobalPosition = new double[Dim];
            Swarm.BestSwarmPosition.CopyTo(BestGlobalPosition, 0);
            BestGlobalValue = Swarm.BestSwarmValue;
        }

        //private void KickHive()
        //{
        //    //TODO: use swarm class?
        //    Swarm = new ParticleSolution[NumberParticles];

        //    //TODO: simplify - best particle?
        //    BestGlobalPosition = new double[Dim];
        //    BestGlobalValue = double.MaxValue;

        //    for (int i = 0; i < Swarm.Length; ++i) // initialize each Particle in the swarm
        //    {
        //        // Random position (starting parameter values)
        //        double[] randomPosition = new double[Dim];
        //        for (int j = 0; j < randomPosition.Length; ++j)
        //        {
        //            double lo = MinX;
        //            double hi = MaxX;
        //            randomPosition[j] = (hi - lo) * Randomizer.NextDouble() + lo;
        //        }
        //        double fitness = ObjectiveFunction.EvaluateAt(randomPosition);

        //        /* Velocity indicates where particle will move next,
        //        /* and is added to the position. */
        //        double[] randomVelocity = new double[Dim];
        //        for (int j = 0; j < randomVelocity.Length; ++j)
        //        {
        //            double lo = -1.0 * Math.Abs(MaxX - MinX);
        //            double hi = Math.Abs(MaxX - MinX);
        //            randomVelocity[j] = (hi - lo) * Randomizer.NextDouble() + lo;
        //        }
        //        //! Swarm[i] = new Particle(randomPosition, fitness, randomVelocity, randomPosition, fitness);
        //        Swarm[i] = new ParticleSolution(ObjectiveFunction, randomPosition, randomVelocity);

        //        // does current Particle have global best position/solution?
        //        if (Swarm[i].Value < BestGlobalValue)
        //        {
        //            BestGlobalValue = Swarm[i].Value;
        //            Swarm[i].Vector.CopyTo(BestGlobalPosition, 0);
        //        }
        //    } 
        //}

        public override ISolution Solve()
        {
            //! MAGIC WEIGHTS... parameters tuned for specific problems. However, PSO is markedly robust.
            /* ----------------------------------------------------------------------------------
             * Eberhart, R. C.; Shi, Y. 
             * Comparing Inertia Weights and Constriction Factors in Particle Swarm Optimization. 
             * In Proceedings of the 2000 Congress on Evolutionary Computation. 
             * CEC00 (Cat. No.00TH8512); 2000; Vol. 1, pp 84–88 vol.1. 
             * https://doi.org/10.1109/CEC.2000.870279. 
             * ---------------------------------------------------------------------------------- */
            double wMin = 0.5;
            double wMax = 1;
            double w = 0.729;   // inertia weight; a particle that is moving tends to keep moving in the same direction
            double c1 = 1.49445; // cognitive/local weight (particle best influence): particle tends to move towards best solution it ever found
            double c2 = 1.49445; // social/global weight (swarm overall best influence): the swarm tends to move towards best solution found by any in the swarm
            double r1;           // cognitive randomization
            double r2;           // social randomization

            int iteration = 0;

            while (iteration < MaxLoop)
            {
                ++iteration;
                w = wMax - (wMax - wMin) * iteration / MaxLoop;
                double[] newVelocity = new double[Dim];
                double[] newPosition = new double[Dim];
                double newFitness;

                for (int i = 0; i < Swarm.Length; ++i) // each Particle
                {
                    ParticleSolution currP = Swarm[i];

                    for (int j = 0; j < currP.Velocity.Length; ++j) // each x value of the velocity
                    {
                        r1 = Randomizer.NextDouble();
                        r2 = Randomizer.NextDouble();

                        newVelocity[j] = (w * currP.Velocity[j]) +
                          (c1 * r1 * (currP.BestPosition[j] - currP.Vector[j])) +
                          (c2 * r2 * (BestGlobalPosition[j] - currP.Vector[j]));

                        if (newVelocity[j] < MinVelocity)
                            newVelocity[j] = MinVelocity;
                        else if (newVelocity[j] > MaxVelocity)
                            newVelocity[j] = MaxVelocity;
                    }

                    newVelocity.CopyTo(currP.Velocity, 0);

                    for (int j = 0; j < currP.Vector.Length; ++j)
                    {
                        newPosition[j] = currP.Vector[j] + newVelocity[j];
                        if (newPosition[j] < MinX)
                            newPosition[j] = MinX;
                        else if (newPosition[j] > MaxX)
                            newPosition[j] = MaxX;
                    }

                    currP.CopyFrom(newPosition);
                    newFitness = currP.Value; 

                    //TODO: automatically check and update BestPosition/Value after CopyFrom
                    if (newFitness < currP.BestValue)
                    {
                        newPosition.CopyTo(currP.BestPosition, 0);
                        currP.BestValue = newFitness;
                    }
                    if (newFitness < BestGlobalValue)
                    {
                        newPosition.CopyTo(BestGlobalPosition, 0);
                        BestGlobalValue = newFitness;
                    }
                }
            }
            ParticleSolution solution = new ParticleSolution(ObjectiveFunction, BestGlobalPosition);
            return solution;
        }
    }
}