using MyNPCLib.NLToCGParsing.PhraseTree;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNAPNodeFactory : BaseATNNodeFactory
    {
        public ATNAPNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal)
            : this(extendedToken, ATNAPNode.State.Init, goal, CompositionCommand.Undefined)
        {
        }

        public ATNAPNodeFactory(ATNExtendedToken extendedToken, ATNAPNode.State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand)
            : base(extendedToken, goal, compositionCommand)
        {
            mInternalState = internalState;
        }

        private ATNAPNode.State mInternalState = ATNAPNode.State.Init;

        public override BaseATNParsingNode Create(ContextOfATNParsing context)
        {
            var result = new ATNAPNode(ExtendedToken, mInternalState, Goal, CompositionCommand, context);
            return result;
        }
    }

    public class ATNAPNode : BaseATNParsingNode
    {
        public enum State
        {
            Init,
            Adjective
        }

        public enum SubGoal
        {
            Undefined,
            Adjective
        }

        public ATNAPNode(ATNExtendedToken extendedToken, State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand, ContextOfATNParsing context)
            : base(goal, compositionCommand, context)
        {
            mTargetExtendedToken = extendedToken;
            mInternalState = internalState;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private State mInternalState = State.Init;
        private AdjectivePhrase mAdjectivePhrase;

        protected override void NormalizeCompositionCommand()
        {
#if DEBUG
            //LogInstance.Log($"CompositionCommand = {CompositionCommand}");
#endif

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
                        mAdjectivePhrase = new AdjectivePhrase();
                        
                        switch (CompositionCommand)
                        {
                            case CompositionCommand.AddToNounPhraseOfSentence:
                                Context.Sentence.NounPhrase = mAdjectivePhrase;
                                break;

                            case CompositionCommand.AddToObjectOfNounLikePhrase:
                                {
                                    var tmpPhrase = Context.PeekCurrentNounPhrase();
                                    tmpPhrase.Object = mAdjectivePhrase;

#if DEBUG
                                    //LogInstance.Log($"tmpPhrase = {tmpPhrase}");
#endif
                                    //throw new NotImplementedException();
                                }
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                        }
                        Context.AddNounLikePhrase(mAdjectivePhrase);

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
                                case SubGoal.Adjective:
                                    AddTask(new ATNAPNodeFactory(mTargetExtendedToken, State.Adjective, Goal, CompositionCommand.AddToObjectOfNounLikePhrase));
                                    break;

                                default: throw new ArgumentOutOfRangeException(nameof(subGoal), subGoal, null);
                            }
                        }
                    }
                    break;

                case State.Adjective:
                    switch (CompositionCommand)
                    {
                        case CompositionCommand.AddToObjectOfNounLikePhrase:
                            mAdjectivePhrase = Context.PeekCurrentNounPhrase().AsAdjectivePhrase;
                            mAdjectivePhrase.Adjective = mTargetExtendedToken;

#if DEBUG
                            //LogInstance.Log($"mAdjectivePhrase = {mAdjectivePhrase}");
#endif

                            //throw new NotImplementedException();

                            mInternalState = State.Adjective;
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(CompositionCommand), CompositionCommand, null);
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            //LogInstance.Log($"mAdjectivePhrase = {mAdjectivePhrase}");
            //LogInstance.Log("End");
#endif
        }

        private List<SubGoal> GetSubGoals(ATNExtendedToken extendedToken)
        {
            var result = new List<SubGoal>();

            var partOfSpeech = extendedToken.PartOfSpeech;

            switch (partOfSpeech)
            {
                case GrammaticalPartOfSpeech.Adjective:
                    result.Add(SubGoal.Adjective);
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
                    case GoalOfATNExtendToken.NP:
                        AddTask(new ATNNPNodeFactory(extendedToken, ATNNPNode.State.Init, goal, CompositionCommand.AddToObjectOfNounLikePhrase));
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
