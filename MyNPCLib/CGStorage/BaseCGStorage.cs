using MyNPCLib.ConvertingPersistLogicalDataToIndexing;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.PersistLogicalDataStorage;
using MyNPCLib.Variants;
using MyNPCLib.VariantsConverting;
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
            mEntityDictionary = Context?.EntityDictionary;
            DictionaryName = mEntityDictionary?.Name;
            mLogicalSearcher = new LogicalSearcher(context);
            mKeyOfVarForProperty = mEntityDictionary.GetKey(mNameOfVarForProperty);
        }

        private IEntityDictionary mEntityDictionary;
        private LogicalSearcher mLogicalSearcher;
        private string mNameOfVarForProperty = "?X";
        private ulong mKeyOfVarForProperty;
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

        public ResultOfVarOfQueryToRelation GetResultOfVar(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultOfVar(keyOfVar);
        }

        public virtual ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar)
        {
            return null;
        }

        public BaseVariant GetResultOfVarAsVariant(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultOfVarAsVariant(keyOfVar);
        }

        public virtual BaseVariant GetResultOfVarAsVariant(ulong keyOfVar)
        {
            return null;
        }

        public object GetResultOfVarAsObject(string varName)
        {
            var keyOfVar = mEntityDictionary.GetKey(varName);
            return GetResultOfVarAsObject(keyOfVar);
        }

        public virtual object GetResultOfVarAsObject(ulong keyOfVar)
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

        public virtual BaseVariant GetPropertyValueAsVariant(ulong entityId, ulong propertyId)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyId = {propertyId}");
#endif

            var searchResult = CreateLogicalSearchResultForGetProperty(entityId, propertyId);
            return searchResult.GetResultOfVarAsVariant(mKeyOfVarForProperty);
        }

        public virtual BaseVariant GetPropertyValueAsVariant(ulong entityId, string propertyName)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyName = {propertyName}");
#endif
            var propertyId = mEntityDictionary.GetKey(propertyName);
            return GetPropertyValueAsVariant(entityId, propertyId);
        }

        public virtual object GetPropertyValueAsObject(ulong entityId, ulong propertyId)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyId = {propertyId}");
#endif

            var searchResult = CreateLogicalSearchResultForGetProperty(entityId, propertyId);
            return searchResult.GetResultOfVarAsObject(mKeyOfVarForProperty);
        }

        private LogicalSearchResult CreateLogicalSearchResultForGetProperty(ulong entityId, ulong propertyId)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyId = {propertyId}");
#endif

            var queryIndexedRuleInstance = CreateGetQuery(entityId, propertyId);

#if DEBUG
            LogInstance.Log($"queryIndexedRuleInstance = {queryIndexedRuleInstance}");
#endif

            var searchOptions = new LogicalSearchOptions();
            searchOptions.DataSource = this;
            searchOptions.QueryExpression = queryIndexedRuleInstance;

            var searchResult = mLogicalSearcher.Run(searchOptions);

#if DEBUG
            LogInstance.Log($"searchResult = {searchResult}");
#endif

            return searchResult;
        }

        private IndexedRuleInstance CreateGetQuery(ulong entityId, ulong propertyId)
        {
            var relationKey = propertyId;
            var relationName = mEntityDictionary.GetName(relationKey);

            var entityName = mEntityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = mEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.QuestionVars;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = mEntityDictionary.GetKey(ruleInstance.Name);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            expr3.Name = relationName;
            expr3.Key = relationKey;

            if(mEntityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

            var param_2 = new QuestionVarExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = mNameOfVarForProperty;
            param_2.Key = mKeyOfVarForProperty;

#if DEBUG
            var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif
            var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

            return indexedRuleInstance;
        }

        public virtual object GetPropertyValueAsObject(ulong entityId, string propertyName)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyName = {propertyName}");
#endif
            var propertyId = mEntityDictionary.GetKey(propertyName);
            return GetPropertyValueAsObject(entityId, propertyId);
        }

        public virtual void SetPropertyValueAsAsVariant(ulong entityId, ulong propertyId, BaseVariant value)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            NSetPropertyValue(entityId, propertyId, value);
        }

        public virtual void SetPropertyValueAsAsVariant(ulong entityId, string propertyName, BaseVariant value)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyName = {propertyName} value = {value}");
#endif
            var propertyId = mEntityDictionary.GetKey(propertyName);

            SetPropertyValueAsAsVariant(entityId, propertyId, value);
        }

        public virtual void SetPropertyValueAsAsObject(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            var variant = VariantsConvertor.ConvertObjectToVariant(value);

#if DEBUG
            LogInstance.Log($"variant = {variant}");
#endif

            NSetPropertyValue(entityId, propertyId, variant);
        }

        public virtual void SetPropertyValueAsAsObject(ulong entityId, string propertyName, object value)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyName = {propertyName} value = {value}");
#endif
            var propertyId = mEntityDictionary.GetKey(propertyName);
            SetPropertyValueAsAsObject(entityId, propertyId, value);
        }

        private void NSetPropertyValue(ulong entityId, ulong propertyId, BaseVariant value)
        {
#if DEBUG
            LogInstance.Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif
            var ruleInstance = CreateRuleInstanceForSetQuery(entityId, propertyId,  value);

            Append(ruleInstance);
        }

        private RuleInstancePackage CreateRuleInstanceForSetQuery(ulong entityId, ulong propertyId, BaseVariant variant)
        {
            var expressionOfVariant = VariantsConvertor.ConvertVariantToExpressionNode(variant);

#if DEBUG
            LogInstance.Log($"expressionOfVariant = {expressionOfVariant}");
#endif

            var relationKey = propertyId;
            var relationName = mEntityDictionary.GetName(relationKey);

            var entityName = mEntityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = mEntityDictionary.Name;
            if(variant.IsEntityCondition)
            {
                ruleInstance.Kind = KindOfRuleInstance.EntityCondition;
            }
            else
            {
                ruleInstance.Kind = KindOfRuleInstance.Fact;
            }
            
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = mEntityDictionary.GetKey(ruleInstance.Name);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            expr3.Name = relationName;
            expr3.Key = relationKey;

            if (mEntityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

            var param_2 = expressionOfVariant;

#if DEBUG
            var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif

            return ruleInstance;
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
