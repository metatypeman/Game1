using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class FactParser : BaseLogicalExpressionParser
    {
        public FactParser(IParserContext context)
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
