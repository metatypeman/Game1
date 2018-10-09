using MyNPCLib.CGStorage;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public abstract class ResolverForBaseExpressionNode: IObjectToString, IShortObjectToString
    {
        public abstract KindOfExpressionNode Kind { get; }
        public abstract BaseExpressionNode Origin { get; }
        public IList<IndexedLogicalAnnotation> Annotations { get; set; }
        public IndexedRuleInstance RuleInstance { get; set; }
        public IndexedRulePart RulePart { get; set; }

        public abstract void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options);
        public abstract void FillExecutingCardForAnnotation(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options);

        public string GetHumanizeDbgString()
        {
            if(Origin == null)
            {
                return string.Empty;
            }

            return DebugHelperForRuleInstance.ToString(Origin);
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public virtual string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            if (Origin == null)
            {
                sb.AppendLine($"{spaces}{nameof(Origin)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Origin)}");
                sb.Append(Origin.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Origin)}");
            }
            return sb.ToString();
        }
    }
}
