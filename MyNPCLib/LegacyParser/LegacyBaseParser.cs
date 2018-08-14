using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyBaseParser
    {
        protected LegacyBaseParser(LegacyParserContext context)
        {
            Context = context;
        }

        protected LegacyParserContext Context;

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

        protected LegacyToken CurrToken;

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
