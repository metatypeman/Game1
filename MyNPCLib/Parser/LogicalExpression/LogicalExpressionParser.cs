using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class LogicalExpressionParser: BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotStandardLogicalExpression,
            GotEntityConditionExpression
        }

        public LogicalExpressionParser(IParserContext context, TokenKind terminateTokenKind)
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
                                var nextToken = GetToken();
                                var nextTokenKind = nextToken.TokenKind;
#if DEBUG
                                LogInstance.Log($"nextToken = {nextToken}");
                                LogInstance.Log($"nextTokenKind = {nextTokenKind}");
#endif

                                switch(nextTokenKind)
                                {
                                    case TokenKind.OpenRoundBracket:
                                        {
                                            Recovery(nextToken);
                                            Recovery(CurrToken);                             
                                            var logicalExpressionParser = new StandardLogicalExpressionParser(Context, mTerminateTokenKind);
                                            logicalExpressionParser.Run();
                                            mState = State.GotStandardLogicalExpression;
                                        }
                                        break;

                                    case TokenKind.Assing:
                                        {
                                            Recovery(nextToken);
                                            Recovery(CurrToken);
                                            var logicalExpressionParser = new EntityConditionLogicalExpressionParser(Context, mTerminateTokenKind);
                                            logicalExpressionParser.Run();
                                            mState = State.GotEntityConditionExpression;
                                        }
                                        break;

                                    default:
                                        throw new UnexpectedTokenException(nextToken);
                                }
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotStandardLogicalExpression:
                    switch (currTokenKind)
                    {
                        default:
                            if (currTokenKind == mTerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Recovery(CurrToken);
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotEntityConditionExpression:
                    switch (currTokenKind)
                    {
                        default:
                            if (currTokenKind == mTerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Recovery(CurrToken);
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
