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
                return mCurrIndex;
            }
        }
    }
}
