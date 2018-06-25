using System;

namespace Regression.Common
{
    public class Matrix : IDisposable
    {
        private Double[][] _data = null;

        public int LinesNumber { get; private set; }
        public int ColumnsNumber { get; private set; }
        public Double[][] Data { get => _data; set => _data = value; }

        public Matrix(Double[][] data)
        {
            this.ColumnsNumber = data[0].Length;
            this.LinesNumber = data.Length;
            Data = data;
        }

        public static implicit operator Matrix(Double[][] data)
        {
            var result = new Matrix(data);
            return result;
        }

        /// <summary>
        /// Creates new Matrix with linesNumber lines and columnsNumber columns and inits elements with 0.0
        /// </summary>
        /// <param name="linesNumber">Number of lines</param>
        /// <param name="columnsNumber">Number of columns</param>
        /// <param name="val">value of each element</param>
        public Matrix(int linesNumber, int columnsNumber, double val = 0.0)
        {
            this.LinesNumber = linesNumber;
            this.ColumnsNumber = columnsNumber;

            Data = new Double[linesNumber][];
            for (var i = 0; i < linesNumber; i++)
            {
                Data[i] = new Double[columnsNumber];
                for (int j = 0; j < columnsNumber; j++)
                {
                    Data[i][j] = val;
                }
            }
        }

        public Double this[int index, int index2]
        {
            get => Data[index][index2];
            set => Data[index][index2] = value;
        }

        public Double[] this[int rowNumber] => Data[rowNumber];

        public void Dispose()
        {
            Data = null;
        }
    }
}
