using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalFrame: IReadOnlyLogicalObject, IObjectToString
    {
        public LogicalFrame(IEntityLogger entityLogger, ulong entityId)
        {
            mEntityLogger = entityLogger;
            mEntityId = entityId;
        }

        private IEntityLogger mEntityLogger;
        private ulong mEntityId;

        public ulong EntityId => mEntityId;

        private readonly object mValuesDictLockObj = new object();
        private Dictionary<ulong, object> mValuesDict = new Dictionary<ulong, object>();
        private Dictionary<ulong, AccessPolicyToFact> mPropertiesAccessPolicyToFactDict = new Dictionary<ulong, AccessPolicyToFact>();

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

        public object this [ulong propertyKey]
        {
            get
            {
                lock(mValuesDictLockObj)
                {
                    if(mValuesDict.ContainsKey(propertyKey))
                    {
                        return mValuesDict[propertyKey];
                    }

                    return null;
                }
            }

            set
            {
                lock (mValuesDictLockObj)
                {
                    mValuesDict[propertyKey] = value;
                }
            }
        }

        public void SetAccessPolicyToFact(ulong propertyKey, AccessPolicyToFact value)
        {
            lock (mValuesDictLockObj)
            {
                mPropertiesAccessPolicyToFactDict[propertyKey] = value;
            }
        }

        public AccessPolicyToFact GetAccessPolicyToFact(ulong propertyKey)
        {
            lock (mValuesDictLockObj)
            {
                if(mPropertiesAccessPolicyToFactDict.ContainsKey(propertyKey))
                {
                    return mPropertiesAccessPolicyToFactDict[propertyKey];
                }

                return AccessPolicyToFact.Public;
            }
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(EntityId)} = {EntityId}");
            Dictionary<ulong, object> valuesDict = null;
            Dictionary<ulong, AccessPolicyToFact> propertiesAccessPolicyToFactDict = null;

            lock (mValuesDictLockObj)
            {
                valuesDict = mValuesDict;
                propertiesAccessPolicyToFactDict = mPropertiesAccessPolicyToFactDict;
            }

            if(valuesDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(valuesDict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(valuesDict)}");
                foreach (var valueItem in valuesDict)
                {
                    sb.AppendLine($"{nextSpaces}valueItemKey = {valueItem.Key}; valueItemValue = {valueItem.Value}");
                }
                sb.AppendLine($"{spaces}End{nameof(valuesDict)}");
            }

            if (propertiesAccessPolicyToFactDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(propertiesAccessPolicyToFactDict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(propertiesAccessPolicyToFactDict)}");
                foreach (var valueItem in valuesDict)
                {
                    sb.AppendLine($"{nextSpaces}valueItemKey = {valueItem.Key}; valueItemValue = {valueItem.Value}");
                }
                sb.AppendLine($"{spaces}End{nameof(propertiesAccessPolicyToFactDict)}");
            }
            return sb.ToString();
        }
    }
}
