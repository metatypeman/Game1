using MyNPCLib.CGStorage;
using MyNPCLib.CGStorage.Helpers;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using MyNPCLib.VariantsConverting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.LogicalHostEnvironment
{
    public class HostLogicalObjectStorage: BaseProxyStorage, IHostLogicalObjectStorageForBus
    {
        public HostLogicalObjectStorage(IEntityDictionary entityDictionary)
            : base(entityDictionary)
        {
            var name = NamesHelper.CreateEntityName();
            EntityId = entityDictionary.GetKey(name);

            GeneralHost = new DefaultHostCGStorage(entityDictionary);
            VisibleHost = new DefaultHostCGStorage(entityDictionary);
            PublicHost = new DefaultHostCGStorage(entityDictionary);

            mLogicalSearcher = new LogicalSearcher(EntityDictionary);
            mKeyOfVarForProperty = entityDictionary.GetKey(mNameOfVarForProperty);
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.OtherProxy;
        private readonly object mHostsLockObj = new object();
        public ulong EntityId { get; private set; }
        public DefaultHostCGStorage GeneralHost { get; private set; }
        public DefaultHostCGStorage VisibleHost { get; private set; }
        public DefaultHostCGStorage PublicHost { get; private set; }

        private LogicalSearcher mLogicalSearcher;
        private string mNameOfVarForProperty = "?X";
        private ulong mKeyOfVarForProperty;

        private readonly object mAccessPolicyToFactLockObj = new object();
        private Dictionary<ulong, KindOfAccessPolicyToFact> mPropertiesAccessPolicyToFactDict = new Dictionary<ulong, KindOfAccessPolicyToFact>();

        public override void Append(RuleInstance ruleInstance)
        {
            lock(mHostsLockObj)
            {
#if DEBUG
                //LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif

                var listOfKindOfAcessPolicy = ruleInstance.AccessPolicyToFactModality.Select(p => p.Kind).ToList();

#if DEBUG
                //LogInstance.Log($"listOfKindOfAcessPolicy.Count = {listOfKindOfAcessPolicy.Count}");
                //foreach(var kindOfAcessPolicy in listOfKindOfAcessPolicy)
                //{
                //    LogInstance.Log($"kindOfAcessPolicy = {kindOfAcessPolicy}");
                //}
#endif

                if (listOfKindOfAcessPolicy.Contains(KindOfAccessPolicyToFact.Public))
                {
                    PublicHost.Append(ruleInstance);
                }

                if (listOfKindOfAcessPolicy.Contains(KindOfAccessPolicyToFact.ForVisible))
                {
                    VisibleHost.Append(ruleInstance);
                }

                GeneralHost.Append(ruleInstance);
            }
        }

        public override void Append(ICGStorage storage)
        {
            lock (mHostsLockObj)
            {
#if DEBUG
                //LogInstance.Log($"storage = {storage}");
#endif
                var mainRuleInstance = storage.MainRuleInstance;

                var listOfKindOfAcessPolicy = mainRuleInstance.AccessPolicyToFactModality.Select(p => p.Kind).ToList();

#if DEBUG
                //LogInstance.Log($"listOfKindOfAcessPolicy.Count = {listOfKindOfAcessPolicy.Count}");
                //foreach (var kindOfAcessPolicy in listOfKindOfAcessPolicy)
                //{
                //    LogInstance.Log($"kindOfAcessPolicy = {kindOfAcessPolicy}");
                //}
#endif

                if(listOfKindOfAcessPolicy.Contains(KindOfAccessPolicyToFact.Public))
                {
                    PublicHost.Append(storage);
                }

                if(listOfKindOfAcessPolicy.Contains(KindOfAccessPolicyToFact.ForVisible))
                {
                    VisibleHost.Append(storage);
                }

                GeneralHost.Append(storage);
            }
        }

        public override void Append(RuleInstancePackage ruleInstancePackage)
        {
            lock (mHostsLockObj)
            {
#if DEBUG
                //LogInstance.Log($"ruleInstancePackage = {ruleInstancePackage}");
#endif

                var mainRuleInstance = ruleInstancePackage.MainRuleInstance;

                var listOfKindOfAcessPolicy = mainRuleInstance.AccessPolicyToFactModality.Select(p => p.Kind).ToList();

#if DEBUG
                //LogInstance.Log($"listOfKindOfAcessPolicy.Count = {listOfKindOfAcessPolicy.Count}");
                //foreach (var kindOfAcessPolicy in listOfKindOfAcessPolicy)
                //{
                //    LogInstance.Log($"kindOfAcessPolicy = {kindOfAcessPolicy}");
                //}
#endif

                if (listOfKindOfAcessPolicy.Contains(KindOfAccessPolicyToFact.Public))
                {
                    PublicHost.Append(ruleInstancePackage);
                }

                if (listOfKindOfAcessPolicy.Contains(KindOfAccessPolicyToFact.ForVisible))
                {
                    VisibleHost.Append(ruleInstancePackage);
                }

                GeneralHost.Append(ruleInstancePackage);
            }
        }

        public override BaseVariant GetPropertyValueAsVariant(ulong entityId, ulong propertyId)
        {
            entityId = EntityId;
            var currentPolicy = GetAccessPolicyToFact(propertyId);

            var searchResult = CreateLogicalSearchResultForGetProperty(entityId, propertyId, currentPolicy);
            return searchResult.GetResultOfVarAsVariant(mKeyOfVarForProperty);
        }

        public BaseVariant GetPropertyValueAsVariant(ulong propertyId)
        {
            return GetPropertyValueAsVariant(EntityId, propertyId);
        }

        public BaseVariant GetPropertyValueAsVariant(string propertyName)
        {
            var propertyId = EntityDictionary.GetKey(propertyName);
            return GetPropertyValueAsVariant(EntityId, propertyId);
        }

        public override object GetPropertyValueAsObject(ulong entityId, ulong propertyId)
        {
            entityId = EntityId;
            var currentPolicy = GetAccessPolicyToFact(propertyId);

            var searchResult = CreateLogicalSearchResultForGetProperty(entityId, propertyId, currentPolicy);
            return searchResult.GetResultOfVarAsObject(mKeyOfVarForProperty);
        }

        private LogicalSearchResult CreateLogicalSearchResultForGetProperty(ulong entityId, ulong propertyId, KindOfAccessPolicyToFact currentPolicy)
        {
#if DEBUG
            //LogInstance.Log($"entityId = {entityId} propertyId = {propertyId}");
#endif

            var queryIndexedRuleInstance = PropertiesOfCGStorageHelper.CreateGetQuery(entityId, propertyId, EntityDictionary, mNameOfVarForProperty, mKeyOfVarForProperty, new List<KindOfAccessPolicyToFact>() { currentPolicy });

#if DEBUG
            //LogInstance.Log($"queryIndexedRuleInstance = {queryIndexedRuleInstance}");
#endif

            var searchOptions = new LogicalSearchOptions();
            searchOptions.DataSource = GeneralHost;
            searchOptions.QueryExpression = queryIndexedRuleInstance;
            searchOptions.IgnoreAccessPolicy = false;

            var searchResult = mLogicalSearcher.Run(searchOptions);

#if DEBUG
            //LogInstance.Log($"searchResult = {searchResult}");
#endif

            return searchResult;
        }

        public object GetPropertyValueAsObject(ulong propertyId)
        {
            return GetPropertyValueAsObject(EntityId, propertyId);
        }

        public object GetPropertyValueAsObject(string propertyName)
        {
            var propertyId = EntityDictionary.GetKey(propertyName);
            return GetPropertyValueAsObject(EntityId, propertyId);
        }

        public override void SetPropertyValueAsAsVariant(ulong entityId, ulong propertyId, BaseVariant value)
        {
            entityId = EntityId;
            var currentPolicy = GetAccessPolicyToFact(propertyId);

            NSetPropertyValue(entityId, propertyId, value, currentPolicy);
        }

        public void SetPropertyValueAsAsVariant(ulong propertyId, BaseVariant value)
        {
            SetPropertyValueAsAsVariant(EntityId, propertyId, value);
        }

        public void SetPropertyValueAsAsVariant(string propertyName, BaseVariant value)
        {
            var propertyId = EntityDictionary.GetKey(propertyName);
            SetPropertyValueAsAsVariant(EntityId, propertyId, value);
        }

        public override void SetPropertyValueAsAsObject(ulong entityId, ulong propertyId, object value)
        {
            entityId = EntityId;
            var currentPolicy = GetAccessPolicyToFact(propertyId);

            var variant = VariantsConvertor.ConvertObjectToVariant(value, EntityDictionary);

#if DEBUG
            //LogInstance.Log($"variant = {variant}");
#endif

            NSetPropertyValue(entityId, propertyId, variant, currentPolicy);
        }

        public void SetPropertyValueAsAsObject(ulong propertyId, object value)
        {
            SetPropertyValueAsAsObject(EntityId, propertyId, value);
        }

        public void SetPropertyValueAsAsObject(string propertyName, object value)
        {
            var propertyId = EntityDictionary.GetKey(propertyName);
            SetPropertyValueAsAsObject(EntityId, propertyId, value);
        }

        private void NSetPropertyValue(ulong entityId, ulong propertyId, BaseVariant value, KindOfAccessPolicyToFact currentPolicy)
        {
            var ruleInstance = PropertiesOfCGStorageHelper.CreateRuleInstanceForSetQuery(entityId, propertyId, value, EntityDictionary, new List<KindOfAccessPolicyToFact>() { currentPolicy });

#if DEBUG
            //LogInstance.Log($"ruleInstance = {ruleInstance}");
            //var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance.MainRuleInstance);

            //LogInstance.Log($"debugStr = {debugStr}");
#endif

            Append(ruleInstance);
        }

        public object this[ulong propertyKey]
        {
            get
            {
                return GetPropertyValueAsObject(propertyKey);
            }

            set
            {
                SetPropertyValueAsAsObject(propertyKey, value);
            }
        }

        public object this[string propertyName]
        {
            get
            {
                return GetPropertyValueAsObject(propertyName);
            }

            set
            {
#if DEBUG
                //LogInstance.Log($"propertyName = {propertyName}");
#endif

                SetPropertyValueAsAsObject(propertyName, value);
            }
        }

        public void SetAccessPolicyToFact(ulong propertyKey, KindOfAccessPolicyToFact value)
        {
            lock(mAccessPolicyToFactLockObj)
            {
                mPropertiesAccessPolicyToFactDict[propertyKey] = value;
            }
        }

        public void SetAccessPolicyToFact(string propertyName, KindOfAccessPolicyToFact value)
        {
            var propertyId = EntityDictionary.GetKey(propertyName);
            SetAccessPolicyToFact(propertyId, value);
        }

        public KindOfAccessPolicyToFact GetAccessPolicyToFact(ulong propertyKey)
        {
            lock (mAccessPolicyToFactLockObj)
            {
                if (mPropertiesAccessPolicyToFactDict.ContainsKey(propertyKey))
                {
                    return mPropertiesAccessPolicyToFactDict[propertyKey];
                }

                return KindOfAccessPolicyToFact.Public;
            }
        }

        public KindOfAccessPolicyToFact GetAccessPolicyToFact(string propertyName)
        {
            var propertyId = EntityDictionary.GetKey(propertyName);
            return GetAccessPolicyToFact(propertyId);
        }
    }
}
