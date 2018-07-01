using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class NounPhraseNodeOfSemanticAnalyzer
    {
        public NounPhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, NounPhrase nounPhrase)
        {
            mContext = context;
            mNounPhrase = nounPhrase;
        }

        private ContextOfSemanticAnalyzer mContext;
        private NounPhrase mNounPhrase;

        public void Run()
        {
#if DEBUG
            LogInstance.Log($"mNounPhrase = {mNounPhrase}");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
