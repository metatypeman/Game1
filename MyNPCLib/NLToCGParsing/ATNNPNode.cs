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
            : this(extendedToken, ATNNPNode.State.Init, goal)
        {
        }

        public ATNNPNodeFactory(ATNExtendedToken extendedToken, ATNNPNode.State internalState, GoalOfATNExtendToken goal)
            : base(extendedToken, goal)
        {
            mInternalState = internalState;
        }

        private ATNNPNode.State mInternalState = ATNNPNode.State.Init;
        public override int? InternalState => (int)mInternalState;

        public override BaseATNParsingNode Create(ContextOfATNParsing context)
        {
            var result = new ATNNPNode(ExtendedToken, mInternalState, context);
            return result;
        }
    }

    public class ATNNPNode : BaseATNParsingNode
    {
        public enum State
        {
            Init
        }

        public ATNNPNode(ATNExtendedToken extendedToken, State internalState, ContextOfATNParsing context)
            : base(context)
        {
            mTargetExtendedToken = extendedToken;
            mInternalState = internalState;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private State mInternalState = State.Init;

        protected override void NRun()
        {
#if DEBUG
            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken} mInternalState= {mInternalState}");
#endif

            ImplementInternalState();
            BornNewNodes();

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        private void ImplementInternalState()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            switch(mInternalState)
            {
                case State.Init:
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        private void BornNewNodes()
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

        //        protected override void NRun()
        //        {
        //#if DEBUG
        //            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken} mState= {mState}");
        //            var n = 0;
        //#endif


        //            while (true)
        //            {
        //                var resultOfIteration = RunItem();

        //                if (resultOfIteration == false)
        //                {
        //                    break;
        //                }

        //                var сlusterOfExtendedTokens = Context.GetСlusterOfExtendedTokens();

        //#if DEBUG
        //                LogInstance.Log($"сlusterOfExtendedTokens.Count = {сlusterOfExtendedTokens?.Count}");
        //#endif

        //                if (сlusterOfExtendedTokens.IsEmpty())
        //                {
        //                    return;
        //                }

        //                var state = Context.State;

        //                foreach (var extendedToken in сlusterOfExtendedTokens)
        //                {
        //#if DEBUG
        //                    LogInstance.Log($"extendedToken = {extendedToken}");
        //#endif

        //                    var goalsList = GetGoals(extendedToken);

        //#if DEBUG
        //                    LogInstance.Log($"goalsList.Count = {goalsList.Count}");
        //#endif

        //                    foreach (var goal in goalsList)
        //                    {
        //#if DEBUG
        //                        LogInstance.Log($"goal = {goal}");
        //#endif
        //                        switch (goal)
        //                        {
        //                            case GoalOfATNExtendToken.BaseV:
        //                                switch(state)
        //                                {
        //                                    case StateOfATNParsing.NP:
        //                                        AddTask(new ATNNP_VPNodeFactory(extendedToken, goal));
        //                                        break;

        //                                    default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
        //                                }
        //                                break;

        //                            default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
        //                        }
        //                    }
        //                }

        //#if DEBUG
        //                n++;

        //                if (n > 10)
        //                {
        //                    break;
        //                }
        //#endif
        //            }

        //#if DEBUG
        //            LogInstance.Log("End");
        //#endif
        //        }

        //        private bool RunItem()
        //        {
        //#if DEBUG
        //            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
        //#endif

        //            return true;
        //        }
        //        private bool mHasNoun;
        //        private NounPhrase mResult;

        //        public NounPhrase Run()
        //        {
        //#if DEBUG
        //            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
        //            var n = 0;
        //#endif

        //            mResult = new NounPhrase();

        //            while(true)
        //            {
        //                var resultOfIteration = RunItem();

        //                if(resultOfIteration == false)
        //                {
        //                    break;
        //                }

        //                var сlusterOfExtendedTokens = Context.GetСlusterOfExtendedTokens();
        //#if DEBUG
        //                LogInstance.Log($"сlusterOfExtendedTokens.Count = {сlusterOfExtendedTokens?.Count}");
        //#endif

        //                if(сlusterOfExtendedTokens.IsEmpty())
        //                {
        //                    return NTakeResult();
        //                }
        //#if DEBUG

        //                foreach (var extendedToken in сlusterOfExtendedTokens)
        //                {
        //                    LogInstance.Log($"extendToken = {extendedToken}");
        //                }
        //#endif

        //                if(сlusterOfExtendedTokens.Count != 1)
        //                {
        //                    throw new NotImplementedException();
        //                }

        //                mTargetExtendedToken = сlusterOfExtendedTokens.Single();

        //                var goalsList = GetGoals(mTargetExtendedToken);

        //#if DEBUG
        //                LogInstance.Log($"goalsList.Count = {goalsList.Count}");
        //                foreach (var goal in goalsList)
        //                {
        //                    LogInstance.Log($"goal = {goal}");
        //                }
        //#endif

        //                if(!goalsList.Contains( GoalOfATNExtendToken.NP))
        //                {
        //                    return NTakeResult();
        //                }

        //                n++;

        //                if (n > 10)
        //                {
        //                    break;
        //                }
        //            }

        //#if DEBUG
        //            LogInstance.Log("End");
        //#endif
        //            return NTakeResult();
        //        }

        //        private NounPhrase NTakeResult()
        //        {
        //            return mResult;
        //        }

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
