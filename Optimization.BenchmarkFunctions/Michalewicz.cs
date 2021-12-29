using Optimus.Core;

namespace Optimus.TestFunctions
{
    public class Michalewicz : IObjectiveFunction
    {
        public int Dim => 5;

        public double EvaluateAt(double[] point)
        {
            double result = 0.0;
            for (int i = 0; i < point.Length; ++i)
            {
                double a = Math.Sin(point[i]);
                double b = Math.Sin(((i + 1) * point[i] * point[i]) / Math.PI);
                double c = Math.Pow(b, 20);
                result += a * c;
            }
            return -1.0 * result;
        }
    }
}