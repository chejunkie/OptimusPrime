using System;
namespace MultiSwarm
{
    class MultiSwarmProgram
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(
                  "\nBegin Multiple Particle Swarm optimization demo\n");

                // Input parameters:
                int dim = 2;            // number of dimensions in the problem to be solved.
                double minX = -100.0;   // low bound constraint
                double maxX = 100.0;    // high bound constraint
                int numParticles = 12;   // particles in each swarm
                int numSwarms = 5;      // swarms in multi-swarm

                // Swarm = collection of particles.
                // Multi-swarm = collection of swarms (of particles).
                Multiswarm ms = new Multiswarm(numSwarms, numParticles, dim, minX, maxX);

                Console.WriteLine("\nInitial multiswarm:");
                Console.WriteLine(ms.ToString());

                int maxLoop = 150;
                ms.Solve(maxLoop);
                
                Console.WriteLine("\nFinal multiswarm:");
                Console.WriteLine(ms.ToString());
                Console.WriteLine("\nBest solution found = " + ms.BestMultiCost.ToString("F6"));
                Console.Write("at x0 = " + ms.BestMultiPos[0].ToString("F4"));
                Console.WriteLine(", x1 = " + ms.BestMultiPos[1].ToString("F4"));
                Console.WriteLine("\nEnd demo\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public static double Cost(double[] position)
        {
            double result = 0.0;
            for (int i = 0; i < position.Length; ++i)
            {
                double xi = position[i];
                result += (xi * xi) - (10 * Math.Cos(2 * Math.PI * xi)) + 10;
            }
            return result;
        }

        //public static double Cost(double[] position)
        //{
        //    double x = position[0];
        //    double y = position[1];
        //    double value = 100.0 * Math.Pow((y - x * x), 2) + Math.Pow(1 - x, 2);
        //    return value;
        //}

    }

    public class Particle
    {
        static Random ran = new Random(0);
        public double[] Position;
        public double[] Velocity;
        public double Cost;
        public double[] BestPartPos;
        public double BestPartCost;

        public Particle(int dim, double minX, double maxX)
        {
            Position = new double[dim];
            Velocity = new double[dim];
            BestPartPos = new double[dim];
            for (int i = 0; i < dim; ++i)
            {
                Position[i] = (maxX - minX) * ran.NextDouble() + minX;
                Velocity[i] = (maxX - minX) * ran.NextDouble() + minX;
            }
            Cost = MultiSwarmProgram.Cost(Position);
            BestPartCost = Cost;
            Array.Copy(Position, BestPartPos, dim);
        }

        public override string ToString()
        {
            string s = "";
            s += "Pos [ ";
            for (int i = 0; i < Position.Length; ++i)
                s += Position[i].ToString("F2") + " ";
            s += "] ";
            s += "Vel [ ";
            for (int i = 0; i < Velocity.Length; ++i)
                s += Velocity[i].ToString("F2") + " ";
            s += "] ";
            s += "Cost = " + Cost.ToString("F3");
            s += " Best Pos [ ";
            for (int i = 0; i < BestPartPos.Length; ++i)
                s += BestPartPos[i].ToString("F2") + " ";
            s += "] ";
            s += "BestCost = " + Cost.ToString("F3");
            return s;
        }
    }

    public class Swarm
    {
        public Particle[] Particles;
        public double[] BestSwarmPos;
        public double BestSwarmCost;

