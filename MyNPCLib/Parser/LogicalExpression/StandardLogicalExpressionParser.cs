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
            GotExpressionTree,
            GotAnnotation,
            GotVar,
            GotAssing,
            GotUnaryOperator,
            GotBinaryOperator
        }

        public StandardLogicalExpressionParser(IParserContext context, TokenKind terminateTokenKind)
            : base(context, terminateTokenKind)
        {
#if DEBUG
            mInitTailOfString = Context.TailOfString;
            LogInstance.Log($"mInitTailOfString = {mInitTailOfString}");
#endif
        }

#if DEBUG
        private string mInitTailOfString;
#endif
        
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
                        case TokenKind.QuestionParam:
                            DispatchWordAsRelation();
                            break;

                        case TokenKind.Var:
                            ProcessVar();
                            break;

                        case TokenKind.OpenRoundBracket:
                            ProcessGroup();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotExpressionTree:
                    switch (currTokenKind)
                    {
                        case TokenKind.BeginAnnotaion:
                            {
                                Recovery(CurrToken);
                                var annotationParser = new AnnotationParser(Context);
                                annotationParser.Run();
                                ProcessAnnotation();
                                mState = State.GotAnnotation;
                            }
                            break;

                        case TokenKind.Not:
                            ProcessUnaryOperator();
                            break;

                        case TokenKind.And:
                        case TokenKind.Or:
                            ProcessBinaryOperator();
                            break;

                        default:
#if DEBUG
                            
                            LogInstance.Log($"mInitTailOfString = {mInitTailOfString}");
                            LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
                            LogInstance.Log($"TerminateTokenKind = {TerminateTokenKind}");

#endif
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                //if(currTokenKind != TokenKind.CloseRoundBracket)
                                //{
                                    Recovery(CurrToken);
                                //}               
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotAnnotation:
                    switch (currTokenKind)
                    {
                        case TokenKind.And:
                        case TokenKind.Or:
                            ProcessBinaryOperator();
                            break;

                        default:
#if DEBUG

                            LogInstance.Log($"mInitTailOfString = {mInitTailOfString}");
                            LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
#endif
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Recovery(CurrToken);
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotVar:
                    switch (currTokenKind)
                    {
                        case TokenKind.Assing:
                            ProcessAssing();
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
                        case TokenKind.QuestionParam:
                            DispatchWordAsRelation();
                            break;

                        case TokenKind.OpenRoundBracket:
                            ProcessGroup();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotUnaryOperator:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            DispatchWordAsRelation();
                            break;

                        case TokenKind.Var:
                            ProcessVar();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotBinaryOperator:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                        case TokenKind.QuestionParam:
                            DispatchWordAsRelation();
                            break;

                        case TokenKind.Var:
                            ProcessVar();
                            break;

                        case TokenKind.Not:
                            ProcessUnaryOperator();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        private void DispatchWordAsRelation()
        {
            Recovery(CurrToken);
            var relationParser = new LogicalRelationParser(Context);
            relationParser.Run();
            ProcessRelation();
            mState = State.GotExpressionTree;
        }

        private void ProcessRelation()
        {
#if DEBUG
            LogInstance.Log("ProcessRelation !!!!!!");
#endif
        }

        private void ProcessVar()
        {
#if DEBUG
            LogInstance.Log($"ProcessVar !!!!! CurrToken = {CurrToken}");
#endif

            mState = State.GotVar;
        }

        private void ProcessAssing()
        {
#if DEBUG
            LogInstance.Log("ProcessAssing !!!!!!");
#endif

            mState = State.GotAssing;
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            LogInstance.Log("ProcessAnnotation !!!!!!!");
#endif
        }

        private void ProcessUnaryOperator()
        {
#if DEBUG
            LogInstance.Log("ProcessUnaryOperator !!!!!!!");
#endif

            mState = State.GotUnaryOperator;
        }

        private void ProcessBinaryOperator()
        {
#if DEBUG
            LogInstance.Log("ProcessBinaryOperator !!!!!!!");
#endif

            mState = State.GotBinaryOperator;
        }

        private void ProcessGroup()
        {
#if DEBUG
            LogInstance.Log("ProcessGroup !!!!!!!");
#endif
            var logicalExpressionParser = new StandardLogicalExpressionParser(Context, TokenKind.CloseRoundBracket);
            logicalExpressionParser.Run();

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

            mState = State.GotExpressionTree;
        }
    }
}
