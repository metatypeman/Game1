using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    /// <summary type="e">
    /// The kind of an expression node.
    /// </summary>
    public enum KindOfExpressionNode
    {
        /// <summary>
        /// Represents a default value.
        /// </summary>
        Unknown,

        /// <summary>
        /// Represents `And` node.
        /// </summary>
        And,

        /// <summary>
        /// Represents `Or` node.
        /// </summary>
        Or,

        /// <summary>
        /// Represents `Not` node.
        /// </summary>
        Not,

        /// <summary>
        /// Represents a relation.
        /// </summary>
        Relation,

        /// <summary>
        /// Represents an common concept
        /// </summary>
        Concept,

        /// <summary>
        /// Represents an entity like #1.
        /// </summary>
        EntityRef,

        /// <summary>
        /// Represents a condition for linking to an entity (or many entities).
        /// </summary>
        EntityCondition,

        /// <summary>
        /// Represents a variable.
        /// </summary>
        Var,

        /// <summary>
        /// Represents variable for qerstion like '?X'.
        /// </summary>
        QuestionVar,

        /// <summary>
        /// Represents a value.
        /// </summary>
        Value,

        /// <summary>
        /// Represents a fuzzy logic value.
        /// </summary>
        FuzzyLogicValue,

        /// <summary>
        /// Represents an other fact or rule
        /// </summary>
        Fact,
        
        ParamStub
    }
}
