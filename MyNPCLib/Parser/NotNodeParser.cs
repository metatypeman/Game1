using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class NotNodeParser: BaseParser
    {
        public NotNodeParser(ParserContext context)
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
            LogInstance.Log($"NotNodeParser OnRun CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}`");
#endif

            switch (CurrToken.TokenKind)
            {
                case TokenKind.OpenRoundBracket:
                    {
                        var parser = new OpenRoundBracketNodeParser(Context);
                        parser.Run();
                        mResult.Left = parser.Result;
                    }
                    break;

                case TokenKind.Word:
                    {
                        Context.Recovery(CurrToken);
                        var parser = new ConditionNodeParser(Context, null);
                        parser.Run();
                        mResult.Left = parser.Result;
                    }
                    break;

                default: throw new UnexpectedTokenException(CurrToken);
            }
        }
    }
}
