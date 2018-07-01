using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class VerbPhraseNodeOfSemanticAnalyzer
    {
        public VerbPhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, VerbPhrase verbPhrase)
        {
            mContext = context;
            mVerbPhrase = verbPhrase;
        }

        private ContextOfSemanticAnalyzer mContext;
        private VerbPhrase mVerbPhrase;

        public void Run()
        {
#if DEBUG
            LogInstance.Log($"mVerbPhrase = {mVerbPhrase}");
#endif

            if (mVerbPhrase.Object != null)
            {
                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(mContext, mVerbPhrase.Object);
                nounPhraseNode.Run();
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
