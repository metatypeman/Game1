using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    public class ParamStubExpressionNode: BaseExpressionNode
    {
        public override KindOfExpressionNode Kind => KindOfExpressionNode.ParamStub;
        public override bool IsParamStub => true;
        public override ParamStubExpressionNode AsParamStub => this;

        public override BaseExpressionNode Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new ParamStubExpressionNode();
            result.Annotations = LogicalAnnotation.CloneListOfAnnotations(Annotations, context);
            return result;
        }
    }
}
