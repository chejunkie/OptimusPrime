using Optimization.Infrastructure;

namespace Optimization.Tests
{
    public class SomeFunction : IObjectiveFunction
    {
        public int Dim => 2;

        public double EvaluateAt(double[] point)
        {
            // f(x) = 3 + x^2 + y^2
            return 3.0 + (point[0] * point[0]) + (point[1] * point[1]);
        }
    }
}