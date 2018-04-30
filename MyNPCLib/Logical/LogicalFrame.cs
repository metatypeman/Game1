using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalFrame: IObjectToString
    {
        public LogicalFrame(ulong entityId)
        {
            mEntityId = entityId;
        }

        private ulong mEntityId;

        public ulong EntityId
        {
            get
            {
                return mEntityId;
            }
        }

        private readonly object mValuesDictLockObj = new object();
        private Dictionary<ulong, object> mValuesDict = new Dictionary<ulong, object>();

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
                    mValuesDict[propertyKey] = propertyKey;
                }
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
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(EntityId)} = {EntityId}");
            Dictionary<ulong, object> valuesDict = null;
            lock(mValuesDictLockObj)
            {
                valuesDict = mValuesDict;
            }
            if(valuesDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(valuesDict)} = null");
            }
            else
            {
                var nextN = n + 4;
                var nextSpaces = StringHelper.Spaces(nextN);
                sb.AppendLine($"{spaces}Begin{nameof(valuesDict)}");
                foreach (var valueItem in valuesDict)
                {
                    sb.AppendLine($"{nextSpaces}valueItemKey = {valueItem.Key}; valueItemValue = {valueItem.Value}");
                }
                sb.AppendLine($"{spaces}End{nameof(valuesDict)}");
            }

            return sb.ToString();
        }
    }
}
