using System;
using System.Linq;
using LinearRegression;
using Regression.Common;
using Xunit;

namespace Test.LinearRegression
{
    public class LinearRegressionTests
    {
        [Fact]
        public void TestLinearRegression()
        {
            double [][] inputs = new double[3][];
            
            inputs[0] = new double[3];
            inputs[1] = new double[3];
            inputs[2] = new double[3];

            /*
              system of equations:
              x1 + x2 + x3 = 35
              x1 + x2 - x3 = -13
              x1 - x2 + x3 = 19
             
            x1 should be 3, x2 should be 8, x3 should be 24

             */
            inputs[0][0] = 1;
            inputs[0][1] = 1;
            inputs[0][2] = 1;

            inputs[1][0] = 1;
            inputs[1][1] = 1;
            inputs[1][2] = -1;

            inputs[2][0] = 1;
            inputs[2][1] = -1;
            inputs[2][2] = 1;

            double [] outputs = new double[3];
            outputs[0] = 35;
            outputs[1] = -13;
            outputs[2] = 19;

            var normalizer = new MultiThreadedNormalization();
            var regressor = new LinearRegressor(normalizer, epsilon:0.0000000001);
            var matrix = new Matrix(inputs);
            var weights = regressor.Fit(matrix, outputs);

        }
    }
}
