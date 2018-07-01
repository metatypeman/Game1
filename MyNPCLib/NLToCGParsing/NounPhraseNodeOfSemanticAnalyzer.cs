using MyNPCLib.CG;
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
        private Dictionary<string, ATNExtendedToken> RolesDict = new Dictionary<string, ATNExtendedToken>();
        private ConceptCGNode mConcept;

        public void Run()
        {
#if DEBUG
            LogInstance.Log($"mNounPhrase = {mNounPhrase}");
#endif
            var conceptualGraph = mContext.ConceptualGraph;
            mConcept = new ConceptCGNode();
            mConcept.Parent = conceptualGraph;
            var noun = mNounPhrase.Noun;
            var rootWord = noun.RootWord;

            if(string.IsNullOrWhiteSpace(rootWord))
            {
                mConcept.Name = noun.Content;
            }
            else
            {
                mConcept.Name = rootWord;
            }
            
#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
