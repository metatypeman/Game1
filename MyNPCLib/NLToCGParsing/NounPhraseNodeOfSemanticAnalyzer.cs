using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.NLToCGParsing.DependencyTree;
using MyNPCLib.NLToCGParsing.PhraseTree;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //private bool mHasDeterminers;

        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            //LogInstance.Log($"mNounPhrase = {mNounPhrase}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;
            var conceptualGraph = Context.ConceptualGraph;
            var noun = mNounPhrase.ExtendedToken;

#if DEBUG
            //LogInstance.Log($"noun = {noun}");
#endif

            if (noun == null)
            {
                return result;
            }

            mConcept = new ConceptCGNode();
            result.RootConcept = mConcept;
            mConcept.Parent = conceptualGraph;
            mConcept.Name = GetName(noun);

            var determinersList = mNounPhrase.DeterminersList;

            if (!determinersList.IsEmpty())
            {
                //mHasDeterminers = true;

#if DEBUG
                //LogInstance.Log($"determinersList.Count = {determinersList.Count}");
#endif

                foreach (var determiner in determinersList)
                {
#if DEBUG
                    //LogInstance.Log($"determiner = {determiner}");
#endif

                    if (determiner.IsDeterminerDTNode)
                    {
                        var determinerDTNode = determiner.AsDeterminerDTNode;

                        CreateDeterminerMark(mConcept, noun, determinerDTNode.ExtendedToken);
                    }
                }

                //throw new NotImplementedException();
            }

            var ajectivesList = mNounPhrase.AjectivesList;

            if(!ajectivesList.IsEmpty())
            {
#if DEBUG
                //LogInstance.Log($"ajectivesList.Count = {ajectivesList.Count}");
#endif

                foreach(var ajective in ajectivesList)
                {
#if DEBUG
                    //LogInstance.Log($"ajective = {ajective}");
#endif

                    var ajectiveNode = new AjectivePhraseNodeOfSemanticAnalyzer(Context, ajective);
                    var ajectiveNodeResult = ajectiveNode.Run();

#if DEBUG
                    //LogInstance.Log($"ajectiveNodeResult = {ajectiveNodeResult}");
#endif
                    var role = ajective.ExtendedToken.LogicalMeaning.FirstOrDefault();

#if DEBUG
                    //LogInstance.Log($"role = {role}");
#endif

                    if(!string.IsNullOrWhiteSpace(role))
                    {
                        var ajectiveConcept = ajectiveNodeResult.RootConcept;

#if DEBUG
                        //LogInstance.Log($"ajectiveConcept = {ajectiveConcept.ToBriefString()}");
#endif

                        var ajectiveRelation = new RelationCGNode();
                        ajectiveRelation.Parent = conceptualGraph;
                        ajectiveRelation.Name = role;

                        MarkAsEntityCondition(ajectiveRelation);

                        ajectiveRelation.AddInputNode(mConcept);
                        ajectiveRelation.AddOutputNode(ajectiveConcept);
                    }
                }
            }

            var nounFullLogicalMeaning = noun.FullLogicalMeaning;

            if (nounFullLogicalMeaning.IsEmpty())
            {
                return result;
            }

            foreach (var logicalMeaning in nounFullLogicalMeaning)
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
