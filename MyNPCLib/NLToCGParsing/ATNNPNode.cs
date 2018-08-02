using MyNPCLib.NLToCGParsing.PhraseTree;
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
            Noun,
            Determiner
        }

        public enum SubGoal
        {
            Undefined,
            Noun,
            Determiner
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
            //LogInstance.Log($"CompositionCommand = {CompositionCommand}");
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
                        mNounPhrase = new NounPhrase();
                        
                        switch (CompositionCommand)
                        {
                            case CompositionCommand.AddToNounPhraseOfSentence:
                                Context.Sentence.NounPhrase = mNounPhrase;
                                break;

                            case CompositionCommand.AddToObjectOfVerbPhrase:
                                {
                                    var tmpVP = Context.PeekCurrentVerbPhrase();
                                    tmpVP.Object = mNounPhrase;
                                }              
                                break;

                            case CompositionCommand.AddToObjectOfNounLikePhrase:
                                {
                                    var tmpPhrase = Context.PeekCurrentNounPhrase();
                                    tmpPhrase.Object = mNounPhrase;
                                }
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                        }

                        Context.AddNounLikePhrase(mNounPhrase);

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
                                case SubGoal.Noun:
                                    AddTask(new ATNNPNodeFactory(mTargetExtendedToken, State.Noun, Goal, CompositionCommand.PutNounInNP));
                                    break;

                                case SubGoal.Determiner:
                                    AddTask(new ATNNPNodeFactory(mTargetExtendedToken, State.Noun, Goal, CompositionCommand.PutDeterminerInNP));
                                    break;

                                default: throw new ArgumentOutOfRangeException(nameof(subGoal), subGoal, null);
                            }
                        }
                    }
                    break;

                case State.Noun:
                    switch (CompositionCommand)
                    {
                        case CompositionCommand.PutNounInNP:
                            mNounPhrase = Context.PeekCurrentNounPhrase().AsNounPhrase;
                            mNounPhrase.Noun = mTargetExtendedToken;
                            mInternalState = State.Noun;
                            break;

                        case CompositionCommand.PutDeterminerInNP:
                            mNounPhrase = Context.PeekCurrentNounPhrase().AsNounPhrase;
                            mNounPhrase.Determiners.Add(mTargetExtendedToken);
                            mInternalState = State.Determiner;
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                    }
                    break;

                case State.Determiner:
                    switch (CompositionCommand)
                    {
                        case CompositionCommand.PutNounInNP:
                            mNounPhrase = Context.PeekCurrentNounPhrase().AsNounPhrase;
                            mNounPhrase.Noun = mTargetExtendedToken;
                            mInternalState = State.Noun;
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            //LogInstance.Log($"mNounPhrase = {mNounPhrase}");
            //LogInstance.Log("End");
#endif
        }

        private List<SubGoal> GetSubGoals(ATNExtendedToken extendedToken)
        {
            var result = new List<SubGoal>();

            if (extendedToken.IsDeterminer)
            {
                result.Add(SubGoal.Determiner);
            }
            else
            {
                var partOfSpeech = extendedToken.PartOfSpeech;

                switch (partOfSpeech)
                {
                    case GrammaticalPartOfSpeech.Noun:
                        if(extendedToken.IsPossessive)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            result.Add(SubGoal.Noun);
                        }                      
                        break;

                    case GrammaticalPartOfSpeech.Pronoun:
                        {
                            var person = extendedToken.Person;
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
            //LogInstance.Log("Begin");
#endif

            var clusterOfExtendedTokensWithGoals = GetСlusterOfExtendedTokensWithGoals();

#if DEBUG
            //LogInstance.Log($"clusterOfExtendedTokensWithGoals.Count = {clusterOfExtendedTokensWithGoals?.Count}");
#endif

            if (clusterOfExtendedTokensWithGoals.IsEmpty())
            {
                PutSentenceToResult();
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
                    case GoalOfATNExtendToken.BaseV:
                        switch (state)
                        {
                            case StateOfATNParsing.NP:
                                AddTask(new ATNNP_VPNodeFactory(extendedToken, goal));
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
                        }
                        break;

                    case GoalOfATNExtendToken.NP:
                        {
                            var subGoalsList = GetSubGoals(extendedToken);

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
                                    case SubGoal.Noun:
                                        AddTask(new ATNNPNodeFactory(extendedToken, mInternalState, goal, CompositionCommand.PutNounInNP));
                                        break;

                                    default: throw new ArgumentOutOfRangeException(nameof(subGoal), subGoal, null);
                                }
                            }
                        }
                        break;

                    case GoalOfATNExtendToken.Point:
                        PutSentenceToResult();
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
