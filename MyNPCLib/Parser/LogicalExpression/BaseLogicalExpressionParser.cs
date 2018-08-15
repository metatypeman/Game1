using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public abstract class BaseLogicalExpressionParser
    {
        protected BaseLogicalExpressionParser(IParserContext context, TokenKind terminateTokenKind)
        {
            TerminateTokenKind = terminateTokenKind;
            Context = context;
        }

        public TokenKind TerminateTokenKind { get; protected set; }

        protected IParserContext Context { get; private set; }

        public void Run()
        {
            while ((CurrToken = Context.GetToken()) != null)
            {
                OnRun();

                if (mIsExited)
                {
                    OnFinish();
                    return;
                }
            }

            OnFinish();
            OnExit();
        }

        protected Token CurrToken;

        protected virtual void OnRun()
        {
        }

        protected virtual void OnExit()
        {
        }

        protected virtual void OnFinish()
        {
        }

        private bool mIsExited = false;

        protected void Exit()
        {
            mIsExited = true;
        }

        protected void Recovery(Token token)
        {
            Context.Recovery(token);
        }

        protected Token GetToken()
        {
            return Context.GetToken();
        }
    }
}
