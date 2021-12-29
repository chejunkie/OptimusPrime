using Optimus.Core;

namespace Optimus.TestFunctions
{
    public class Rastrigin3D : IObjectiveFunction
    {
        public int Dim => 3;

        public double EvaluateAt(double[] point)
        {
            double result = 0.0;
            for (int i = 0; i < point.Length; ++i)
            {
                double xi = point[i];
                result += (xi * xi) - (10 * Math.Cos(2 * Math.PI * xi)) + 10;
            }
            return result;
        }
    }
}