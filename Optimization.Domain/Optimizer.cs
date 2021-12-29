using Optimus.Core;

namespace Optimus.Domain
{
    public abstract class Optimizer : IOptimizer
    {
        private double _tolerance;
        private int _sigFigs;
        private static readonly long NegativeZeroBits = BitConverter.DoubleToInt64Bits(-0.0);

        public Optimizer(IObjectiveFunction aux)
        {
            Tolerance = 0.00000001; // reasonable for most high-precision work.
            ObjectiveFunction = aux;
        }

        public IObjectiveFunction ObjectiveFunction { get; private set; }

        public int SigFigs { get { return _sigFigs; } }

        public double Tolerance
        {
            get
            {
                return _tolerance;
            }
            set
            {
                // Calculate machine percision eps.
                double eps = 1.0d;
                do
                {
                    eps /= 2.0d;
                }
                while ((double)(1.0 + eps) != 1.0);

                // Set tolerance, making sure it is below eps.
                if ((value > eps) && (value <= 0.1))
                {
                    _tolerance = value; // accept value
                }
                else
                {
                    _tolerance = Math.Sqrt(eps); // use default value instead
                }

                // Base SigFigs on solution tolerance:
                _sigFigs = 0;
                double dbl = value;
                do
                {
                    _sigFigs++;
                    dbl *= 10;
                } while (dbl < 0.1);
            }
        }

        public ISolution FormatSolution(ISolution solution)
        {
            double[] newPosition = solution.Position();
            for (int i = 0; i < solution.Length; i++)
            {
                newPosition[i] = Math.Round(solution[i], SigFigs);

                if (IsNegativeZero(newPosition[i]))
                {
                    newPosition[i] = Math.Abs(newPosition[i]);
                }
            }
            return solution.Move(newPosition);
        }

        public abstract ISolution Solve();

        private static bool IsNegativeZero(double x)
        {
            return BitConverter.DoubleToInt64Bits(x) == NegativeZeroBits;
        }
    }
}