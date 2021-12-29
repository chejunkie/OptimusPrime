using Optimus.Core;

namespace Optimus.Domain
{
    public class Particle : Solution
    {
        public double[] Velocity;
        private readonly Solution _best;

        public Particle(IObjectiveFunction aux, double[] position) : base(aux, position)
        {
            Velocity = new double[position.Length];
            _best = new Solution(aux, position);
        }

        public Particle(IObjectiveFunction aux, double[] position, double[] velocity) : this(aux, position)
        {
            velocity.CopyTo(Velocity, 0);
        }

        public Particle(IObjectiveFunction aux, int dim, double minX, double maxX) : base(aux, dim, minX, maxX)
        {
            Velocity = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                Velocity[i] = (maxX - minX) * Randomizer.NextDouble() + minX;
            }
            _best = new Solution(aux, Position());
        }

        public ISolution Best => _best;

        public override Particle Clone()
        {
            return new Particle(Aux, Position(), (double[])Velocity.Clone());
        }

        public override string ToString()
        {
            //string s = "";
            //s += "Current position [ ";
            //for (int i = 0; i < Length; ++i)
            //    s += this[i].ToString("F2") + " ";
            //s += "] = " + Value.ToString("F4");

            //if (null != Velocity)
            //{
            //    s += "Velocity [ ";
            //    for (int i = 0; i < Length; ++i)
            //        s += Velocity[i].ToString("F2") + " ";
            //    s += "] ";
            //    s += " Best [ ";
            //    for (int i = 0; i < Length; ++i)
            //        s += Best[i].ToString("F2") + " ";
            //    s += "] = " + Value.ToString("F4");
            //}
            //return s;

            string s = "Particle [ ";
            for (int i = 0; i < Length; ++i)
            {
                s += this[i].ToString("F2") + " ";
            }
            s += "] => " + Value.ToString("F4");
            s += " | Best Position [ ";
            for (int i = 0; i < Length; ++i)
            {
                s += Best[i].ToString("F2") + " ";
            }
            s += "] => " + Value.ToString("F4");
            return s;
        }
    }
}