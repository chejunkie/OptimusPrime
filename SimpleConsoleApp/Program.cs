
using Optimus.TestFunctions;
using Optimus.Core;
using Optimus.Domain;

IObjectiveFunction aux = new Rastrigin3D();

double[] vector = new double[3] {1,2,3};
Solution position = new Solution(aux, vector);

double[] v2 = new double[3] { 100, 200, 300 };
//position.CopyFrom(v2);
position.Move(v2);

int stop = 0;
