using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class ASTNodeOfLogicalQuery: IObjectToString, IShortObjectToString, IObjectToBriefString
    {
        public ASTNodeOfLogicalQuery Parent { get; set; }
        public KindOfASTNodeOfLogicalQuery Kind { get; set; } = KindOfASTNodeOfLogicalQuery.Unknown;
        public KindOfOperatorOfASTNodeOfLogicalQuery KindOfOperator { get; set; } = KindOfOperatorOfASTNodeOfLogicalQuery.Unknown;
        public string Name { get; set; }
        public ASTNodeOfLogicalQuery Part_1 { get; set; }
        public ASTNodeOfLogicalQuery Part_2 { get; set; }
        public bool IsActivePart { get; set; }
        public ASTNodeOfLogicalQuery Expression { get; set; }
        public bool IsQuestion { get; set; }
        public List<ASTNodeOfLogicalQuery> ParamsList { get; set; }
        public ASTNodeOfLogicalQuery Left { get; set; }
        public ASTNodeOfLogicalQuery Right { get; set; }
        public bool IsGroup { get; set; }
        public List<ASTNodeOfLogicalQuery> AnnotationsList { get; set; }

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
            if (Parent == null)
            {
                sb.AppendLine($"{spaces}{nameof(Parent)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Parent)}");
                sb.Append(Parent.ToBriefString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Parent)}");
            }
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(KindOfOperator)} = {KindOfOperator}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            if (Part_1 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_1)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_1)}");
                sb.Append(Part_1.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_1)}");
            }
            if (Part_2 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_2)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_2)}");
                sb.Append(Part_2.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_2)}");
            }
            sb.AppendLine($"{spaces}{nameof(IsActivePart)} = {IsActivePart}");
            if (Expression == null)
            {
                sb.AppendLine($"{spaces}{nameof(Expression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Expression)}");
                sb.Append(Expression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Expression)}");
            }
            sb.AppendLine($"{spaces}{nameof(IsQuestion)} = {IsQuestion}");
            
            if (ParamsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ParamsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ParamsList)}");
                foreach (var item in ParamsList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ParamsList)}");
            }

            if (Left == null)
            {
                sb.AppendLine($"{spaces}{nameof(Left)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Left)}");
                sb.Append(Left.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Left)}");
            }

            if (Right == null)
            {
                sb.AppendLine($"{spaces}{nameof(Right)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Right)}");
                sb.Append(Right.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Right)}");
            }
            sb.AppendLine($"{spaces}{nameof(IsGroup)} = {IsGroup}");
            if (AnnotationsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(AnnotationsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AnnotationsList)}");
                foreach (var annotation in AnnotationsList)
                {
                    sb.Append(annotation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(AnnotationsList)}");
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
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(KindOfOperator)} = {KindOfOperator}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            if (Part_1 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_1)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_1)}");
                sb.Append(Part_1.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_1)}");
            }
            if (Part_2 == null)
            {
                sb.AppendLine($"{spaces}{nameof(Part_2)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Part_2)}");
                sb.Append(Part_2.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Part_2)}");
            }
            sb.AppendLine($"{spaces}{nameof(IsActivePart)} = {IsActivePart}");
            if (Expression == null)
            {
                sb.AppendLine($"{spaces}{nameof(Expression)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Expression)}");
                sb.Append(Expression.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Expression)}");
            }
            sb.AppendLine($"{spaces}{nameof(IsQuestion)} = {IsQuestion}");
            if (ParamsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(ParamsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(ParamsList)}");
                foreach (var item in ParamsList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(ParamsList)}");
            }

            if (Left == null)
            {
                sb.AppendLine($"{spaces}{nameof(Left)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Left)}");
                sb.Append(Left.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Left)}");
            }

            if (Right == null)
            {
                sb.AppendLine($"{spaces}{nameof(Right)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Right)}");
                sb.Append(Right.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Right)}");
            }

            sb.AppendLine($"{spaces}{nameof(IsGroup)} = {IsGroup}");

            if (AnnotationsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(AnnotationsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AnnotationsList)}");
                foreach (var annotation in AnnotationsList)
                {
                    sb.Append(annotation.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(AnnotationsList)}");
            }
            return sb.ToString();
        }

        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        public string PropertiesToBriefSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(KindOfOperator)} = {KindOfOperator}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(IsActivePart)} = {IsActivePart}");
            sb.AppendLine($"{spaces}{nameof(IsQuestion)} = {IsQuestion}");
            sb.AppendLine($"{spaces}{nameof(IsGroup)} = {IsGroup}");
            return sb.ToString();
        }
    }
}
