using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalIndexingFrame : IObjectToString
    {
        public LogicalIndexingFrame(ulong propertyId)
        {
            mPropertyId = propertyId;
        }

        private ulong mPropertyId;
        public ulong PropertyId => mPropertyId;

        private readonly object mValuesDictLockObj = new object();
        private Dictionary<object, List<ulong>> mValuesDict = new Dictionary<object, List<ulong>>();
        private Dictionary<ulong, object> mEntitiesDict = new Dictionary<ulong, object>();

        public IList<ulong> Get(object key)
        {
            lock (mValuesDictLockObj)
            {
                if (mValuesDict.ContainsKey(key))
                {
                    return mValuesDict[key];
                }

                return new List<ulong>();
            }
        }

        public void Set(object key, ulong entityId)
        {
            lock (mValuesDictLockObj)
            {
                object oldValue = null;

                if(mEntitiesDict.ContainsKey(entityId))
                {
                    oldValue = mEntitiesDict[entityId];
                }

                mEntitiesDict[entityId] = key;

                if(oldValue != null)
                {
                    if (mValuesDict.ContainsKey(oldValue))
                    {
                        mValuesDict[oldValue].Remove(entityId);
                    }
                }

                if(mValuesDict.ContainsKey(key))
                {
                    var entitiesIdsList = mValuesDict[key];

                    if(!entitiesIdsList.Contains(entityId))
                    {
                        entitiesIdsList.Add(entityId);
                    }
                }
                else
                {
                    var entitiesIdsList = new List<ulong>();
                    mValuesDict[key] = entitiesIdsList;
                    entitiesIdsList.Add(entityId);
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
            sb.AppendLine($"{spaces}{nameof(PropertyId)} = {PropertyId}");
            throw new NotImplementedException();
            /*Dictionary<object, ulong> valuesDict = null;
            lock (mValuesDictLockObj)
            {
                valuesDict = mValuesDict;
            }
            if (valuesDict == null)
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
            }*/

            return sb.ToString();
        }
    }
}
