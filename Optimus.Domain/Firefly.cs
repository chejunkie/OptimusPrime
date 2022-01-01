using Optimus.Core;

namespace Optimus.Domain
{
    public class Firefly : Solution
    {
        private readonly Solution _best;

        public Firefly(IObjectiveFunction aux, double[] position) : base(aux, position)
        {
            _best = new Solution(aux, position);
        }

        public Firefly(IObjectiveFunction aux, int dim, double minX, double maxX) : base(aux, dim, minX, maxX)
        {
            _best = new Solution(aux, Position());
        }

        public ISolution Best => _best;

        public double Intensity => 1 / (Value + 1);

        public override Firefly Clone()
        {
            return new Firefly(Aux, Position());
        }

        public override string ToString()
        {
            string s = "Firefly [ ";
            for (int i = 0; i < Dim; ++i)
            {
                s += this[i].ToString("F2") + " ";
            }
            s += "] => " + Value.ToString("F4");
            s += " | Best Position [ ";
            for (int i = 0; i < Dim; ++i)
            {
                s += Best[i].ToString("F2") + " ";
            }
            s += "] => " + Value.ToString("F4");
            return s;
        }
    }
}
