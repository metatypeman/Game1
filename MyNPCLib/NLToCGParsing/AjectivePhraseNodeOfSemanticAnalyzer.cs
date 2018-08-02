using MyNPCLib.CG;
using MyNPCLib.NLToCGParsing.DependencyTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class AjectivePhraseNodeOfSemanticAnalyzer: BaseNodeOfSemanticAnalyzer
    {
        public AjectivePhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, AdjectiveDTNode adjectiveDTNode)
            : base(context)
        {
            mAdjectiveDTNode = adjectiveDTNode;
        }

        private AdjectiveDTNode mAdjectiveDTNode;
        private ConceptCGNode mConcept;

        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            //LogInstance.Log($"mAdjectiveDTNode = {mAdjectiveDTNode}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;
            var conceptualGraph = Context.ConceptualGraph;

            var ajective = mAdjectiveDTNode.ExtendedToken;

#if DEBUG
            //LogInstance.Log($"ajective = {ajective}");
#endif

            mConcept = new ConceptCGNode();
            result.RootConcept = mConcept;
            mConcept.Parent = conceptualGraph;
            mConcept.Name = GetName(ajective);

            var ajectiveFullLogicalMeaning = ajective.FullLogicalMeaning;

            if (ajectiveFullLogicalMeaning.IsEmpty())
            {
                return result;
            }

            foreach (var logicalMeaning in ajectiveFullLogicalMeaning)
            {
#if DEBUG
                //LogInstance.Log($"logicalMeaning = {logicalMeaning}");
#endif

                PrimaryRolesDict.Add(logicalMeaning, mConcept);
                resultPrimaryRolesDict.Add(logicalMeaning, mConcept);
            }

#if DEBUG
            //LogInstance.Log($"PrimaryRolesDict = {PrimaryRolesDict}");
            //LogInstance.Log("End");
#endif

            return result;
        }
    }
}
