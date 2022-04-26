using Optimus.Core;
using System;

namespace Optimus.TestFunctions
{
    public class Michalewicz : IObjectiveFunction
    {
        // dim = 2, global best (2.20319, 1.57049) => -1.8013
        // dim = 5, global best (2.2029 1.5707, 1.2850, 1.9231, 1.7205) => -4.6877

        private readonly bool EvaluateError = false;  

        public Michalewicz(bool evaluateError = false)
        {
            EvaluateError = evaluateError;
        }

        public double ErrorAt(double[] position)
        {
            double calculated = MichalewiczFunction(position);
            double trueMin = GlobalMinimum(position.Length);
            return (trueMin - calculated) * (trueMin - calculated);
        }

        public double EvaluateAt(double[] position)
        {
            if (true == EvaluateError)
            {
                return ErrorAt(position);
            }
            return MichalewiczFunction(position);
        }

        public double GlobalMinimum(int dim)
        {
            if (2 == dim)
            {
                return -1.8013;
            }
            if (5 == dim)
            {
                return -4.6877;
            }
            throw new NotImplementedException();
        }

        public double[] GlobalPosition(int dim)
        {
            if (2 == dim)
            {
                return new double[2] { 2.20319, 1.57049 };
            }
            if (5 == dim)
            {
                return new double[5] { 2.2029, 1.5707, 1.2850, 1.9231, 1.7205};
            }
            throw new NotImplementedException();
        }

        public double MichalewiczFunction(double[] points)
        {
            double result = 0.0;
            for (int i = 0; i < points.Length; ++i)
            {
                double a = Math.Sin(points[i]);
                double b = Math.Sin(((i + 1) * points[i] * points[i]) / Math.PI);
                double c = Math.Pow(b, 20);
                result += a * c;
            }
            return -1.0 * result;
        }
    }
}