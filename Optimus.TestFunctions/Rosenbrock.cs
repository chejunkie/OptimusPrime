using Optimus.Core;

namespace Optimus.TestFunctions
{
    public class Rosenbrock : IObjectiveFunction
    {
        // Global minimum = 0, x = (1, ..., 1)

        public double GlobalMinimum => 0;

        public double EvaluateAt(double[] point)
        {
            double x = point[0];
            double y = point[1];
            double value = 100.0 * Math.Pow((y - x * x), 2) + Math.Pow(1 - x, 2);
            return value;
        }

        public double[] GlobalPosition(int dim)
        {
            double[] position = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                position[i] = 1;
            }
            return position;
        }
    }
}