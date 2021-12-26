using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimization.Infrastructure;
using SimplexNelderMead;

namespace Optimization.Tests
{
    [TestClass]
    public class AmoebaTests
    {
        // <image url="$(SolutionDir)Images\AmoebaOptimization.jpg" scale="0.4" />
        
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
            IOptimizer amoeba = new AmoebaOptimizer(aux, amoebaSize, dim, minX, maxX, maxLoop);
            ISolution solution = amoeba.Solve();

            // Assert
            CollectionAssert.AreEqual(new double[2] { 1, 1 }, solution.Vector);
        }
    }
}