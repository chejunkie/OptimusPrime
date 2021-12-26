using Optimization.Infrastructure;
using System.Collections.ObjectModel;

namespace SimplexNelderMead
{
    public class AmoebaOptimizer : Optimizer
    {
        private const double ALPHA = 1.0;   // reflection
        private const double BETA = 0.5;    // contraction
        private const double GAMMA = 2;     // expansion

        private int AmoebaSize;  // number of solutions
        private int Dim;         // vector-solution size, also problem dimension
        private VertexSolution[] Solutions;  // potential solutions (vector + value)

        //private double MinX;
        //private double MaxX;

        private int maxLoop;   // limits main solving loop

        public AmoebaOptimizer(IObjectiveFunction aux, int amoebaSize, int dim, 
            double minX, double maxX, int maxLoop) : base(aux)
        {
            this.AmoebaSize = amoebaSize;
            this.Dim = dim;
            this.maxLoop = maxLoop;
            //? this.MinX = minX;
            //? this.MaxX = maxX;
            //? Tolerance = 0.00000001;

            this.Solutions = new VertexSolution[amoebaSize];
            for (int i = 0; i < Solutions.Length; ++i)
                Solutions[i] = new VertexSolution(aux, dim, minX, maxX);  // the Solution ctor calls the objective function to compute value

            Array.Sort(Solutions);
        }

        private VertexSolution Centroid()
        {
            // return the centroid of all solution vectors except for the worst (highest index) vector
            double[] c = new double[Dim];
            for (int i = 0; i < AmoebaSize - 1; ++i)
                for (int j = 0; j < Dim; ++j)
                    c[j] += Solutions[i].Vector[j];  // accumulate sum of each vector component

            for (int j = 0; j < Dim; ++j)
                c[j] = c[j] / (AmoebaSize - 1);

            VertexSolution s = new VertexSolution(ObjectiveFunction, c);  // feed vector to ctor which calls objective function to compute value
            return s;
        }

        private VertexSolution Reflected(VertexSolution centroid)
        {
            // the reflected solution extends from the worst (lowest index) solution through the centroid
            double[] r = new double[Dim];
            double[] worst = this.Solutions[AmoebaSize - 1].Vector;  // convenience only
            for (int j = 0; j < Dim; ++j)
                r[j] = ((1 + ALPHA) * centroid.Vector[j]) - (ALPHA * worst[j]);
            VertexSolution s = new VertexSolution(ObjectiveFunction, r);
            return s;
        }

        private VertexSolution Expanded(VertexSolution reflected, VertexSolution centroid)
        {
            // expanded extends even more, from centroid, thru reflected
            double[] e = new double[Dim];
            for (int j = 0; j < Dim; ++j)
                e[j] = (GAMMA * reflected.Vector[j]) + ((1 - GAMMA) * centroid.Vector[j]);
            VertexSolution s = new VertexSolution(ObjectiveFunction, e);
            return s;
        }

        private VertexSolution Contracted(VertexSolution centroid)
        {
            // contracted extends from worst (lowest index) towards centoid, but not past centroid
            double[] v = new double[Dim];  // didn't want to reuse 'c' from centoid routine
            double[] worst = this.Solutions[AmoebaSize - 1].Vector;  // convenience only
            for (int j = 0; j < Dim; ++j)
                v[j] = (BETA * worst[j]) + ((1 - BETA) * centroid.Vector[j]);
            VertexSolution s = new VertexSolution(ObjectiveFunction, v);
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
                    Solutions[i][j] = (Solutions[i].Vector[j] + Solutions[0].Vector[j]) / 2.0;
                    //Solutions[i].Value = ObjectiveFunction.EvaluateAt(Solutions[i].Vector);
                }
            }
            Array.Sort(Solutions);
        }

        private void ReplaceWorst(VertexSolution newSolution)
        {
            // replace the worst solution (at index size-1) with contents of parameter newSolution's vector
            //x for (int j = 0; j < Dim; ++j)
            //x     Solutions[AmoebaSize - 1].Vector[j] = newSolution.Vector[j];
            //x Solutions[AmoebaSize - 1].Value = newSolution.Value;
            Solutions[AmoebaSize - 1].CopyFrom(newSolution.Vector);
            Array.Sort(Solutions);
        }

        private bool IsWorseThanAllButWorst(VertexSolution reflected)
        {
            // Solve needs to know if the reflected vector is worse (greater value) than every vector in the amoeba, except for the worst vector (highest index)
            for (int i = 0; i < AmoebaSize - 1; ++i)  // not the highest index (worst)
            {
                if (reflected.Value <= Solutions[i].Value)  // reflected is better (smaller value) than at least one of the non-worst solution vectors
                    return false;
            }
            return true;
        }

        private double Convergence()
        {
            double value = (2 * Math.Abs(Solutions[AmoebaSize - 1].Value - Solutions[0].Value))
                / (Math.Abs(Solutions[AmoebaSize - 1].Value) + Math.Abs(Solutions[0].Value) + Tolerance);
            return value;
        }

        public override ISolution Solve()
        {
            int t = 0;  // loop counter
            double convergedValue = double.MaxValue; 
            while (t < maxLoop)
            {
                convergedValue = Convergence(); // so you can see value when debugging
                if (convergedValue < Tolerance)
                {
                    break;
                }
                ++t;

                //if (t % 10 == 0)
                //{
                //    Debug.WriteLine("At t = " + t + " curr best solution = " + this.Solutions[0]);
                //}

                VertexSolution centroid = Centroid();  // compute centroid
                VertexSolution reflected = Reflected(centroid);  // compute reflected

                if (reflected.Value < Solutions[0].Value)  // reflected is better than the curr best
                {
                    VertexSolution expanded = Expanded(reflected, centroid);  // can we do even better??
                    if (expanded.Value < Solutions[0].Value)  // winner! expanded is better than curr best
                    {
                        ReplaceWorst(expanded);  // replace curr worst solution with expanded
                    }
                    else
                    {
                        ReplaceWorst(reflected);  // it was worth a try ...
                    }
                    continue;
                }

                if (true == IsWorseThanAllButWorst(reflected))  // reflected is worse (larger value) than all solution vectors (except possibly the worst one)
                {
                    if (reflected.Value <= Solutions[AmoebaSize - 1].Value)  // reflected is better (smaller) than the curr worst (last index) vector
                    {
                        ReplaceWorst(reflected);
                    }

                    VertexSolution contracted = Contracted(centroid);  // compute a point 'inside' the amoeba

                    if (contracted.Value > Solutions[AmoebaSize - 1].Value)  // contracted is worse (larger value) than curr worst (last index) solution vector
                    {
                        Shrink();
                    }
                    else
                    {
                        ReplaceWorst(contracted);
                    }
                    continue;
                }
                ReplaceWorst(reflected);
            } 

            for (int i = 0; i < Solutions[0].Vector.Length; i++)
            {
                Solutions[0][i] = Math.Round(Solutions[0].Vector[i], SigFigs);
            }
            return Solutions[0];  // best solution is always at [0]
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Solutions.Length; ++i)
                s += "[" + i + "] " + Solutions[i].ToString() + Environment.NewLine;
            return s;
        }
    }
}