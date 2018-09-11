using System;
using System.Collections.Generic;
using System.Globalization;
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
                        case TokenKind.Word:
                            ProcessPropertyName();
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

                case State.GotPropetyName:
                    switch (currTokenKind)
                    {
                        case TokenKind.Assing:
                            mState = State.GotAssing;
                            break;

                        case TokenKind.BeginAnnotaion:
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
                        case TokenKind.FuzzyLogicalValue:
                            ProcessPropertyValue();                      
                            break;

                        case TokenKind.BeginFact:
                            ProcessFactValue();
                            break;

                        case TokenKind.BindedParam:
                            DispatchBindedVarInParam();
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
                            break;

                        case TokenKind.BeginAnnotaion:
                            ProcessAnnotation();
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

        private static CultureInfo mDefaultCultureInfo = new CultureInfo("en-US");

        private void ProcessPropertyName()
        {
#if DEBUG
            //LogInstance.Log($"ProcessPropertyName !!!!!!!!! CurrToken = {CurrToken}");
#endif

            mPropertyName = CurrToken.Content;

            mState = State.GotPropetyName;
        }

        private void ProcessPropertyValue()
        {
#if DEBUG
            //LogInstance.Log($"ProcessPropertyValue !!!!!!!! CurrToken = {CurrToken}");
            //LogInstance.Log($"mPropertyName = {mPropertyName}");
#endif

            var tokenKind = CurrToken.TokenKind;

            switch(tokenKind)
            {
                case TokenKind.Word:
                    {
                        var keyWordTokenKind = CurrToken.KeyWordTokenKind;

                        switch (keyWordTokenKind)
                        {
                            case TokenKind.Unknown:
                                {
                                    var propertyValue = new ASTNodeOfLogicalQuery();
                                    propertyValue.Kind = KindOfASTNodeOfLogicalQuery.Concept;
                                    propertyValue.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition;
                                    propertyValue.Name = CurrToken.Content;

                                    NProcessPropertyValue(propertyValue);
                                }
                                break;

                            case TokenKind.True:
                                ProcessTrueLogicalConst();
                                break;

                            case TokenKind.False:
                                ProcessFalseLogicalConst();
                                break;

                            default:
                                throw new UnexpectedTokenException(CurrToken);
                        }
                    }
                    break;

                case TokenKind.FuzzyLogicalValue:
                    ProcessNumberLogicalConst();
                    break;

                default:
                    throw new UnexpectedTokenException(CurrToken);
            }
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

            var propertyValue = new ASTNodeOfLogicalQuery();
            propertyValue.Kind = KindOfASTNodeOfLogicalQuery.BindedParam;
            propertyValue.BindedValue = valueOfParam;
            propertyValue.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition;

            NProcessPropertyValue(propertyValue);

            mLastNode = propertyValue;

            mState = State.GotProperyValue;
        }

        private void ProcessTrueLogicalConst()
        {
            NProcessLogicalValue("1.0");
        }

        private void ProcessFalseLogicalConst()
        {
            NProcessLogicalValue("0.0");
        }

        private void ProcessNumberLogicalConst()
        {
            NProcessLogicalValue(CurrToken.Content);
        }

        private void NProcessLogicalValue(string content)
        {
            var propertyValue = new ASTNodeOfLogicalQuery();
            propertyValue.Kind = KindOfASTNodeOfLogicalQuery.LogicalValue;
            propertyValue.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition;

            if (content.IndexOf(".") == -1)
            {
                content = $"{content}.0";
            }

            propertyValue.ObjValue = float.Parse(content, mDefaultCultureInfo);

            NProcessPropertyValue(propertyValue);

            mLastNode = propertyValue;

#if DEBUG
            //LogInstance.Log($"propertyValue = {propertyValue}");
#endif

            mState = State.GotProperyValue;
        }

        private void ProcessFactValue()
        {
            Recovery(CurrToken);

            var factParser = new FactParser(Context);
            factParser.Run();

            var factResult = factParser.Result;

            NProcessPropertyValue(factResult);
        }

        private void NProcessPropertyValue(ASTNodeOfLogicalQuery node)
        {
            var condition = new ASTNodeOfLogicalQuery();
            condition.Kind = KindOfASTNodeOfLogicalQuery.Condition;
            condition.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition;
            condition.Name = mPropertyName;
            condition.PropertyValue = node;

            PutRelationLikeNodeToTree(condition);

            mLastNode = node;

            mState = State.GotProperyValue;
        }

        private void PutRelationLikeNodeToTree(ASTNodeOfLogicalQuery node)
        {
#if DEBUG
            //LogInstance.Log($"node = {node}");
            //LogInstance.Log($"mASTNode = {mASTNode}");
            //LogInstance.Log($"mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"mClusterNode = {mClusterNode}");
#endif
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
                                break;

                            case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                                tmpNode.Left = node;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfCurrentNode), kindOfCurrentNode, null);
                        }
                    }
                    break;
            }

