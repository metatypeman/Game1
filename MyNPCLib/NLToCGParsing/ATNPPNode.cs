using MyNPCLib.NLToCGParsing.PhraseTree;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNPPNodeFactory : BaseATNNodeFactory
    {
        public ATNPPNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal)
            : this(extendedToken, ATNPPNode.State.Init, goal, CompositionCommand.Undefined)
        {
        }

        public ATNPPNodeFactory(ATNExtendedToken extendedToken, ATNPPNode.State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand)
            : base(extendedToken, goal, compositionCommand)
        {
            mInternalState = internalState;
        }

        private ATNPPNode.State mInternalState = ATNPPNode.State.Init;

        public override BaseATNParsingNode Create(ContextOfATNParsing context)
        {
            var result = new ATNPPNode(ExtendedToken, mInternalState, Goal, CompositionCommand, context);
            return result;
        }
    }

    public class ATNPPNode : BaseATNParsingNode
    {
        public enum State
        {
            Init,
            Preposition
        }

        public enum SubGoal
        {
            Undefined,
            Preposition
        }

        public ATNPPNode(ATNExtendedToken extendedToken, State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand, ContextOfATNParsing context)
            : base(goal, compositionCommand, context)
        {
            mTargetExtendedToken = extendedToken;
            mInternalState = internalState;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private State mInternalState = State.Init;
        private PrepositionalPhrase mPrepositionalPhrase;

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
                        mPrepositionalPhrase = new PrepositionalPhrase();
                        
                        switch (CompositionCommand)
                        {
                            case CompositionCommand.AddToNounPhraseOfSentence:
                                Context.Sentence.NounPhrase = mPrepositionalPhrase;
                                break;

                            case CompositionCommand.AddToObjectOfVerbPhrase:
                                {
                                    var tmpVP = Context.PeekCurrentVerbPhrase();
                                    tmpVP.Object = mPrepositionalPhrase;
                                }
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                        }

                        Context.AddNounLikePhrase(mPrepositionalPhrase);

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
                                case SubGoal.Preposition:
                                    AddTask(new ATNPPNodeFactory(mTargetExtendedToken, State.Preposition, Goal, CompositionCommand.PutNounInNP));
                                    break;

                                default: throw new ArgumentOutOfRangeException(nameof(subGoal), subGoal, null);
                            }
                        }
                    }
                    break;

                case State.Preposition:
                    switch (CompositionCommand)
                    {
                        case CompositionCommand.PutNounInNP:
                            mPrepositionalPhrase = Context.PeekCurrentNounPhrase().AsPrepositionalPhrase;
                            mPrepositionalPhrase.Preposition = mTargetExtendedToken;
                            mInternalState = State.Preposition;
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            //LogInstance.Log($"mPrepositionalPhrase = {mPrepositionalPhrase}");
            //LogInstance.Log("End");
#endif
        }

        private List<SubGoal> GetSubGoals(ATNExtendedToken extendedToken)
        {
            var result = new List<SubGoal>();

            var partOfSpeech = extendedToken.PartOfSpeech;

            switch (partOfSpeech)
            {
                case GrammaticalPartOfSpeech.Preposition:
                    result.Add(SubGoal.Preposition);
                    break;

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
                    case GoalOfATNExtendToken.AP:
                        AddTask(new ATNAPNodeFactory(extendedToken, ATNAPNode.State.Init, goal, CompositionCommand.AddToObjectOfNounLikePhrase));
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
