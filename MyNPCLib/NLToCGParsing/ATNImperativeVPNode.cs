using MyNPCLib.NLToCGParsing.PhraseTree;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNImperativeVPNodeFactory : BaseATNNodeFactory
    {
        public ATNImperativeVPNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal)
            : this(extendedToken, ATNImperativeVPNode.State.Init, goal, CompositionCommand.Undefined)
        {
        }

        public ATNImperativeVPNodeFactory(ATNExtendedToken extendedToken, ATNImperativeVPNode.State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand)
            : base(extendedToken, goal, compositionCommand)
        {
            mInternalState = internalState;
        }

        private ATNImperativeVPNode.State mInternalState = ATNImperativeVPNode.State.Init;

        public override BaseATNParsingNode Create(ContextOfATNParsing context)
        {
            var result = new ATNImperativeVPNode(ExtendedToken, mInternalState, Goal, CompositionCommand, context);
            return result;
        }
    }

    public class ATNImperativeVPNode : BaseATNParsingNode
    {
        public enum State
        {
            Init,
            Verb
        }

        public enum SubGoal
        {
            Init,
            Verb
        }

        public ATNImperativeVPNode(ATNExtendedToken extendedToken, State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand, ContextOfATNParsing context)
            : base(goal, compositionCommand, context)
        {
            mTargetExtendedToken = extendedToken;
            mInternalState = internalState;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private State mInternalState = State.Init;
        private VerbPhrase mVerbPhrase;

        protected override void NormalizeCompositionCommand()
        {
#if DEBUG
            //LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
            //LogInstance.Log($"mInternalState = {mInternalState}");
            //LogInstance.Log($"CompositionCommand = {CompositionCommand}");
#endif

            switch (mInternalState)
            {
                case State.Init:
                    CompositionCommand = CompositionCommand.AddToVerbPhraseOfSentence;
                    break;

                case State.Verb:
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            //LogInstance.Log("End");
#endif
        }

        protected override void ImplementInternalState()
        {
#if DEBUG
            //LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
            //LogInstance.Log($"mInternalState = {mInternalState}");
            //LogInstance.Log($"CompositionCommand = {CompositionCommand}");
#endif

            switch (mInternalState)
            {
                case State.Init:
                    {
                        SuppressBornNewNodes = true;
                        mVerbPhrase = new VerbPhrase();
                        
                        switch (CompositionCommand)
                        {
                            case CompositionCommand.AddToVerbPhraseOfSentence:
                                {
                                    var sentence = Context.Sentence;
                                    sentence.VerbPhrase = mVerbPhrase;
                                    sentence.Aspect = GrammaticalAspect.Simple;
                                    sentence.Voice = GrammaticalVoice.Active;
                                    sentence.Mood = GrammaticalMood.Imperative;
                                    sentence.Modal = KindOfModal.None;
                                }
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                        }

                        Context.AddVerbPhrase(mVerbPhrase);

                        var subGoalsList = GetSubGoals(mTargetExtendedToken);

#if DEBUG
                        //LogInstance.Log($"subGoalsList.Count = {subGoalsList.Count}");
#endif

                        foreach (var subGoal in subGoalsList)
                        {
#if DEBUG
                            //LogInstance.Log($"subGoal = {subGoal}");
#endif

                            switch (subGoal)
                            {
                                case SubGoal.Verb:
                                    AddTask(new ATNImperativeVPNodeFactory(mTargetExtendedToken, State.Verb, Goal, CompositionCommand.PutVerbInVP));
                                    break;

                                default: throw new ArgumentOutOfRangeException(nameof(subGoal), subGoal, null);
                            }
                        }
                    }
                    break;

                case State.Verb:
                    {
                        var sentence = Context.Sentence;
                        mVerbPhrase = Context.PeekCurrentVerbPhrase();
                        mVerbPhrase.Verb = mTargetExtendedToken;
                        mInternalState = State.Verb;

                        switch (Goal)
                        {
                            case GoalOfATNExtendToken.BaseV:
                                sentence.Tense = GrammaticalTenses.Present;
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(Goal), Goal, null);
                        }
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            //LogInstance.Log("End");
#endif
        }

        private List<SubGoal> GetSubGoals(ATNExtendedToken extendedToken)
        {
            var result = new List<SubGoal>();
            var partOfSpeech = extendedToken.PartOfSpeech;

            switch (partOfSpeech)
            {
                case GrammaticalPartOfSpeech.Noun:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Pronoun:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Adjective:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Verb:
                    switch (Goal)
                    {
                        case GoalOfATNExtendToken.BaseV:
                            result.Add(SubGoal.Verb);
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(Goal), Goal, null);
                    }
                    break;

                case GrammaticalPartOfSpeech.Adverb:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Preposition:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Conjunction:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Interjection:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Article:
                    throw new NotImplementedException();

                case GrammaticalPartOfSpeech.Numeral:
                    throw new NotImplementedException();

                default: throw new ArgumentOutOfRangeException(nameof(partOfSpeech), partOfSpeech, null);
            }

            return result;
        }

        protected override void BornNewNodes()
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif

            var clusterOfExtendedTokensWithGoals = GetСlusterOfExtendedTokensWithGoals();

#if DEBUG
            //LogInstance.Log($"clusterOfExtendedTokensWithGoals.Count = {clusterOfExtendedTokensWithGoals?.Count}");
#endif

            if (clusterOfExtendedTokensWithGoals.IsEmpty())
            {
                return;
            }

            var state = Context.State;

            foreach (var clusterOfExtendedTokensWithGoalsKVPItem in clusterOfExtendedTokensWithGoals)
            {
                var extendedToken = clusterOfExtendedTokensWithGoalsKVPItem.Key;
                var goal = clusterOfExtendedTokensWithGoalsKVPItem.Value;

#if DEBUG
                //LogInstance.Log($"extendedToken = {extendedToken}");
                //LogInstance.Log($"goal = {goal}");
#endif

                switch (goal)
                {
                    case GoalOfATNExtendToken.PP:
                        AddTask(new ATNPPNodeFactory(extendedToken, ATNPPNode.State.Init, goal, CompositionCommand.AddToObjectOfVerbPhrase));
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
                }
            }

#if DEBUG
            //LogInstance.Log("End");
#endif
        }
    }
}
