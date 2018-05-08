using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class ConditionNodeParser : BaseParser
    {
        public enum State
        {
            Init,
            AfterName,
            AfterEqual,
            AfterValue
        }

        public ConditionNodeParser(ParserContext context, TokenKind? closingToken)
            : base(context)
        {
            mConditionNode = new ConditionOfQueryASTNode();

            mClosingToken = closingToken;
        }

        private ConditionOfQueryASTNode mConditionNode;
        private TokenKind? mClosingToken;
        private BaseQueryASTNode mResult;

        public BaseQueryASTNode Result
        {
            get
            {
                return mResult;
            }
        }

        private State mState = State.Init;

        protected override void OnRun()
        {
#if DEBUG
            LogInstance.Log($"ConditionNodeParser OnRun mState = {mState} CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}` mClosingToken = {mClosingToken}");
#endif

            switch(mState)
            {
                case State.Init:
                    switch (CurrToken.TokenKind)
                    {
                        case TokenKind.Word:
                            mConditionNode.PropertyId = Context.GetKey(CurrToken.Content);
                            mState = State.AfterName;
                            break;

                        default: throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterName:
                    switch (CurrToken.TokenKind)
                    {
                        case TokenKind.Assing:
                            mState = State.AfterEqual;
                            break;

                        default: throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterEqual:
                    switch (CurrToken.TokenKind)
                    {
                        case TokenKind.Word:
                            mConditionNode.Value = CurrToken.Content;
                            mState = State.AfterValue;
                            break;

                        default: throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterValue:
                    switch (CurrToken.TokenKind)
                    {
                        case TokenKind.Or:
                            {
                                var parentNode = new OrNodeParser(Context, mConditionNode, mClosingToken);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        case TokenKind.And:
                            {
                                var parentNode = new AndNodeParser(Context, mConditionNode, mClosingToken);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        case TokenKind.CloseRoundBracket:
                            if(mClosingToken == TokenKind.CloseRoundBracket)
                            {
                                Context.Recovery(CurrToken);
                                mResult = mConditionNode;
                                Exit();
                                break;
                            }
                            throw new UnexpectedTokenException(CurrToken);


                        default: throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
