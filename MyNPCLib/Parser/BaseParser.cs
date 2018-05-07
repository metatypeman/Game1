using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class BaseParser
    {
        protected BaseParser(ParserContext context)
        {
            Context = context;
        }

        protected ParserContext Context;

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
    }
}
