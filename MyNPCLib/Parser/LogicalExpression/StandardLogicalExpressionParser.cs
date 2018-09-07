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
            GotVar,
            GotAssing,
            GotUnaryOperator,
            GotBinaryOperator
        }

        public StandardLogicalExpressionParser(IParserContext context, TokenKind terminateTokenKind)
            : base(context, terminateTokenKind)
        {
#if DEBUG
            //mInitTailOfString = Context.TailOfString;
            //LogInstance.Log($"mInitTailOfString = {mInitTailOfString}");
#endif
        }

#if DEBUG
        //private string mInitTailOfString;
#endif
        
        private State mState = State.Init;

        public ASTNodeOfLogicalQuery Result => mASTNode;
        private ASTNodeOfLogicalQuery mASTNode;

        private ASTNodeOfLogicalQuery mCurrentNode;
        private ASTNodeOfLogicalQuery mClusterNode;

        private List<ASTNodeOfLogicalQuery> mVarsOfRelationsList = new List<ASTNodeOfLogicalQuery>();

        private ASTNodeOfLogicalQuery mLastNode;

        protected override void OnRun()
        {
#if DEBUG
            //LogInstance.Log($"mState = {mState}");
            //LogInstance.Log($"CurrToken = {CurrToken}");
            //LogInstance.Log($"mLastNode = {mLastNode}");        
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

                        case TokenKind.Not:
                            ProcessUnaryOperator();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotExpressionTree:
                    switch (currTokenKind)
                    {
                        case TokenKind.BeginAnnotaion:
                            Recovery(CurrToken);
                            ProcessAnnotation();
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
                            
                            //LogInstance.Log($"mInitTailOfString = {mInitTailOfString}");
                            //LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
                            //LogInstance.Log($"TerminateTokenKind = {TerminateTokenKind}");

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
                            Recovery(CurrToken);
                            ProcessAnnotation();
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

                        case TokenKind.Not:
                            ProcessUnaryOperator();
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

        private void DispatchWordAsRelation()
        {
            Recovery(CurrToken);
            var relationParser = new LogicalRelationParser(Context);
            relationParser.Run();
            ProcessRelation(relationParser.Result);
            mState = State.GotExpressionTree;
        }

        private void ProcessRelation(ASTNodeOfLogicalQuery relation)
        {
#if DEBUG
            //LogInstance.Log($"ProcessRelation !!!!!! relation = {relation}");
            //LogInstance.Log($"mVarsOfRelationsList.Count = {mVarsOfRelationsList.Count}");
#endif

            relation.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;

            if (mVarsOfRelationsList.Count > 0)
            {
                relation.VarsList = mVarsOfRelationsList;
                mVarsOfRelationsList = new List<ASTNodeOfLogicalQuery>();
            }

#if DEBUG
            //LogInstance.Log($"after relation = {relation}");
#endif

            PutRelationLikeNodeToTree(relation);
        }

        private void ProcessVar()
        {
#if DEBUG
            //LogInstance.Log($"ProcessVar !!!!! CurrToken = {CurrToken}");
#endif

            var varNode = new ASTNodeOfLogicalQuery();
            varNode.Kind = KindOfASTNodeOfLogicalQuery.Var;
            varNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
            varNode.Name = CurrToken.Content;

            mVarsOfRelationsList.Add(varNode);

            mLastNode = varNode;

            mState = State.GotVar;
        }

        private void ProcessAssing()
        {
#if DEBUG
            //LogInstance.Log("ProcessAssing !!!!!!");
#endif

            mState = State.GotAssing;
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            //LogInstance.Log("ProcessAnnotation !!!!!!!");
            //LogInstance.Log($"mLastNode = {mLastNode}");
#endif

            var annotationParser = new AnnotationParser(Context);
            annotationParser.Run();

            var annotationsResult = annotationParser.ResultsList;

            mLastNode.AnnotationsList = annotationsResult;

#if DEBUG
            //LogInstance.Log($"after mLastNode = {mLastNode}");
#endif
            
            //throw new NotImplementedException();
        }

        private void ProcessUnaryOperator()
        {
#if DEBUG
            //LogInstance.Log("ProcessUnaryOperator !!!!!!!");
#endif
            var operatorToken = CurrToken;
            var tokenKindOfOperatorToken = operatorToken.TokenKind;

            var operatorASTNode = new ASTNodeOfLogicalQuery();
            operatorASTNode.Kind = KindOfASTNodeOfLogicalQuery.UnaryOperator;
            operatorASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;

            switch (tokenKindOfOperatorToken)
            {
                case TokenKind.Not:
                    operatorASTNode.KindOfOperator = KindOfOperatorOfASTNodeOfLogicalQuery.Not;
                    break;

                default:
                    throw new UnexpectedTokenException(operatorToken);
            }

#if DEBUG
            //LogInstance.Log($"mASTNode = {mASTNode}");
            //LogInstance.Log($"mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"mClusterNode = {mClusterNode}");
#endif

            if (mASTNode == null)
            {
                mASTNode = operatorASTNode;
                mCurrentNode = operatorASTNode;
                mClusterNode = operatorASTNode;

#if DEBUG
                //LogInstance.Log($"after mASTNode = {mASTNode}");
                //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
                //LogInstance.Log($"after mClusterNode = {mClusterNode}");
                //throw new NotImplementedException();
#endif

                mState = State.GotUnaryOperator;
                return;
            }

            var kindOfClusterNode = mClusterNode.Kind;

            switch (kindOfClusterNode)
            {
                case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                    {
                        var tmpNode = mCurrentNode;
                        if (tmpNode.IsGroup)
                        {
                            throw new NotSupportedException();
                        }
                        tmpNode.Left = operatorASTNode;
                        mCurrentNode = operatorASTNode;
                    }
                    break;

                case KindOfASTNodeOfLogicalQuery.BinaryOperator:
                    {
                        var tmpNode = mCurrentNode;

                        if (tmpNode.IsGroup)
                        {
                            throw new NotSupportedException();
                        }

                        var kindOfCurrentNode = mCurrentNode.Kind;

                        switch(kindOfCurrentNode)
                        {
                            case KindOfASTNodeOfLogicalQuery.BinaryOperator:
                                tmpNode.Right = operatorASTNode;
                                mCurrentNode = operatorASTNode;
                                break;

                            case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                                tmpNode.Left = operatorASTNode;
                                mCurrentNode = operatorASTNode;
#if DEBUG
                                //LogInstance.Log($"mASTNode = {mASTNode}");
                                //LogInstance.Log($"mCurrentNode = {mCurrentNode}");
                                //LogInstance.Log($"mClusterNode = {mClusterNode}");
#endif
                                //throw new NotImplementedException();
                                break;

                            case KindOfASTNodeOfLogicalQuery.Relation:
                                mClusterNode.Right = operatorASTNode;
                                operatorASTNode.Left = tmpNode;
                                mCurrentNode = operatorASTNode;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfCurrentNode), kindOfCurrentNode, null);
                        }
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfClusterNode), kindOfClusterNode, null);
            }

