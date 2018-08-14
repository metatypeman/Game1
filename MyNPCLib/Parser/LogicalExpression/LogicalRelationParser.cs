using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class LogicalRelationParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotName,
            WaitForParam,
            GotParam
        }

        public LogicalRelationParser(IParserContext context)
            : base(context)
        {
        }

        private State mState = State.Init;
        private Token mTokenOfName;

        protected override void OnRun()
        {
#if DEBUG
            LogInstance.Log($"mState = {mState}");
            LogInstance.Log($"CurrToken = {CurrToken}");
            LogInstance.Log($"mTokenOfName = {mTokenOfName}");
#endif

            var currTokenKind = CurrToken.TokenKind;

            switch (mState)
            {
                case State.Init:
                    switch (currTokenKind)
                    {
                        default:
                        case TokenKind.Word:
                            {
                                mTokenOfName = CurrToken;
                                mState = State.GotName;
                            }
                            break;

                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotName:
                    switch (currTokenKind)
                    {
                        case TokenKind.OpenRoundBracket:
                            mState = State.WaitForParam;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.WaitForParam:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                        case TokenKind.BeginFact:
                            {
                                Recovery(CurrToken);
                                var paramExpressionParser = new LogicalExpressionSwitcherParser(Context);
                                paramExpressionParser.Run();
                                mState = State.GotParam;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotParam:
                    switch (currTokenKind)
                    {
                        case TokenKind.Comma:
                            {
                                mState = State.WaitForParam;
                            }
                            break;

                        case TokenKind.CloseRoundBracket:
                            Exit();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
