using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimus.Core;
using Optimus.TestFunctions;
using Optimus.ParticleSwarm;

namespace Optimus.Tests
{
    [TestClass]
    public class ParticleSwarmTests
    {
        [TestMethod]
        public void ParticleSwarm_Rosenbrock2D_Solves()
        {
            // Arrange
            Rosenbrock aux = new Rosenbrock();
            int dim = 2;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 1000;
            int numberParticles = 25;

            // Act
            IOptimizer pso = new ParticleSwarmOptimizer(aux, dim, minX, maxX, numberParticles, maxLoop);
            ISolution solution = pso.FormatSolution(pso.Solve());

            // Assert
            Assert.AreEqual(aux.GlobalMinimum, solution.Value);
            CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }

        [TestMethod]
        public void ParticleSwarm_Rastrigin2D_Solves()
        {
            // Arrange
            Rastrigin aux = new Rastrigin();
            int dim = 2;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 500;
            int numberParticles = 25; // more particles => better accuracy, but at the cost of performance

            // Act
            IOptimizer pso = new ParticleSwarmOptimizer(aux, dim, minX, maxX, numberParticles, maxLoop);
            ISolution solution = pso.FormatSolution(pso.Solve());

            // Assert
            Assert.AreEqual(aux.GlobalMinimum, solution.Value);
            CollectionAssert.AreEqual(aux.GlobalPosition(dim), solution.Position());
        }
    }
}