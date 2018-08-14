using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyNotNodeParser: LegacyBaseParser
    {
        public LegacyNotNodeParser(LegacyParserContext context)
            : base(context)
        {
            mResult = new UnaryOperatorOfQueryASTNode();
            mResult.OperatorId = KindOfUnaryOperators.Not;
        }

        private UnaryOperatorOfQueryASTNode mResult;

        public BaseQueryASTNode Result
        {
            get
            {
                return mResult;
            }
        }

        protected override void OnRun()
        {
#if DEBUG
            //LogInstance.Log($"CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}`");
#endif

            switch (CurrToken.TokenKind)
            {
                case LegacyTokenKind.OpenRoundBracket:
                    {
                        var parser = new LegacyOpenRoundBracketNodeParser(Context);
                        parser.Run();
                        mResult.Left = parser.Result;
                    }
                    break;

                case LegacyTokenKind.Word:
                    {
                        Context.Recovery(CurrToken);
                        var parser = new LegacyConditionNodeParser(Context, null);
                        parser.Run();
                        mResult.Left = parser.Result;
                    }
                    break;

                default: throw new LegacyUnexpectedTokenException(CurrToken);
            }
        }
    }
}
