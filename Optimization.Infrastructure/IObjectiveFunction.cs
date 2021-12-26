using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Infrastructure
{
    public interface IObjectiveFunction
    {
        int Dim { get; }
        double EvaluateAt(double[] point);
    }
}
