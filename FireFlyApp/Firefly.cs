using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireFlyApp
{
    public class Firefly : IComparable<Firefly> // so arrays and lists containing the object can be automatically sorted.
    {
        public double[] Position;
        public double Error;
        public double Intensity;

        public Firefly(int dim)
        {
            this.Position = new double[dim];
            this.Error = 0.0;
            this.Intensity = 0.0;
        }

        // Defined here
        public int CompareTo(Firefly other)
        {
            if (this.Error < other.Error) return -1;
            else if (this.Error > other.Error) return +1;
            else return 0;
        }
    }
}
