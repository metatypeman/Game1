using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class RulePartParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotUnbracketsContent,
            GotBrakedContent
        }

        public RulePartParser(IParserContext context, TokenKind terminateTokenKind)
            : base(context, terminateTokenKind)
        {
            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.RulePart;
        }

        private State mState = State.Init;

        private ASTNodeOfLogicalQuery mASTNode;
        public ASTNodeOfLogicalQuery Result => mASTNode;

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
                        case TokenKind.Var:
                        case TokenKind.QuestionParam:
                        case TokenKind.OpenRoundBracket:
                        case TokenKind.Not:
                            {
                                Recovery(CurrToken);
                                var logicalExpressionParser = new LogicalExpressionParser(Context, TerminateTokenKind);
                                logicalExpressionParser.Run();
                                mASTNode.Expression = logicalExpressionParser.Result;
                                mASTNode.SecondaryKind = mASTNode.Expression.SecondaryKind;
                                mState = State.GotUnbracketsContent;
                            }
                            break;

                        case TokenKind.OpenFigureBracket:
                            {
                                var logicalExpressionParser = new LogicalExpressionParser(Context, TerminateTokenKind);
                                logicalExpressionParser.Run();
                                mASTNode.Expression = logicalExpressionParser.Result;
                                mASTNode.SecondaryKind = mASTNode.Expression.SecondaryKind;
                                mState = State.GotBrakedContent;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotUnbracketsContent:
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

                case State.GotBrakedContent:
                    switch (currTokenKind)
                    {
                        default:
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
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
