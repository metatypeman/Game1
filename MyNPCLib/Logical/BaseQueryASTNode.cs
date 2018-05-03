using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class BaseQueryASTNode: IObjectToString
    {
        public abstract QueryASTNodeKind Kind { get; }
        public virtual bool IsCondition => false;
        public virtual ConditionOfQueryASTNode AsCondition => null;
        public virtual bool IsBinaryOperator => false;
        public virtual BinaryOperatorOfQueryASTNode AsBinaryOperator => null;
        public virtual bool IsUnaryOperator => false;
        public virtual UnaryOperatorOfQueryASTNode AsUnaryOperator => null;

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public abstract string PropertiesToSting(uint n);
    }
}
