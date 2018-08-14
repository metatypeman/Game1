using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class StandardLogicalExpressionParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotExpressionTree
        }

        public StandardLogicalExpressionParser(IParserContext context, TokenKind terminateTokenKind)
            : base(context)
        {
            mTerminateTokenKind = terminateTokenKind;
        }

        private TokenKind mTerminateTokenKind = TokenKind.Unknown;
        private State mState = State.Init;

        protected override void OnRun()
        {
#if DEBUG
            LogInstance.Log($"mState = {mState}");
            LogInstance.Log($"CurrToken = {CurrToken}");
#endif

            var currTokenKind = CurrToken.TokenKind;

            switch (mState)
            {
                case State.Init:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            {
                                Recovery(CurrToken);
                                var relationParser = new LogicalRelationParser(Context);
                                relationParser.Run();
                                mState = State.GotExpressionTree;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotExpressionTree:
                    switch (currTokenKind)
                    {
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
