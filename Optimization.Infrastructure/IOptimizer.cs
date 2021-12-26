namespace Optimization.Infrastructure
{
    public interface IOptimizer
    {
        IObjectiveFunction ObjectiveFunction { get; }

        public ISolution FormatSolution(ISolution solution);

        public ISolution Solve();
    }
}