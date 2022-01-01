using Optimus.Core;
using Optimus.Domain;
using System.Diagnostics;

namespace Optimus.Firefly
{
    public class FireflyOptimizer : Optimizer
    {
        private readonly int Dim;
        private readonly double MinX;
        private readonly double MaxX;
        private readonly int NumberFireflys;
        private readonly long MaxLoop;
        private readonly FireflySwarm Swarm;

        private Solution _best;

        public FireflyOptimizer(IObjectiveFunction aux, int dim, double minX, double maxX,
            int numberFireflys, long maxLoop) : base(aux)
        {
            Dim = dim;
            MinX = minX;
            MaxX = maxX;
            NumberFireflys = numberFireflys;
            MaxLoop = maxLoop;
            _best = new Solution(aux, dim, minX, maxX);

            Swarm = new FireflySwarm(ObjectiveFunction, NumberFireflys, Dim, MinX, MaxX);
            if (Swarm.Best.Value < Best.Value)
            {
                Best.Move(Swarm.Best.Position());
            }
        }

        public Solution Best => _best;

        private static double Distance(Domain.Firefly fA, Domain.Firefly fB)
        {
            double ssd = 0.0; // sum squared diffrences
            for (int i = 0; i < fA.Dim; ++i)
            {
                ssd += (fA[i] - fB[i]) * (fA[i] - fB[i]);
            }
            return Math.Sqrt(ssd);
        }

        public override ISolution Solve()
        {
            // minX and maxX establish boundaries for each firefly's position.
            // The values used here are specific to Michalewicz function.
            // For macine learning optimizatino with normalized data, values of -10.0 and +10.0 are common.
            Random rnd = new Random(0);
            double minX = 0.0;
            double maxX = 3.2;

            //! BAND AID !!!
            for (int i = 0; i < NumberFireflys; ++i)
            {
                rnd.NextDouble();
            }

            // B0 (base beta), g (gamma) and a (alpha) control the attractiveness of one firefly to another.
            // The values used (1.0, 1.0, and 0.20) were recommended by the source research paper.
            double B0 = 1.0;
            double g = 1.0;
            double a = 0.20;

            // Control how often to display a progress message:
            long displayInterval = MaxLoop / 10;

            // Main processing loop begins here.
            int epoch = 0;
            while (epoch < MaxLoop)
            {
                if (epoch % displayInterval == 0 && epoch < MaxLoop)
                {
                    Debug.WriteLine("Epoch = " + epoch + ", Best " + Best.ToString());
                    //x string sEpoch = epoch.ToString().PadLeft(6);
                    //x Console.Write("epoch = " + sEpoch);
                    //x Console.Write(" error - " + bestError.ToString("F14"));
                    //x Console.WriteLine(" dt - " + Math.Round((timer.Elapsed.TotalSeconds - lastTime.TotalSeconds), 2) + " sec");
                    //x lastTime = timer.Elapsed;
                }

                /* An alternative to a fixed number of iterations is to break
                 *  when the value of bestError drops below some small threshold value (0.00001 is common). */
                // Each firefly is compared with all other fireflies using nested for loops:
                for (int i = 0; i < NumberFireflys; ++i)
                {
                    for (int j = 0; j < NumberFireflys; ++j)
                    {
                        if (Swarm[i].Intensity < Swarm[j].Intensity)
                        {
                            // Move firefly[i] toward firefl[j]
                            /* In order to move a firefly toward another firefly with higher intesnity,
                             *  first the attractiveness must be calculated. */
                            // Distance returns the Euclidean distance between two positions.
                            /* Beta uses squared distance, which is the inverse of the square root operation (from Distance),
                             *  which enables different measures of distance to be used. */
                            double r = Distance(Swarm[i], Swarm[j]);
                            double beta = B0 * Math.Exp(-g * r * r); // defines attraction

                            // The actual movement.
                            for (int k = 0; k < Dim; ++k)
                            {
                                // The kth omponent of the position of firefly[i] is moved a beta-fraction of the distance between i and j toward j.
                                Swarm[i][k] += beta * (Swarm[j][k] - Swarm[i][k]);

                                // Then a small random term is added to each kth position component to help prevent the algorithm from getting stuck in non-optimal solutions.
                                Swarm[i][k] += a * (rnd.NextDouble() - 0.5);

                                // Each position component is checked to see if it went out of range, and if so, a random in-range value is assigned.
                                if (Swarm[i][k] < minX)
                                {
                                    Swarm[i][k] = (maxX - minX) * rnd.NextDouble() + minX;
                                }
                                if (Swarm[i][k] > maxX)
                                {
                                    Swarm[i][k] = (maxX - minX) * rnd.NextDouble() + minX;
                                }
                            }
                        }
                    }
                }

                /* After each pair of fireflies has been compared and less intense fireflies have moved toward more intense fireflies,
                 *  the array of FireFly objects is sorted from low error to high error so that the best one is at Swarm[0]. */
                /* Sorting the array of FireFly objects also has the imporant effect of changing their locaiton within the array
                 *  so that the objects are processed in a diferent order each time through th ewhile loop. */
                Swarm.Sort();
                if (Swarm[0].Value < Best.Value)
                {
                    Best.Move(Swarm[0].Position());
                }
                ++epoch;
            }
            return Best.Clone();
        }
    }
}