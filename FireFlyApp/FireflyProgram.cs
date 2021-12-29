using System.Diagnostics;

namespace FireFlyApp
{
    internal class FireflyProgram
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Begin firefly demo\n");

            // Purpose
            Console.WriteLine("Goal is to solve the Michalewicz benchmark function");
            Console.WriteLine("The function has a known minimum value of -4.687658");
            Console.WriteLine("x = 2.2029 1.5707 1.2850 1.9231 1.7205");

            // Parameters needed for FA are set and displayed:
            int numFireflies = 50;
            int dim = 5;
            int maxEpochs = 5000;
            int seed = 0;

            Console.WriteLine("Setting numFireflies = " + numFireflies);
            Console.WriteLine("Setting problem dim = " + dim);
            Console.WriteLine("Setting maxEpochs = " + maxEpochs);
            Console.WriteLine("Setting initialization seed = " + seed);

            // FA is invoked like so:
            Console.WriteLine("Starting firefly algorithm");
            double[] bestPosition = Solve(numFireflies, dim, seed, maxEpochs);
            Console.WriteLine("Finished");

            // The main method concludes by displaying the FA results:
            Console.WriteLine("Best solution found: ");
            Console.Write("x = ");
            ShowVector(bestPosition, 4, true);
            double z = Michalewicz(bestPosition);
            Console.Write("Value of function at best position = ");
            Console.WriteLine(z.ToString("F6"));
            double error = Error(bestPosition);
            Console.Write("Error at best position = ");
            Console.WriteLine(error.ToString("F4"));

