using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyLogicalExpressionParser: LegacyBaseParser
    {
        public LegacyLogicalExpressionParser(LegacyParserContext context)
            : base(context)
        {
        }

        protected override void OnRun()
        {
#if DEBUG
            LogInstance.Log($"CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}`");
#endif

            switch (CurrToken.TokenKind)
            {
                default: throw new LegacyUnexpectedTokenException(CurrToken);
            }
        }
    }
}
