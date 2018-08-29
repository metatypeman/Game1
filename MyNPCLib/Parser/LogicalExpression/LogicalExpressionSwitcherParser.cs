﻿using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class LogicalExpressionSwitcherParser: BaseLogicalExpressionParser
    {
        private enum State
        {
            Init
        }

        public LogicalExpressionSwitcherParser(IParserContext context)
            : base(context, TokenKind.Unknown)
        {
        }

        private State mState = State.Init;

        public ASTNodeOfLogicalQuery Result => mASTNode;
        private ASTNodeOfLogicalQuery mASTNode;

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
                                var nextToken = GetToken();
                                var nextTokenKind = nextToken.TokenKind;
#if DEBUG
                                LogInstance.Log($"nextToken = {nextToken}");
                                LogInstance.Log($"nextTokenKind = {nextTokenKind}");
#endif

                                switch(nextTokenKind)
                                {
                                    case TokenKind.Comma:
                                    case TokenKind.CloseRoundBracket:
                                    case TokenKind.BeginAnnotaion:
                                        Recovery(nextToken);
                                        ProcessConcept();
                                        Exit();
                                        break;

                                    case TokenKind.OpenRoundBracket:
                                        Recovery(nextToken);
                                        ProcessRelation();
                                        Exit();
                                        break;

                                    default:
                                        throw new UnexpectedTokenException(nextToken);
                                }
                            }
                            break;

                        case TokenKind.Number:
                            ProcessNumber();
                            Exit();
                            break;

                        case TokenKind.BeginFact:
                            ProcessFact();
                            Exit();
                            break;

                        case TokenKind.QuestionParam:
                            {
                                var nextToken = GetToken();
                                var nextTokenKind = nextToken.TokenKind;
#if DEBUG
                                LogInstance.Log($"nextToken = {nextToken}");
                                LogInstance.Log($"nextTokenKind = {nextTokenKind}");
#endif

                                switch (nextTokenKind)
                                {
                                    case TokenKind.Comma:
                                    case TokenKind.CloseRoundBracket:
                                    case TokenKind.BeginAnnotaion:
                                        Recovery(nextToken);
                                        ProcessQuestionParam();
                                        Exit();
                                        break;

                                    default:
                                        throw new UnexpectedTokenException(nextToken);
                                }
                            }
                            break;

                        case TokenKind.Mul:
                            ProcessStubOfConcept();
                            Exit();
                            break;

                        case TokenKind.Entity:
                            ProcessEntity();
                            Exit();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        private void ProcessEntity()
        {
#if DEBUG
            LogInstance.Log($"ProcessEntity!!!!!!!! CurrToken = {CurrToken}");
#endif
            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.EntityRef;
            mASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
            mASTNode.Name = CurrToken.Content;
        }

        private void ProcessNumber()
        {
#if DEBUG
            LogInstance.Log($"ProcessNumber!!!!!!!! CurrToken = {CurrToken}");
#endif

            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.Value;
            mASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
            mASTNode.KindOfValueType = KindOfValueType.Number;

            throw new NotImplementedException();
        }

        private void ProcessConcept()
        {
#if DEBUG
            LogInstance.Log($"CONCEPT!!!!!!!! CurrToken = {CurrToken}");
#endif

            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.Concept;
            mASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
            mASTNode.Name = CurrToken.Content;
        }

        private void ProcessQuestionParam()
        {
#if DEBUG
            LogInstance.Log($"ProcessQuestionParam!!!!!!!! CurrToken = {CurrToken}");
#endif

            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.QuestionParam;
            mASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
            mASTNode.Name = CurrToken.Content;
            mASTNode.IsQuestion = true;
        }

        private void ProcessStubOfConcept()
        {
#if DEBUG
            LogInstance.Log("ProcessStubOfConcept!!!!!!!!");
#endif

            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.StubParam;
            mASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
        }

        private void ProcessFact()
        {
            Recovery(CurrToken);
            var factParser = new FactParser(Context);
            factParser.Run();

            mASTNode = factParser.Result;
        }

        private void ProcessRelation()
        {
#if DEBUG
            LogInstance.Log("ProcessRelation!!!!!!!!");
#endif

            Recovery(CurrToken);
            var relationParser = new LogicalRelationParser(Context);
            relationParser.Run();

            mASTNode = relationParser.Result;
            mASTNode.SecondaryKind = SecondaryKindOfASTNodeOfLogicalQuery.StandardExpression;
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
