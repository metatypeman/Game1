using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
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

                throw new NotImplementedException();
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

                throw new NotImplementedException();
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

                throw new NotImplementedException();
            }
        }
    }
}
