namespace SimplexNelderMead
{
    public class Solution : IComparable<Solution>, ISolution
    {
        // a potential solution (array of double) and associated value (so can be sorted against several potential solutions
        private double[] _vector;

        private double _value;

        private static Random Random = new Random(1);  // to allow creation of random solutions

        public Solution(IObjectiveFunction aux, int dim, double minX, double maxX)
        {
            // a random Solution
            this.Vector = new double[dim];
            for (int i = 0; i < dim; ++i)
                this.Vector[i] = (maxX - minX) * Random.NextDouble() + minX;
            this.Value = aux.Evaluate(this.Vector);
        }

        public Solution(IObjectiveFunction aux, double[] vector)
        {
            // a specifiede solution
            this.Vector = new double[vector.Length];
            Array.Copy(vector, this.Vector, vector.Length);
            this.Value = aux.Evaluate(this.Vector);
        }

        public double[] Vector
        {
            get
            {
                return _vector;
            }
            internal set
            {
                _vector = value;
            }
        }

        public double Value
        {
            get
            {
                return _value;
            }
            internal set
            {
                _value = value;
            }
        }

        public int CompareTo(Solution other) // based on vector/solution value
        {
            if (this.Value < other.Value)
                return -1;
            else if (this.Value > other.Value)
                return 1;
            else
                return 0;
        }

        public override string ToString()
        {
            string s = "[ ";
            for (int i = 0; i < this.Vector.Length; ++i)
            {
                if (Vector[i] >= 0.0) s += " ";
                s += Vector[i].ToString("F2") + " ";
            }
            s += "]  val = " + this.Value.ToString("F4");
            return s;
        }
    }
}