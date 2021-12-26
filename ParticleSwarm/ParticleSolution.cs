using Optimization.Infrastructure;
using System.Collections.ObjectModel;

namespace ParticleSwarm
{
    public class ParticleSolution : Solution
    {
        public double[]? Velocity;

        // Each particle has memory of the best position found, and associated error
        public double[]? BestPosition; 
        public double BestValue;

        public ParticleSolution(IObjectiveFunction aux, double[] vector) : base(aux) 
        {
            CopyFrom(vector);
            BestPosition = new double[Vector.Length];
            Vector.CopyTo(BestPosition, 0);
            BestValue = Value;
        }

        public ParticleSolution(IObjectiveFunction aux, double[] vector, double[] velocity) : this(aux, vector)
        {
            Velocity = new double[velocity.Length];
            velocity.CopyTo(Velocity, 0);
        }

        public override ParticleSolution Clone()
        {
            return new ParticleSolution(ObjectiveFunction, (double[])Vector.Clone());
        }

        public override string ToString()
        {
            string s = "";
            s += "Position [ ";
            for (int i = 0; i < Length; ++i)
                s += Vector[i].ToString(VectorFormat) + " ";
            s += "] ";
            s += "Value = " + Value.ToString(ValueFormat);

            if (null != Velocity)
            {
                s += "Velocity [ ";
                for (int i = 0; i < Length; ++i)
                    s += Velocity[i].ToString("F2") + " ";
                s += "] ";
                s += " BestPosition [ ";
                for (int i = 0; i < Length; ++i)
                    s += BestPosition[i].ToString(VectorFormat) + " ";
                s += "] ";
                s += "BestValue = " + Value.ToString(ValueFormat);
            }
            return s;
        }
    }
}