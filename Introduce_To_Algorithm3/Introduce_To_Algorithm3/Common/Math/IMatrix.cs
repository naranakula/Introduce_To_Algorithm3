using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.Math
{
    public interface IMatrix<T>
    {
        T Get(int row, int column);
        void Set(int row, int column,T value);
    }
}
