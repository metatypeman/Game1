using MyNPCLib.CG;
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
                grammarRelation.Name = "__aspect";

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
                grammarRelation.Name = "__tense";

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
                grammarRelation.Name = "__voice";

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
                grammarRelation.Name = "__mood";

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
                grammarRelation.Name = "__modal";

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
                case GrammaticalAspect.Simple:
                case GrammaticalAspect.Continuous,
                case GrammaticalAspect.Perfect,
                case GrammaticalAspect.PerfectContinuous

                default: throw new ArgumentOutOfRangeException(nameof(aspect), aspect, null);
            }
        }

        private string GetTenseName()
        {
            var tense = mSentence.Tense;

            switch (tense)
            {
                     Undefined,
        All,
        Present,
        Past,
        Future,
        FutureInThePast

                default: throw new ArgumentOutOfRangeException(nameof(tense), tense, null);
            }
        }

        private string GetVoiceName()
        {
            var voice = mSentence.Voice;

            switch (voice)
            {
                                    Undefined,
        Active,
        Passive

                default: throw new ArgumentOutOfRangeException(nameof(voice), voice, null);
            }
        }

        private string GetMoodName()
        {
            var mood = mSentence.Mood;

            switch (mood)
            {
                                    Undefined,
        Indicative,
        Subjunctive,
        Imperative,
        Jussive,
        Potential,
        Hypothetical,
        Hortative,
        Optative

                default: throw new ArgumentOutOfRangeException(nameof(mood), mood, null);
            }
        }

        private string GetModalName()
        {
            var modal = mSentence.Modal;
            switch (modal)
            {
                                    Undefined,
        None,
        Must,
        Can,
        May

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
