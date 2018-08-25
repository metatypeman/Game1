using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.PersistLogicalDataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public abstract class BaseCGStorage: ICGStorage
    {
        protected BaseCGStorage(ContextOfCGStorage context)
        {
            Context = context;
            DictionaryName = Context?.EntityDictionary?.Name;
        }

        public ContextOfCGStorage Context { get; private set; }
        public abstract KindOfCGStorage KindOfStorage { get; }

        public virtual void Append(RuleInstance ruleInstance)
        {
            throw new NotImplementedException();
        }

        public virtual void Append(ICGStorage storage)
        {
            throw new NotImplementedException();
        }

        public virtual void Append(RuleInstancePackage ruleInstancePackage)
        {
            throw new NotImplementedException();
        }

        public virtual IList<RuleInstance> AllRuleInstances => null;

        public string DictionaryName { get; private set; }

        public virtual IList<IndexedRulePart> GetIndexedRulePartOfFactsByKeyOfRelation(ulong key)
        {
            throw new NotImplementedException();
        }

        public virtual IList<IndexedRulePart> GetIndexedRulePartWithOneRelationWithVarsByKeyOfRelation(ulong key)
        {
            throw new NotImplementedException();
        }

        public virtual IList<ResolverForRelationExpressionNode> GetAllRelations()
        {
            throw new NotImplementedException();
        }

        public virtual RuleInstance GetRuleInstanceByKey(ulong key)
        {
            throw new NotImplementedException();
        }

        public virtual IndexedRuleInstance GetIndexedRuleInstanceByKey(ulong key)
        {
            throw new NotImplementedException();
        }

        public virtual IndexedRuleInstance GetIndexedAdditionalRuleInstanceByKey(ulong key)
        {
            var a = this;
            throw new NotImplementedException();
        }

        public virtual IList<ResolverForRelationExpressionNode> AllRelationsForProductions => throw new NotImplementedException();

        public virtual RuleInstance MainRuleInstance => throw new NotImplementedException();
        public virtual IndexedRuleInstance MainIndexedRuleInstance => throw new NotImplementedException();

        public virtual ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar)
        {
            return null;
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
            sb.AppendLine($"{spaces}{nameof(KindOfStorage)} = {KindOfStorage}");
            return sb.ToString();
        }
    }
}
