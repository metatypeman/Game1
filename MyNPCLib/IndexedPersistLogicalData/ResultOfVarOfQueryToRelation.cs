using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using MyNPCLib.VariantsConverting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class ResultOfVarOfQueryToRelation : IObjectToString
    {
        public LogicalSearchResultItem Parent { get; set; }
        public ICGStorage Storage { get; set; }
        public ulong KeyOfVar { get; set; }
        public BaseExpressionNode FoundExpression { get; set; }
        public IDictionary<ulong, OriginOfVarOfQueryToRelation> OriginDict { get; set; } = new Dictionary<ulong, OriginOfVarOfQueryToRelation>();
        private readonly object mVariantLockObj = new object();
        private BaseVariant mVariant;

        public RuleInstance GetEntityConditionRuleInstance()
        {
            var foundExpressionOfValueOfDirectionAsEntityCondition = FoundExpression.AsEntityCondition;

            if(foundExpressionOfValueOfDirectionAsEntityCondition == null)
            {
                return null;
            }

            var keyOfEntityConditionFact = foundExpressionOfValueOfDirectionAsEntityCondition.Key;

#if DEBUG
            LogInstance.Log($"keyOfEntityConditionFact = {keyOfEntityConditionFact}");
#endif
            var entityConditionRuleInstance = Storage.GetRuleInstanceByKey(keyOfEntityConditionFact);

#if DEBUG
            LogInstance.Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");
#endif

            return entityConditionRuleInstance;
        }

        public BaseVariant AsVariant
        {
            get
            {
                lock(mVariantLockObj)
                {
                    return NGetVariant();
                }
            }
        }

        private BaseVariant NGetVariant()
        {
            if (mVariant == null)
            {
                mVariant = VariantsConvertor.ConvertResultOfVarToVariant(this);
            }
            return mVariant;
        }

        public object AsObject
        {
            get
            {
                lock (mVariantLockObj)
                {
                    var variant = NGetVariant();
                    return variant?.AsValue?.Value;
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
            var nextN = n + 4;
            var nextNSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(KeyOfVar)} = {KeyOfVar}");
            if (FoundExpression == null)
            {
                sb.AppendLine($"{spaces}{nameof(FoundExpression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(FoundExpression)}");
                sb.Append(FoundExpression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(FoundExpression)}");
            }
            if(OriginDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(OriginDict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(OriginDict)}");
                var keysOfRuleInstancesList = OriginDict.Keys.ToList();
                foreach (var keyOfRuleInstance in keysOfRuleInstancesList)
                {
                    sb.AppendLine($"{nextNSpaces}{nameof(keyOfRuleInstance)} = {keyOfRuleInstance}");
                }
                sb.AppendLine($"{spaces}End {nameof(OriginDict)}");
            }
            return sb.ToString();
        }
    }
}
