using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    /// <summary>
    /// a matrix int[,]
    /// </summary>
    public class IntMatrix:Matrix<int>
    {
        public IntMatrix(int row, int column) : base(row, column)
        {
        }
    }
}
