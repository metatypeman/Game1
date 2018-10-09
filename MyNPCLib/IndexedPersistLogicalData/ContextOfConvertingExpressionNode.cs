using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public class ContextOfConvertingExpressionNode : IObjectToString, IShortObjectToString
    {
        public IList<ResolverForRelationExpressionNode> RelationsList = new List<ResolverForRelationExpressionNode>();
        public IList<VarExpressionNode> VarsList = new List<VarExpressionNode>();
        public IList<QuestionVarExpressionNode> QuestionVarsList = new List<QuestionVarExpressionNode>();

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
            var sb = new StringBuilder();
            if (RelationsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(RelationsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RelationsList)}");
                foreach (var relation in RelationsList)
                {
                    sb.Append(relation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(RelationsList)}");
            }
            if (VarsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(VarsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VarsList)}");
                foreach (var varItem in VarsList)
                {
                    sb.Append(varItem.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VarsList)}");
            }
            if (QuestionVarsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(QuestionVarsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(QuestionVarsList)}");
                foreach (var questionVar in QuestionVarsList)
                {
                    sb.Append(questionVar.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(QuestionVarsList)}");
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

        public string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (RelationsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(RelationsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(RelationsList)}");
                foreach (var relation in RelationsList)
                {
                    sb.Append(relation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(RelationsList)}");
            }
            if (VarsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(VarsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VarsList)}");
                foreach (var varItem in VarsList)
                {
                    sb.Append(varItem.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VarsList)}");
            }
            if (QuestionVarsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(QuestionVarsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(QuestionVarsList)}");
                foreach (var questionVar in QuestionVarsList)
                {
                    sb.Append(questionVar.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(QuestionVarsList)}");
            }
            return sb.ToString();
        }
    }
}
