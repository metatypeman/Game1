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
        
        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            LogInstance.Log($"mVerbPhrase = {mVerbPhrase}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;
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

                PrimaryRolesDict.Add(logicalMeaning, verb);
                resultPrimaryRolesDict.Add(logicalMeaning, verb);
            }

            if (mVerbPhrase.Object != null)
            {
                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, mVerbPhrase.Object);
                var nounResult = nounPhraseNode.Run();

#if DEBUG
                LogInstance.Log($"nounResult = {nounResult}");
#endif

                PrimaryRolesDict.Assing(nounResult.PrimaryRolesDict);
            }

#if DEBUG
            LogInstance.Log($"PrimaryRolesDict = {PrimaryRolesDict}");
#endif



#if DEBUG
            LogInstance.Log("End");
#endif

            return result;
        }
    }
}
