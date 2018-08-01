using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.NLToCGParsing.DependencyTree;
using MyNPCLib.NLToCGParsing.PhraseTree;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class SentenceNodeOfSemanticAnalyzer: BaseNodeOfSemanticAnalyzer
    {
        public SentenceNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, SentenceDTNode sentence)
            : base(context)
        {         
            mSentence = sentence;
        }
    
        private SentenceDTNode mSentence;
        private ConceptualGraph mConceptualGraph;

        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            LogInstance.Log($"mSentence = {mSentence}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;

            mConceptualGraph = new ConceptualGraph();
            mConceptualGraph.Parent = Context.OuterConceptualGraph;
            Context.ConceptualGraph = mConceptualGraph;
            mConceptualGraph.Name = NamesHelper.CreateEntityName();

            CreateGrammaticalRelations();

            var rootVerb = mSentence.Verb;

#if DEBUG
            LogInstance.Log($"rootVerb = {rootVerb}");
#endif

            if(rootVerb != null)
            {
                var verbPhraseNode = new VerbPhraseNodeOfSemanticAnalyzer(Context, rootVerb);
                var verbResult = verbPhraseNode.Run();

#if DEBUG
                LogInstance.Log($"verbResult = {verbResult}");
#endif

                //throw new NotImplementedException();
            }

            //throw new NotImplementedException();

//            if (mSentence.NounPhrase != null)
//            {
//                throw new NotImplementedException();
//                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, mSentence.NounPhrase.AsNounPhrase);
//                var nounResult = nounPhraseNode.Run();

//#if DEBUG
//                LogInstance.Log($"nounResult = {nounResult}");
//#endif

//                PrimaryRolesDict.Assing(nounResult.PrimaryRolesDict);
//            }

//            if (mSentence.VerbPhrase != null)
//            {
//                var verbPhraseNode = new VerbPhraseNodeOfSemanticAnalyzer(Context, mSentence.VerbPhrase);
//                var verbResult = verbPhraseNode.Run();

//#if DEBUG
//                LogInstance.Log($"verbResult = {verbResult}");
//#endif

//                PrimaryRolesDict.Assing(verbResult.PrimaryRolesDict);
//            }

//#if DEBUG
//            LogInstance.Log($"PrimaryRolesDict = {PrimaryRolesDict}");
//#endif

//            var primaryAnimatesList = PrimaryRolesDict.GetByRole("animate");

//            if(!primaryAnimatesList.IsEmpty())
//            {
//                //state -> experiencer -> animate

//                var primaryStatesList = PrimaryRolesDict.GetByRole("state");

//                if (!primaryStatesList.IsEmpty())
//                {
//                    foreach(var state in primaryStatesList)
//                    {
//                        foreach(var animate in primaryAnimatesList)
//                        {
//                            CreateExperiencerRelation(state, animate);
//                            CreateStateRelation(state, animate);
//                        }
//                    }
//                }
//                //act -> agent -> animate

//                var primaryActsList = PrimaryRolesDict.GetByRole("act");

//                if(!primaryActsList.IsEmpty())
//                {
//                    foreach(var act in primaryActsList)
//                    {
//                        foreach (var animate in primaryAnimatesList)
//                        {
//                            CreateAgentRelation(act, animate);
//                            CreateActionRelation(act, animate);
//                        }
//                    }
//                }
//            }

#if DEBUG
            LogInstance.Log("End");
#endif

            return result;
        }

        private void CreateGrammaticalRelations()
        {
            var conceptualGraph = Context.ConceptualGraph;
            var outerConceptualGraph = Context.OuterConceptualGraph;

            var aspectName = GrammaticalElementsHeper.GetAspectName(mSentence.Aspect);

#if DEBUG
            LogInstance.Log($"aspectName = {aspectName}");
#endif

            if (!string.IsNullOrWhiteSpace(aspectName))
            {
                var grammarConcept = new ConceptCGNode();
                grammarConcept.Parent = outerConceptualGraph;
                grammarConcept.Name = aspectName;

                var grammarRelation = new RelationCGNode();
                grammarRelation.Parent = outerConceptualGraph;
                grammarRelation.Name = CGGramamaticalNamesOfRelations.AspectName;

                conceptualGraph.AddOutputNode(grammarRelation);
                grammarRelation.AddOutputNode(grammarConcept);
            }

            var tenseName = GrammaticalElementsHeper.GetTenseName(mSentence.Tense);

#if DEBUG
            LogInstance.Log($"tenseName = {tenseName}");
#endif 

            if(!string.IsNullOrWhiteSpace(tenseName))
            {
                var grammarConcept = new ConceptCGNode();
                grammarConcept.Parent = outerConceptualGraph;
                grammarConcept.Name = tenseName;

                var grammarRelation = new RelationCGNode();
                grammarRelation.Parent = outerConceptualGraph;
                grammarRelation.Name = CGGramamaticalNamesOfRelations.TenseName;

                conceptualGraph.AddOutputNode(grammarRelation);
                grammarRelation.AddOutputNode(grammarConcept);
            }

            var voiceName = GrammaticalElementsHeper.GetVoiceName(mSentence.Voice);

#if DEBUG
            LogInstance.Log($"voiceName = {voiceName}");
#endif
            if (!string.IsNullOrWhiteSpace(voiceName))
            {
                var grammarConcept = new ConceptCGNode();
                grammarConcept.Parent = outerConceptualGraph;
                grammarConcept.Name = voiceName;

                var grammarRelation = new RelationCGNode();
                grammarRelation.Parent = outerConceptualGraph;
                grammarRelation.Name = CGGramamaticalNamesOfRelations.VoiceName;

                conceptualGraph.AddOutputNode(grammarRelation);
                grammarRelation.AddOutputNode(grammarConcept);
            }

            var moodName = GrammaticalElementsHeper.GetMoodName(mSentence.Mood);

#if DEBUG
            LogInstance.Log($"moodName = {moodName}");
#endif

            if (!string.IsNullOrWhiteSpace(moodName))
            {
                var grammarConcept = new ConceptCGNode();
                grammarConcept.Parent = outerConceptualGraph;
                grammarConcept.Name = moodName;

                var grammarRelation = new RelationCGNode();
                grammarRelation.Parent = outerConceptualGraph;
                grammarRelation.Name = CGGramamaticalNamesOfRelations.MoodName;

                conceptualGraph.AddOutputNode(grammarRelation);
                grammarRelation.AddOutputNode(grammarConcept);
            }

            var modalName = GrammaticalElementsHeper.GetModalName(mSentence.Modal);

#if DEBUG
            LogInstance.Log($"modalName = {modalName}");
#endif

            if (!string.IsNullOrWhiteSpace(modalName))
            {
                var grammarConcept = new ConceptCGNode();
                grammarConcept.Parent = outerConceptualGraph;
                grammarConcept.Name = modalName;

                var grammarRelation = new RelationCGNode();
                grammarRelation.Parent = outerConceptualGraph;
                grammarRelation.Name = CGGramamaticalNamesOfRelations.ModalName;

                conceptualGraph.AddOutputNode(grammarRelation);
                grammarRelation.AddOutputNode(grammarConcept);
            }
        }

        private void CreateExperiencerRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = SpecialNamesOfRelations.ExperiencerRelationName;

            if (Context.RelationStorage.ContainsRelation(verbConcept.Name, nounConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            verbConcept.AddOutputNode(relation);
            relation.AddOutputNode(nounConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, nounConcept.Name, relationName);
        }

        private void CreateStateRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = SpecialNamesOfRelations.StateRelationName;

            if(Context.RelationStorage.ContainsRelation(nounConcept.Name, verbConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            nounConcept.AddOutputNode(relation);
            relation.AddOutputNode(verbConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, nounConcept.Name, relationName);
        }

        private void CreateAgentRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = SpecialNamesOfRelations.AgentRelationName;

            if (Context.RelationStorage.ContainsRelation(verbConcept.Name, nounConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            verbConcept.AddOutputNode(relation);
            relation.AddOutputNode(nounConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, nounConcept.Name, relationName);
        }

        private void CreateActionRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = SpecialNamesOfRelations.ActionRelationName;

            if (Context.RelationStorage.ContainsRelation(nounConcept.Name, verbConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            nounConcept.AddOutputNode(relation);
            relation.AddOutputNode(verbConcept);

            Context.RelationStorage.AddRelation(nounConcept.Name, verbConcept.Name, relationName);
        }
    }
}
