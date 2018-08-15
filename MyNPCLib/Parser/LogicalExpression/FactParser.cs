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
        }

        private State mState = State.Init;
        private Token mArrowToken;

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
                            {
                                Recovery(CurrToken);
                                var rulePartParser = new RulePartParser(Context, TokenKind.EndFact);
                                rulePartParser.Run();
                                //TerminateTokenKind = rulePartParser.TerminateTokenKind;
                                mState = State.GotUnbracketsContent;
                            }
                            break;

                        case TokenKind.OpenFigureBracket:
                            ProcessRulePart();
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
                            LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
#endif

                            ProcessEndFact();
                            break;

                        default:
#if DEBUG
                            LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
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
                            LogInstance.Log($"Context.TailOfString = {Context.TailOfString}");
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
                            ProcessRulePart();
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

        private void ProcessRulePart()
        {
#if DEBUG
            LogInstance.Log("ProcessRulePart !!!!!!");
#endif

            Recovery(CurrToken);
            var rulePartParser = new RulePartParser(Context, TokenKind.CloseFigureBracket);
            rulePartParser.Run();
            mState = State.GotBracketContent;
        }

        private void ProcessArrow()
        {
#if DEBUG
            LogInstance.Log($"ProcessArrow !!!!! CurrToken = {CurrToken}");
#endif

            mArrowToken = CurrToken;
            mState = State.GotArrow;
        }

        private void ProcessEndFact()
        {
#if DEBUG
            LogInstance.Log("ProcessEndFact !!!!!!");
#endif

            var nextToken = GetToken();

            if (nextToken == null)
            {
                return;
            }

            var nextTokenKind = nextToken.TokenKind;

#if DEBUG
            LogInstance.Log($"nextToken = {nextToken}");
#endif
            Recovery(nextToken);

            switch (nextTokenKind)
            {
                case TokenKind.BeginAnnotaion:
                    {
                        var annotationParser = new AnnotationParser(Context);
                        annotationParser.Run();
                        ProcessAnnotation();
                        mState = State.GotAnnotationOfTheFact;
                    }           
                    break;

                default:
                    Exit();
                    break;
            }          
        }

        private void ProcessAnnotation()
        {
#if DEBUG
            LogInstance.Log("ProcessAnnotation !!!!!!!");
#endif
        }
    }
}
