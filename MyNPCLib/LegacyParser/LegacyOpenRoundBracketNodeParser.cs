using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyOpenRoundBracketNodeParser : LegacyBaseParser
    {
        public enum State
        {
            Init,
            AfterCloseRoundBracket
        }

        public LegacyOpenRoundBracketNodeParser(LegacyParserContext context)
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
            //LogInstance.Log($"mState = {mState} CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}`");
#endif

            switch(mState)
            {
                case State.Init:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.Word:
                            Context.Recovery(CurrToken);
                            mResult = LegacyLogicalExpressionParserHelper.CreateNode(Context, LegacyTokenKind.CloseRoundBracket);
                            break;

                        case LegacyTokenKind.OpenRoundBracket:
                            {
                                var parser = new LegacyOpenRoundBracketNodeParser(Context);
                                parser.Run();
                                mResult = parser.Result;
                            }
                            break;

                        case LegacyTokenKind.CloseRoundBracket:
                            mState = State.AfterCloseRoundBracket;
                            break;

                        default:
#if DEBUG
                            LogInstance.Log($"mResult = {mResult}");
#endif
                            throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterCloseRoundBracket:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.And:
                            {
                                var parentNode = new LegacyAndNodeParser(Context, mResult, LegacyTokenKind.CloseRoundBracket);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        case LegacyTokenKind.Or:
                            {
                                var parentNode = new LegacyOrNodeParser(Context, mResult, LegacyTokenKind.CloseRoundBracket);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        default:
#if DEBUG
                            LogInstance.Log($"mResult = {mResult}");
#endif
                            throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
