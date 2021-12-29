using Optimus.Core;
using Optimus.Domain;

namespace Optimus.ParticleSwarm
{
    public class ParticleSwarmOptimizer : Optimizer
    { 
        // TODO: kill off non-performers (or randomly kill off) and rebirth at new random location.
        private static Random Randomizer = new Random(1);

        private readonly int NumberParticles;
        private readonly long MaxLoop;
        private readonly int Dim;
        private readonly double MinX;
        private readonly double MaxX;

        private Swarm Swarm;
        private Particle _best;

        public ParticleSwarmOptimizer(IObjectiveFunction aux, int dim, double minX, double maxX,
            int numberParticles, long maxLoop) : base(aux)
        {
            NumberParticles = numberParticles;
            MaxLoop = maxLoop;
            Dim = dim;
            MinX = minX;
            MaxX = maxX;
            //x MinVelocity = -1.0 * maxX;
            //x MaxVelocity = maxX;
            _best = new Particle(aux, dim, minX, maxX);
            
            // Kick the hive!
            Swarm = new Swarm(ObjectiveFunction, NumberParticles, Dim, MinX, MaxX);
            if (Swarm.Best.Value < Best.Value)
            {
                Best.Move(Swarm.Best.Position());
            }
        }

        public Particle Best => _best;

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
                w = wMax - (wMax - wMin) * iteration / MaxLoop; //! automatically scale inertia weight
                double[] newVelocity = new double[Dim];
                double[] newPosition = new double[Dim];
                double newFitness;

                for (int i = 0; i < Swarm.Length; ++i) // each Particle
                {
                    Particle currP = Swarm[i];

                    // Calcualte new velocity (~momentum)
                    for (int j = 0; j < currP.Velocity.Length; ++j)
                    {
                        r1 = Randomizer.NextDouble();
                        r2 = Randomizer.NextDouble();

                        newVelocity[j] = (w * currP.Velocity[j]) 
                            + (c1 * r1 * (currP.Best[j] - currP[j])) 
                            + (c2 * r2 * (Best[j] - currP[j]));

                        if (newVelocity[j] < MinX)
                        {
                            newVelocity[j] = MinX;
                        }
                        else if (newVelocity[j] > MaxX)
                        {
                            newVelocity[j] = MaxX;
                        }
                    }

                    // Calculate new position by adding velocity to current position
                    newVelocity.CopyTo(currP.Velocity, 0);

                    for (int j = 0; j < currP.Length; ++j)
                    {
                        newPosition[j] = currP[j] + newVelocity[j];
                        if (newPosition[j] < MinX)
                        {
                            newPosition[j] = MinX;
                        }
                        else if (newPosition[j] > MaxX)
                        {
                            newPosition[j] = MaxX;
                        }
                    }

                    // Is the new position better?
                    currP.Move(newPosition);

                    if (currP.Value < currP.Best.Value)
                    {
                        currP.Best.Move(newPosition);
                    }
                    if (currP.Value < Best.Value)
                    {
                        Best.Move(newPosition);
                    }
                }
            }
            return Best.Clone();
        }
    }
}