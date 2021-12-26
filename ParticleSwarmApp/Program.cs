using System;

namespace ParticleSwarmApp
{
    class Program
    {
        static Random ran = null;
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("\nBegin Particle Swarm Optimization demonstration\n");
                Console.WriteLine("\nObjective function to minimize has dimension = 2");
                Console.WriteLine("Objective function is f(x) = 3 + (x0^2 + x1^2)");

                ran = new Random(0);

                int numberParticles = 10;
                int numberIterations = 1000;
                int iteration = 0;
                int Dim = 2; // dimensions
                double minX = -100.0;
                double maxX = 100.0;

                Console.WriteLine("Range for all x values is " + minX + " <= x <= " + maxX);
                Console.WriteLine("\nNumber iterations = " + numberIterations);
                Console.WriteLine("Number particles in swarm = " + numberParticles);

                Particle[] swarm = new Particle[numberParticles];
                double[] bestGlobalPosition = new double[Dim]; // best solution found by any particle in the swarm. implicit initialization to all 0.0
                double bestGlobalFitness = double.MaxValue; // smaller values better

                double minV = -1.0 * maxX;
                double maxV = maxX;

                Console.WriteLine("\nInitializing swarm with random positions/solutions");
                for (int i = 0; i < swarm.Length; ++i) // initialize each Particle in the swarm
                {
                    double[] randomPosition = new double[Dim];
                    for (int j = 0; j < randomPosition.Length; ++j)
                    {
                        double lo = minX;
                        double hi = maxX;
                        randomPosition[j] = (hi - lo) * ran.NextDouble() + lo; // 
                    }
                    //double fitness = SphereFunction(randomPosition); // smaller values are better
                    //double fitness = GP(randomPosition); // smaller values are better
                    double fitness = ObjectiveFunction(randomPosition);
                    double[] randomVelocity = new double[Dim];

                    for (int j = 0; j < randomVelocity.Length; ++j)
                    {
                        double lo = -1.0 * Math.Abs(maxX - minX);
                        double hi = Math.Abs(maxX - minX);
                        randomVelocity[j] = (hi - lo) * ran.NextDouble() + lo;
                    }
                    swarm[i] = new Particle(randomPosition, fitness, randomVelocity, randomPosition, fitness);

                    // does current Particle have global best position/solution?
                    if (swarm[i].fitness < bestGlobalFitness)
                    {
                        bestGlobalFitness = swarm[i].fitness;
                        swarm[i].position.CopyTo(bestGlobalPosition, 0);
                    }
                } // initialization

                Console.WriteLine("\nInitialization complete");
                Console.WriteLine("Initial best fitness = " + bestGlobalFitness.ToString("F4"));
                Console.WriteLine("Best initial position/solution:");
                for (int i = 0; i < bestGlobalPosition.Length; ++i)
                {
                    Console.WriteLine("x" + i + " = " + bestGlobalPosition[i].ToString("F4") + " ");
                }

                double w = 0.729; // inertia weight. see http://ieeexplore.ieee.org/stamp/stamp.jsp?arnumber=00870279
                double c1 = 1.49445; // cognitive/local weight
                double c2 = 1.49445; // social/global weight
                double r1, r2; // cognitive and social randomizations

                Console.WriteLine("\nEntering main PSO processing loop");
                while (iteration < numberIterations)
                {
                    ++iteration;
                    double[] newVelocity = new double[Dim];
                    double[] newPosition = new double[Dim];
                    double newFitness;

                    for (int i = 0; i < swarm.Length; ++i) // each Particle
                    {
                        Particle currP = swarm[i];

                        for (int j = 0; j < currP.velocity.Length; ++j) // each x value of the velocity
                        {
                            r1 = ran.NextDouble();
                            r2 = ran.NextDouble();

                            newVelocity[j] = (w * currP.velocity[j]) +
                              (c1 * r1 * (currP.bestPosition[j] - currP.position[j])) +
                              (c2 * r2 * (bestGlobalPosition[j] - currP.position[j]));

                            if (newVelocity[j] < minV)
                                newVelocity[j] = minV;
                            else if (newVelocity[j] > maxV)
                                newVelocity[j] = maxV;
                        }

                        newVelocity.CopyTo(currP.velocity, 0);

                        for (int j = 0; j < currP.position.Length; ++j)
                        {
                            newPosition[j] = currP.position[j] + newVelocity[j];
                            if (newPosition[j] < minX)
                                newPosition[j] = minX;
                            else if (newPosition[j] > maxX)
                                newPosition[j] = maxX;
                        }

                        newPosition.CopyTo(currP.position, 0);
                        newFitness = ObjectiveFunction(newPosition);
                        currP.fitness = newFitness;

                        if (newFitness < currP.bestFitness)
                        {
                            newPosition.CopyTo(currP.bestPosition, 0);
                            currP.bestFitness = newFitness;
                        }

                        if (newFitness < bestGlobalFitness)
                        {
                            newPosition.CopyTo(bestGlobalPosition, 0);
                            bestGlobalFitness = newFitness;
                        }

                    } // each Particle

                    Console.WriteLine(swarm[0].ToString());
                    Console.ReadLine();

                } // while

                Console.WriteLine("\nProcessing complete");
                Console.Write("Final best fitness = ");
                Console.WriteLine(bestGlobalFitness.ToString("F4"));
                Console.WriteLine("Best position/solution:");
                for (int i = 0; i < bestGlobalPosition.Length; ++i)
                {
                    Console.Write("x" + i + " = ");
                    Console.WriteLine(bestGlobalPosition[i].ToString("F4") + " ");
                }
                Console.WriteLine("");

                Console.WriteLine("\nEnd PSO demonstration\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error: " + ex.Message);
                Console.ReadLine();
            }
        } // Main()

        static double ObjectiveFunction(double[] x)
        {
            return 3.0 + (x[0] * x[0]) + (x[1] * x[1]); // f(x) = 3 + x^2 + y^2
        }

    } // class Program

    public class Particle
    {
        public double[] position; // equivalent to x-Values and/or solution
        public double fitness;
        public double[] velocity;

        public double[] bestPosition; // best position found so far by this Particle
        public double bestFitness;

        public Particle(double[] position, double fitness, double[] velocity, double[] bestPosition, double bestFitness)
        {
            this.position = new double[position.Length];
            position.CopyTo(this.position, 0);
            this.fitness = fitness;
            this.velocity = new double[velocity.Length];
            velocity.CopyTo(this.velocity, 0);
            this.bestPosition = new double[bestPosition.Length];
            bestPosition.CopyTo(this.bestPosition, 0);
            this.bestFitness = bestFitness;
        }

        public override string ToString()
        {
            string s = "";
            s += "==========================\n";
            s += "Position: ";
            for (int i = 0; i < this.position.Length; ++i)
                s += this.position[i].ToString("F2") + " ";
            s += "\n";
            s += "Fitness = " + this.fitness.ToString("F4") + "\n";
            s += "Velocity: ";
            for (int i = 0; i < this.velocity.Length; ++i)
                s += this.velocity[i].ToString("F2") + " ";
            s += "\n";
            s += "Best Position: ";
            for (int i = 0; i < this.bestPosition.Length; ++i)
                s += this.bestPosition[i].ToString("F2") + " ";
            s += "\n";
            s += "Best Fitness = " + this.bestFitness.ToString("F4") + "\n";
            s += "==========================\n";
            return s;
        }

    } // class Particle

} // ns
