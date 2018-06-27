using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regression.Common.Base
{
    public interface IRegression
    {
        double InitialEta { get;set; } 
        double[] Weights { get; set; }
        double Epsilon { get; set; }
        /// <summary>
        /// Trains model. Returns vector of weights w[] which you can use for making predictions
        /// </summary>
        /// <param name="fit">Matrix of inputs</param>
        /// <param name="y">desired outputs</param>
        /// <returns></returns>
        double[] Fit(Matrix fit, double[] y);
        /// <summary>
        /// Returns prediction made by Regression algorithm
        /// </summary>
        /// <param name="predictionData"></param>
        /// <returns></returns>
        double[] Predict(Matrix predictionData);
    }
}
