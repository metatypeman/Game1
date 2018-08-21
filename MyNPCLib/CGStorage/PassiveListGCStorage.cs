using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class PassiveListGCStorage : BaseCGStorage
    {
        public PassiveListGCStorage(ContextOfCGStorage context, IList<RuleInstance> ruleInstances)
            : base(context)
        {
            mRuleInstances = ruleInstances;
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.PassiveList;

        private IList<RuleInstance> mRuleInstances;

        public override IList<RuleInstance> AllRuleInstances => mRuleInstances;
    }
}
