using Optimus.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Optimus.Domain
{
    public class Solution : ISolution, IComparable<Solution>
    {
        protected static Random Randomizer = new Random(0);

        protected IObjectiveFunction Aux;
        private double _value = double.MaxValue;
        private ObservableCollection<Point> _points = new ObservableCollection<Point>();
        private bool UpdateValue;

        public Solution(IObjectiveFunction aux, double[] vector)
        {
            Aux = aux;
            _points.CollectionChanged += OnCollectionChanged;
            
            for (int i = 0; i < vector.Length; i++)
            {
                Point p = new Point(vector[i]);
                _points.Add(p);
            }
            UpdateValue = true;
        }

        public Solution(IObjectiveFunction aux, int dim, double minX, double maxX)
        {
            Aux = aux;
            _points.CollectionChanged += OnCollectionChanged;

            for (int i = 0; i < dim; i++)
            {
                Point p = new Point((maxX - minX) * Randomizer.NextDouble() + minX); 
                _points.Add(p);
            }
            UpdateValue=true;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (null == sender)
            {
                return;
            }
            if (e.NewItems != null)
            {
                foreach (Point newItem in e.NewItems)
                {
                    newItem.PropertyChanged += OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Point oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= OnItemPropertyChanged;
                }
            }
        }

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (null == sender)
            {
                return;
            }
            UpdateValue = true;
        }

        public double this[int index]
        {
            get => _points[index].Value;
            set
            {
                if (value != _points[index].Value)
                {
                    _points[index].Value = value;
                }
            }
        }

        public int Length => _points.Count;

        public double Value
        {
            get
            {
                if (true == UpdateValue)
                {
                    _value = Aux.EvaluateAt(Position());
                    UpdateValue = false;
                }
                return _value;
            }
        }

        public ISolution Move(double[] newPosition)
        {
            for (int i = 0; i < Length; i++)
            {
                this[i] = newPosition[i];
            }
            return this;
        }

        public double[] Position()
        {
            double[] vector = new double[_points.Count];
            for (int i = 0; i < _points.Count; i++)
            {
                vector[i] = _points[i].Value;
            }
            return vector;
        }

        public virtual ISolution Clone()
        {
            return new Solution(Aux, Position());
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// Comparison is based on the fitness Value of each object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table"><listheader><term> Value</term><term> Meaning</term></listheader><item><description> Less than zero</description><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><description> Zero</description><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><description> Greater than zero</description><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
        public int CompareTo(Solution? other) // based on vector/solution value
        {
            if (other == null || Value < other.Value)
                return -1; // this instance preceeds other in the sort order
            else if (Value > other.Value)
                return 1; // this value follows other in the sort order
            else
                return 0; // this instance ocurrs in the same position in the sort order as other
        }

        public override string ToString()
        {
            string s = "Solution [ ";
            for (int i = 0; i < Length; ++i)
            {
                s += this[i].ToString("F2") + " ";
            }
            s += "] => " + Value.ToString("F4");
            return s;
        }
    }
}