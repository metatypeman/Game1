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
    }
}
