namespace Optimization.Infrastructure
{
    public interface ISolution
    {
        double Value { get; }

        double[] Vector { get; }

        public double this[int index] { get; set; }

        public ISolution Clone();
        public int Length { get; }
    }
}