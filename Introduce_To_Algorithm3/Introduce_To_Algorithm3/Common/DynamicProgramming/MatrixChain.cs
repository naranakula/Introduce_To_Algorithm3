using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.DynamicProgramming
{
    public class MatrixChain
    {

        /// <summary>
        /// p represents A0A1.....An  matrix multiply
        /// m[i,j] = the cost of  AiAi+1....Aj
        /// 
        /// m[i,j] = 0, if i=j,  m[i,j] = min{ m[i,k]+m[k+1,j]+Pi-1PkPj }, if i&lt;j

        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static  int[,] MatrixChainOrder(List<int[,]> p,out int[,] s)
        {
            int n = p.Count - 1;
            int[,] m= new int[n+1,n+1];
            s = new int[n+1,n+1];
            for (int l = 0; l <= n; l++)
            {
                for (int i = 1; i <= n-l+1; i++)
                {
                    int j = i + l - 1;
                    m[i, j] = int.MaxValue;
                    for (int k = i; k <=j-1; k++)
                    {
                        int q = m[i, k] + m[k + 1, j] + p[i].GetLength(0)*p[k].GetLength(1)*p[j].GetLength(1);
                        if(q<m[i,j])
                        {
                            m[i, j] = q;
                            s[i, j] = k;
                        }
                    }
                }
            }

            return m;
        }
    }
}
