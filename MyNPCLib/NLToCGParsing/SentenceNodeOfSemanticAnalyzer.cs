using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class SentenceNodeOfSemanticAnalyzer: BaseNodeOfSemanticAnalyzer
    {
        public SentenceNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, Sentence sentence)
            : base(context)
        {         
            mSentence = sentence;
        }
    
        private Sentence mSentence;
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

            if (mSentence.NounPhrase != null)
            {
                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, mSentence.NounPhrase);
                var nounResult = nounPhraseNode.Run();

#if DEBUG
                LogInstance.Log($"nounResult = {nounResult}");
#endif

                PrimaryRolesDict.Assing(nounResult.PrimaryRolesDict);
            }

            if (mSentence.VerbPhrase != null)
            {
                var verbPhraseNode = new VerbPhraseNodeOfSemanticAnalyzer(Context, mSentence.VerbPhrase);
                var verbResult = verbPhraseNode.Run();

#if DEBUG
                LogInstance.Log($"verbResult = {verbResult}");
#endif

                PrimaryRolesDict.Assing(verbResult.PrimaryRolesDict);
            }

#if DEBUG
            LogInstance.Log($"PrimaryRolesDict = {PrimaryRolesDict}");
#endif

            var primaryAnimatesList = PrimaryRolesDict.GetByRole("animate");

            if(!primaryAnimatesList.IsEmpty())
            {
                //state -> experiencer -> animate

                var primaryStatesList = PrimaryRolesDict.GetByRole("state");

                if (!primaryStatesList.IsEmpty())
                {
                    foreach(var state in primaryStatesList)
                    {
                        foreach(var animate in primaryAnimatesList)
                        {
                            CreateExperiencerRelation(state, animate);
                        }
                    }
                }
                //act -> agent -> animate

                var primaryActsList = PrimaryRolesDict.GetByRole("act");

                if(!primaryActsList.IsEmpty())
                {
                    foreach(var act in primaryActsList)
                    {
                        foreach (var animate in primaryAnimatesList)
                        {
                            CreateAgentRelation(act, animate);
                            CreateActionRelation(act, animate);
                        }
                    }
                }
            }

#if DEBUG
            LogInstance.Log("End");
#endif

            return result;
        }

        private void CreateGrammaticalRelations()
        {
            var conceptualGraph = Context.ConceptualGraph;
            var outerConceptualGraph = Context.OuterConceptualGraph;

            var aspectName = GetAspectName();

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

            var tenseName = GetTenseName();

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

            var voiceName = GetVoiceName();

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

            var moodName = GetMoodName();

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

            var modalName = GetModalName();

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

        private string GetAspectName()
        {
            var aspect = mSentence.Aspect;

            switch(aspect)
            {
                case GrammaticalAspect.Undefined:
                    return string.Empty;

                case GrammaticalAspect.Simple:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_Simple;

                case GrammaticalAspect.Continuous:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_Continuous;

                case GrammaticalAspect.Perfect:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_Perfect;

                case GrammaticalAspect.PerfectContinuous:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_PerfectContinuous;

                default: throw new ArgumentOutOfRangeException(nameof(aspect), aspect, null);
            }
        }

        private string GetTenseName()
        {
            var tense = mSentence.Tense;

            switch (tense)
            {
                case GrammaticalTenses.Undefined:
                    return string.Empty;

                case GrammaticalTenses.All:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_All;

                case GrammaticalTenses.Present:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_Present;

                case GrammaticalTenses.Past:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_Past;

                case GrammaticalTenses.Future:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_Future;

                case GrammaticalTenses.FutureInThePast:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_FutureInThePast;

                default: throw new ArgumentOutOfRangeException(nameof(tense), tense, null);
            }
        }

        private string GetVoiceName()
        {
            var voice = mSentence.Voice;

            switch (voice)
            {
                case GrammaticalVoice.Undefined:
                    return string.Empty;

                case GrammaticalVoice.Active:
                    return CGGramamaticalNamesOfConcepts.GrammaticalVoice_Active;

                case GrammaticalVoice.Passive:
                    return CGGramamaticalNamesOfConcepts.GrammaticalVoice_Passive;

                default: throw new ArgumentOutOfRangeException(nameof(voice), voice, null);
            }
        }

        private string GetMoodName()
        {
            var mood = mSentence.Mood;

            switch (mood)
            {
                case GrammaticalMood.Undefined:
                    return string.Empty;

                case GrammaticalMood.Indicative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Indicative;

                case GrammaticalMood.Subjunctive:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Subjunctive;

                case GrammaticalMood.Imperative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Imperative;

                case GrammaticalMood.Jussive:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Jussive;

                case GrammaticalMood.Potential:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Potential;

                case GrammaticalMood.Hypothetical:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Hypothetical;

                case GrammaticalMood.Hortative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Hortative;

                case GrammaticalMood.Optative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Optative;

                default: throw new ArgumentOutOfRangeException(nameof(mood), mood, null);
            }
        }

        private string GetModalName()
        {
            var modal = mSentence.Modal;
            switch (modal)
            {
                case KindOfModal.Undefined:
                    return string.Empty;

                case KindOfModal.None:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_None;

                case KindOfModal.Must:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_Must;

                case KindOfModal.Can:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_Can;

                case KindOfModal.May:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_May;

                default: throw new ArgumentOutOfRangeException(nameof(modal), modal, null);
            }
        }

        private void CreateExperiencerRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = "experiencer";

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

        private void CreateAgentRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = "agent";

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
            var relationName = "action";

            if (Context.RelationStorage.ContainsRelation(verbConcept.Name, nounConcept.Name, relationName))
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
    }
}
