using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class AndNodeParser : BaseParser
    {
        public enum State
        {
            Init,
            ArterRigthNode
        }

        public AndNodeParser(ParserContext context, BaseQueryASTNode left, TokenKind? closingToken)
            : base(context)
        {
            mBinaryOperatorOfQueryASTNode = new BinaryOperatorOfQueryASTNode();
            mBinaryOperatorOfQueryASTNode.Left = left;
            mBinaryOperatorOfQueryASTNode.OperatorId = KindOfBinaryOperators.And;

            mClosingToken = closingToken;
        }

        private BinaryOperatorOfQueryASTNode mBinaryOperatorOfQueryASTNode;
        private TokenKind? mClosingToken;

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
                        case TokenKind.Word:
                            {
                                Context.Recovery(CurrToken);
                                var parentNode = new ConditionNodeParser(Context, mClosingToken);
                                parentNode.Run();
                                var result = parentNode.Result;
                                mBinaryOperatorOfQueryASTNode.Right = result;
                                mState = State.ArterRigthNode;

#if DEBUG
                                //LogInstance.Log($"result = {result}");
#endif
                            }
                            break;

                        case TokenKind.OpenRoundBracket:
                            {
                                var parser = new OpenRoundBracketNodeParser(Context);
                                parser.Run();
                                var result = parser.Result;
                                mBinaryOperatorOfQueryASTNode.Right = result;
                                mState = State.ArterRigthNode;

#if DEBUG
                                //LogInstance.Log($"result = {result}");
#endif
                            }
                            break;

                        case TokenKind.Not:
                            {
                                var parentNode = new NotNodeParser(Context);
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
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.ArterRigthNode:
                    switch (CurrToken.TokenKind)
                    {
                        case TokenKind.And:
                            {
                                var parser = new AndNodeParser(Context, mBinaryOperatorOfQueryASTNode.Right, mClosingToken);
                                parser.Run();
                                mBinaryOperatorOfQueryASTNode.Right = parser.Result;
                                Exit();
                            }
                            break;

                        case TokenKind.CloseRoundBracket:
                            Context.Recovery(CurrToken);
                            Exit();
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
