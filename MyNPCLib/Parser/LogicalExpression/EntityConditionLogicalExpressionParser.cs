using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class EntityConditionLogicalExpressionParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotPropetyName,
            GotAssing,
            GotProperyValue,
            GotNegation,
            GotAnd,
            GotOr
        }

        public EntityConditionLogicalExpressionParser(IParserContext context, TokenKind terminateTokenKind)
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
                            {
                                ProcessPropertyName();
                                mState = State.GotPropetyName;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotPropetyName:
                    switch (currTokenKind)
                    {
                        case TokenKind.Assing:
                            mState = State.GotAssing;
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


                case State.GotAssing:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            ProcessPropertyValue();
                            mState = State.GotProperyValue;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotProperyValue:
                    switch (currTokenKind)
                    {
                        case TokenKind.And:
                            ProcessAnd();
                            mState = State.GotAnd;
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
                            if(currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Recovery(CurrToken);
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotNegation:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            ProcessPropertyName();
                            mState = State.GotPropetyName;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotAnd:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            ProcessPropertyName();
                            mState = State.GotPropetyName;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotOr:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            ProcessPropertyName();
                            mState = State.GotPropetyName;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        private void ProcessPropertyName()
        {
#if DEBUG
            LogInstance.Log($"ProcessPropertyName !!!!!!!!! CurrToken = {CurrToken}");
#endif
        }

        private void ProcessPropertyValue()
        {
#if DEBUG
            LogInstance.Log($"ProcessPropertyValue !!!!!!!! CurrToken = {CurrToken}");
#endif
        }

        private void ProcessNegation()
        {
#if DEBUG
            LogInstance.Log("ProcessNegation !!!!!");
#endif
        }

        private void ProcessAnd()
        {
#if DEBUG
            LogInstance.Log("ProcessAnd !!!!!!!");
#endif
        }

        private void ProcessOr()
        {
#if DEBUG
            LogInstance.Log("ProcessOr !!!!!!!");
#endif
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            LogInstance.Log("ProcessAnnotation !!!!!!!");
#endif
        }
    }
}
