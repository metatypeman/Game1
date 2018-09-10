using MyNPCLib.CGStorage.Helpers;
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
        protected BaseCGStorage(IEntityDictionary entityDictionary)
        {
            EntityDictionary = entityDictionary;
            DictionaryName = EntityDictionary?.Name;
            mLogicalSearcher = new LogicalSearcher(EntityDictionary);
            mKeyOfVarForProperty = EntityDictionary.GetKey(mNameOfVarForProperty);
        }

        public IEntityDictionary EntityDictionary { get; private set; }
        private LogicalSearcher mLogicalSearcher;
        private string mNameOfVarForProperty = "?X";
        private ulong mKeyOfVarForProperty;

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
#if DEBUG
            LogInstance.Log($"GetType().FullName = {GetType().FullName}");
#endif

            throw new NotImplementedException();
        }

        public virtual RuleInstance GetRuleInstanceByKey(ulong key)
        {
#if DEBUG
            LogInstance.Log($"GetType().FullName = {GetType().FullName}");
#endif

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

        public IList<ResultOfVarOfQueryToRelation> GetResultsListOfVar(string varName)
        {
            var keyOfVar = EntityDictionary.GetKey(varName);
            return GetResultsListOfVar(keyOfVar);
        }

        public virtual IList<ResultOfVarOfQueryToRelation> GetResultsListOfVar(ulong keyOfVar)
        {
            return null;
        }

        public IList<BaseVariant> GetResultsListOfVarAsVariant(string varName)
        {
            var keyOfVar = EntityDictionary.GetKey(varName);
            return GetResultsListOfVarAsVariant(keyOfVar);
        }

        public virtual IList<BaseVariant> GetResultsListOfVarAsVariant(ulong keyOfVar)
        {
            return null;
        }

        public IList<object> GetResultsListOfVarAsObject(string varName)
        {
            var keyOfVar = EntityDictionary.GetKey(varName);
            return GetResultsListOfVarAsObject(keyOfVar);
        }

        public virtual IList<object> GetResultsListOfVarAsObject(ulong keyOfVar)
        {
            return null;
        }

        public ResultOfVarOfQueryToRelation GetResultOfVar(string varName)
        {
            var keyOfVar = EntityDictionary.GetKey(varName);
            return GetResultOfVar(keyOfVar);
        }

        public virtual ResultOfVarOfQueryToRelation GetResultOfVar(ulong keyOfVar)
        {
            return null;
        }

        public BaseVariant GetResultOfVarAsVariant(string varName)
        {
            var keyOfVar = EntityDictionary.GetKey(varName);
            return GetResultOfVarAsVariant(keyOfVar);
        }

        public virtual BaseVariant GetResultOfVarAsVariant(ulong keyOfVar)
        {
            return null;
        }

        public object GetResultOfVarAsObject(string varName)
        {
            var keyOfVar = EntityDictionary.GetKey(varName);
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

        public ICGStorage Search(LogicalSearchOptions options)
        {
            options.DataSource = this;

            var searchResult = mLogicalSearcher.Run(options);

            var querySearchResultCGStorage = new QueryResultCGStorage(EntityDictionary, searchResult);
            return querySearchResultCGStorage;
        }

        public ICGStorage Search(ICGStorage query)
        {
            var searchOptions = new LogicalSearchOptions();
            searchOptions.DataSource = this;
            searchOptions.QuerySource = query;

            var searchResult = mLogicalSearcher.Run(searchOptions);

#if DEBUG
            //LogInstance.Log($"searchResult = {searchResult}");
#endif

            var querySearchResultCGStorage = new QueryResultCGStorage(EntityDictionary, searchResult);
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
            //LogInstance.Log($"entityId = {entityId} propertyName = {propertyName}");
#endif
            var propertyId = EntityDictionary.GetKey(propertyName);
            return GetPropertyValueAsVariant(entityId, propertyId);
        }

        public virtual object GetPropertyValueAsObject(ulong entityId, ulong propertyId)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyId = {propertyId}");
#endif

            var searchResult = CreateLogicalSearchResultForGetProperty(entityId, propertyId);
            return searchResult.GetResultOfVarAsObject(mKeyOfVarForProperty);
        }

        private LogicalSearchResult CreateLogicalSearchResultForGetProperty(ulong entityId, ulong propertyId)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyId = {propertyId}");
#endif

            var queryIndexedRuleInstance = PropertiesOfCGStorageHelper.CreateGetQuery(entityId, propertyId, EntityDictionary, mNameOfVarForProperty, mKeyOfVarForProperty);

#if DEBUG
            //LogInstance.Log($"queryIndexedRuleInstance = {queryIndexedRuleInstance}");
#endif

            var searchOptions = new LogicalSearchOptions();
            searchOptions.DataSource = this;
            searchOptions.QueryExpression = queryIndexedRuleInstance;

            var searchResult = mLogicalSearcher.Run(searchOptions);

#if DEBUG
            //LogInstance.Log($"searchResult = {searchResult}");
#endif

            return searchResult;
        }

        public virtual object GetPropertyValueAsObject(ulong entityId, string propertyName)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyName = {propertyName}");
#endif
            var propertyId = EntityDictionary.GetKey(propertyName);
            return GetPropertyValueAsObject(entityId, propertyId);
        }

        public virtual void SetPropertyValueAsAsVariant(ulong entityId, ulong propertyId, BaseVariant value)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            NSetPropertyValue(entityId, propertyId, value);
        }

        public virtual void SetPropertyValueAsAsVariant(ulong entityId, string propertyName, BaseVariant value)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyName = {propertyName} value = {value}");
#endif
            var propertyId = EntityDictionary.GetKey(propertyName);

            SetPropertyValueAsAsVariant(entityId, propertyId, value);
        }

        public virtual void SetPropertyValueAsAsObject(ulong entityId, ulong propertyId, object value)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif

            var variant = VariantsConvertor.ConvertObjectToVariant(value, EntityDictionary);

#if DEBUG
            //LogInstance.Log($"variant = {variant}");
#endif

            NSetPropertyValue(entityId, propertyId, variant);
        }

        public virtual void SetPropertyValueAsAsObject(ulong entityId, string propertyName, object value)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyName = {propertyName} value = {value}");
#endif
            var propertyId = EntityDictionary.GetKey(propertyName);
            SetPropertyValueAsAsObject(entityId, propertyId, value);
        }

        private void NSetPropertyValue(ulong entityId, ulong propertyId, BaseVariant value)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyId = {propertyId} value = {value}");
#endif
            var ruleInstance = PropertiesOfCGStorageHelper.CreateRuleInstanceForSetQuery(entityId, propertyId,  value, EntityDictionary);

            Append(ruleInstance);
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
