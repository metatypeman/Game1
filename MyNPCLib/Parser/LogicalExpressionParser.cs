using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class LogicalExpressionParser: BaseParser
    {
        public LogicalExpressionParser(ParserContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if DEBUG
            LogInstance.Log($"LogicalExpressionParser OnRun CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}`");
#endif

            switch (CurrToken.TokenKind)
            {
                default: throw new UnexpectedTokenException(CurrToken);
            }
        }
    }
}
