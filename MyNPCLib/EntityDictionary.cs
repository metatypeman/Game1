using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class EntityDictionary : IEntityDictionary
    {
        public EntityDictionary()
        {
            mName = Guid.NewGuid().ToString("D");
        }

        private object mLockObj = new object();
        private string mName;
        private Dictionary<string, ulong> mCaseInsensitiveWordsDict = new Dictionary<string, ulong>();
        private Dictionary<ulong, string> mCaseInsensitiveBackWordsDict = new Dictionary<ulong, string>();
        private ulong mCurrIndex;

        public string Name => mName;

        public ulong GetKey(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return 0ul;
            }

            name = name.ToLower().Trim();

            lock(mLockObj)
            {
                if(mCaseInsensitiveWordsDict.ContainsKey(name))
                {
                    return mCaseInsensitiveWordsDict[name];
                }

                mCurrIndex++;
                mCaseInsensitiveWordsDict[name] = mCurrIndex;
                mCaseInsensitiveBackWordsDict[mCurrIndex] = name;
                return mCurrIndex;
            }
        }

        public string GetName(ulong key)
        {
            lock (mLockObj)
            {
                if(mCaseInsensitiveBackWordsDict.ContainsKey(key))
                {
                    return mCaseInsensitiveBackWordsDict[key];
                }
                return string.Empty;
            }
        }
    }
}
