using System.Diagnostics;

namespace SimplexNelderMead
{
    public class Amoeba
    {
        private int AmoebaSize;  // number of solutions
        private int Dim;         // vector-solution size, also problem dimension
        private Solution[] Solutions;  // potential solutions (vector + value)

        private double MinX;
        private double MaxX;

        private const double ALPHA = 1.0;   // reflection
        private const double BETA = 0.5;    // contraction
        private const double GAMMA = 2;     // expansion

        private double _tolerance;
        private int _sigFigs;

        private int maxLoop;   // limits main solving loop

        public Amoeba(IObjectiveFunction aux, int amoebaSize, int dim, double minX, double maxX, int maxLoop)
        {
            ObjectiveFunction = aux;
            this.AmoebaSize = amoebaSize;
            this.Dim = dim;
            this.MinX = minX;
            this.MaxX = maxX;
            this.maxLoop = maxLoop;
            Tolerance = 0.00000001;

            this.Solutions = new Solution[amoebaSize];
            for (int i = 0; i < Solutions.Length; ++i)
                Solutions[i] = new Solution(aux, dim, minX, maxX);  // the Solution ctor calls the objective function to compute value

            Array.Sort(Solutions);
        }

        public IObjectiveFunction ObjectiveFunction { get; private set; }

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
                    _tolerance = value;
                }
                else
                {
                    _tolerance = Math.Sqrt(eps); // default
                }

