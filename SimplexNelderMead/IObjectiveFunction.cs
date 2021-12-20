using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexNelderMead
{
    public interface IObjectiveFunction
    {
        double Evaluate(double[] vector);
    }
}
