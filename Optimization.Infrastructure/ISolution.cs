namespace Optimus.Core
{
    public interface ISolution
    {
        public double this[int index] { get; set; }
        public int Length { get; }
        double Value { get; }

        public ISolution Clone();
        public ISolution Move(double[] newPosition);
        double[] Position();
    }
}