#if DEBUG
            //LogInstance.Log($"after mASTNode = {mASTNode}");
            //LogInstance.Log($"after mCurrentNode = {mCurrentNode}");
            //LogInstance.Log($"after mClusterNode = {mClusterNode}");
#endif

            //throw new NotImplementedException();
        }

        private void ProcessUnaryOperator()
        {
#if DEBUG
            //LogInstance.Log("ProcessUnaryOperator !!!!!");
#endif

            var operatorToken = CurrToken;
            var tokenKindOfOperatorToken = operatorToken.TokenKind;

            var operatorASTNode = new ASTNodeOfLogicalQuery();
            operatorASTNode.Kind = KindOfASTNodeOfLogicalQuery.UnaryOperator;
            operatorASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition;

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

                        switch (kindOfCurrentNode)
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

                            case KindOfASTNodeOfLogicalQuery.Condition:
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

            mState = State.GotUnaryOperator;
            //throw new NotImplementedException();
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
            operatorASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition;

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

            switch (kindOfClusterNode)
            {
                case KindOfASTNodeOfLogicalQuery.Condition:
                    {
                        var tmpNode = mClusterNode;
                        mClusterNode = operatorASTNode;
                        mCurrentNode = operatorASTNode;
                        mASTNode = operatorASTNode;
                        operatorASTNode.Left = tmpNode;
                    }
                    break;

                case KindOfASTNodeOfLogicalQuery.UnaryOperator:
                    {
                        var tmpNode = mClusterNode;
                        mClusterNode = operatorASTNode;
                        mCurrentNode = operatorASTNode;
                        mASTNode = operatorASTNode;
                        operatorASTNode.Left = tmpNode;
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

            mState = State.GotBinaryOperator;

            //throw new NotImplementedException();
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            //LogInstance.Log("ProcessAnnotation !!!!!!!");
#endif

            Recovery(CurrToken);
            var annotationParser = new AnnotationParser(Context);
            annotationParser.Run();

#if DEBUG
            //LogInstance.Log($"mLastNode = {mLastNode}");
#endif

            var annotationsResult = annotationParser.ResultsList;

            mLastNode.AnnotationsList = annotationsResult;

#if DEBUG
            //LogInstance.Log($"after mLastNode = {mLastNode}");
#endif

            //throw new NotImplementedException();
        }

        private void ProcessGroup()
        {
#if DEBUG
            //LogInstance.Log("ProcessGroup !!!!!!!");
#endif
            var logicalExpressionParser = new EntityConditionLogicalExpressionParser(Context, TokenKind.CloseRoundBracket);
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
            resultNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.EntityCondition;

            PutRelationLikeNodeToTree(resultNode);

            mState = State.GotProperyValue;
#if DEBUG
            //LogInstance.Log("End ProcessGroup !!!!!!!");
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

#if DEBUG
            //LogInstance.Log("End");
#endif
        }
    }
}
