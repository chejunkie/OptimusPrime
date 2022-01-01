using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimus.Firefly;
using Optimus.Core;
using Optimus.TestFunctions;

namespace Optimus.Tests
{
    [TestClass]
    public class FireflyTests
    {
        [TestMethod]
        public void Firefly_Michalewicz5D_Solves()
        {
            // Arrange
            Michalewicz aux = new Michalewicz();
            int dim = 5;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 5000;
            int numberFireflys = 50;

            // Act
            IOptimizer fa = new FireflyOptimizer(aux, dim, minX, maxX, numberFireflys, maxLoop);
            ISolution solution = fa.FormatSolution(fa.Solve());

            // Assert
            double delta = (solution.Value - aux.GlobalMinimum(dim));
            bool bSuccess = false;
            if (delta < 1e-7)
            {
                bSuccess = true;
            }
            Assert.AreEqual(true, bSuccess);
            //? CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }

        [TestMethod]
        public void Firefly_Michalewicz2D_Solves()
        {
            /* For two variables, x and y, the global minimum value is approximately 
             * z = -1.8013 when x = 2.20319 and y = 1.57049. */

            // Arrange
            Michalewicz aux = new Michalewicz();
            int dim = 2;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 5000;
            int numberFireflys = 50;

            // Act
            IOptimizer fa = new FireflyOptimizer(aux, dim, minX, maxX, numberFireflys, maxLoop);
            ISolution solution = fa.FormatSolution(fa.Solve());
            double fitness = aux.EvaluateAt(solution.Position());

            // Assert
            double delta = (solution.Value - aux.GlobalMinimum(dim));
            bool bSuccess = false;
            if (delta < 1e-7)
            {
                bSuccess = true;
            }
            Assert.AreEqual(true, bSuccess);
            //? CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }
    }
}