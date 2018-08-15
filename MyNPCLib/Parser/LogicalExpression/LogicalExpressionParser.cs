using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class LogicalExpressionParser: BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotStandardLogicalExpression,
            GotEntityConditionExpression
        }

        private enum DetectingOfKindOfExpression
        {
            Undefined,
            Standard,
            EntityCondition,
            SingleConcept
        }

        public LogicalExpressionParser(IParserContext context, TokenKind terminateTokenKind)
            : base(context, terminateTokenKind)
        {
#if DEBUG
            mInitTailOfString = Context.TailOfString;
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
                    {
                        var kindOfExpression = DetectKindOfExpression();

                        switch(kindOfExpression)
                        {
                            case DetectingOfKindOfExpression.Standard:
                                {
                                    var logicalExpressionParser = new StandardLogicalExpressionParser(Context, TerminateTokenKind);
                                    logicalExpressionParser.Run();
                                    //mLastParser = logicalExpressionParser;
                                    //TerminateTokenKind = logicalExpressionParser.TerminateTokenKind;
                                    mState = State.GotStandardLogicalExpression;
                                }
                                break;

                            case DetectingOfKindOfExpression.EntityCondition:
                                {
                                    var logicalExpressionParser = new EntityConditionLogicalExpressionParser(Context, TerminateTokenKind);
                                    logicalExpressionParser.Run();
                                    //mLastParser = logicalExpressionParser;
                                    //TerminateTokenKind = logicalExpressionParser.TerminateTokenKind;
                                    mState = State.GotEntityConditionExpression;
                                }
                                break;

                            case DetectingOfKindOfExpression.SingleConcept:
                                ProcessSingleConcept();
                                Exit();
                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(kindOfExpression), kindOfExpression, null);
                        }
                    }
                    break;

                case State.GotStandardLogicalExpression:
                    switch (currTokenKind)
                    {
                        default:
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Recovery(CurrToken);
                                Exit();
                                return;
                            }
#if DEBUG
                            LogInstance.Log($"mInitTailOfString = {mInitTailOfString}");
                            LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
#endif

                            //if(TerminateTokenKind != mLastParser.TerminateTokenKind && mLastParser.TerminateTokenKind == TokenKind.CloseRoundBracket)
                            //{
                            //    Exit();
                            //    return;
                            //}
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotEntityConditionExpression:
                    switch (currTokenKind)
                    {
                        default:
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Recovery(CurrToken);
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        private void ProcessSingleConcept()
        {
#if DEBUG
            LogInstance.Log("ProcessSingleConcept !!!!!!");
#endif

            var nextToken = GetToken();
            var nextTokenKind = nextToken.TokenKind;

            switch (nextTokenKind)
            {
                case TokenKind.Word:
                    break;

                default:
                    throw new UnexpectedTokenException(CurrToken);
            }

#if DEBUG
            LogInstance.Log($"ProcessSingleConcept !!!!!! nextToken = {nextToken}");
#endif
        }

        private enum StateOfDetectKindOfExpression
        {
            Init,
            GotWord,
            GotGroup,
            GotWordInGroup
        }

        private Token mLocalCurrentToken;
        private StateOfDetectKindOfExpression mLocalState = StateOfDetectKindOfExpression.Init;
        private List<Token> mLocalUsedTokensList = new List<Token>();

        private DetectingOfKindOfExpression DetectKindOfExpression()
        {
#if DEBUG
            LogInstance.Log("Begin");
            LogInstance.Log($"CurrToken = {CurrToken}");
#endif

            Recovery(CurrToken);
            mLocalState = StateOfDetectKindOfExpression.Init;

            var result = NDetectKindOfExpression();

#if DEBUG
            LogInstance.Log($"result = {result}");
            LogInstance.Log($"mLocalUsedTokensList.Count = {mLocalUsedTokensList.Count}");
#endif

            if(mLocalUsedTokensList.Count > 0)
            {
                mLocalUsedTokensList.Reverse();

                foreach (var localUsedToken in mLocalUsedTokensList)
                {
#if DEBUG
                    LogInstance.Log($"localUsedToken = {localUsedToken}");
#endif
                    Recovery(localUsedToken);
                }
            }

            return result;
        }

        private DetectingOfKindOfExpression NDetectKindOfExpression()
        {
            mLocalCurrentToken = GetToken();

#if DEBUG
            LogInstance.Log($"localCurrentToken = {mLocalCurrentToken}");
            LogInstance.Log($"localState = {mLocalState}");
#endif

            var kindOfLocalCurrentToken = mLocalCurrentToken.TokenKind;

            switch (mLocalState)
            {
                case StateOfDetectKindOfExpression.Init:
                    switch(kindOfLocalCurrentToken)
                    {
                        case TokenKind.Word:
                        case TokenKind.QuestionParam:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            mLocalState = StateOfDetectKindOfExpression.GotWord;
                            return NDetectKindOfExpression();

                        case TokenKind.Var:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            return DetectingOfKindOfExpression.Standard;

                        case TokenKind.OpenRoundBracket:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            mLocalState = StateOfDetectKindOfExpression.GotGroup;
                            return NDetectKindOfExpression();

                        default:
                            throw new UnexpectedTokenException(mLocalCurrentToken);
                    }

                case StateOfDetectKindOfExpression.GotWord:
                    switch (kindOfLocalCurrentToken)
                    {
                        case TokenKind.OpenRoundBracket:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            return DetectingOfKindOfExpression.Standard;

                        case TokenKind.Assing:
                        case TokenKind.BeginAnnotaion:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            return DetectingOfKindOfExpression.EntityCondition;

                        case TokenKind.EndFact:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            return DetectingOfKindOfExpression.SingleConcept;

                        default:
                            throw new UnexpectedTokenException(mLocalCurrentToken);
                    }

                case StateOfDetectKindOfExpression.GotGroup:
                    switch (kindOfLocalCurrentToken)
                    {
                        case TokenKind.Word:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            mLocalState = StateOfDetectKindOfExpression.GotWordInGroup;
                            return NDetectKindOfExpression();

                        default:
                            throw new UnexpectedTokenException(mLocalCurrentToken);
                    }

                case StateOfDetectKindOfExpression.GotWordInGroup:
                    switch (kindOfLocalCurrentToken)
                    {
                        case TokenKind.OpenRoundBracket:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            return DetectingOfKindOfExpression.Standard;

                        case TokenKind.Assing:
                        case TokenKind.BeginAnnotaion:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            return DetectingOfKindOfExpression.EntityCondition;

                        case TokenKind.EndFact:
                            mLocalUsedTokensList.Add(mLocalCurrentToken);
                            return DetectingOfKindOfExpression.SingleConcept;

                        default:
                            throw new UnexpectedTokenException(mLocalCurrentToken);
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(mLocalState), mLocalState, null);
            }
        }
    }
}
