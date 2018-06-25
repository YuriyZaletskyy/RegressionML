using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Regression.Common.Base;

namespace Regression.Common
{
    public class SingleThreadedNormalization : BaseNormalizer
    {
        /// <summary>
        /// Single threaded normalization with centring around zero
        /// </summary>
        /// <param name="matrixForNormalization"></param>
        /// <returns></returns>
        public override Matrix Normalize(Matrix matrixForNormalization)
        {
            Matrix result = new Matrix(matrixForNormalization.LinesNumber, 
                matrixForNormalization.ColumnsNumber);
            
            var min = matrixForNormalization[0].Min();
            var max = matrixForNormalization[0].Max();

            for (int i = 1; i < matrixForNormalization.LinesNumber; i++)
            {
                var temp = matrixForNormalization[i].Min();
                if (temp < min)
                {
                    min = temp;
                }

                temp = matrixForNormalization[i].Max();
                if (temp > max)
                {
                    max = temp;
                }
            }

            for (int i = 0; i < matrixForNormalization.LinesNumber; i++)
            {
                for (int j = 0; j < matrixForNormalization.ColumnsNumber; j++)
                {
                    result[i, j] = -0.5 + (matrixForNormalization[i, j] - min)/ (max - min);
                }
            }
            return result;
        }
    }
}
