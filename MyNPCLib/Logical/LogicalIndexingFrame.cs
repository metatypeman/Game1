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
        private Dictionary<object, ulong> mValuesDict = new Dictionary<object, ulong>();

        public ulong this[object val]
        {
            get
            {
                lock (mValuesDictLockObj)
                {
                    if (mValuesDict.ContainsKey(val))
                    {
                        return mValuesDict[val];
                    }

                    return 0ul;
                }
            }

            set
            {
                lock (mValuesDictLockObj)
                {
                    mValuesDict[val] = value;
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
            Dictionary<object, ulong> valuesDict = null;
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
            }

            return sb.ToString();
        }
    }
}
