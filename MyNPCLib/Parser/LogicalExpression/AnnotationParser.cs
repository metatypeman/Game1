using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class AnnotationParser : BaseLogicalExpressionParser
    {
        public AnnotationParser(IParserContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if DEBUG
            LogInstance.Log($"CurrToken = {CurrToken}");
#endif
        }
    }
}
