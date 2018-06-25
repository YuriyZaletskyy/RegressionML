using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Regression.Common;
using Regression.Common.Base;
using Xunit;

namespace Test.CommonFunctionality
{
    public class TestsMatrix
    {
        private const int linesNumber = 5;
        private const int columnsNumber = 3;
        public Double[][] GetTestData()
        {
            Double[][] testData = new Double[linesNumber][];
            for (int i = 0; i < linesNumber; i++)
            {
                testData[i] = new Double[columnsNumber];
            }

            testData[0][0] = 45.0;
            testData[0][1] = 43245.0;
            testData[0][2] = 2845.0;
            testData[1][0] = 47245.0;
            testData[1][1] = 3245.0;
            testData[1][1] = 5645.0;
            testData[2][0] = -34545.0;
            testData[2][1] = 5645.0;
            testData[2][2] = -3245.0;
            testData[3][0] = -8945.0;
            testData[3][1] = 7845.0;
            testData[3][2] = 98745.0;
            testData[4][0] = 2131145.0;
            testData[4][1] = 3545.0;
            testData[4][2] = 46345.0;

            return testData;
        }

        public Double[][] GetDataForSingleThreadedNormalization()
        {
            int goodDivisionColumnsNumber = 5;
            Double[][] testData = new Double[linesNumber][];
            
            for (int i = 0; i < linesNumber; i++)
            {
                testData[i] = new Double[goodDivisionColumnsNumber];

                for (int j = 0; j < goodDivisionColumnsNumber; j++)
                {
                    testData[i][j] = j + 1;
                }
            }
            
            return testData;
        }

        public Double[][] GetDataForMultyThreadedNormalization()
        {
            int linesNumberMultiplier = 4000;
            int columnsNumberMultiplier = 2000;

            Double[][] testData = new Double[linesNumber * linesNumberMultiplier][];
            
            for (int i = 0; i < linesNumber * linesNumberMultiplier; i++)
            {
                testData[i] = new Double[linesNumber * columnsNumberMultiplier];

                for (int j = 0; j < 5 * columnsNumberMultiplier; j++)
                {
                    testData[i][j] = j + 1;
                }
            }
            
            return testData;
        }
        
        [Fact]
        public void TestDoubleConstructor()
        {
            var testData = GetTestData();
            var m = new Matrix(testData);

            Assert.Equal(m.ColumnsNumber, columnsNumber);
            Assert.Equal(m.LinesNumber, linesNumber);

        }
        
        [Fact]
        public void TestSingleThreadedNormalization()
        {
            BaseNormalizer normlize = new SingleThreadedNormalization();
            var mtrx = GetDataForSingleThreadedNormalization();
            var normalizedMatrix = normlize.Normalize(mtrx);

            Assert.Equal(normalizedMatrix[0, 0], -0.5);
            Assert.Equal(normalizedMatrix[1, 1], -0.25);
            Assert.Equal(normalizedMatrix[3, 2], 0);
            Assert.Equal(normalizedMatrix[4, 3], 0.25);
            Assert.Equal(normalizedMatrix[4, 4], 0.5);
        }

        [Fact]
        public void TestMultyThreadedNormalizationData()
        {
            BaseNormalizer normlize = new MultiThreadedNormalization();
            var mtrx = GetDataForSingleThreadedNormalization();
            var normalizedMatrix = normlize.Normalize(mtrx);

            Assert.Equal(normalizedMatrix[0, 0], -0.5);
            Assert.Equal(normalizedMatrix[1, 1], -0.25);
            Assert.Equal(normalizedMatrix[3, 2], 0);
            Assert.Equal(normalizedMatrix[4, 3], 0.25);
            Assert.Equal(normalizedMatrix[4, 4], 0.5);
        }

        /// <summary>
        /// Checks if Miltithreded implementation gives the same result as single threaded
        /// </summary>
        [Fact]
        public void TestMultyThreadedNormalization()
        {
            
            BaseNormalizer normlize = new MultiThreadedNormalization();
            var mtrx = GetDataForSingleThreadedNormalization();
            var normalizedMatrix = normlize.Normalize(mtrx);
            

           
            BaseNormalizer normlize1 = new SingleThreadedNormalization();
            var mtrx1 = GetDataForSingleThreadedNormalization();
            var normalizedMatrix1 = normlize1.Normalize(mtrx1);
            

            for (int i = 0; i < normalizedMatrix.LinesNumber; i++)
            {
                for (int j = 0; j < normalizedMatrix.ColumnsNumber; j++)
                {
                    //var diff = normalizedMatrix[i, j] - normalizedMatrix1[i, j];
                    //Assert.True(diff < 0.001);
                    Assert.Equal(normalizedMatrix[i, j], normalizedMatrix1[i, j]);
                }
            }
        }
    }
}
