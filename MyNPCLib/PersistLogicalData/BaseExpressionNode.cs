using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public abstract class BaseExpressionNode : ILogicalyAnnotated, IObjectToString, IShortObjectToString
    {
        public abstract KindOfExpressionNode Kind { get; }
        public IList<LogicalAnnotation> Annotations { get; set; }
        public virtual bool IsUnaryOperator => false;
        public virtual UnaryOperatorExpressionNode UnaryOperator => null;
        public virtual bool IsOperatorNot => false;
        public virtual OperatorNotExpressionNode AsOperatorNot => null;
        public virtual bool IsBinaryOperator => false;
        public virtual BinaryOperatorExpressionNode AsBinaryOperator => null;
        public virtual bool IsOperatorAnd => false;
        public virtual OperatorAndExpressionNode AsOperatorAnd => null;
        public virtual bool IsOperatorOr => false;
        public virtual OperatorOrExpressionNode AsOperatorOr => null;
        public virtual bool IsBaseRef => false;
        public virtual BaseRefExpressionNode AsBaseRef => null;
        public virtual bool IsConcept => false;
        public virtual ConceptExpressionNode AsConcept => null;
        public virtual bool IsEntityRef => false;
        public virtual EntityRefExpressionNode AsEntityRef => null;
        public virtual bool IsEntityCondition => false;
        public virtual EntityConditionExpressionNode AsEntityCondition => null;
        public virtual bool IsVar => false;
        public virtual VarExpressionNode AsVar => null;
        public virtual bool IsQuestionVar => false;
        public virtual QuestionVarExpressionNode AsQuestionVar => null;
        public virtual bool IsFact => false;
        public virtual FactExpressionNode AsFact => null;
        public virtual bool IsRelation => false;
        public virtual RelationExpressionNode AsRelation => null;
        public virtual bool IsValue => false;
        public virtual ValueExpressionNode AsValue => null;
        public virtual bool IsFuzzyLogicValue => false;
        public virtual FuzzyLogicValueExpressionNode AsFuzzyLogicValue => null;
        public virtual bool IsParamStub => false;
        public virtual ParamStubExpressionNode AsParamStub => null;
        public abstract BaseExpressionNode Clone(CloneContextOfPersistLogicalData context);

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
            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
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
            if (Annotations == null)
            {
                sb.AppendLine($"{spaces}{nameof(Annotations)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Annotations)}");
                foreach (var annotation in Annotations)
                {
                    sb.Append(annotation.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(Annotations)}");
            }
            return sb.ToString();
        }
    }
}
