using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class LogicalExpressionSwitcherParser: BaseLogicalExpressionParser
    {
        private enum State
        {
            Init
        }

        public LogicalExpressionSwitcherParser(IParserContext context)
            : base(context, TokenKind.Unknown)
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
                            {
                                var nextToken = GetToken();
                                var nextTokenKind = nextToken.TokenKind;
#if DEBUG
                                LogInstance.Log($"nextToken = {nextToken}");
                                LogInstance.Log($"nextTokenKind = {nextTokenKind}");
#endif

                                switch(nextTokenKind)
                                {
                                    case TokenKind.Comma:
                                    case TokenKind.CloseRoundBracket:
                                    case TokenKind.BeginAnnotaion:
                                        Recovery(nextToken);
                                        ProcessConcept();
                                        Exit();
                                        break;

                                    default:
                                        throw new UnexpectedTokenException(nextToken);
                                }
                            }
                            break;

                        case TokenKind.BeginFact:
                            {
                                Recovery(CurrToken);
                                var factParser = new FactParser(Context);
                                factParser.Run();
                                Exit();
                            }
                            break;

                        case TokenKind.QuestionParam:
                            {
                                var nextToken = GetToken();
                                var nextTokenKind = nextToken.TokenKind;
#if DEBUG
                                LogInstance.Log($"nextToken = {nextToken}");
                                LogInstance.Log($"nextTokenKind = {nextTokenKind}");
#endif

                                switch (nextTokenKind)
                                {
                                    case TokenKind.Comma:
                                    case TokenKind.CloseRoundBracket:
                                        Recovery(nextToken);
                                        ProcessQuestionParam();
                                        Exit();
                                        break;

                                    default:
                                        throw new UnexpectedTokenException(nextToken);
                                }
                            }
                            break;

                        case TokenKind.Mul:
                            ProcessStubOfConcept();
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

        private void ProcessConcept()
        {
#if DEBUG
            LogInstance.Log($"CONCEPT!!!!!!!! CurrToken = {CurrToken}");
#endif
        }

        private void ProcessQuestionParam()
        {
#if DEBUG
            LogInstance.Log($"ProcessQuestionParam!!!!!!!! CurrToken = {CurrToken}");
#endif
        }

        private void ProcessStubOfConcept()
        {
#if DEBUG
            LogInstance.Log("ProcessStubOfConcept!!!!!!!!");
#endif
        }

        protected override void OnExit()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
