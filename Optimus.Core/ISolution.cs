namespace Optimus.Core
{
    public interface ISolution
    {
        /// <summary>Gets or sets the position at the specified index.</summary>
        /// <param name="index">The index.</param>
        /// <returns>System.Double.</returns>
        public double this[int index] { get; set; }

        /// <summary>Gets the dimension of the solution vector (position).</summary>
        /// <value>The dim.</value>
        public int Dim { get; }

        /// <summary>Gets the fiteness value for the current position.</summary>
        /// <value>The value.</value>
        double Value { get; }

        /// <summary>Clones this instance by returning a new object at the same position.</summary>
        /// <returns>ISolution.</returns>
        public ISolution Clone();

        /// <summary>Moves the solution to a new position.</summary>
        /// <param name="newPosition">The new position.</param>
        /// <returns>ISolution.</returns>
        public ISolution Move(double[] newPosition);

        /// <summary>Retruns an array representing the current position.</summary>
        /// <returns>System.Double[].</returns>
        double[] Position();
    }
}