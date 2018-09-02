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
        private Dictionary<ulong, KindOfKey> mKindOfKeyDict = new Dictionary<ulong, KindOfKey>();
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

                var kindOfKey = NGetKindOfKeyByName(name);
                mKindOfKeyDict[mCurrIndex] = kindOfKey;
                return mCurrIndex;
            }
        }

        private KindOfKey NGetKindOfKeyByName(string name)
        {
            if (name.StartsWith("#"))
            {
                return KindOfKey.Entity;
            }

            if (name.StartsWith("@#"))
            {
                return KindOfKey.EntityConditionVar;
            }

            if (name.StartsWith("@$"))
            {
                return KindOfKey.ExternalParamVar;
            }

            if (name.StartsWith("@"))
            {
                return KindOfKey.Var;
            }

            if (name.StartsWith("?"))
            {
                return KindOfKey.QuestionVar;
            }

            return KindOfKey.Concept;
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

        public KindOfKey GetKindOfKey(ulong key)
        {
            lock (mLockObj)
            {
                return NGetKindOfKey(key);
            }
        }

        private KindOfKey NGetKindOfKey(ulong key)
        {
            if(mKindOfKeyDict.ContainsKey(key))
            {
                return mKindOfKeyDict[key];
            }

            return KindOfKey.Unknown;
        }

        public bool IsEntity(ulong key)
        {
            lock (mLockObj)
            {
                var kindOfKey = NGetKindOfKey(key);
                return kindOfKey == KindOfKey.Entity;
            }
        }
    }
}
