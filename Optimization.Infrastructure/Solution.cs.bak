namespace Optimization.Infrastructure
{
    public abstract class Solution : ISolution, IComparable<Solution>
    {
        private double[] _vector; // parameter or coefficient values being adjusted
        private double _value;    // fitness value
        private bool UpdateValue; // flag to update fitness when needed vs. automatically on every single change (which is costly).
        public readonly IObjectiveFunction ObjectiveFunction;
        protected string VectorFormat = "F4";
        protected string ValueFormat = "F4";

        public Solution(IObjectiveFunction aux)
        {
            ObjectiveFunction = aux;
            _vector = Array.Empty<double>();
        }

        /// <summary>Copies from other to Vector array, and flags Value to update when needed.</summary>
        /// <param name="other">The other vector to be copied from.</param>
        public ISolution CopyFrom(double[] other)
        {
            _vector = new double[other.Length];
            other.CopyTo(_vector, 0);
            //_vector = other;
            UpdateValue = true;
            return this;
        }

        public int Length
        {
            get { return _vector.Length; }
        }

        public double this[int index]
        {
            get => _vector[index];
            set
            {
                if (value != _vector[index])
                {
                    _vector[index] = value;
                    UpdateValue = true;
                }
            }
        }

        public double Value
        {
            get
            {
                if (true == UpdateValue)
                {
                    _value = ObjectiveFunction.EvaluateAt(_vector);
                    UpdateValue = false;
                }
                return _value;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// Comparison is based on the fitness Value of each object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><term> Meaning</term></listheader><item><description> Less than zero</description><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><description> Zero</description><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><description> Greater than zero</description><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public int CompareTo(Solution other) // based on vector/solution value
        {
            if (this.Value < other.Value)
                return -1; // this instance preceeds other in the sort order
            else if (this.Value > other.Value)
                return 1; // this value follows other in the sort order
            else
                return 0; // this instance ocurrs in the same position in the sort order as other
        }

        public override string ToString()
        {
            string s = "Vector [ ";
            for (int i = 0; i < this.Vector.Length; ++i)
            {
                if (Vector[i] >= 0.0) s += " ";
                s += Vector[i].ToString(VectorFormat) + " ";
            }
            s += "]  Value = " + this.Value.ToString(ValueFormat);
            return s;
        }

        public abstract ISolution Clone();

        public double[] Vector
        {
            get => _vector;
        }
    }
}