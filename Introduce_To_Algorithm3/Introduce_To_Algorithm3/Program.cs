using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.GraphEx;
using Introduce_To_Algorithm3.Common.Utils;
using Introduce_To_Algorithm3.Common.Structs;
using Introduce_To_Algorithm3.Common.Utils.sockets;
using System.Net.Sockets;
using System.Threading;
using Introduce_To_Algorithm3.Common.DynamicProgramming;

namespace Introduce_To_Algorithm3
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<int, int> tree = new Tree<int, int>();
            for (int i = 0; i < 1000000; i++)
            {
                tree.Insert(0, 0);
            }
            var r = tree.InorderTreeWalk();
        }
    }

    public class Node<K,V>where K : IComparable<K>, IEquatable<K>
    {
        public K Key;
        public V Value;

        public Node()
        {
            Key = default(K);
            Value = default(V);
        }

        public Node(K key, V val)
        {
            Key = key;
            Value = val;
        }

        public Node<K, V> Parent;
        public Node<K, V> Left;
        public Node<K, V> Right;
    }

    public class Tree<K, V> where K : IComparable<K>, IEquatable<K>
    {
        private Node<K, V> root;
        public void Insert(K key, V value)
        {
            Node<K, V> node = new Node<K, V>(key,value);
            if (root == null)
            {
                root = node;
                return;
            }

            root.Parent = node;
            node.Left = root;
            root = node;
        }

        /// <summary>
        /// inorder tree walk, which runs at O(n) and return a sorted list
        /// </summary>
        /// <returns></returns>
        public List<Node<K, V>> InorderTreeWalk()
        {
            List<Node<K, V>> lists = new List<Node<K, V>>();

            InorderTreeWalk(ref lists, root);

            return lists;
        }

        /// <summary>
        /// inorder tree walk
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="node"></param>
        private void InorderTreeWalk(ref List<Node<K, V>> lists, Node<K, V> node)
        {
            if (node == null)
            {
                return;
            }

            InorderTreeWalk(ref lists, node.Left);
            lists.Add(node);
            InorderTreeWalk(ref lists, node.Right);
        }
    }
}