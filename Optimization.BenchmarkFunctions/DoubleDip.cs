using Optimus.Core;

namespace Optimus.TestFunctions
{
    public class DoubleDip : IObjectiveFunction
    {
        public int Dim => 2;

        public double EvaluateAt(double[] point)
        {
            return point[0] * Math.Exp(-(point[0] * point[0] + point[1] * point[1]));
        }
    }
}