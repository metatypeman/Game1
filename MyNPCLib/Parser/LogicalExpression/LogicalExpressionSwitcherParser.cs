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
            : base(context)
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
    }
}
