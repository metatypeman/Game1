﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class OperatorOrExpressionNode: BinaryOperatorExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Or;
        public override bool IsOperatorOr => true;
        public override OperatorOrExpressionNode AsOperatorOr => this;
    }
}