            Console.WriteLine("End firefly demo");
            Console.ReadLine();
        }

        private static void ShowVector(double[] v, int dec, bool nl)
        {
            for (int i = 0; i < v.Length; ++i)
                Console.Write(v[i].ToString("F" + dec) + " ");
            if (nl == true)
                Console.WriteLine("");
        }

        private static double[] Solve(int numFireflies, int dim, int seed, int maxEpochs)
        {
            // minX and maxX establish boundaries for each firefly's position.
            // The values used here are specific to Michalewicz function.
            // For macine learning optimizatino with normalized data, values of -10.0 and +10.0 are common.
            Random rnd = new Random(seed);
            double minX = 0.0;
            double maxX = 3.2;

            // B0 (base beta), g (gamma) and a (alpha) control the attractiveness of one firefly to another.
            // The values used (1.0, 1.0, and 0.20) were recommended by the source research paper.
            double B0 = 1.0;
            double g = 1.0;
            double a = 0.20;

            // Control how often to display a progress message:
            int displayInterval = maxEpochs / 10;

            // Empty swarm of fireflys are created.
            double bestError = double.MaxValue;
            double[] bestPosition = new double[dim];     // best ever
            Firefly[] swarm = new Firefly[numFireflies]; // initially all null.

            /* A FireFly object is program-defined and encapsulates a position,
             *  an associated error and the corresponding intensity. */
            // Next, the swarm is instantiated and placed at random positions.
            for (int i = 0; i < numFireflies; ++i)
            {
                swarm[i] = new Firefly(dim);
                for (int k = 0; k < dim; ++k) // random position
                {
                    swarm[i].Position[k] = (maxX - minX) * rnd.NextDouble() + minX;
                }
                swarm[i].Error = Error(swarm[i].Position);
                swarm[i].Intensity = 1 / (swarm[i].Error + 1);
                if (swarm[i].Error < bestError)
                {
                    bestError = swarm[i].Error;
                    for (int k = 0; k < dim; ++k)
                    {
                        bestPosition[k] = swarm[i].Position[k];
                    }
                }
            }

            // Main processing loop begins here.
            int epoch = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            TimeSpan lastTime = timer.Elapsed;
            while (epoch < maxEpochs)
            {
                if (epoch % displayInterval == 0 && epoch < maxEpochs)
                {
                    string sEpoch = epoch.ToString().PadLeft(6);
                    Console.Write("epoch = " + sEpoch);
                    Console.Write(" error - " + bestError.ToString("F14"));
                    Console.WriteLine(" dt - " + Math.Round((timer.Elapsed.TotalSeconds - lastTime.TotalSeconds), 2) + " sec");
                    lastTime = timer.Elapsed;
                }

                /* An alternative to a fixed number of iterations is to break
                 *  when the value of bestError drops below some small threshold value (0.00001 is common). */
                // Each firefly is compared with all other fireflies using nested for loops:
                for (int i = 0; i < numFireflies; ++i)
                {
                    for (int j = 0; j < numFireflies; ++j)
                    {
                        if (swarm[i].Intensity < swarm[j].Intensity)
                        {
                            // Move firefly[i] toward firefl[j]
                            /* In order to move a firefly toward another firefly with higher intesnity,
                             *  first the attractiveness must be calculated. */
                            // Distance returns the Euclidean distance between two positions.
                            /* Beta uses squared distance, which is the inverse of the square root operation (from Distance),
                             *  which enables different measures of distance to be used. */
                            double r = Distance(swarm[i].Position, swarm[j].Position);
                            double beta = B0 * Math.Exp(-g * r * r); // defines attraction

                            // The actual movement.
                            for (int k = 0; k < dim; ++k)
                            {
                                // The kth omponent of the position of firefly[i] is moved a beta-fraction of the distance between i and j toward j.
                                swarm[i].Position[k] += beta * (swarm[j].Position[k] - swarm[i].Position[k]);

                                // Then a small random term is added to each kth position component to help prevent the algorithm from getting stuck in non-optimal solutions.
                                swarm[i].Position[k] += a * (rnd.NextDouble() - 0.5);

                                // Each position component is checked to see if it went out of range, and if so, a random in-range value is assigned.
                                if (swarm[i].Position[k] < minX)
                                {
                                    swarm[i].Position[k] = (maxX - minX) * rnd.NextDouble() + minX;
                                }
                                if (swarm[i].Position[k] > maxX)
                                {
                                    swarm[i].Position[k] = (maxX - minX) * rnd.NextDouble() + minX;
                                }

                                // Now, the error and intensity of the just-moved firefly must be updated:
                                swarm[i].Error = Error(swarm[i].Position);
                                swarm[i].Intensity = 1 / (swarm[i].Error + 1);
                            }
                        }
                    }
                }

                /* After each pair of fireflies has been compared and less intense fireflies have moved toward more intense fireflies,
                 *  the array of FireFly objects is sorted from low error to high error so that the best one is at swarm[0]. */
                /* Sorting the array of FireFly objects also has the imporant effect of changing their locaiton within the array
                 *  so that the objects are processed in a diferent order each time through th ewhile loop. */
                Array.Sort(swarm); // low to high error.
                if (swarm[0].Error < bestError)
                {
                    bestError = swarm[0].Error;
                    for (int k = 0; k < dim; ++k)
                    {
                        bestPosition[k] = swarm[0].Position[k];
                    }
                }
                ++epoch;
            }
            return bestPosition;
        }

        private static double Distance(double[] posA, double[] posB)
        {
            double ssd = 0.0; // sum squared diffrences
            for (int i = 0; i < posA.Length; ++i)
                ssd += (posA[i] - posB[i]) * (posA[i] - posB[i]);
            return Math.Sqrt(ssd);
        }

        private static double Michalewicz(double[] xValues)
        {
            double result = 0.0;
            for (int i = 0; i < xValues.Length; ++i)
            {
                double a = Math.Sin(xValues[i]);
                double b = Math.Sin(((i + 1) * xValues[i] * xValues[i]) / Math.PI);
                double c = Math.Pow(b, 20);
                result += a * c;
            }
            return -1.0 * result;
        }

        private static double Error(double[] xValues)
        {
            int dim = xValues.Length;
            double trueMin = 0.0;
            if (dim == 2)
                trueMin = -1.8013; // Approx.
            else if (dim == 5)
                trueMin = -4.687658; // Approx.
            double calculated = Michalewicz(xValues);
            return (trueMin - calculated) * (trueMin - calculated);
        }
    }
}