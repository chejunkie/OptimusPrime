using Optimus.Core;
using System;

namespace Optimus.Domain
{
    public class FireflySwarm
    {
        private readonly Firefly[] _fireflies;
        private readonly Solution _best;

        public FireflySwarm(IObjectiveFunction aux, int numberFireflys, int dim, double minX, double maxX)
        {
            _fireflies = new Firefly[numberFireflys];
            _best = new Solution(aux, dim, minX, maxX);

            for (int i = 0; i < numberFireflys; i++)
            {
                _fireflies[i] = new Firefly(aux, dim, minX, maxX);
                if (_fireflies[i].Value < Best.Value)
                {
                    Best.Move(_fireflies[i].Position());
                }
            }
        }

        public Firefly this[int index]
        {
            get => _fireflies[index];
        }

        //x public Firefly[] Fireflies => _fireflies;

        public ISolution Best => _best;

        public int Length
        {
            get { return _fireflies.Length; }
        }

        public void Sort()
        {
            Array.Sort(_fireflies);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _fireflies.Length; ++i)
                s += "[" + i + "] " + _fireflies[i].ToString() + "\n";
            s += "Best " + Best.ToString();
            return s;
        }
    }
}