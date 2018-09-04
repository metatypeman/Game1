using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class PassiveLogicalObject : IPassiveLogicalObject, IReadOnlyLogicalObject, ILogicalObject
    {
        public PassiveLogicalObject(IEntityLogger entityLogger, IEntityDictionary entityDictionary, IOldLogicalStorage logicalIndexingBus)
        {
            mEntityLogger = entityLogger;
            mEntityDictionary = entityDictionary;
            mLogicalIndexingBus = logicalIndexingBus;

            var name = Guid.NewGuid().ToString("D");
            mEntityId = mEntityDictionary.GetKey(name);
            mLogicalFrame = new LogicalFrame(entityLogger, mEntityId);
        }

        private IEntityLogger mEntityLogger;
        private IEntityDictionary mEntityDictionary;
        private IOldLogicalStorage mLogicalIndexingBus;
        private LogicalFrame mLogicalFrame;
        private ulong mEntityId;

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

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
                Error($"propertyKey = {propertyKey} = {e}");
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
                Error($"propertyName = {propertyName} = {e}");
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
