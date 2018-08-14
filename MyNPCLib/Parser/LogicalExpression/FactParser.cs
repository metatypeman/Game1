﻿using System;
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
            GotUnbracketsContent
        }

        public FactParser(IParserContext context)
            : base(context)
        {
        }

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
                            {
                                Recovery(CurrToken);
                                var rulePartParser = new RulePartParser(Context, TokenKind.EndFact);
                                rulePartParser.Run();
                                mState = State.GotUnbracketsContent;
                            }
                            break;

                        default:
                            throw new UnexpectedTokenException(CurrToken);
                    }
                    break;

                case State.GotUnbracketsContent:
                    switch (currTokenKind)
                    {
                        case TokenKind.EndFact:
                            Exit();
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