#if DEBUG
            //LogInstance.Log($"after mASTNode = {mASTNode}");
            //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"after mClusterNode = {mClusterNode}");
#endif

            //throw new NotImplementedException();

            mState = State.GotUnaryOperator;
        }

        private void ProcessBinaryOperator()
        {
#if DEBUG
            //LogInstance.Log("ProcessBinaryOperator !!!!!!!");
#endif
            var operatorToken = CurrToken;
            var tokenKindOfOperatorToken = operatorToken.TokenKind;

            var operatorASTNode = new ASTNodeOfLogicalQuery();
            operatorASTNode.Kind = KindOfASTNodeOfLogicalQuery.BinaryOperator;
            operatorASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;

            switch (tokenKindOfOperatorToken)
            {
                case TokenKind.And:
                    operatorASTNode.KindOfOperator = KindOfOperatorOfASTNodeOfLogicalQuery.And;
                    break;

                case TokenKind.Or:
                    operatorASTNode.KindOfOperator = KindOfOperatorOfASTNodeOfLogicalQuery.Or;
                    break;

                default:
                    throw new UnexpectedTokenException(operatorToken);
            }

            if (mASTNode == null)
            {
                throw new NotSupportedException();
            }

#if DEBUG
            //LogInstance.Log($"mASTNode = {mASTNode}");
            //LogInstance.Log($"mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"mClusterNode = {mClusterNode}");     
#endif

            var kindOfClusterNode = mClusterNode.Kind;

            switch(kindOfClusterNode)
            {
                case KindOfASTNodeOfLogicalQuery.Relation:
                    //throw new NotImplementedException();
                    {
                        var tmpNode = mClusterNode;
                        mClusterNode = operatorASTNode;
                        mCurrentNode = operatorASTNode;
                        mASTNode = operatorASTNode;
                        operatorASTNode.Left = tmpNode;

#if DEBUG
                        //LogInstance.Log($"after mASTNode = {mASTNode}");
                        //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
                        //LogInstance.Log($"after mClusterNode = {mClusterNode}");
#endif
                    }
                    break;

                case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                    {
                        var tmpNode = mClusterNode;
                        mClusterNode = operatorASTNode;
                        mCurrentNode = operatorASTNode;
                        mASTNode = operatorASTNode;
                        operatorASTNode.Left = tmpNode;

#if DEBUG
                        //LogInstance.Log($"after mASTNode = {mASTNode}");
                        //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
                        //LogInstance.Log($"after mClusterNode = {mClusterNode}");
                        //throw new NotImplementedException();
#endif
                    }
                    break;

                case KindOfASTNodeOfLogicalQuery.BinaryOperator:
                    {
                        var tmpNode = mClusterNode;
                        if (tmpNode.IsGroup)
                        {
                            mClusterNode = operatorASTNode;
                            mCurrentNode = operatorASTNode;
                            mASTNode = operatorASTNode;
                            operatorASTNode.Left = tmpNode;
                        }
                        else
                        {
                            mClusterNode = operatorASTNode;
                            mCurrentNode = operatorASTNode;
                            var oldRightNode = tmpNode.Right;
                            tmpNode.Right = operatorASTNode;
                            operatorASTNode.Left = oldRightNode;
                        }

#if DEBUG
                        //LogInstance.Log($"after mASTNode = {mASTNode}");
                        //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
                        //LogInstance.Log($"after mClusterNode = {mClusterNode}");
                        //if (operatorASTNode.KindOfOperator == KindOfOperatorOfASTNodeOfLogicalQuery.Or)
                        //{
                        //    throw new NotImplementedException();
                        //}
#endif
                    }
                break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfClusterNode), kindOfClusterNode, null);
            }

