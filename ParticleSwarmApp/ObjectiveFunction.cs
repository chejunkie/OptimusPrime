using Optimization.Infrastructure;

namespace ParticleSwarmApp
{
    public static class ObjectiveFunction //x: IObjectiveFunction
    {
        public static double EvaluateAt(double[] x)
        {
            return 3.0 + (x[0] * x[0]) + (x[1] * x[1]); // f(x) = 3 + x^2 + y^2
        }
    }
}