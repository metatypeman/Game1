﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class RelationExpressionNode : BaseExpressionNode, IRefToRecord
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Relation;
        public string Name { get; set; }
        public ulong Key { get; set; }
        public bool IsQuestion { get; set; }
        public IList<BaseExpressionNode> Params { get; set; }
        public VarExpressionNode LinkedVar { get; set; }
        public override bool IsRelation => true;
        public override RelationExpressionNode AsRelation => this; 
        
        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new RelationExpressionNode();
            result.Name = Name;
            result.Key = Key;
            if(Params != null)
            {
                var paramsList = new List<BaseExpressionNode>();
                foreach(var item in Params)
                {
                    paramsList.Add(item.Clone(context));
                }
                result.Params = paramsList;
            }
            result.Annotations = LogicalAnnotation.CloneListOfAnnotations(Annotations, context);
            return result;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(IsQuestion)} = {IsQuestion}");
            if (Params == null)
            {
                sb.AppendLine($"{spaces}{nameof(Params)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Params)}");
                foreach (var param in Params)
                {
                    sb.Append(param.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Params)}");
            }
            sb.Append(base.PropertiesToSting(n));
            if (LinkedVar == null)
            {
                sb.AppendLine($"{spaces}{nameof(LinkedVar)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(LinkedVar)}");
                sb.Append(LinkedVar.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(LinkedVar)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}{nameof(IsQuestion)} = {IsQuestion}");
            if (Params == null)
            {
                sb.AppendLine($"{spaces}{nameof(Params)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Params)}");
                foreach (var param in Params)
                {
                    sb.Append(param.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Params)}");
            }
            sb.Append(base.PropertiesToShortSting(n));
            if (LinkedVar == null)
            {
                sb.AppendLine($"{spaces}{nameof(LinkedVar)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(LinkedVar)}");
                sb.Append(LinkedVar.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(LinkedVar)}");
            }
            return sb.ToString();
        }
    }
}