                // Significant figures is based on the solution tolerance:
                _sigFigs = 0;
                double dbl = value;
                do
                {
                    _sigFigs++;
                    dbl *= 10;
                } while (dbl < 0.1);
            }
        }

        private Solution Centroid()
        {
            // return the centroid of all solution vectors except for the worst (highest index) vector
            double[] c = new double[Dim];
            for (int i = 0; i < AmoebaSize - 1; ++i)
                for (int j = 0; j < Dim; ++j)
                    c[j] += Solutions[i].Vector[j];  // accumulate sum of each vector component

            for (int j = 0; j < Dim; ++j)
                c[j] = c[j] / (AmoebaSize - 1);

            Solution s = new Solution(ObjectiveFunction, c);  // feed vector to ctor which calls objective function to compute value
            return s;
        }

        private Solution Reflected(Solution centroid)
        {
            // the reflected solution extends from the worst (lowest index) solution through the centroid
            double[] r = new double[Dim];
            double[] worst = this.Solutions[AmoebaSize - 1].Vector;  // convenience only
            for (int j = 0; j < Dim; ++j)
                r[j] = ((1 + ALPHA) * centroid.Vector[j]) - (ALPHA * worst[j]);
            Solution s = new Solution(ObjectiveFunction, r);
            return s;
        }

        private Solution Expanded(Solution reflected, Solution centroid)
        {
            // expanded extends even more, from centroid, thru reflected
            double[] e = new double[Dim];
            for (int j = 0; j < Dim; ++j)
                e[j] = (GAMMA * reflected.Vector[j]) + ((1 - GAMMA) * centroid.Vector[j]);
            Solution s = new Solution(ObjectiveFunction, e);
            return s;
        }

        private Solution Contracted(Solution centroid)
        {
            // contracted extends from worst (lowest index) towards centoid, but not past centroid
            double[] v = new double[Dim];  // didn't want to reuse 'c' from centoid routine
            double[] worst = this.Solutions[AmoebaSize - 1].Vector;  // convenience only
            for (int j = 0; j < Dim; ++j)
                v[j] = (BETA * worst[j]) + ((1 - BETA) * centroid.Vector[j]);
            Solution s = new Solution(ObjectiveFunction, v);
            return s;
        }

        private void Shrink()
        {
            // move all vectors, except for the best vector (at index 0), halfway to the best vector
            // compute new objective function values and sort result
            for (int i = 1; i < AmoebaSize; ++i)  // note we don't start at [0]
            {
                for (int j = 0; j < Dim; ++j)
                {
                    Solutions[i].Vector[j] = (Solutions[i].Vector[j] + Solutions[0].Vector[j]) / 2.0;
                    Solutions[i].Value = ObjectiveFunction.Evaluate(Solutions[i].Vector);
                }
            }
            Array.Sort(Solutions);
        }

        private void ReplaceWorst(Solution newSolution)
        {
            // replace the worst solution (at index size-1) with contents of parameter newSolution's vector
            for (int j = 0; j < Dim; ++j)
                Solutions[AmoebaSize - 1].Vector[j] = newSolution.Vector[j];
            Solutions[AmoebaSize - 1].Value = newSolution.Value;
            Array.Sort(Solutions);
        }

        private bool IsWorseThanAllButWorst(Solution reflected)
        {
            // Solve needs to know if the reflected vector is worse (greater value) than every vector in the amoeba, except for the worst vector (highest index)
            for (int i = 0; i < AmoebaSize - 1; ++i)  // not the highest index (worst)
            {
                if (reflected.Value <= Solutions[i].Value)  // reflected is better (smaller value) than at least one of the non-worst solution vectors
                    return false;
            }
            return true;
        }

        private double Fitness()
        {
            double aux = (2 * Math.Abs(Solutions[AmoebaSize - 1].Value - Solutions[0].Value))
                / (Math.Abs(Solutions[AmoebaSize - 1].Value) + Math.Abs(Solutions[0].Value) + Tolerance);
            return aux;
        }

        public Solution Solve()
        {
            int t = 0;  // loop counter
            double aux = Double.MaxValue;
            while (t < maxLoop)
            {
                aux = Fitness();
                if (aux < Tolerance)
                {
                    break;
                }
                ++t;

                //if (t % 10 == 0)
                //{
                //    Debug.WriteLine("At t = " + t + " curr best solution = " + this.Solutions[0]);
                //}

                Solution centroid = Centroid();  // compute centroid
                Solution reflected = Reflected(centroid);  // compute reflected

                if (reflected.Value < Solutions[0].Value)  // reflected is better than the curr best
                {
                    Solution expanded = Expanded(reflected, centroid);  // can we do even better??
                    if (expanded.Value < Solutions[0].Value)  // winner! expanded is better than curr best
                        ReplaceWorst(expanded);  // replace curr worst solution with expanded
                    else
                        ReplaceWorst(reflected);  // it was worth a try . . .
                    continue;
                }

                if (IsWorseThanAllButWorst(reflected) == true)  // reflected is worse (larger value) than all solution vectors (except possibly the worst one)
                {
                    if (reflected.Value <= Solutions[AmoebaSize - 1].Value)  // reflected is better (smaller) than the curr worst (last index) vector
                        ReplaceWorst(reflected);

                    Solution contracted = Contracted(centroid);  // compute a point 'inside' the amoeba

                    if (contracted.Value > Solutions[AmoebaSize - 1].Value)  // contracted is worse (larger value) than curr worst (last index) solution vector
                        Shrink();
                    else
                        ReplaceWorst(contracted);

                    continue;
                }

                ReplaceWorst(reflected);

                //if (IsSorted() == false)
                //  throw new Exception("Unsorted at k = " + k);
            }  // solve loop

            for (int i = 0; i < Solutions[0].Vector.Length; i++)
            {
                Solutions[0].Vector[i] = Math.Round(Solutions[0].Vector[i], _sigFigs);
            }
            return Solutions[0];  // best solution is always at [0]
        }

        //public bool IsSorted()  // state invariant. used during development
        //{
        //  for (int i = 0; i < solutions.Length - 1; ++i)
        //    if (solutions[i].value > solutions[i + 1].value)
        //      return false;
        //  return true;
        //}

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Solutions.Length; ++i)
                s += "[" + i + "] " + Solutions[i].ToString() + Environment.NewLine;
            return s;
        }
    }
}