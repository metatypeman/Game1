using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class AccessPolicyParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            WaitForContent,
            WaitForContentInFigureBracket,
            GotContentInFigureBracket
        }

        public AccessPolicyParser(IParserContext context)
            : base(context, TokenKind.EndFact)
        {
        }

        private State mState = State.Init;

        private List<ASTNodeOfLogicalQuery> mResult = new List<ASTNodeOfLogicalQuery>();
        public List<ASTNodeOfLogicalQuery> Result => mResult;

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
                        case TokenKind.BeginAccessPolicy:
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
                            {
                                var kindOfKeyWord = CurrToken.KeyWordTokenKind;

                                switch(kindOfKeyWord)
                                {
                                    case TokenKind.Public:
                                        PutSingleAccessPolicy(KindOfAccessPolicyToFact.Public);
                                        Exit();
                                        break;

                                    case TokenKind.Private:
                                        PutSingleAccessPolicy(KindOfAccessPolicyToFact.Private);
                                        Exit();
                                        break;

                                    case TokenKind.Visible:
                                        PutSingleAccessPolicy(KindOfAccessPolicyToFact.ForVisible);
                                        Exit();
                                        break;

                                    default:
                                        throw new UnexpectedTokenException(CurrToken);
                                }
                            }
                            break;

                        case TokenKind.OpenFigureBracket:
                            mState = State.WaitForContentInFigureBracket;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.WaitForContentInFigureBracket:
                    switch (currTokenKind)
                    {
                        case TokenKind.Word:
                            {
                                var kindOfKeyWord = CurrToken.KeyWordTokenKind;

                                switch (kindOfKeyWord)
                                {
                                    case TokenKind.Public:
                                        PutSingleAccessPolicy(KindOfAccessPolicyToFact.Public);
                                        mState = State.GotContentInFigureBracket;
                                        break;

                                    case TokenKind.Private:
                                        PutSingleAccessPolicy(KindOfAccessPolicyToFact.Private);
                                        mState = State.GotContentInFigureBracket;
                                        break;

                                    case TokenKind.Visible:
                                        PutSingleAccessPolicy(KindOfAccessPolicyToFact.ForVisible);
                                        mState = State.GotContentInFigureBracket;
                                        break;

                                    default:
                                        throw new UnexpectedTokenException(CurrToken);
                                }
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotContentInFigureBracket:
                    switch (currTokenKind)
                    {
                        case TokenKind.CloseFigureBracket:
                            Exit();
                            break;

                        case TokenKind.Comma:
                            mState = State.WaitForContentInFigureBracket;
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        private void PutSingleAccessPolicy(KindOfAccessPolicyToFact kindOfAccessPolicy)
        {
#if DEBUG
            //LogInstance.Log($"kindOfAccessPolicy = {kindOfAccessPolicy}");
#endif

            var astNode = new ASTNodeOfLogicalQuery();
            astNode.Kind = KindOfASTNodeOfLogicalQuery.AccessPolicy;
            astNode.KindOfAccessPolicy = kindOfAccessPolicy;

            mResult.Add(astNode);
        }
    }
}
