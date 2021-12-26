using Optimization.Infrastructure;
using System;
using System.Numerics;

namespace Optimization.Tests
{
    public class Rosenbrock : IObjectiveFunction
    {
        public int Dim => 2;

        public double EvaluateAt(double[] point)
        {
            double x = point[0];
            double y = point[1];
            double value = 100.0 * Math.Pow((y - x * x), 2) + Math.Pow(1 - x, 2);
            return value;
        }
    }
}