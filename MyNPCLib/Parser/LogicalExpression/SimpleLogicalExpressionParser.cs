using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class SimpleLogicalExpressionParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init
        }

        public SimpleLogicalExpressionParser(IParserContext context)
            : base(context, TokenKind.Unknown)
        {
        }

        private State mState = State.Init;

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
                        default:
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
            //LogInstance.Log("End");
#endif
        }
    }
}
