using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class RulePartParser : BaseLogicalExpressionParser
    {
        public RulePartParser(IParserContext context)
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
