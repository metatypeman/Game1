using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class RulePartParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotUnbracketsContent,
            GotBrakedContent
        }

        public RulePartParser(IParserContext context, TokenKind terminateTokenKind)
            : base(context, terminateTokenKind)
        {
        }

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
                        case TokenKind.Var:
                        case TokenKind.QuestionParam:
                        case TokenKind.OpenRoundBracket:
                        case TokenKind.Not:
                            {
                                Recovery(CurrToken);
                                var logicalExpressionParser = new LogicalExpressionParser(Context, TerminateTokenKind);
                                logicalExpressionParser.Run();
                                //TerminateTokenKind = logicalExpressionParser.TerminateTokenKind;
                                mState = State.GotUnbracketsContent;
                            }
                            break;

                        case TokenKind.OpenFigureBracket:
                            {
                                var logicalExpressionParser = new LogicalExpressionParser(Context, TerminateTokenKind);
                                logicalExpressionParser.Run();
                                //TerminateTokenKind = logicalExpressionParser.TerminateTokenKind;
                                mState = State.GotBrakedContent;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotUnbracketsContent:
                    switch (currTokenKind)
                    {
                        default:
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Recovery(CurrToken);
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotBrakedContent:
                    switch (currTokenKind)
                    {
                        default:
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
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
