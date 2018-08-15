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
            GotUnaryOperator,
            GotBinaryOperator,
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

                        case TokenKind.OpenRoundBracket:
                            ProcessGroup();
                            break;

                        case TokenKind.Not:
                            ProcessUnaryOperator();
                            mState = State.GotUnaryOperator;
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
                        case TokenKind.Or:
                            ProcessBinaryOperator();
                            mState = State.GotBinaryOperator;
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

                case State.GotUnaryOperator:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            ProcessPropertyName();
                            mState = State.GotPropetyName;
                            break;

                        case TokenKind.OpenRoundBracket:
                            ProcessGroup();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotBinaryOperator:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            ProcessPropertyName();
                            mState = State.GotPropetyName;
                            break;

                        case TokenKind.Not:
                            ProcessUnaryOperator();
                            mState = State.GotUnaryOperator;
                            break;

                        case TokenKind.OpenRoundBracket:
                            ProcessGroup();
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

        private void ProcessUnaryOperator()
        {
#if DEBUG
            LogInstance.Log("ProcessUnaryOperator !!!!!");
#endif
        }

        private void ProcessBinaryOperator()
        {
#if DEBUG
            LogInstance.Log("ProcessBinaryOperator !!!!!!!");
#endif
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            LogInstance.Log("ProcessAnnotation !!!!!!!");
#endif
        }

        private void ProcessGroup()
        {
#if DEBUG
            LogInstance.Log("ProcessGroup !!!!!!!");
#endif
            var logicalExpressionParser = new EntityConditionLogicalExpressionParser(Context, TokenKind.CloseRoundBracket);
            logicalExpressionParser.Run();
            mState = State.GotProperyValue;

            var nextToken = GetToken();
            var nextTokenKind = nextToken.TokenKind;

            switch (nextTokenKind)
            {
                case TokenKind.CloseRoundBracket:
                    break;

                default:
                    throw new UnexpectedTokenException(nextToken);
            }

#if DEBUG
            LogInstance.Log("End ProcessGroup !!!!!!!");
#endif
        }
    }
}
