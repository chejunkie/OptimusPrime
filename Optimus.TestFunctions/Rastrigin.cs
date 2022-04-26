using Optimus.Core;
using System;

namespace Optimus.TestFunctions
{
    public class Rastrigin : IObjectiveFunction
    {
        // Minimum = 0, x = (0...0)

        private readonly bool EvaluateError = false;

        public Rastrigin(bool evaluateError = false)
        {
            EvaluateError = evaluateError;
        }

        public double ErrorAt(double[] position)
        {
            double calculated = RastriginFunction(position);
            double trueMin = GlobalMinimum;
            return (trueMin - calculated) * (trueMin - calculated);
        }


        public double EvaluateAt(double[] position)
        {
            if (true == EvaluateError)
            {
                return ErrorAt(position);
            }
            return RastriginFunction(position);
        }

        public double GlobalMinimum => 0;

        public double[] GlobalPosition(int dim)
        {
            double[] position = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                position[i] = 0;
            }
            return position;
        }

        private double RastriginFunction(double[] position)
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