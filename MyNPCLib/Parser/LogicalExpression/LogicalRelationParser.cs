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
            GotParam,
            GotVarInParam,
            GotAssingInParam
        }

        public LogicalRelationParser(IParserContext context)
            : base(context, TokenKind.Unknown)
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
                        case TokenKind.QuestionParam:
                        case TokenKind.BeginFact:
                        case TokenKind.Mul:
                            DispatchValueInParam();
                            break;

                        case TokenKind.Var:
                            DispatchVar();
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

                        case TokenKind.BeginAnnotaion:
                            {
                                Recovery(CurrToken);
                                var annotationParser = new AnnotationParser(Context);
                                annotationParser.Run();
                                ProcessAnnotation();
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotVarInParam:
                    switch (currTokenKind)
                    {
                        case TokenKind.Assing:
                            mState = State.GotAssingInParam;
                            break;

                        case TokenKind.Comma:
                            mState = State.WaitForParam;
                            break;

                        case TokenKind.CloseRoundBracket:
                            Exit();
                            break;

                        case TokenKind.BeginAnnotaion:
                            {
                                Recovery(CurrToken);
                                var annotationParser = new AnnotationParser(Context);
                                annotationParser.Run();
                                ProcessAnnotation();
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotAssingInParam:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                        case TokenKind.BeginFact:
                            DispatchValueInParam();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        private void DispatchValueInParam()
        {
#if DEBUG
            LogInstance.Log("DispatchValueInParam !!!!!!");
#endif

            Recovery(CurrToken);
            var paramExpressionParser = new LogicalExpressionSwitcherParser(Context);
            paramExpressionParser.Run();
            mState = State.GotParam;
        }

        private void DispatchVar()
        {
#if DEBUG
            LogInstance.Log("DispatchVar !!!!!!");
#endif

            mState = State.GotVarInParam;
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            LogInstance.Log("ProcessAnnotation !!!!!!!");
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
