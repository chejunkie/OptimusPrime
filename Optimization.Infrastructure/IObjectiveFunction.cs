namespace Optimus.Core
{
    public interface IObjectiveFunction
    {
        int Dim { get; }

        double EvaluateAt(double[] point);
    }
}