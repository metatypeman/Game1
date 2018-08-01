using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.NLToCGParsing.DependencyTree;
using MyNPCLib.NLToCGParsing.PhraseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class NounPhraseNodeOfSemanticAnalyzer : BaseNodeOfSemanticAnalyzer
    {
        public NounPhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, NounDTNode nounPhrase)
            : base(context)
        {
            mNounPhrase = nounPhrase;
        }

        private NounDTNode mNounPhrase;
        private ConceptCGNode mConcept;
        private bool mHasDeterminers;

        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            LogInstance.Log($"mNounPhrase = {mNounPhrase}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;
            var conceptualGraph = Context.ConceptualGraph;
            var noun = mNounPhrase.ExtendedToken;

            if (noun != null)
            {
#if DEBUG
            LogInstance.Log($"noun = {noun}");
#endif

                mConcept = new ConceptCGNode();
                mConcept.Parent = conceptualGraph;
                mConcept.Name = GetName(noun);

                var determinersList = mNounPhrase.DeterminersList;

                if(!determinersList.IsEmpty())
                {
                    mHasDeterminers = true;

#if DEBUG
                    LogInstance.Log($"determinersList.Count = {determinersList.Count}");
#endif

                    foreach (var determiner in determinersList)
                    {
#if DEBUG
                        LogInstance.Log($"determiner = {determiner}");
#endif

                        if(determiner.IsDeterminerDTNode)
                        {
                            var determinerDTNode = determiner.AsDeterminerDTNode;

                            CreateDeterminerMark(mConcept, noun, determinerDTNode.ExtendedToken);
                        }
                    }

                    //throw new NotImplementedException();
                }

                var nounFullLogicalMeaning = noun.FullLogicalMeaning;

                if (nounFullLogicalMeaning.IsEmpty())
                {
                    return result;
                }

                foreach (var logicalMeaning in nounFullLogicalMeaning)
                {
#if DEBUG
                    LogInstance.Log($"logicalMeaning = {logicalMeaning}");
#endif

                    PrimaryRolesDict.Add(logicalMeaning, mConcept);
                    resultPrimaryRolesDict.Add(logicalMeaning, mConcept);
                }

                //throw new NotImplementedException();

                //                

                //                if (!determinersList.IsEmpty())
                //                {
                //                    mHasDeterminers = true;

                //                    foreach (var determiner in determinersList)
                //                    {
                //#if DEBUG
                //                        LogInstance.Log($"determiner = {determiner}");
                //#endif

                //                        CreateDeterminerMark(mConcept, noun, determiner);
                //                    }
                //                }

                //                var nounFullLogicalMeaning = noun.FullLogicalMeaning;

                //                if (nounFullLogicalMeaning.IsEmpty())
                //                {
                //                    return result;
                //                }

                //                foreach (var logicalMeaning in nounFullLogicalMeaning)
                //                {
                //#if DEBUG
                //                    LogInstance.Log($"logicalMeaning = {logicalMeaning}");
                //#endif

                //                    PrimaryRolesDict.Add(logicalMeaning, mConcept);
                //                    resultPrimaryRolesDict.Add(logicalMeaning, mConcept);
                //                }
            }

#if DEBUG
            LogInstance.Log($"PrimaryRolesDict = {PrimaryRolesDict}");
            LogInstance.Log("End");
#endif

            return result;
        }

        private void CreateDeterminerMark(ConceptCGNode concept, ATNExtendedToken conceptExtendedToken, ATNExtendedToken determiner)
        {
            var relationName = CGGramamaticalNamesOfRelations.DeterminerName; 
            var determinerConceptName = GetName(determiner);

            if (Context.RelationStorage.ContainsRelation(concept.Name, determinerConceptName, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var determinerConcept = new ConceptCGNode();
            determinerConcept.Parent = conceptualGraph;
            determinerConcept.Name = determinerConceptName;

            var determinerRelation = new RelationCGNode();
            determinerRelation.Parent = conceptualGraph;
            determinerRelation.Name = relationName;

            concept.AddOutputNode(determinerRelation);
            determinerRelation.AddOutputNode(determinerConcept);

            Context.RelationStorage.AddRelation(concept.Name, determinerConceptName, relationName);

            MarkAsEntityCondition(determinerRelation);
        }
    }
}
