using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public enum KindOfASTNodeOfLogicalQuery
    {
        Unknown,
        Fact,
        RulePart,
        Relation,
        Concept,
        EntityRef,
        EntityCondition,
        Var,
        QuestionParam,
        BinaryOperator,
        UnaryOperator,
        Condition,
        StubParam,
        Value,
        LogicalValue,
        BindedParam,
        AccessPolicy
    }
}
