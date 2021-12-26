using Optimization.Infrastructure;

namespace MultiSwarm
{
    public class ParticleSolution : Solution
    {
        private static Random Randomizer = new Random(0);

        public double[]? Velocity;

        // Each particle has memory of the best position found, and associated error
        public double[]? BestPosition;
        public double BestValue;

        public ParticleSolution(IObjectiveFunction aux, double[] vector) : base(aux)
        {
            CopyFrom(vector);
            BestPosition = new double[Vector.Length];
            Vector.CopyTo(BestPosition, 0);
            BestValue = Value;
        }

        public ParticleSolution(IObjectiveFunction aux, double[] vector, double[] velocity) : this(aux, vector)
        {
            Velocity = new double[velocity.Length];
            velocity.CopyTo(Velocity, 0);
        }

        public ParticleSolution(IObjectiveFunction aux, int dim, double minX, double maxX) : base(aux)
        {
            double[] vector = new double[dim];
            double[] velocity = new double[dim];
            for (int i = 0; i < dim; ++i)
            {
                vector[i] = (maxX - minX) * Randomizer.NextDouble() + minX;
                velocity[i] = (maxX - minX) * Randomizer.NextDouble() + minX;
            }

            CopyFrom(vector);
            Velocity = new double[velocity.Length];
            velocity.CopyTo(Velocity, 0);

            BestPosition = new double[Vector.Length];
            Vector.CopyTo(BestPosition, 0);
            BestValue = Value;

            //x Cost = MultiSwarmProgram.Cost(Position);
            //x BestPartCost = Cost;
            //x Array.Copy(Position, BestPartPos, dim);
        }

        //public ParticleSolution(IObjectiveFunction aux, int dim, double minX, double maxX) : base(aux)
        //{
        //    // Random position (starting parameter values)
        //    double[] randomPosition = new double[dim];
        //    for (int j = 0; j < randomPosition.Length; ++j)
        //    {
        //        double lo = minX;
        //        double hi = maxX;
        //        randomPosition[j] = (hi - lo) * Randomizer.NextDouble() + lo;
        //    }
        //    double fitness = aux.EvaluateAt(randomPosition);

        //    /* Velocity indicates where particle will move next,
        //    /* and is added to the position. */
        //    double[] randomVelocity = new double[dim];
        //    for (int j = 0; j < randomVelocity.Length; ++j)
        //    {
        //        double lo = -1.0 * Math.Abs(maxX - minX);
        //        double hi = Math.Abs(maxX - minX);
        //        randomVelocity[j] = (hi - lo) * Randomizer.NextDouble() + lo;
        //    }
        //    CopyFrom(randomPosition);
        //    BestPosition = new double[dim];
        //    Velocity = new double[dim];
        //    randomVelocity.CopyTo(Velocity, 0);
        //    randomPosition.CopyTo(BestPosition, 0);
        //    BestValue = Value;
        //}

        public override ParticleSolution Clone()
        {
            return new ParticleSolution(ObjectiveFunction, (double[])Vector.Clone());
        }

        public override string ToString()
        {
            string s = "";
            s += "Position [ ";
            for (int i = 0; i < Length; ++i)
                s += Vector[i].ToString(VectorFormat) + " ";
            s += "] ";
            s += "Value = " + Value.ToString(ValueFormat);

            if (null != Velocity)
            {
                s += "Velocity [ ";
                for (int i = 0; i < Length; ++i)
                    s += Velocity[i].ToString("F2") + " ";
                s += "] ";
                s += " BestPosition [ ";
                for (int i = 0; i < Length; ++i)
                    s += BestPosition[i].ToString(VectorFormat) + " ";
                s += "] ";
                s += "BestValue = " + Value.ToString(ValueFormat);
            }
            return s;
        }
    }
}