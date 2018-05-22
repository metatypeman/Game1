using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class PassiveLogicalObject : IPassiveLogicalObject, IReadOnlyLogicalObject, ILogicalObject
    {
        public PassiveLogicalObject(IEntityDictionary entityDictionary, ILogicalStorage logicalIndexingBus)
        {
            mEntityDictionary = entityDictionary;
            mLogicalIndexingBus = logicalIndexingBus;

            var name = Guid.NewGuid().ToString("D");
            mEntityId = mEntityDictionary.GetKey(name);
            mLogicalFrame = new LogicalFrame(mEntityId);
        }

        private IEntityDictionary mEntityDictionary;
        private ILogicalStorage mLogicalIndexingBus;
        private LogicalFrame mLogicalFrame;
        private ulong mEntityId;

        public ulong EntityId => mEntityId;
        public object this[ulong propertyKey]
        {
            get
            {
                return mLogicalFrame[propertyKey];
            }

            set
            {
                SetProperty(propertyKey, value);
            }
        }

        public object this[string propertyName]
        {
            get
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);
                return mLogicalFrame[propertyKey];
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);
                SetProperty(propertyKey, value);
            }
        }

        public T GetValue<T>(ulong propertyKey)
        {
            T result = default(T);

            try
            {
                result = (T)this[propertyKey];
            }
            catch (Exception e)
            {
#if DEBUG
                LogInstance.Log($"PassiveLogicalObject GetValue<T> propertyKey = {propertyKey} = {e}");
#endif
            }

            return result;
        }

        public T GetValue<T>(string propertyName)
        {
            T result = default(T);

            try
            {
                result = (T)this[propertyName];
            }
            catch (Exception e)
            {
#if DEBUG
                LogInstance.Log($"PassiveLogicalObject GetValue<T> propertyName = {propertyName} = {e}");
#endif
            }

            return result;
        }

        private void SetProperty(ulong propertyKey, object value)
        {
            var currentValue = mLogicalFrame[propertyKey];

            if (currentValue == value)
            {
                return;
            }

            mLogicalFrame[propertyKey] = value;
            mLogicalIndexingBus.PutPropertyValueAsIndex(mEntityId, propertyKey, value);
        }

        public void SetAccessPolicyToFact(ulong propertyKey, AccessPolicyToFact value)
        {
            NSetAccessPolicyToFact(propertyKey, value);
        }

        public void SetAccessPolicyToFact(string propertyName, AccessPolicyToFact value)
        {
            var propertyKey = mEntityDictionary.GetKey(propertyName);
            NSetAccessPolicyToFact(propertyKey, value);
        }

        private void NSetAccessPolicyToFact(ulong propertyKey, AccessPolicyToFact value)
        {
            var currentPopicy = mLogicalFrame.GetAccessPolicyToFact(propertyKey);

            if (currentPopicy == value)
            {
                return;
            }

            mLogicalFrame.SetAccessPolicyToFact(propertyKey, value);
            mLogicalIndexingBus.PutAccessPolicyToFactAsIndex(mEntityId, propertyKey, value);
        }

        public AccessPolicyToFact GetAccessPolicyToFact(ulong propertyKey)
        {
            return mLogicalFrame.GetAccessPolicyToFact(propertyKey);
        }

        public AccessPolicyToFact GetAccessPolicyToFact(string propertyName)
        {
            var propertyKey = mEntityDictionary.GetKey(propertyName);
            return mLogicalFrame.GetAccessPolicyToFact(propertyKey);
        }
    }
}
