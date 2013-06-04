using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3.Common.GreedyAlgorithm
{
    /// <summary>
    /// Huffman code compress data very effectively
    /// We can use prefix free code to unambiguous a file.
    /// </summary>
    public class HuffmanTree<T>
    {
        /// <summary>
        /// we can use a binary tree to represent prefix free code where 0 means go to the left child and 1 means go to the right child.
        /// Let c.freq denote the frequency of c in the file and let dt(c) denote the depth of c's leaf in the tree.
        /// The number of bits required to encode a file is 
        /// sum(c.freq*dt(c))
        /// </summary>
        public static Character<T> Build(List<Tuple<T,double>> characters)
        {
            if (characters == null || characters.Count <= 0)
            {
                throw new ArgumentNullException("argument cann't be null or empty");
            }

            int n = characters.Count;
            Character<T>[] arrs = new Character<T>[n];
            //build a min priority queue using minheap
            for (int i = 0; i < n; i++)
            {
                arrs[i] = new Character<T>(characters[i].Item1,characters[i].Item2);
            }

            MinHeap<Character<T>> heap = MinHeap<Character<T>>.BuildMinHeap(arrs);

            for (int i = 0; i < n-1; i++)
            {
                Character<T> combine = new Character<T>();
                Character<T> left = heap.Pop();
                Character<T> right = heap.Pop();
                combine.Left = left;
                combine.Right = right;
                combine.Freq = left.Freq + right.Freq;
                heap.Insert(combine);
            }

            return heap.Peek();
        }
    }

    
    public class Character<T>:IComparable<Character<T>> 
    {
        public T Alphabet { get;internal set; }

        /// <summary>
        /// the freqency of character, must be > 0
        /// </summary>
        public double Freq { get; internal set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Character<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("argument can not be null");
            }

            if (this.Freq > other.Freq)
            {
                return 1;
            }
            else if (this.Freq < other.Freq)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public Character<T> Left { get; set; }
        public Character<T> Right { get; set; }


        public Character()
        {
            
        }


        public Character(T alphabet, double freq)
        {
            Alphabet = alphabet;
            Freq = freq;
        }
    }
}
