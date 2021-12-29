using Optimization.Infrastructure;

namespace SimplexNelderMead
{
    public class VertexSolution : Solution
    {
        private static Random Randomizer = new Random(0);  // to allow creation of random solutions

        public VertexSolution(IObjectiveFunction aux, double[] vector) : base(aux)
        {
            CopyFrom(vector);
        }

        public VertexSolution(IObjectiveFunction aux, int dim, double minX, double maxX) : base(aux)
        {
            // a random Solution
            double[] point = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                point[i] = (maxX - minX) * Randomizer.NextDouble() + minX;
            }
            CopyFrom(point);
        }

        //public VertexSolution(IObjectiveFunction aux, int dim, double[] minX, double[] maxX) : base(aux)
        //{
        //    double[] point = new double[dim];
        //    for (int i = 0; i < dim; i++)
        //    {
        //        point[i] = (maxX[i] - minX[i]) * Randomizer.NextDouble() + minX[i];
        //    }
        //    CopyFrom(point);
        //}

        public override VertexSolution Clone()
        {
            return new VertexSolution(ObjectiveFunction, (double[])Vector.Clone());
        }
    }
}