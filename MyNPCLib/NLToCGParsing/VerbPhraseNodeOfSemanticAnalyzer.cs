using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.NLToCGParsing.DependencyTree;
using MyNPCLib.NLToCGParsing.PhraseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class VerbPhraseNodeOfSemanticAnalyzer : BaseNodeOfSemanticAnalyzer
    {
        public VerbPhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, VerbDTNode verbPhrase)
            : base(context)
        {
            mVerbPhrase = verbPhrase;
        }

        private VerbDTNode mVerbPhrase;
        private ConceptCGNode mConcept;
        
        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            LogInstance.Log($"mVerbPhrase = {mVerbPhrase}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;
            var verb = mVerbPhrase.ExtendedToken;
            var conceptualGraph = Context.ConceptualGraph;
            mConcept = new ConceptCGNode();
            mConcept.Parent = conceptualGraph;

            mConcept.Name = GetName(verb);

#if DEBUG
            LogInstance.Log($"verb = {verb}");
#endif

            var nounSubjectsList = mVerbPhrase.NounSubjectsList;

            if(!nounSubjectsList.IsEmpty())
            {
#if DEBUG
                LogInstance.Log($"nounSubjectsList.Count = {nounSubjectsList.Count}");
#endif

                foreach (var nounSubject in nounSubjectsList)
                {
#if DEBUG
                    LogInstance.Log($"nounSubject = {nounSubject}");
#endif

                    var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, nounSubject);
                    var nounResult = nounPhraseNode.Run();

#if DEBUG
                    LogInstance.Log($"nounResult = {nounResult}");
#endif
                }
            }

            throw new NotImplementedException();

            //            var verbFullLogicalMeaning = verb.FullLogicalMeaning;

            //            if(verbFullLogicalMeaning.IsEmpty())
            //            {
            //                return result;
            //            }

            //            foreach(var logicalMeaning in verbFullLogicalMeaning)
            //            {
            //#if DEBUG
            //                LogInstance.Log($"logicalMeaning = {logicalMeaning}");
            //#endif

            //                PrimaryRolesDict.Add(logicalMeaning, mConcept);
            //                resultPrimaryRolesDict.Add(logicalMeaning, mConcept);
            //            }

            //            if (mVerbPhrase.Object != null)
            //            {
            //                throw new NotImplementedException();
            //                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, mVerbPhrase.Object.AsNounPhrase);
            //                var nounResult = nounPhraseNode.Run();

            //#if DEBUG
            //                LogInstance.Log($"nounResult = {nounResult}");
            //#endif

            //                PrimaryRolesDict.Assing(nounResult.PrimaryRolesDict);
            //            }

            //#if DEBUG
            //            LogInstance.Log($"PrimaryRolesDict = {PrimaryRolesDict}");
            //#endif

            //            if(verbFullLogicalMeaning.Contains("event") || verbFullLogicalMeaning.Contains("state"))
            //            {
            //                var entitiesList = PrimaryRolesDict.GetByRole("entity");

            //                if(!entitiesList.IsEmpty())
            //                {
            //                    foreach(var entityConcept in entitiesList)
            //                    {
            //#if DEBUG
            //                        LogInstance.Log($"entityConcept = {entityConcept}");
            //#endif

            //                        CreateObjectRelation(mConcept, entityConcept);
            //                    }
            //                }
            //            }

#if DEBUG
            LogInstance.Log("End");
#endif

            return result;
        }

        private void CreateObjectRelation(ConceptCGNode verbConcept, ConceptCGNode objectConcept)
        {
            var relationName = SpecialNamesOfRelations.ObjectRelationName;

            if (Context.RelationStorage.ContainsRelation(verbConcept.Name, objectConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            verbConcept.AddOutputNode(relation);
            relation.AddOutputNode(objectConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, objectConcept.Name, relationName);
        }
    }
}
