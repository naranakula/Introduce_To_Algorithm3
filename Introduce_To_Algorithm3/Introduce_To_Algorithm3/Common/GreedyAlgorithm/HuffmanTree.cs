using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Introduce_To_Algorithm3.Common.GreedyAlgorithm
{
    /// <summary>
    /// Huffman code compress data very effectively
    /// We can use prefix free code to unambiguous a file.
    /// </summary>
    public class HuffmanTree
    {
        /// <summary>
        /// we can use a binary tree to represent prefix free code where 0 means go to the left child and 1 means go to the right child.
        /// Let c.freq denote the frequency of c in the file and let dt(c) denote the depth of c's leaf in the tree.
        /// The number of bits required to encode a file is 
        /// sum(c.freq*dt(c))
        /// </summary>
        public void Build()
        {

        }
    }

    /*
    public class Character:IComparable<double>
    {
        public Char Alphabet { get; set; }

        /// <summary>
        /// the freqency of character, must be > 0
        /// </summary>
        public double Freq { get; set; }
    }*/
}
