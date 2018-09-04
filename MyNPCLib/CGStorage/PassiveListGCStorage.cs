using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class PassiveListGCStorage : BaseProxyStorage
    {
        public PassiveListGCStorage(IEntityDictionary entityDictionary, IList<RuleInstance> ruleInstances)
            : base(entityDictionary)
        {
            mRuleInstancesList = ruleInstances;

            if(!mRuleInstancesList.IsEmpty())
            {
                foreach(var ruleInstance in mRuleInstancesList)
                {
                    ruleInstance.DataSource = this;
                }
            }
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.PassiveList;

        public override IList<RuleInstance> AllRuleInstances => mRuleInstancesList;

        private IList<RuleInstance> mRuleInstancesList;

        public override RuleInstance GetRuleInstanceByKey(ulong key)
        {
            return mRuleInstancesList.FirstOrDefault(p => p.Key == key);
        }
    }
}
