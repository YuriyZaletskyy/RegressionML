using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Regression.Common.Base;

namespace Regression.Common
{
    public class MultiThreadedNormalization : BaseNormalizer
    {
        private static object normLockMin = new object();
        private static object normLockMax = new object();
        public override Matrix Normalize(Matrix matrixForNormalization)
        {
            Matrix result = new Matrix(matrixForNormalization.LinesNumber, 
                matrixForNormalization.ColumnsNumber);

            double min = matrixForNormalization[0,0], 
                max = matrixForNormalization[0,0];

            Parallel.For(0, matrixForNormalization.LinesNumber, (i) =>
            {
                var currentMin = matrixForNormalization[i].Min();
                var currentMax = matrixForNormalization[i].Max();

                if (currentMin < min)
                {
                    lock (normLockMin)
                    {
                        min = currentMin;
                    }
                }

                if (currentMax > max)
                {
                    lock (normLockMax)
                    {
                        max = currentMax;
                    }
                }
            });

            var divider = max - min;
            

            Parallel.For(0, matrixForNormalization.LinesNumber, (i) =>
            {
                for (var j = 0; j < result.ColumnsNumber; j++)
                {
                    result[i, j] = -0.5 + (matrixForNormalization[i, j] -min)/ divider;
                }

            });
            return result;
        }

        
    }
}
