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
            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.Relation;
            mASTNode.ParamsList = new List<ASTNodeOfLogicalQuery>();
        }

        private State mState = State.Init;

        private ASTNodeOfLogicalQuery mASTNode;
        public ASTNodeOfLogicalQuery Result => mASTNode;

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
                        default:
                        case TokenKind.Word:
                            ProcessNameOfRelation();
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

        private void ProcessNameOfRelation()
        {
            var tokenOfName = CurrToken;
            var kindOfTokenOfName = tokenOfName.TokenKind;

            switch (kindOfTokenOfName)
            {
                case TokenKind.Word:
                    mASTNode.Name = tokenOfName.Content;
                    break;

                case TokenKind.QuestionParam:
                    mASTNode.Name = tokenOfName.Content;
                    mASTNode.IsQuestion = true;
                    break;

                default:
                    throw new UnexpectedTokenException(tokenOfName);
            }

            mState = State.GotName;
        }

        private void DispatchValueInParam()
        {
#if DEBUG
            LogInstance.Log("DispatchValueInParam !!!!!!");
#endif

            Recovery(CurrToken);
            var paramExpressionParser = new LogicalExpressionSwitcherParser(Context);
            paramExpressionParser.Run();

            var paramASTNode = paramExpressionParser.Result;

#if DEBUG
            LogInstance.Log($"DispatchValueInParam !!!!!! paramASTNode = {paramASTNode}");
#endif

            mASTNode.ParamsList.Add(paramASTNode);
            
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
