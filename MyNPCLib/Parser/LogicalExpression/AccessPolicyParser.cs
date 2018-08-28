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
            WaitForContent
        }

        public AccessPolicyParser(IParserContext context)
            : base(context, TokenKind.EndFact)
        {
            mASTNode = new ASTNodeOfLogicalQuery();
            mASTNode.Kind = KindOfASTNodeOfLogicalQuery.AccessPolicy;
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
                                        mASTNode.KindOfAccessPolicy = KindOfAccessPolicyToFact.Public;
                                        Exit();
                                        break;

                                    case TokenKind.Private:
                                        mASTNode.KindOfAccessPolicy = KindOfAccessPolicyToFact.Private;
                                        Exit();
                                        break;

                                    case TokenKind.Visible:
                                        mASTNode.KindOfAccessPolicy = KindOfAccessPolicyToFact.ForVisible;
                                        Exit();
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

                default:
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
