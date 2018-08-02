using MyNPCLib.CG;
using MyNPCLib.NLToCGParsing.DependencyTree;
using MyNPCLib.NLToCGParsing.PhraseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class SemanticAnalyzer
    {
        private readonly object mRunLockObj = new object();

        public ConceptualGraph Run(SentenceDTNode sentence)
        {
            lock(mRunLockObj)
            {
#if DEBUG
                //LogInstance.Log($"sentence = {sentence}");
#endif
                var outerConceptualGraph = new ConceptualGraph();
                var context = new ContextOfSemanticAnalyzer();
                context.OuterConceptualGraph = outerConceptualGraph;

                var sentenceNode = new SentenceNodeOfSemanticAnalyzer(context, sentence);
                var sentenceResult = sentenceNode.Run();

#if DEBUG
                //LogInstance.Log($"sentenceResult = {sentenceResult}");
#endif

                var result = outerConceptualGraph;

#if DEBUG
                //LogInstance.Log("End");
#endif

                return result;
            }
        }
    }
}
