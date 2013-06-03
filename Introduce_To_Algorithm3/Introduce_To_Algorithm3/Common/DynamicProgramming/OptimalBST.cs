using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Introduce_To_Algorithm3.Common.Structs;

namespace Introduce_To_Algorithm3.Common.DynamicProgramming
{
    /// <summary>
    /// Optimal binary search tree
    /// we want to translate english sentence to Chinese sentence.
    /// suppose we already break english sentence to a couple of words.
    /// we use bst to maintain the relationship of english word and chinese word
    /// we use the english word as key and the chinese word as satellite data.
    /// 
    /// as we know some words are used frequently while others are rarely used.
    /// How can we minimum the total search numbers for all words?
    /// we need optimal binary search tree.
    /// 
    /// suppose we have {K1,K2,... ..., Kn} different keys/words in sorted order. for each Ki the probability is Pi.
    /// we then have {D0,D1,D2,... ..., Dn} kind keys/words which doesn't appear in obst because of we don't have chinese word for those english word. Di have probability Qi;
    /// Di are all the values between Ki and Ki+1, which are leaf of obst.
    /// we have substruct that: Kr is the root of the Optimal binary search tree. The left and right tree of Kr must be Optimal binary search tree also. we can use cut and paste theory to prove it.
    /// Suppose the Left tree of Kr is Krl, if Krl isn't Optimal binary search tree, then we must have a tree T have better performance than Krl, we cut krl and paste T to replace it. Then we have a tree better then Kr which offends to Kr is OBST.
    /// 
    /// 
    /// 
    /// suppose we have subproblem ki,......,kj.
    /// define e[i,j] is the expect cost of searching obst contains ki,... ...,kj
    /// 
    /// if j=i-1,    e[i,j] = Qi-1
    /// if i<=j,     e[i,j] = min{e[i,r-1]+e[r+1,j]+w[i,j]}  where i=<r<=j    w[i,j] is the sum of probilities of ki,......kj
    /// </summary>
    public class OptimalBst<K, V> where K : IComparable<K>, IEquatable<K>
    {
    }
}
