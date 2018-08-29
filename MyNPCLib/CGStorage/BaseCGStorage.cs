using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
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
            mLogicalSearcher = new LogicalSearcher(context);
        }

        private LogicalSearcher mLogicalSearcher;
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

        public virtual RuleInstance MainRuleInstance => null;
        public virtual IndexedRuleInstance MainIndexedRuleInstance => null;

        public virtual ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar)
        {
            return null;
        }

        public event Action OnChanged;

        protected void EmitOnChanged()
        {
            OnChanged?.Invoke();
        }

        public IList<ulong> GetEntitiesIdList(ICGStorage query)
        {
            var searchOptions = new LogicalSearchOptions();
            searchOptions.DataSource = this;
            searchOptions.QuerySource = query;

            return mLogicalSearcher.GetEntitiesIdList(searchOptions);
        }

        public ICGStorage Search(ICGStorage query)
        {
            var searchOptions = new LogicalSearchOptions();
            searchOptions.DataSource = this;
            searchOptions.QuerySource = query;

            var searchResult = mLogicalSearcher.Run(searchOptions);

            var querySearchResultCGStorage = new QueryResultCGStorage(Context, searchResult);
            return querySearchResultCGStorage;
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
