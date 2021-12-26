using Microsoft.VisualStudio.TestTools.UnitTesting;
using Optimization.BenchmarkFunctions;
using Optimization.Infrastructure;
using ParticleSwarm;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Optimization.Tests
{
    [TestClass]
    public class ParticleSwarmTests
    {
        // <image url="$(SolutionDir)Images\ParticleSwarmOptimization.jpg" scale="0.4" />

        [TestMethod]
        public void ParticleSwarm_Rosenbrock_Solves()
        {
            // Arrange
            IObjectiveFunction aux = new Rosenbrock();
            int dim = 2;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 1000;
            int numberParticles = 50;

            // Act
            IOptimizer pso = new ParticleSwarmOptimizer(aux, dim, minX, maxX, numberParticles, maxLoop);
            ISolution solution = pso.FormatSolution(pso.Solve());

            // Assert
            CollectionAssert.AreEqual(new double[2] { 1, 1 }, solution.Vector);
        }

        [TestMethod]
        public void ParticleSwarm_Rastrigin_Solves()
        {
            // Arrange
            IObjectiveFunction aux = new Rastrigin();
            int dim = aux.Dim;
            double minX = -100.0;
            double maxX = 100.0;
            long maxLoop = 1000;
            int numberParticles = 50; // more particles => better accuracy, but at the cost of performance

            // Act
            IOptimizer pso = new ParticleSwarmOptimizer(aux, dim, minX, maxX, numberParticles, maxLoop);
            ISolution solution = pso.FormatSolution(pso.Solve()); 

            // Assert
            CollectionAssert.AreEqual(new double[3] { 0.0, 0.0, 0.0 }, solution.Vector);
        }
    }
}