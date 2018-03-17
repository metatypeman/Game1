using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class EntityDictionary : IEntityDictionary
    {
        private object mLockObj = new object();
        private Dictionary<string, ulong> mCaseInsensitiveWordsDict = new Dictionary<string, ulong>();
        private ulong mCurrIndex;

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
