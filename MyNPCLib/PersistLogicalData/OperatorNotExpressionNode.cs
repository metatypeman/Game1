﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class OperatorNotExpressionNode: UnaryOperatorExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Not;

        public override bool IsOperatorNot => true;
        public override OperatorNotExpressionNode AsOperatorNot => this;
    }
}