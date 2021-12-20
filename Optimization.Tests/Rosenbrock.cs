using SimplexNelderMead;
using System;

namespace Optimization.Tests
{
    public class Rosenbrock : IObjectiveFunction
    {
        public double Evaluate(double[] vector)
        {
            double x = vector[0];
            double y = vector[1];
            double aux = 100.0 * Math.Pow((y - x * x), 2) + Math.Pow(1 - x, 2);
            return aux; // fitness value
        }
    }
}