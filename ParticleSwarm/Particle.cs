using Optimization.Infrastructure;

namespace ParticleSwarm
{
    public class Particle : Solution
    {
        public double[] Velocity;
        private readonly Solution _best;

        public Particle(IObjectiveFunction aux, double[] vector) : base(aux, vector)
        {
            Velocity = new double[vector.Length];
            _best = new Solution(aux, vector);
        }

        public Particle(IObjectiveFunction aux, double[] vector, double[] velocity) : this(aux, vector)
        {
            velocity.CopyTo(Velocity, 0);
        }

        public Solution Best => _best;

        public override Particle Clone()
        {
            return new Particle(Aux, Position(), (double[])Velocity.Clone());
        }

        public override string ToString()
        {
            string s = "";
            s += "Current position [ ";
            for (int i = 0; i < Length; ++i)
                s += this[i].ToString("F2") + " ";
            s += "] = " + Value.ToString("F4");

            if (null != Velocity)
            {
                s += "Velocity [ ";
                for (int i = 0; i < Length; ++i)
                    s += Velocity[i].ToString("F2") + " ";
                s += "] ";
                s += " Best [ ";
                for (int i = 0; i < Length; ++i)
                    s += Best[i].ToString("F2") + " ";
                s += "] = " + Value.ToString("F4");
            }
            return s;
        }
    }
}