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
            mASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
            mASTNode.ParamsList = new List<ASTNodeOfLogicalQuery>();
        }

        private State mState = State.Init;

        private ASTNodeOfLogicalQuery mASTNode;
        public ASTNodeOfLogicalQuery Result => mASTNode;

        private List<ASTNodeOfLogicalQuery> mVarsOfRelationsList = new List<ASTNodeOfLogicalQuery>();

        private ASTNodeOfLogicalQuery mLastNode;

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
                        case TokenKind.Number:
                        case TokenKind.FuzzyLogicalValue:
                        case TokenKind.QuestionParam:
                        case TokenKind.BeginFact:
                        case TokenKind.Mul:
                        case TokenKind.Entity:
                            DispatchValueInParam();
                            break;

                        case TokenKind.BindedParam:
                            DispatchBindedVarInParam();
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
                            ProcessAnnotation();
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
                            PutVarInParam();
                            mState = State.WaitForParam;
                            break;

                        case TokenKind.CloseRoundBracket:
                            PutVarInParam();
                            Exit();
                            break;

                        case TokenKind.BeginAnnotaion:
                            ProcessAnnotation();
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

                        case TokenKind.BindedParam:
                            DispatchBindedVarInParam();
                            break;

                        case TokenKind.Var:
                            DispatchVar();
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
            //LogInstance.Log("DispatchValueInParam !!!!!!");
#endif

            Recovery(CurrToken);
            var paramExpressionParser = new LogicalExpressionSwitcherParser(Context);
            paramExpressionParser.Run();

            var paramASTNode = paramExpressionParser.Result;
            paramASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
#if DEBUG
            //LogInstance.Log($"DispatchValueInParam !!!!!! paramASTNode = {paramASTNode}");
#endif

            mLastNode = paramASTNode;

            if(mVarsOfRelationsList.Count > 0)
            {
                if(paramASTNode.Kind != KindOfASTNodeOfLogicalQuery.Relation)
                {
                    throw new NotSupportedException();
                }

                paramASTNode.VarsList = mVarsOfRelationsList;

                mVarsOfRelationsList = new List<ASTNodeOfLogicalQuery>();
            }
            
            mASTNode.ParamsList.Add(paramASTNode);

            mState = State.GotParam;
        }

        private void DispatchBindedVarInParam()
        {
#if DEBUG
            //LogInstance.Log("DispatchBindedVarInParam !!!!!!");
#endif

            var valueOfParam = Context.GetVariantByParamName(CurrToken.Content);

#if DEBUG
            //LogInstance.Log($"valueOfParam = {valueOfParam}");
#endif

            var paramASTNode = new ASTNodeOfLogicalQuery();
            paramASTNode.Kind = KindOfASTNodeOfLogicalQuery.BindedParam;
            paramASTNode.BindedValue = valueOfParam;
            paramASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;

            mLastNode = paramASTNode;

            mASTNode.ParamsList.Add(paramASTNode);

            mState = State.GotParam;
        }

        private void DispatchVar()
        {
#if DEBUG
            //LogInstance.Log("DispatchVar !!!!!!");
#endif

            var varNode = new ASTNodeOfLogicalQuery();
            varNode.Kind = KindOfASTNodeOfLogicalQuery.Var;
            varNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
            varNode.Name = CurrToken.Content;

            mVarsOfRelationsList.Add(varNode);

            mLastNode = varNode;

            //throw new NotImplementedException();

            mState = State.GotVarInParam;
        }

        private void PutVarInParam()
        {
#if DEBUG
            //LogInstance.Log("PutVarInParam !!!!!!");
#endif

            mASTNode.ParamsList.Add(mLastNode);
            mLastNode = null;
            mVarsOfRelationsList = new List<ASTNodeOfLogicalQuery>();

            //throw new NotImplementedException();
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            //LogInstance.Log("ProcessAnnotation !!!!!!!");
            //LogInstance.Log($"mLastNode = {mLastNode}");
#endif
            Recovery(CurrToken);
            var annotationParser = new AnnotationParser(Context);
            annotationParser.Run();

            var annotationsResult = annotationParser.ResultsList;

            mLastNode.AnnotationsList = annotationsResult;

#if DEBUG
            //LogInstance.Log($"after mLastNode = {mLastNode}");
#endif

            //throw new NotImplementedException();
        }

        protected override void OnExit()
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif

#if DEBUG
            //LogInstance.Log($"mASTNode = {mASTNode}");
#endif

            //throw new NotImplementedException();

#if DEBUG
            //LogInstance.Log("End");
#endif
        }
    }
}
