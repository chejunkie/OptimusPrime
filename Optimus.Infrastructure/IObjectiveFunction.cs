namespace Optimus.Core
{
    public interface IObjectiveFunction
    {
        double EvaluateAt(double[] point);
    }
}