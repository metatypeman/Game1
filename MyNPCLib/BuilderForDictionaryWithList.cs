using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BuilderForDictionaryWithList<K, V>
    {
        private Dictionary<K, List<V>> mDict = new Dictionary<K, List<V>>();

        public void Add(K key, V value)
        {
            if(mDict.ContainsKey(key))
            {
                var list = mDict[key];
                if(!list.Contains(value))
                {
                    list.Add(value);
                }
                return;
            }

            {
                var list = new List<V>() { value };
                mDict[key] = list;
            }
        }

        public Dictionary<K, List<V>> GetResult()
        {
            return mDict;
        }
    }
}
