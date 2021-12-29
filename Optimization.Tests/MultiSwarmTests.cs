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
        public void MultiSwarm_Rosenbrock_Solves()
        {
            // Arrange
            IObjectiveFunction aux = new Rosenbrock();
            int dim = aux.Dim; // 2
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 500;
            int numberParticles = 25;
            int numberSwarms = 5;

            // Act
            IOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            CollectionAssert.AreEqual(new double[2] { 1, 1 }, solution.Position());
        }

        [TestMethod]
        public void MultiSwarm_Rastrigin2D_Solves()
        {
            // Arrange
            IObjectiveFunction aux = new Rastrigin2D();
            int dim = aux.Dim; // 2
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 500;
            int numberParticles = 25;
            int numberSwarms = 5;

            // Act
            MultiSwarmOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            CollectionAssert.AreEqual(new double[2] { 0.0, 0.0 }, solution.Position());
        }

        [TestMethod]
        public void MultiSwarm_Rastrigin3D_Solves()
        {
            // Arrange
            IObjectiveFunction aux = new Rastrigin3D();
            int dim = aux.Dim; // 3
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 500;
            int numberParticles = 25;
            int numberSwarms = 5;

            // Act
            MultiSwarmOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            CollectionAssert.AreEqual(new double[3] { 0.0, 0.0, 0.0 }, solution.Position());
        }
    }
}