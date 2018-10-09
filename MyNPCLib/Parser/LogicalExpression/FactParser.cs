using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class FactParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            WaitForContent,
            GotUnbracketsContent,
            GotBracketContent,
            GotArrow,
            GotAnnotationOfTheFact
        }

        public FactParser(IParserContext context)
            : base(context, TokenKind.EndFact)
        {
            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.Fact;
        }

        private State mState = State.Init;

        public ASTNodeOfLogicalQuery Result => mASTNode;

        private ASTNodeOfLogicalQuery mASTNode;
        private Token mArrowToken;
        private bool Part_1Filled;
   
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
                    switch(currTokenKind)
                    {
                        case TokenKind.BeginFact:
                            mState = State.WaitForContent;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.WaitForContent:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                        case TokenKind.Var:
                        case TokenKind.QuestionParam:
                        case TokenKind.OpenRoundBracket:
                        case TokenKind.Not:
                            ProcessUnBracketRulePart();
                            break;

                        case TokenKind.OpenFigureBracket:
                            ProcessBracketRulePart();
                            break;

                        case TokenKind.BeginAccessPolicy:
                            ProcessAccessPolicy();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotUnbracketsContent:
                    switch (currTokenKind)
                    {
                        case TokenKind.EndFact:
#if DEBUG
                            //LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
#endif

                            ProcessEndFact();
                            break;

                        default:
#if DEBUG
                            //LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
#endif
                            if (currTokenKind == TerminateTokenKind && currTokenKind != TokenKind.Unknown)
                            {
                                Exit();
                                return;
                            }
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotBracketContent:
                    switch (currTokenKind)
                    {
                        case TokenKind.EndFact:
#if DEBUG
                            //LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
#endif
                            ProcessEndFact();
                            break;

                        case TokenKind.RightwardArrow:
                        case TokenKind.LeftwardArrow:
                        case TokenKind.LeftRightArrow:
                            ProcessArrow();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotArrow:
                    switch (currTokenKind)
                    {
                        case TokenKind.OpenFigureBracket:
                            ProcessBracketRulePart();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotAnnotationOfTheFact:
                    ProcessEndFact();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        private void ProcessAccessPolicy()
        {
            Recovery(CurrToken);

            var accessPolicyParser = new AccessPolicyParser(Context);
            accessPolicyParser.Run();
            var accessPolicyResult = accessPolicyParser.Result;

            mASTNode.AccessPolicy = accessPolicyResult;

#if DEBUG
            //LogInstance.Log($"accessPolicyResult = {accessPolicyResult}");
#endif

            mState = State.WaitForContent;
        }

        private void ProcessUnBracketRulePart()
        {
            Recovery(CurrToken);
            var rulePartParser = new RulePartParser(Context, TokenKind.EndFact);
            NProcessRulePart(rulePartParser);
            mState = State.GotUnbracketsContent;
        }

        private void ProcessBracketRulePart()
        {
            Recovery(CurrToken);
            var rulePartParser = new RulePartParser(Context, TokenKind.CloseFigureBracket);
            NProcessRulePart(rulePartParser);
            mState = State.GotBracketContent;
        }

        private void NProcessRulePart(RulePartParser rulePartParser)
        {
            rulePartParser.Run();
            var rulePartResult = rulePartParser.Result;
            mASTNode.SecondaryKind = rulePartResult.SecondaryKind;

#if DEBUG
            //LogInstance.Log($"Part_1Filled = {Part_1Filled}");
#endif

            if(Part_1Filled)
            {
                mASTNode.Part_2 = rulePartResult;
            }
            else
            {
                mASTNode.Part_1 = rulePartResult;
                Part_1Filled = true;
            }
        }

        private void ProcessArrow()
        {
#if DEBUG
            //LogInstance.Log($"ProcessArrow !!!!! CurrToken = {CurrToken}");
#endif

            mArrowToken = CurrToken;
            mState = State.GotArrow;
        }

        private void ProcessEndFact()
        {
#if DEBUG
            //LogInstance.Log("ProcessEndFact !!!!!!");
#endif

            var nextToken = GetToken();

            if (nextToken == null)
            {
                return;
            }

            var nextTokenKind = nextToken.TokenKind;

#if DEBUG
            //LogInstance.Log($"nextToken = {nextToken}");
#endif
            Recovery(nextToken);

            switch (nextTokenKind)
            {
                case TokenKind.BeginAnnotaion:
                    ProcessAnnotation();         
                    break;

                default:
                    Exit();
                    break;
            }          
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            //LogInstance.Log("ProcessAnnotation !!!!!!!");
#endif

            var annotationParser = new AnnotationParser(Context);
            annotationParser.Run();
            mASTNode.AnnotationsList = annotationParser.ResultsList;
            mState = State.GotAnnotationOfTheFact;
        }

        protected override void OnExit()
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif

#if DEBUG
            //LogInstance.Log($"mASTNode = {mASTNode}");
#endif

            if(mArrowToken == null)
            {
                if(mASTNode.Part_1 != null && mASTNode.Part_2 != null)
                {
                    throw new NotSupportedException();
                }

                if(mASTNode.Part_1 != null)
                {
                    mASTNode.Part_1.IsActivePart = true;
                }
                else
                {
                    if(mASTNode.Part_2 != null)
                    {
                        mASTNode.Part_2.IsActivePart = true;
                    }
                }
            }
            else
            {
                if (mASTNode.Part_1 == null || mASTNode.Part_2 == null)
                {
                    throw new NotSupportedException();
                }

                var arrowTokenKind = mArrowToken.TokenKind;

                switch(arrowTokenKind)
                {
                    case TokenKind.RightwardArrow:
                        mASTNode.Part_1.IsActivePart = true;
                        mASTNode.Part_2.IsActivePart = false;
                        break;

                    case TokenKind.LeftwardArrow:
                        mASTNode.Part_2.IsActivePart = true;
                        mASTNode.Part_1.IsActivePart = false;
                        break;

                    case TokenKind.LeftRightArrow:
                        mASTNode.Part_1.IsActivePart = true;
                        mASTNode.Part_2.IsActivePart = true;
                        break;

                    default:
                        throw new UnexpectedTokenException(mArrowToken);
                }
            }

#if DEBUG
            //LogInstance.Log("End");
#endif
        }
    }
}
