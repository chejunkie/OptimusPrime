using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexNelderMead
{
    public interface ISolution
    {
        double[] Vector { get; }
        double Value { get; }
    }
}
