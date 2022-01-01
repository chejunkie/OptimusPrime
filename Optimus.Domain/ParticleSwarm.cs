using Optimus.Core;

namespace Optimus.Domain
{
    public class ParticleSwarm
    {
        //x private static Random Randomizer = new Random(0);

        private readonly Particle[] _particles;
        private readonly Solution _best;

        public ParticleSwarm(IObjectiveFunction aux, int numberParticles, int dim, double minX, double maxX)
        {
            _particles = new Particle[numberParticles];
            _best = new Solution(aux, dim, minX, maxX);

            for (int i = 0; i < numberParticles; i++)
            {
                _particles[i] = new Particle(aux, dim, minX, maxX);
                if (_particles[i].Value < Best.Value)
                {
                    Best.Move(_particles[i].Position());
                }
            }
        }

        public Particle this[int index]
        {
            get => _particles[index];
        }

        public Particle[] Particles => _particles;

        public ISolution Best => _best;

        public int Length
        {
            get { return _particles.Length; }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _particles.Length; ++i)
                s += "[" + i + "] " + _particles[i].ToString() + "\n";
            s += "Best " + Best.ToString();
            return s;
        }
    }
}