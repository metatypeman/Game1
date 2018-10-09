using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class ContextOfConvertorToCompleteRuleInstance
    {
        public IEntityDictionary EntityDictionary { get; set; }
        public IDictionary<ulong, BaseExpressionNode> ResultOfVarOfQueryToRelationDict { get; set; }
        public IDictionary<RulePart, RulePart> RulePartsDict { get; set; } = new Dictionary<RulePart, RulePart>();
    }
}
