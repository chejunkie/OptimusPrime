using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimus.MultiSwarm;
using Optimus.Core;
using Optimus.TestFunctions;

namespace Optimus.Tests
{
    [TestClass]
    public class MultiSwarmTests
    {
        [TestMethod]
        public void MultiSwarm_Rosenbrock2D_Solves()
        {
            // Arrange
            Rosenbrock aux = new Rosenbrock();
            int dim = 2;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 500;
            int numberParticles = 25;
            int numberSwarms = 5;

            // Act
            IOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            Assert.AreEqual(aux.GlobalMinimum, solution.Value);
            CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }

        [TestMethod]
        public void MultiSwarm_Rastrigin2D_Solves()
        {
            // Arrange
            Rastrigin aux = new Rastrigin();
            int dim = 2;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 500;
            int numberParticles = 25;
            int numberSwarms = 5;

            // Act
            MultiSwarmOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            Assert.AreEqual(aux.GlobalMinimum, solution.Value);
            CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }

        [TestMethod]
        public void MultiSwarm_Rastrigin3D_Solves()
        {
            // Arrange
            Rastrigin aux = new Rastrigin();
            int dim = 3; 
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 500;
            int numberParticles = 25;
            int numberSwarms = 5;

            // Act
            MultiSwarmOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            Assert.AreEqual(aux.GlobalMinimum, solution.Value);
            CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }
    }
}