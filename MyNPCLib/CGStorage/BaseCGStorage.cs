using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public abstract class BaseCGStorage: ICGStorage
    {
        protected BaseCGStorage(ContextOfCGStorage context)
        {
            mContext = context;
            DictionaryName = mContext.EntityDictionary.Name;
        }

        private ContextOfCGStorage mContext;
        public abstract KindOfCGStorage Kind { get; }

        //It is temporary public for construction time. It will be private after complete construction.
        public string DictionaryName { get; set; }
        private readonly object mDataLockObj = new object();
        //It is temporary public for construction time. It will be private after complete construction.
        public IList<RuleInstance> mRuleInstancesList { get; set; }
        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IndexedRuleInstance> mIndexedRuleInstancesDict { get; set; }
        //It is temporary public for construction time. It will be private after complete construction.
        public IDictionary<ulong, IList<IndexedRulePart>> mIndexedRulePartsDict { get; set; }

        public void Init()
        {
            lock(mDataLockObj)
            {
                mRuleInstancesList = new List<RuleInstance>();
                mIndexedRuleInstancesDict = new Dictionary<ulong, IndexedRuleInstance>();
                mIndexedRulePartsDict = new Dictionary<ulong, IList<IndexedRulePart>>();
            }
        }

        public IList<IndexedRulePart> GetIndexedRulePartByKeyOfRelation(ulong key)
        {
            lock (mDataLockObj)
            {
                if(mIndexedRulePartsDict.ContainsKey(key))
                {
                    return mIndexedRulePartsDict[key];
                }

                return null;
            }
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            return sb.ToString();
        }
    }
}
