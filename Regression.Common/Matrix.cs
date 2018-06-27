using System;
using System.Linq;

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
            InitCleanData(linesNumber, columnsNumber, val);
        }

        private void InitCleanData(int linesNumber, int columnsNumber, double val = 0.0)
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

        /// <summary>
        /// Creates instance from csv file. Requirements for csv file are the following:
        /// First line will be skipped ( as usually it has some header values )
        /// columns should be separated by ,
        /// For numbers as delimiter use .
        /// </summary>
        /// <param name="csvFileName">File name for loading data</param>
        public Matrix(string csvFileName)
        {
            var lines = System.IO.File.ReadAllLines(csvFileName).Skip(1).Where(l => !string.IsNullOrEmpty(l)).ToList();
            int numberOfColumns = lines[0].Count(x => x == ',') + 1;
            int numberfOfLines = lines.Count;

            InitCleanData(numberfOfLines, numberOfColumns);

            int lineNumber = 0;
            foreach (var line in lines)
            {
                var elements = line.Split(',').Select(a => double.Parse(a));
                int idxElement = 0;
                foreach (var element in elements)
                {
                    Data[lineNumber][idxElement] = element;
                    idxElement++;
                }

                lineNumber++;
            }
        }

        public Matrix AddColumn(double[] column)
        {
            var matrix = new Matrix(this.LinesNumber, this.ColumnsNumber + 1);

            for (int i = 0; i < matrix.LinesNumber; i++)
            {
                int j;
                for ( j = 0; j < this.ColumnsNumber; j++)
                {
                    matrix[i, j] = this[i, j];
                }

                matrix[i, j] = column[i];
            }
            return matrix;
        }

        public Double this[int row, int column]
        {
            get => Data[row][column];
            set => Data[row][column] = value;
        }

        public Double[] this[int rowNumber] => Data[rowNumber];

        public void Dispose()
        {
            Data = null;
        }
    }
}
