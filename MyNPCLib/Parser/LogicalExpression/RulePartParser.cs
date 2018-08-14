﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class RulePartParser : BaseLogicalExpressionParser
    {
        private enum State
        {
            Init,
            GotUnbracketsContent
        }

        public RulePartParser(IParserContext context, TokenKind terminateTokenKind)
            : base(context)
        {
            mTerminateTokenKind = terminateTokenKind;
        }

        private TokenKind mTerminateTokenKind = TokenKind.Unknown;
        private State mState = State.Init;

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
                        case TokenKind.Word:
                            {
                                Recovery(CurrToken);
                                var logicalExpressionParser = new LogicalExpressionParser(Context, mTerminateTokenKind);
                                logicalExpressionParser.Run();
                                mState = State.GotUnbracketsContent;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                default:
                    if (currTokenKind == mTerminateTokenKind && currTokenKind != TokenKind.Unknown)
                    {
                        Recovery(CurrToken);
                        Exit();
                        return;
                    }
                    throw new ArgumentOutOfRangeException(nameof(mState), mState, null);
            }
        }
    }
}