#if DEBUG
            //LogInstance.Log($"after mASTNode = {mASTNode}");
            //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"after mClusterNode = {mClusterNode}");
#endif

            //throw new NotImplementedException();

            mState = State.GotBinaryOperator;
        }

        private void ProcessGroup()
        {
#if DEBUG
            //LogInstance.Log("ProcessGroup !!!!!!!");
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

            var resultNode = logicalExpressionParser.Result;
            resultNode.IsGroup = true;
            resultNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;

            PutRelationLikeNodeToTree(resultNode);

            mState = State.GotExpressionTree;

#if DEBUG
            //LogInstance.Log("End ProcessGroup !!!!!!!");
#endif
        }

        private void PutRelationLikeNodeToTree(ASTNodeOfLogicalQuery node)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
            //LogInstance.Log($"mASTNode = {mASTNode}");
            //LogInstance.Log($"mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"mClusterNode = {mClusterNode}");
#endif

            mLastNode = node;

            if (mASTNode == null)
            {
                mASTNode = node;
                mCurrentNode = node;
                mClusterNode = node;
                return;
            }

            var kindOfClusterNode = mClusterNode.Kind;

            switch (kindOfClusterNode)
            {
                case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                    {
                        var tmpNode = mCurrentNode;
                        tmpNode.Left = node;
                        mCurrentNode = node;
                    }
                    break;

                case KindOfASTNodeOfLogicalQuery.BinaryOperator:
                    {
                        var tmpNode = mCurrentNode;
                        var kindOfCurrentNode = mCurrentNode.Kind;

                        switch (kindOfCurrentNode)
                        {
                            case KindOfASTNodeOfLogicalQuery.BinaryOperator:
                                tmpNode.Right = node;
                                mCurrentNode = node;

#if DEBUG
                                //if (node.Name == "son")
                                //{
                                //    LogInstance.Log($"after mASTNode = {mASTNode}");
                                //    LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
                                //    LogInstance.Log($"after mClusterNode = {mClusterNode}");
                                //    throw new NotImplementedException();
                                //}
#endif
                                break;

                            case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                                tmpNode.Left = node;
#if DEBUG
                                //LogInstance.Log($"after mASTNode = {mASTNode}");
                                //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
                                //LogInstance.Log($"after mClusterNode = {mClusterNode}");
                                //throw new NotImplementedException();
#endif
                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfCurrentNode), kindOfCurrentNode, null);
                        }
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfClusterNode), kindOfClusterNode, null);
            }

#if DEBUG
            //LogInstance.Log($"after mASTNode = {mASTNode}");
            //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"after mClusterNode = {mClusterNode}");
            //if (node.Name == "male")
            //{
            //    throw new NotImplementedException();
            //}
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
