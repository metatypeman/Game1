using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance
    {
        public List<RuleInstance> ResultsList { get; set; } = new List<RuleInstance>();
        public Dictionary<ASTNodeOfLogicalQuery, RulePart> RulePartsDict { get; set; } = new Dictionary<ASTNodeOfLogicalQuery, RulePart>();
        public IEntityDictionary EntityDictionary { get; set; }
    }
}
