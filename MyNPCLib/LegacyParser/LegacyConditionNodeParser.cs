using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyConditionNodeParser : LegacyBaseParser
    {
        public enum State
        {
            Init,
            AfterName,
            AfterEqual,
            AfterValue
        }

        public LegacyConditionNodeParser(LegacyParserContext context, LegacyTokenKind? closingToken)
            : base(context)
        {
            mConditionNode = new ConditionOfQueryASTNode();

            mClosingToken = closingToken;
        }

        private ConditionOfQueryASTNode mConditionNode;
        private LegacyTokenKind? mClosingToken;
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
            //LogInstance.Log($"mState = {mState} CurrToken.TokenKind = {CurrToken.TokenKind} CurrToken.Content = `{CurrToken.Content}` mClosingToken = {mClosingToken}");
#endif

            switch(mState)
            {
                case State.Init:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.Word:
                            mConditionNode.PropertyId = Context.GetKey(CurrToken.Content);
                            mState = State.AfterName;
                            break;

                        default: throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterName:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.Assing:
                            mState = State.AfterEqual;
                            break;

                        default: throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterEqual:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.Word:
                            mConditionNode.Value = CurrToken.Content;
                            mState = State.AfterValue;
                            break;

                        default: throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.AfterValue:
                    switch (CurrToken.TokenKind)
                    {
                        case LegacyTokenKind.Or:
                            {
                                var parentNode = new LegacyOrNodeParser(Context, mConditionNode, mClosingToken);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        case LegacyTokenKind.And:
                            {
                                var parentNode = new LegacyAndNodeParser(Context, mConditionNode, mClosingToken);
                                parentNode.Run();
                                mResult = parentNode.Result;
                                Exit();
                            }
                            break;

                        case LegacyTokenKind.CloseRoundBracket:
                            if(mClosingToken == LegacyTokenKind.CloseRoundBracket)
                            {
                                Context.Recovery(CurrToken);
                                mResult = mConditionNode;
                                Exit();
                                break;
                            }
                            throw new LegacyUnexpectedTokenException(CurrToken);


                        default: throw new LegacyUnexpectedTokenException(CurrToken);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }

        protected override void OnFinish()
        {
#if DEBUG
            //LogInstance.Log($"mState = {mState} Context.Count = {Context.Count} mResult = {mResult} mConditionNode = {mConditionNode}");
#endif

            if(Context.Count == 0 && mResult == null && mConditionNode != null)
            {
                mResult = mConditionNode;
                return;
            }
        }
    }
}
