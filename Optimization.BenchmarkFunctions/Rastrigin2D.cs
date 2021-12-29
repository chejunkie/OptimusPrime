using Optimus.Core;

namespace Optimus.TestFunctions
{
    public class Rastrigin2D : IObjectiveFunction
    {
        public int Dim => 2;

        public double EvaluateAt(double[] position)
        {
            double result = 0.0;
            for (int i = 0; i < position.Length; ++i)
            {
                double xi = position[i];
                result += (xi * xi) - (10 * Math.Cos(2 * Math.PI * xi)) + 10;
            }
            return result;
        }
    }
}