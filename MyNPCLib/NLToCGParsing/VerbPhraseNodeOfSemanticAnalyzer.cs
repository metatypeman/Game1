using MyNPCLib.CG;
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
        private ConceptCGNode mConcept;
        private Dictionary<string, ATNExtendedToken> RolesDict = new Dictionary<string, ATNExtendedToken>();

        public void Run()
        {
#if DEBUG
            LogInstance.Log($"mVerbPhrase = {mVerbPhrase}");
#endif

            var verb = mVerbPhrase.Verb;
            var conceptualGraph = mContext.ConceptualGraph;
            mConcept = new ConceptCGNode();
            mConcept.Parent = conceptualGraph;
            var rootWord = verb.RootWord;

            if (string.IsNullOrWhiteSpace(rootWord))
            {
                mConcept.Name = verb.Content;
            }
            else
            {
                mConcept.Name = rootWord;
            }

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
