﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class OperatorAndExpressionNode : BinaryOperatorExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.And;

        public override bool IsOperatorAnd => true;
        public override OperatorAndExpressionNode AsOperatorAnd => this;
    }
}