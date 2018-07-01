using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class SentenceNodeOfSemanticAnalyzer
    {
        public SentenceNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, Sentence sentence)
        {
            mContext = context;
            mSentence = sentence;
        }

        private ContextOfSemanticAnalyzer mContext;
        private Sentence mSentence;
        private ConceptualGraph mConceptualGraph;

        public void Run()
        {
#if DEBUG
            LogInstance.Log($"mSentence = {mSentence}");
#endif

            mConceptualGraph = new ConceptualGraph();
            mContext.ConceptualGraph = mConceptualGraph;
            mConceptualGraph.Name = NamesHelper.CreateEntityName();

            if(mSentence.NounPhrase != null)
            {
                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(mContext, mSentence.NounPhrase);
                nounPhraseNode.Run();
            }

            if(mSentence.VerbPhrase != null)
            {
                var verbPhraseNode = new VerbPhraseNodeOfSemanticAnalyzer(mContext, mSentence.VerbPhrase);
                verbPhraseNode.Run();
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