        public Swarm(int numParticles, int dim, double minX, double maxX)
        {
            BestSwarmCost = double.MaxValue;
            BestSwarmPos = new double[dim];
            Particles = new Particle[numParticles];
            for (int i = 0; i < Particles.Length; ++i)
            {
                Particles[i] = new Particle(dim, minX, maxX);
                if (Particles[i].Cost < BestSwarmCost)
                {
                    BestSwarmCost = Particles[i].Cost;
                    Array.Copy(Particles[i].Position, BestSwarmPos, dim);
                }
            }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Particles.Length; ++i)
                s += "[" + i + "] " + Particles[i].ToString() + "\n";
            s += "Best Swarm Pos [ ";
            for (int i = 0; i < BestSwarmPos.Length; ++i)
                s += BestSwarmPos[i].ToString("F2") + " ";
            s += "] ";
            s += "Best Swarm Cost = " + BestSwarmCost.ToString("F3");
            s += "\n";
            return s;
        }
    }

    public class Multiswarm
    {
        public Swarm[] Swarms;
        public double[] BestMultiPos;
        public double BestMultiCost;
        public int Dim;
        public double MinX;
        public double MaxX;
        static Random ran = new Random(0);

        public Multiswarm(int numSwarms, int numParticles, int dim, double minX, double maxX)
        {
            Swarms = new Swarm[numSwarms];
            BestMultiPos = new double[dim];
            BestMultiCost = double.MaxValue;
            this.Dim = dim;
            this.MinX = minX;
            this.MaxX = maxX;
            for (int i = 0; i < numSwarms; ++i)
            {
                Swarms[i] = new Swarm(numParticles, dim, minX, maxX);
                if (Swarms[i].BestSwarmCost < BestMultiCost)
                {
                    BestMultiCost = Swarms[i].BestSwarmCost;
                    Array.Copy(Swarms[i].BestSwarmPos, BestMultiPos, dim);
                }
            }
        }

        public void Solve(int maxLoop)
        {
            int ct = 0;
            //? double wMin = 0.5;
            //? double wMax = 1;
            double w = 0.729; // inertia
            double c1 = 1.49445; // particle / cogntive
            double c2 = 1.49445; // swarm / social
            double c3 = 0.3645; // multiswarm / global
            double death = 0.005; ; // prob of particle death
            double immigrate = 0.005;  // prob of particle immigration

            while (ct < maxLoop)
            {
                ++ct;
                for (int i = 0; i < Swarms.Length; ++i) // each swarm
                {
                    for (int j = 0; j < Swarms[i].Particles.Length; ++j) // each particle
                    {
                        double p = ran.NextDouble();
                        if (p < death)
                        {
                            Swarms[i].Particles[j] = new Particle(Dim, MinX, MaxX);
                        }

                        double q = ran.NextDouble();
                        if (q < immigrate)
                        {
                            Immigration(i, j); // swap curr particle with a random particle in diff swarm
                        }

                        //! automatically scale inertia weight
                        //? w = wMax - (wMax - wMin) * (ct + i * 25 + j) / (500 + 5 * 25);

                        for (int k = 0; k < Dim; ++k) // update velocity. each x position component
                        {
                            double r1 = ran.NextDouble();
                            double r2 = ran.NextDouble();
                            double r3 = ran.NextDouble();

                            Swarms[i].Particles[j].Velocity[k] = (w * Swarms[i].Particles[j].Velocity[k]) +
                              (c1 * r1 * (Swarms[i].Particles[j].BestPartPos[k] - Swarms[i].Particles[j].Position[k])) +
                              (c2 * r2 * (Swarms[i].BestSwarmPos[k] - Swarms[i].Particles[j].Position[k])) +
                              (c3 * r3 * (BestMultiPos[k] - Swarms[i].Particles[j].Position[k]));

                            if (Swarms[i].Particles[j].Velocity[k] < MinX)
                                Swarms[i].Particles[j].Velocity[k] = MinX;
                            else if (Swarms[i].Particles[j].Velocity[k] > MaxX)
                                Swarms[i].Particles[j].Velocity[k] = MaxX;

                        }

                        for (int k = 0; k < Dim; ++k) // update position
                        {
                            Swarms[i].Particles[j].Position[k] += Swarms[i].Particles[j].Velocity[k];
                        }

                        // update cost
                        Swarms[i].Particles[j].Cost = MultiSwarmProgram.Cost(Swarms[i].Particles[j].Position);

                        // check if new best cost
                        if (Swarms[i].Particles[j].Cost < Swarms[i].Particles[j].BestPartCost)
                        {
                            Swarms[i].Particles[j].BestPartCost = Swarms[i].Particles[j].Cost;
                            Array.Copy(Swarms[i].Particles[j].Position, Swarms[i].Particles[j].BestPartPos, Dim);
                        }

                        if (Swarms[i].Particles[j].Cost < Swarms[i].BestSwarmCost)
                        {
                            Swarms[i].BestSwarmCost = Swarms[i].Particles[j].Cost;
                            Array.Copy(Swarms[i].Particles[j].Position, Swarms[i].BestSwarmPos, Dim);
                        }

                        if (Swarms[i].Particles[j].Cost < BestMultiCost)
                        {
                            BestMultiCost = Swarms[i].Particles[j].Cost;
                            Array.Copy(Swarms[i].Particles[j].Position, BestMultiPos, Dim);
                        }
                    }
                }
            }
        }

        private void Immigration(int i, int j)
        {
            // Swap particle j in swarm i
            // with a random particle in a random swarm
            int otheri = ran.Next(0, Swarms.Length);
            int otherj = ran.Next(0, Swarms[0].Particles.Length);
            Particle tmp = Swarms[i].Particles[j];
            Swarms[i].Particles[j] = Swarms[otheri].Particles[otherj];
            Swarms[otheri].Particles[otherj] = tmp;
        }
    }
}