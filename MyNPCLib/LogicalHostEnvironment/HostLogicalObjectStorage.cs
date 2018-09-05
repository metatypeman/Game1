﻿using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
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
        }

        public override KindOfCGStorage KindOfStorage => KindOfCGStorage.OtherProxy;
        private readonly object mLockObj = new object();
        public ulong EntityId { get; private set; }
        public DefaultHostCGStorage GeneralHost { get; private set; }
        public DefaultHostCGStorage VisibleHost { get; private set; }
        public DefaultHostCGStorage PublicHost { get; private set; }

        public override void Append(RuleInstance ruleInstance)
        {
            lock(mLockObj)
            {
#if DEBUG
                LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif

                var listOfKindOfAcessPolicy = ruleInstance.AccessPolicyToFactModality.Select(p => p.Kind).ToList();

#if DEBUG
                LogInstance.Log($"listOfKindOfAcessPolicy.Count = {listOfKindOfAcessPolicy.Count}");
                foreach(var kindOfAcessPolicy in listOfKindOfAcessPolicy)
                {
                    LogInstance.Log($"kindOfAcessPolicy = {kindOfAcessPolicy}");
                }
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
            lock (mLockObj)
            {
#if DEBUG
                LogInstance.Log($"storage = {storage}");
#endif
                var mainRuleInstance = storage.MainRuleInstance;

                var listOfKindOfAcessPolicy = mainRuleInstance.AccessPolicyToFactModality.Select(p => p.Kind).ToList();

#if DEBUG
                LogInstance.Log($"listOfKindOfAcessPolicy.Count = {listOfKindOfAcessPolicy.Count}");
                foreach (var kindOfAcessPolicy in listOfKindOfAcessPolicy)
                {
                    LogInstance.Log($"kindOfAcessPolicy = {kindOfAcessPolicy}");
                }
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
            lock (mLockObj)
            {
#if DEBUG
                LogInstance.Log($"ruleInstancePackage = {ruleInstancePackage}");
#endif

                var mainRuleInstance = ruleInstancePackage.MainRuleInstance;

                var listOfKindOfAcessPolicy = mainRuleInstance.AccessPolicyToFactModality.Select(p => p.Kind).ToList();

#if DEBUG
                LogInstance.Log($"listOfKindOfAcessPolicy.Count = {listOfKindOfAcessPolicy.Count}");
                foreach (var kindOfAcessPolicy in listOfKindOfAcessPolicy)
                {
                    LogInstance.Log($"kindOfAcessPolicy = {kindOfAcessPolicy}");
                }
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

        public override BaseVariant GetPropertyValueAsVariant(ulong entityId, ulong propertyId);
        public BaseVariant GetPropertyValueAsVariant(ulong propertyId);
        public override BaseVariant GetPropertyValueAsVariant(ulong entityId, string propertyName);
        public BaseVariant GetPropertyValueAsVariant(string propertyName);
        public override object GetPropertyValueAsObject(ulong entityId, ulong propertyId);
        public object GetPropertyValueAsObject(ulong propertyId);
        public override object GetPropertyValueAsObject(ulong entityId, string propertyName);
        public object GetPropertyValueAsObject(string propertyName);
        public override void SetPropertyValueAsAsVariant(ulong entityId, ulong propertyId, BaseVariant value);
        public void SetPropertyValueAsAsVariant(ulong propertyId, BaseVariant value);
        public override void SetPropertyValueAsAsVariant(ulong entityId, string propertyName, BaseVariant value);
        public void SetPropertyValueAsAsVariant(string propertyName, BaseVariant value);
        public override void SetPropertyValueAsAsObject(ulong entityId, ulong propertyId, object value);
        public void SetPropertyValueAsAsObject(ulong propertyId, object value);
        public override void SetPropertyValueAsAsObject(ulong entityId, string propertyName, object value);
        public void SetPropertyValueAsAsObject(string propertyName, object value);
    }
}
