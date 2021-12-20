using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexNelderMead;

namespace Optimization.Tests
{
    [TestClass]
    public class AmoebaTests
    {
        [TestMethod]
        public void Amoeba_Rosenbrock_Solves()
        {
            // Arrange
            Rosenbrock aux = new Rosenbrock(); 
            int dim = 2;
            int amoebaSize = 3;
            double minX = -100.0;
            double maxX = 100.0;
            int maxLoop = 1000;

            // Act
            Amoeba amoeba = new Amoeba(aux, amoebaSize, dim, minX, maxX, maxLoop);
            ISolution solution = amoeba.Solve();

            // Assert
            CollectionAssert.AreEqual(new double[2] { 1, 1 }, solution.Vector);
        }
    }
}