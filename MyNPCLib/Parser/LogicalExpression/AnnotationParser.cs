using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class AnnotationParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            WaitForFact,
            GotFact
        }

        public AnnotationParser(IParserContext context)
            : base(context, TokenKind.Unknown)
        {
        }

        private State mState = State.Init;
        private List<ASTNodeOfLogicalQuery> mResultsList = new List<ASTNodeOfLogicalQuery>();

        public List<ASTNodeOfLogicalQuery> ResultsList => mResultsList;

        protected override void OnRun()
        {
#if DEBUG
            //LogInstance.Log($"mState = {mState}");
            //LogInstance.Log($"CurrToken = {CurrToken}");
#endif

            var currTokenKind = CurrToken.TokenKind;

            switch (mState)
            {
                case State.Init:
                    switch (currTokenKind)
                    {
                        case TokenKind.BeginAnnotaion:
                            mState = State.WaitForFact;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.WaitForFact:
                    switch (currTokenKind)
                    {
                        case TokenKind.BeginFact:
                            {
                                Recovery(CurrToken);
                                var factParser = new FactParser(Context);
                                factParser.Run();
                                mResultsList.Add(factParser.Result);
                                mState = State.GotFact;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotFact:
                    switch (currTokenKind)
                    {
                        case TokenKind.EndAnnotation:
                            Exit();
                            break;

                        case TokenKind.Comma:
                            mState = State.WaitForFact;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        protected override void OnExit()
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif

#if DEBUG
            //LogInstance.Log("End");
#endif
        }
    }
}
