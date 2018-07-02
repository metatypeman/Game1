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
                return result;
            }

            foreach(var logicalMeaning in verbFullLogicalMeaning)
            {
#if DEBUG
                LogInstance.Log($"logicalMeaning = {logicalMeaning}");
#endif

                PrimaryRolesDict.Add(logicalMeaning, mConcept);
                resultPrimaryRolesDict.Add(logicalMeaning, mConcept);
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

            if(verbFullLogicalMeaning.Contains("event") || verbFullLogicalMeaning.Contains("state"))
            {
                var entitiesList = PrimaryRolesDict.GetByRole("entity");

                if(!entitiesList.IsEmpty())
                {
                    foreach(var entityConcept in entitiesList)
                    {
#if DEBUG
                        LogInstance.Log($"entityConcept = {entityConcept}");
#endif

                        CreateObjectRelation(mConcept, entityConcept);
                    }
                }
            }

#if DEBUG
            LogInstance.Log("End");
#endif

            return result;
        }

        private void CreateObjectRelation(ConceptCGNode verbConcept, ConceptCGNode objectConcept)
        {
            var relationName = "object";

            if(Context.RelationStorage.ContainsRelation(verbConcept.Name, objectConcept.Name, relationName))
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
