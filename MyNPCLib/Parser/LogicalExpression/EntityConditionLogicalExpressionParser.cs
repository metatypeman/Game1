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

        public ASTNodeOfLogicalQuery Result => mASTNode;
        private ASTNodeOfLogicalQuery mASTNode;

        private ASTNodeOfLogicalQuery mCurrentNode;
        private ASTNodeOfLogicalQuery mClusterNode;
        private string mPropertyName;

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

            mPropertyName = CurrToken.Content;
        }

        private void ProcessPropertyValue()
        {
#if DEBUG
            LogInstance.Log($"ProcessPropertyValue !!!!!!!! CurrToken = {CurrToken}");
            LogInstance.Log($"mPropertyName = {mPropertyName}");
#endif

            var propertyValue = new ASTNodeOfLogicalQuery();
            propertyValue.Kind = KindOfASTNodeOfLogicalQuery.Concept;
            propertyValue.Name = CurrToken.Content;

            var condition = new ASTNodeOfLogicalQuery();
            condition.Kind = KindOfASTNodeOfLogicalQuery.Condition;
            condition.Name = mPropertyName;
            condition.PropertyValue = propertyValue;

            PutRelationLikeNodeToTree(condition);
        }

        private void PutRelationLikeNodeToTree(ASTNodeOfLogicalQuery node)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
            LogInstance.Log($"mASTNode = {mASTNode}");
            LogInstance.Log($"mCurrentNode = {mCurrentNode}");
            LogInstance.Log($"mClusterNode = {mClusterNode}");
#endif
            if (mASTNode == null)
            {
                mASTNode = node;
                mCurrentNode = node;
                mClusterNode = node;
                return;
            }

            throw new NotImplementedException();
        }

        private void ProcessUnaryOperator()
        {
#if DEBUG
            LogInstance.Log("ProcessUnaryOperator !!!!!");
#endif

            throw new NotImplementedException();
        }

        private void ProcessBinaryOperator()
        {
#if DEBUG
            LogInstance.Log("ProcessBinaryOperator !!!!!!!");
#endif

            var operatorToken = CurrToken;
            var tokenKindOfOperatorToken = operatorToken.TokenKind;

            var operatorASTNode = new ASTNodeOfLogicalQuery();
            operatorASTNode.Kind = KindOfASTNodeOfLogicalQuery.BinaryOperator;

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
            LogInstance.Log($"mASTNode = {mASTNode}");
            LogInstance.Log($"mCurrentNode = {mCurrentNode}");
            LogInstance.Log($"mClusterNode = {mClusterNode}");
#endif

            throw new NotImplementedException();
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            LogInstance.Log("ProcessAnnotation !!!!!!!");
#endif

            throw new NotImplementedException();
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

            throw new NotImplementedException();

#if DEBUG
            LogInstance.Log("End ProcessGroup !!!!!!!");
#endif
        }

        protected override void OnExit()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

#if DEBUG
            LogInstance.Log($"mASTNode = {mASTNode}");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
