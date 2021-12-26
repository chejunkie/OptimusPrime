using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimization.BenchmarkFunctions;
using Optimization.Infrastructure;
using MultiSwarm;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Optimization.Tests
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
            long maxLoop = 1000;
            int numberParticles = 50;
            int numberSwarms = 5;

            // Act
            IOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            CollectionAssert.AreEqual(new double[2] { 1, 1 }, solution.Vector);
        }

        [TestMethod]
        public void MultiSwarm_Rastrigin_Solves()
        {
            // Arrange
            IObjectiveFunction aux = new Rastrigin();
            int dim = aux.Dim; // 3
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 10000;
            int numberParticles = 50;
            int numberSwarms = 5;

            // Act
            IOptimizer mso = new MultiSwarmOptimizer(aux, dim, minX, maxX, numberParticles, numberSwarms, maxLoop);
            ISolution solution = mso.FormatSolution(mso.Solve());

            // Assert
            CollectionAssert.AreEqual(new double[3] { 0.0, 0.0, 0.0 }, solution.Vector);
        }
    }
}