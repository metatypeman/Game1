using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class QuestionVarExpressionNode : BaseRefExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.QuestionVar;

        public override bool IsQuestionVar => true;
        public override QuestionVarExpressionNode AsQuestionVar => this;
    }
}
