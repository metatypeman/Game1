using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyAndNodeParser : LegacyBaseParser
    {
        public enum State
        {
            Init,
            ArterRigthNode
        }

        public LegacyAndNodeParser(LegacyParserContext context, BaseQueryASTNode left, LegacyTokenKind? closingToken)
            : base(context)
        {
            mBinaryOperatorOfQueryASTNode = new BinaryOperatorOfQueryASTNode();
            mBinaryOperatorOfQueryASTNode.Left = left;
            mBinaryOperatorOfQueryASTNode.OperatorId = KindOfBinaryOperators.And;

            mClosingToken = closingToken;
        }

        private BinaryOperatorOfQueryASTNode mBinaryOperatorOfQueryASTNode;
        private LegacyTokenKind? mClosingToken;

        public BaseQueryASTNode Result
        {
            get
            {
                return mBinaryOperatorOfQueryASTNode;
            }
        }

        private State mState = State.Init;

        protected override void OnRun()
        {
#if DEBUG
            //LogInstance.Log($"mState = {mState} CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}` mClosingToken = {mClosingToken}");
#endif
            switch (mState)
            {
                case State.Init:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.Word:
                            {
                                Context.Recovery(CurrToken);
                                var parentNode = new LegacyConditionNodeParser(Context, mClosingToken);
                                parentNode.Run();
                                var result = parentNode.Result;
                                mBinaryOperatorOfQueryASTNode.Right = result;
                                mState = State.ArterRigthNode;

#if DEBUG
                                //LogInstance.Log($"result = {result}");
#endif
                            }
                            break;

                        case LegacyTokenKind.OpenRoundBracket:
                            {
                                var parser = new LegacyOpenRoundBracketNodeParser(Context);
                                parser.Run();
                                var result = parser.Result;
                                mBinaryOperatorOfQueryASTNode.Right = result;
                                mState = State.ArterRigthNode;

#if DEBUG
                                //LogInstance.Log($"result = {result}");
#endif
                            }
                            break;

                        case LegacyTokenKind.Not:
                            {
                                var parentNode = new LegacyNotNodeParser(Context);
                                parentNode.Run();
                                var result = parentNode.Result;
                                mBinaryOperatorOfQueryASTNode.Right = result;
                                mState = State.ArterRigthNode;

#if DEBUG
                                //LogInstance.Log($"result = {result}");
#endif
                            }
                            break;

                        default:
                            throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.ArterRigthNode:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.And:
                            {
                                var parser = new LegacyAndNodeParser(Context, mBinaryOperatorOfQueryASTNode.Right, mClosingToken);
                                parser.Run();
                                mBinaryOperatorOfQueryASTNode.Right = parser.Result;
                                Exit();
                            }
                            break;

                        case LegacyTokenKind.CloseRoundBracket:
                            Context.Recovery(CurrToken);
                            Exit();
                            break;

                        default:
                            throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
