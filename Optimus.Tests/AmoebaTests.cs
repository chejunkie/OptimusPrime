using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimus.Core;
using Optimus.Amoeba;
using Optimus.TestFunctions;

namespace Optimus.Tests
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
            IOptimizer amoeba = new AmoebaOptimizer(aux, amoebaSize, dim, minX, maxX, maxLoop);
            ISolution solution = amoeba.Solve();

            // Assert
            Assert.AreEqual(aux.GlobalMinimum, solution.Value);
            CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }
    }
}