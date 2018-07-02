using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class VerbPhraseNodeOfSemanticAnalyzer : BaseNodeOfSemanticAnalyzer
    {
        public VerbPhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, VerbPhrase verbPhrase)
            : base(context)
        {
            mVerbPhrase = verbPhrase;
        }

        private VerbPhrase mVerbPhrase;
        private ConceptCGNode mConcept;
        private RolesStorageOfSemanticAnalyzer mRolesDict = new RolesStorageOfSemanticAnalyzer();

        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            LogInstance.Log($"mVerbPhrase = {mVerbPhrase}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultRolesDict = result.RolesDict;
            var verb = mVerbPhrase.Verb;
            var conceptualGraph = Context.ConceptualGraph;
            mConcept = new ConceptCGNode();
            mConcept.Parent = conceptualGraph;

            mConcept.Name = GetName(verb);

            var verbFullLogicalMeaning = verb.FullLogicalMeaning;

            if(verbFullLogicalMeaning.IsEmpty())
            {
                verbFullLogicalMeaning = new List<string>() { "act" };
            }

            foreach(var logicalMeaning in verbFullLogicalMeaning)
            {
#if DEBUG
                LogInstance.Log($"logicalMeaning = {logicalMeaning}");
#endif

                mRolesDict.Add(logicalMeaning, verb);
                resultRolesDict.Add(logicalMeaning, verb);
            }

            if (mVerbPhrase.Object != null)
            {
                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, mVerbPhrase.Object);
                var nounResult = nounPhraseNode.Run();

#if DEBUG
                LogInstance.Log($"nounResult = {nounResult}");
#endif
            }

#if DEBUG
            LogInstance.Log($"mRolesDict = {mRolesDict}");
            LogInstance.Log("End");
#endif

            return result;
        }
    }
}
