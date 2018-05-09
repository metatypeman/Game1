using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class OpenRoundBracketNodeParser : BaseParser
    {
        public enum State
        {
            Init,
            AfterCloseRoundBracket
        }

        public OpenRoundBracketNodeParser(ParserContext context)
            : base(context)
        {
        }

        private BaseQueryASTNode mResult;

        public BaseQueryASTNode Result
        {
            get
            {
                return mResult;
            }
        }

        private State mState = State.Init;

        protected override void OnRun()
        {
#if DEBUG
            //LogInstance.Log($"OpenRoundBracketNodeParser OnRun mState = {mState} CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}`");
#endif

            switch(mState)
            {
                case State.Init:
                    switch (CurrToken.TokenKind)
                    {
                        case TokenKind.Word:
                            Context.Recovery(CurrToken);
                            mResult = LogicalExpressionParserHelper.CreateNode(Context, TokenKind.CloseRoundBracket);
                            break;

                        case TokenKind.OpenRoundBracket:
                            {
                                var parser = new OpenRoundBracketNodeParser(Context);
                                parser.Run();
                                mResult = parser.Result;
                            }
                            break;

                        case TokenKind.CloseRoundBracket:
                            mState = State.AfterCloseRoundBracket;
                            break;

                        default:
#if DEBUG
                            LogInstance.Log($"OpenRoundBracketNodeParser OnRun mResult = {mResult}");
#endif
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterCloseRoundBracket:
                    switch (CurrToken.TokenKind)
                    {
                        case TokenKind.And:
                            {
                                var parentNode = new AndNodeParser(Context, mResult, TokenKind.CloseRoundBracket);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        case TokenKind.Or:
                            {
                                var parentNode = new OrNodeParser(Context, mResult, TokenKind.CloseRoundBracket);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        default:
#if DEBUG
                            LogInstance.Log($"OpenRoundBracketNodeParser OnRun mResult = {mResult}");
#endif
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
