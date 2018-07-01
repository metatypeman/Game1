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

#if DEBUG
                LogInstance.Log("End");
                var tmpResult = new ConceptualGraph();
                tmpResult.Name = NamesHelper.CreateEntityName();
                return tmpResult;
#endif
            }
        }
    }
}
