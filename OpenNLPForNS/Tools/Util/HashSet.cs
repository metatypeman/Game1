//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenNLP.Tools.Util
{
    /// <summary> 
    /// This class manages a hash set of elements.
    /// </summary>
    public class HashSet<T> : Set<T>
    {
        /// <summary>
        /// Creates a new hash set collection.
        /// </summary>
        public HashSet() { }

        /// <summary>
        /// Creates a new hash set collection.
        /// </summary>
        /// <param name="collection">
        /// The collection to initialize the hash set with.
        /// </param>
        public HashSet(IEnumerable<T> collection)
        {
            this.AddRange(collection);
        }

        /// <summary>
        /// Creates a new hash set with the given capacity.
        /// </summary>
        /// <param name="capacity">
        /// The initial capacity of the hash set.
        /// </param>
        public HashSet(int capacity)
        {
            this.Capacity = capacity;
        }

        /// <summary>
        /// Creates a new hash set with the given capacity.
        /// </summary>
        /// <param name="capacity">
        /// The initial capacity of the hash set.
        /// </param>
        /// <param name="loadFactor">
        /// The load factor of the hash set.
        /// </param>
        public HashSet(int capacity, float loadFactor)
        {
            this.Capacity = capacity;
        }

        /// <summary>
        /// Creates a copy of the HashSet.
        /// </summary>
        /// <returns> A copy of the HashSet.</returns>
        public virtual object HashSetClone()
        {
            return MemberwiseClone();
        }

        public static Set<IDictionaryEnumerator> EntrySet(IDictionary hashtable)
        {
            IDictionaryEnumerator hashEnumerator = hashtable.GetEnumerator();
            var hashSet = new Set<IDictionaryEnumerator>();
            while (hashEnumerator.MoveNext())
            {
                var hash = new Hashtable { { hashEnumerator.Key, hashEnumerator.Value } };
                hashSet.Add(hash.GetEnumerator());
            }
            return hashSet;
        }
    }
}
