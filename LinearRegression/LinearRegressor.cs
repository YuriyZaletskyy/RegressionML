using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Regression.Common;
using Regression.Common.Base;

namespace LinearRegression
{
    public class LinearRegressor : IRegression
    {
        public LinearRegressor(BaseNormalizer normalizer, double epsilon = 0.0001, double initialEta = 0.001)
        {
            Normalizer = normalizer;
            Epsilon = epsilon;
            InitialEta = 0.001;
        }
        public BaseNormalizer Normalizer { get; set; }

        public double InitialEta { get;set; }

        public double Epsilon { get; set; }
        /// <summary>
        /// Structure of the weights is the following:
        /// w[0] has value of w1 which is multiplied on x1, 
        /// w[1] has value of w2 which is multipled on x2
        /// .
        /// .
        /// .
        /// w[latest] has value of w0, which is multiplied on 1 always. So called free coefficient. 
        /// </summary>
        public double[] Weights { get; set; }
        public double[] Fit(Matrix fit, double[] y)
        {
            var completeMatrix = fit.AddColumn(y);
            
            var normalizedMatrix = Normalizer.Normalize(completeMatrix);
            Weights = new double[normalizedMatrix.ColumnsNumber];

            var rss = CalculateRss(normalizedMatrix);
            while (rss > Epsilon)
            {
                double [] outputs = new double[normalizedMatrix.LinesNumber];

                Parallel.For(0, normalizedMatrix.LinesNumber, (i, inp) =>
                {
                    var outputOfSingleRow = CalculateOutputOfSingleRow(normalizedMatrix, i);
                    outputs[i] = outputOfSingleRow;
                });

                double[] diffs = new double[normalizedMatrix.LinesNumber];

                Parallel.For(0, normalizedMatrix.LinesNumber, (i, inp) =>
                    {
                        diffs[i] = outputs[i] - normalizedMatrix[i, normalizedMatrix.ColumnsNumber - 1];
                    });
                
                double [] partials = new double[normalizedMatrix.ColumnsNumber];

                for (int i = 0; i < normalizedMatrix.LinesNumber; i++)
                {
                    int j;
                    for (j = 0; j < normalizedMatrix.ColumnsNumber - 1; j++)
                    {
                        partials[j] = partials[j] + -2 * diffs[i] * normalizedMatrix[i, j];
                    }
                    partials[j] = partials[j] + -2 * diffs[i] * 1; // coefficient near last weight is always zero
                }

                for (int i = 0; i < normalizedMatrix.ColumnsNumber; i++)
                {
                    Weights[i] = Weights[i] + InitialEta * partials[i];
                }
                rss = CalculateRss(normalizedMatrix);
            }

            return Weights;
        }

        public double[] Predict(Matrix predictionData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates residual square error
        /// </summary>
        /// <param name="inptusAndOutputs"></param>
        /// <returns>resdual square error</returns>
        private double CalculateRss(Matrix inptusAndOutputs)
        {
            double result = 0.0;
            double [] sumOfOutpus = new double[inptusAndOutputs.LinesNumber];

            double[] targets = new double[inptusAndOutputs.LinesNumber];
            for (int i = 0; i < inptusAndOutputs.LinesNumber; i++)
            {
                targets[i] = inptusAndOutputs[i, inptusAndOutputs.ColumnsNumber - 1];
            }

            Parallel.For(0, inptusAndOutputs.LinesNumber, (i, inp) =>
            {
                var outputOfSingleRow = CalculateOutputOfSingleRow(inptusAndOutputs, i);
                double diff = targets[i] - outputOfSingleRow;
                sumOfOutpus[i] += diff * diff;
            });

            result = sumOfOutpus.Sum();
            return result;
        }

        /// <summary>
        /// Calculates output of single row
        /// </summary>
        /// <param name="inptusAndOutputs">Matrix which has inputs and outputs</param>
        /// <param name="rowIndex">Index of row for which to get result</param>
        /// <returns>Multiplication of weights on values of inputs</returns>
        private double CalculateOutputOfSingleRow(Matrix inptusAndOutputs, int rowIndex)
        {
            double rowValue = 0.0;
            for (int z = 0; z < inptusAndOutputs.ColumnsNumber - 1; z++)
            {
                rowValue += Weights[z] * inptusAndOutputs[rowIndex, z];
            }

            rowValue += Weights[inptusAndOutputs.ColumnsNumber - 1];
            return rowValue;
        }
    }
}
