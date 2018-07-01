using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class SemanticAnalyzer
    {
        private readonly object mRunLockObj = new object();

        public ConceptualGraph Run(Sentence sentence)
        {
            lock(mRunLockObj)
            {
#if DEBUG
                LogInstance.Log($"sentence = {sentence}");
#endif

                var context = new ContextOfSemanticAnalyzer();

                var sentenceNode = new SentenceNodeOfSemanticAnalyzer(context, sentence);
                sentenceNode.Run();

                var result = context.ConceptualGraph;

#if DEBUG
                LogInstance.Log("End");
#endif

                return result;
            }
        }
    }
}
