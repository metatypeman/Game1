using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public enum SecondaryKindOfASTNodeOfLogicalQuery
    {
        Unknown,
        StandardExpression,
        EntityCondition,
        SimpleConcept
    }
}
