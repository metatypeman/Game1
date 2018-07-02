using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class NounPhraseNodeOfSemanticAnalyzer : BaseNodeOfSemanticAnalyzer
    {
        public NounPhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, NounPhrase nounPhrase)
            : base(context)
        {
            mNounPhrase = nounPhrase;
        }

        private NounPhrase mNounPhrase;
        private RolesStorageOfSemanticAnalyzer mRolesDict = new RolesStorageOfSemanticAnalyzer();
        private ConceptCGNode mConcept;
        private bool mHasDeterminers;

        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            LogInstance.Log($"mNounPhrase = {mNounPhrase}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultRolesDict = result.RolesDict;
            var conceptualGraph = Context.ConceptualGraph;
            var noun = mNounPhrase.Noun;

            if (noun != null)
            {
                mConcept = new ConceptCGNode();
                mConcept.Parent = conceptualGraph;
                mConcept.Name = GetName(noun);

                var determinersList = mNounPhrase.Determiners;

                if (!determinersList.IsEmpty())
                {
                    mHasDeterminers = true;

                    foreach (var determiner in determinersList)
                    {
#if DEBUG
                        LogInstance.Log($"determiner = {determiner}");
#endif

                        CreateDeterminerMark(mConcept, determiner, conceptualGraph);
                    }
                }

                var nounFullLogicalMeaning = noun.FullLogicalMeaning;

                if (nounFullLogicalMeaning.IsEmpty())
                {
                    nounFullLogicalMeaning = new List<string>() { "phisobj" };
                }

                foreach (var logicalMeaning in nounFullLogicalMeaning)
                {
#if DEBUG
                    LogInstance.Log($"logicalMeaning = {logicalMeaning}");
#endif

                    mRolesDict.Add(logicalMeaning, noun);
                    resultRolesDict.Add(logicalMeaning, noun);
                }
            }

#if DEBUG
            LogInstance.Log($"mRolesDict = {mRolesDict}");
            LogInstance.Log("End");
#endif

            return result;
        }

        private void CreateDeterminerMark(ConceptCGNode concept, ATNExtendedToken determiner, ConceptualGraph conceptualGraph)
        {
            var determinerConcept = new ConceptCGNode();
            determinerConcept.Parent = conceptualGraph;
            determinerConcept.Name = GetName(determiner);

            var determinerRelation = new RelationCGNode();
            determinerRelation.Parent = conceptualGraph;
            determinerRelation.Name = "determiner";

            concept.AddOutputNode(determinerRelation);
            determinerRelation.AddOutputNode(determinerConcept);
        }
    }
}
