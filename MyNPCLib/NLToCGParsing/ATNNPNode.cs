using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNNPNodeFactory: BaseATNNodeFactory
    {
        public ATNNPNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal)
            : this(extendedToken, ATNNPNode.State.Init, goal, CompositionCommand.Undefined)
        {
        }

        public ATNNPNodeFactory(ATNExtendedToken extendedToken, ATNNPNode.State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand)
            : base(extendedToken, goal, compositionCommand)
        {
            mInternalState = internalState;
        }

        private ATNNPNode.State mInternalState = ATNNPNode.State.Init;
        
        public override BaseATNParsingNode Create(ContextOfATNParsing context)
        {
            var result = new ATNNPNode(ExtendedToken, mInternalState, Goal, CompositionCommand, context);
            return result;
        }
    }

    public class ATNNPNode : BaseATNParsingNode
    {
        public enum State
        {
            Init,
            Noun
        }

        public enum SubGoal
        {
            Undefined,
            Noun
        }

        public ATNNPNode(ATNExtendedToken extendedToken, State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand, ContextOfATNParsing context)
            : base(goal, compositionCommand, context)
        {
            mTargetExtendedToken = extendedToken;
            mInternalState = internalState;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private State mInternalState = State.Init;
        private NounPhrase mNounPhrase;

        protected override void NormalizeCompositionCommand()
        {
#if DEBUG
            LogInstance.Log($"CompositionCommand= {CompositionCommand}");
#endif

            switch (mInternalState)
            {
                case State.Init:
                    switch (CompositionCommand)
                    {
                        case CompositionCommand.Undefined:
                            CompositionCommand = CompositionCommand.AddToNounPhraseOfSentence;
                            break;
                    }
                    break;
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        protected override void ImplementInternalState()
        {
#if DEBUG
            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
            LogInstance.Log($"mInternalState = {mInternalState}");
            LogInstance.Log($"CompositionCommand = {CompositionCommand}");
#endif

            switch (mInternalState)
            {
                case State.Init:
                    mNounPhrase = new NounPhrase();
                    Context.AddNounPhrase(mNounPhrase);
                    switch(CompositionCommand)
                    {
                        case CompositionCommand.AddToNounPhraseOfSentence:
                            Context.Sentence.NounPhrase = mNounPhrase;
                            break;
                    }
                    var subGoalsList = GetSubGoals();

                    if (subGoalsList.Count != 1)
                    {
                        throw new NotImplementedException();
                    }

                    var subGoal = subGoalsList.First();

                    switch(subGoal)
                    {
                        case SubGoal.Noun:
                            mNounPhrase.Noun = mTargetExtendedToken;
                            mInternalState = State.Noun;
                            break;
                    }

                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        private List<SubGoal> GetSubGoals()
        {
            var result = new List<SubGoal>();

            if (mTargetExtendedToken.IsDeterminer)
            {
                throw new NotImplementedException();
            }
            else
            {
                var partOfSpeech = mTargetExtendedToken.PartOfSpeech;

                switch (partOfSpeech)
                {
                    case GrammaticalPartOfSpeech.Noun:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Pronoun:
                        {
                            var person = mTargetExtendedToken.Person;
                            switch (person)
                            {
                                case GrammaticalPerson.First:
                                    result.Add(SubGoal.Noun);
                                    break;
                            }
                        }
                        break;

                    case GrammaticalPartOfSpeech.Adjective:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Verb:
                        throw new NotImplementedException();

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
            }
            return result;
        }

        protected override void BornNewNodes()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            var clusterOfExtendedTokensWithGoals = GetСlusterOfExtendedTokensWithGoals();

#if DEBUG
            LogInstance.Log($"clusterOfExtendedTokensWithGoals.Count = {clusterOfExtendedTokensWithGoals?.Count}");
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
                LogInstance.Log($"extendedToken = {extendedToken}");
                LogInstance.Log($"goal = {goal}");
#endif

                switch (goal)
                {
                    case GoalOfATNExtendToken.BaseV:
                        switch (state)
                        {
                            case StateOfATNParsing.NP:
                                AddTask(new ATNNP_VPNodeFactory(extendedToken, goal));
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
                }
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        //        private bool RunItem()
        //        {
        //#if DEBUG
        //            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
        //#endif

        //            if (mTargetExtendedToken.IsDeterminer)
        //            {
        //                throw new NotImplementedException();
        //            }
        //            else
        //            {
        //                var partOfSpeech = mTargetExtendedToken.PartOfSpeech;

        //                switch (partOfSpeech)
        //                {
        //                    case GrammaticalPartOfSpeech.Noun:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Pronoun:
        //                        {
        //                            var person = mTargetExtendedToken.Person;
        //                            switch(person)
        //                            {
        //                                case GrammaticalPerson.First:
        //                                    mResult.Noun = mTargetExtendedToken;
        //                                    mHasNoun = true;
        //                                    return true;
        //                            }
        //                            throw new NotImplementedException();
        //                        }
        //                        break;

        //                    case GrammaticalPartOfSpeech.Adjective:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Verb:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Adverb:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Preposition:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Conjunction:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Interjection:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Article:
        //                        throw new NotImplementedException();

        //                    case GrammaticalPartOfSpeech.Numeral:
        //                        throw new NotImplementedException();

        //                    default: throw new ArgumentOutOfRangeException(nameof(partOfSpeech), partOfSpeech, null);
        //                }
        //            }
        //        }
    }
}
