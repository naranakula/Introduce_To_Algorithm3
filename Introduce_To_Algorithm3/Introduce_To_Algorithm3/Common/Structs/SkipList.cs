using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Structs
{
    /// <summary>
    /// skip list can search, insert, delete at O(lgn)
    /// a skip list which constructed by many list{S0,S1,......,Sn}  must follow 3 rules:
    /// the height of skip list are expected at O(lgn), use memory O(n)
    /// 1) each list must contains two special element:+∞和-∞
    /// 2）S0 contains all the element, and the element are increasing sorted
    /// 3) ifi&lt;j, then for any e∈Sj, e∈Si,
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class SkipList<K,V> where K:IComparable<K>,IEquatable<K>
    {
        #region Member

        /// <summary>
        /// bottom support list for store struct
        /// Si = supportList[i]
        /// </summary>
        private List<SortedSkipLinkedList<K,V>> supportList;

        /// <summary>
        /// Probability factor[0,1]
        /// the probability go to next height
        /// </summary>
        private double ProbFactor;

        /// <summary>
        /// random generator
        /// </summary>
        private Random random;

        /// <summary>
        /// the number of element in list
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// the height of skip list
        /// </summary>
        public int Height
        {
            get { return supportList.Count; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="probFactor"></param>
        public SkipList(double probFactor = 0.5)
        {
            supportList = new List<SortedSkipLinkedList<K, V>>();
            Count = 0;
            this.ProbFactor = probFactor;
            random = new Random();
        } 

        #endregion

        #region RandomDecision

        /// <summary>
        /// if true, increase height by 1; if false, just insert
        /// </summary>
        /// <returns></returns>
        public bool RandomDecision()
        {
            if (random.NextDouble() < ProbFactor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Search

        /// <summary>
        /// find the node, if not found, return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SkipListNode<K, V> Search(K key)
        {
            if (supportList.Count <= 0)
            {
                return null;
            }

            SortedSkipLinkedList<K, V> sh = supportList[supportList.Count - 1];
            if (sh.HeadNode == null)
            {
                return null;
            }

            SkipListNode<K, V> node = sh.HeadNode;
            SkipListNode<K, V> rightNode = node.Next;

            while (true)
            {
                //reach to end
                if (rightNode.IsMaxNode)
                {
                    node = node.Down;
                    if (node == null)
                    {
                        return null;
                    }
                    else
                    {
                        rightNode = node.Next;
                        continue;
                    }
                }

                int result = rightNode.Key.CompareTo(key);
                if (result == 0)
                {
                    return rightNode;
                }
                else if (result >= 0)
                {
                    node = node.Down;
                    if (node == null)
                    {
                        return null;
                    }
                    else
                    {
                        rightNode = node.Next;
                    }
                }
                else
                {
                    node = node.Next;
                    rightNode = node.Next;
                }
            }
        }

        /// <summary>
        /// search, if not found, return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<K, V> SearchKey(K key)
        {
            var node = Search(key);
            if (node == null)
            {
                return null;
            }
            else
            {
                return new Tuple<K, V>(node.Key,node.Value);
            }
        }


        /// <summary>
        /// find the node & the height of the node in, if not found, return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<SkipListNode<K, V>,int> SearchBy(K key)
        {
            if (supportList.Count <= 0)
            {
                return null;
            }

            SortedSkipLinkedList<K, V> sh = supportList[supportList.Count - 1];
            int height = supportList.Count;
            if (sh.HeadNode == null)
            {
                return null;
            }

            SkipListNode<K, V> node = sh.HeadNode;
            SkipListNode<K, V> rightNode = node.Next;

            while (true)
            {
                //reach to end
                if (rightNode.IsMaxNode)
                {
                    node = node.Down;
                    height--;
                    if (node == null)
                    {
                        return null;
                    }
                    else
                    {
                        rightNode = node.Next;
                        continue;
                    }
                }

                int result = rightNode.Key.CompareTo(key);
                if (result == 0)
                {
                    return new Tuple<SkipListNode<K, V>, int>(rightNode,height);
                }
                else if (result >= 0)
                {
                    node = node.Down;
                    height--;
                    if (node == null)
                    {
                        return null;
                    }
                    else
                    {
                        rightNode = node.Next;
                    }
                }
                else
                {
                    node = node.Next;
                    rightNode = node.Next;
                }
            }
        }


        #endregion

        #region Insert

        /// <summary>
        /// if randonDecision = true, increase height by 1, else just insert
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Insert(K key, V val)
        {
            Count++;

            bool decision = RandomDecision();

            int height = 1;
            while (decision)
            {
                //increase height by 1;
                height++;
                decision = RandomDecision();
            }

            while (supportList.Count < height)
            {
                var listn = new SortedSkipLinkedList<K, V>();
                if (supportList.Count > 0)
                {
                    var listn_1 = supportList[supportList.Count - 1];
                    listn_1.HeadNode.Up = listn.HeadNode;
                    listn.HeadNode.Down = listn_1.HeadNode;
                    listn_1.TailNode.Up = listn.TailNode;
                    listn.TailNode.Down = listn_1.TailNode;
                }
                supportList.Add(listn);
            }

            //the height column
            var sh = supportList[height - 1];
            var insertPoint = sh.SearchInsertPoint(key);

            SkipListNode<K,V> upNode = null;
            while (insertPoint != null)
            {
                var newNode = new SkipListNode<K, V>(key,val);
                newNode.Next = insertPoint.Next;
                newNode.Prev = insertPoint;
                insertPoint.Next.Prev = newNode;
                insertPoint.Next = newNode;
                sh.Count++;
                if (upNode != null)
                {
                    upNode.Down = newNode;
                    newNode.Up = upNode;
                }
                upNode = newNode;

                insertPoint = insertPoint.Down;

                if (insertPoint == null)
                {
                    break;
                }
                var rightNode = insertPoint.Next;
                while (true)
                {
                    if (rightNode.IsMaxNode)
                    {
                        break;
                    }

                    if (rightNode.Key.CompareTo(key) < 0)
                    {
                        insertPoint = rightNode;
                        rightNode = insertPoint.Next;
                    }
                    else
                    {
                        break;
                    }
                }
                height--;
                sh = supportList[height - 1];
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// if contains key, delete & return true, else return false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(K key)
        {
            var tuple = SearchBy(key);

            if (tuple == null)
            {
                return false;
            }
            else
            {
                Count--;

                var top = tuple.Item1;
                int height = tuple.Item2;
                while (top != null)
                {
                    height--;
                    supportList[height].Count--;
                    top.Next.Prev = top.Prev;
                    top.Prev.Next = top.Next;
                    top = top.Down;
                    if (supportList[height].Count <= 0)
                    {
                        supportList.RemoveAt(height);
                        if (top != null)
                        {
                            supportList[height - 1].HeadNode.Up = null;
                            supportList[height - 1].TailNode.Up = null;
                        }
                    }
                }

                return true;
            }
        }

        #endregion

    }
}
