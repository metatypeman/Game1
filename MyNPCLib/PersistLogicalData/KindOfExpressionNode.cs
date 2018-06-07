﻿using System;
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
        /// Represents an entity.
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
        /// Represents a value.
        /// </summary>
        Value
    }
}
