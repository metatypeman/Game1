using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNSentenceNode: BaseATNParsingNode
    {
        public enum State
        {
            Init
        }

        public ATNSentenceNode(ContextOfATNParsing context)
            : this(GoalOfATNExtendToken.Undefined, CompositionCommand.Undefined, context)
        {
        }

        public ATNSentenceNode(GoalOfATNExtendToken goal, CompositionCommand compositionCommand, ContextOfATNParsing context)
            : base(goal, compositionCommand, context)
        {
        }

        private State mInternalState = State.Init;

        protected override void NormalizeCompositionCommand()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        protected override void ImplementInternalState()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            switch (mInternalState)
            {
                case State.Init:
                    Context.Sentence = new Sentence();
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            LogInstance.Log("End");
#endif
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

            foreach (var clusterOfExtendedTokensWithGoalsKVPItem in clusterOfExtendedTokensWithGoals)
            {
                var extendedToken = clusterOfExtendedTokensWithGoalsKVPItem.Key;
                var goal = clusterOfExtendedTokensWithGoalsKVPItem.Value;

#if DEBUG
                LogInstance.Log($"extendedToken = {extendedToken}");
                LogInstance.Log($"goal = {goal}");

                switch (goal)
                {
                    case GoalOfATNExtendToken.NP:
                        AddTask(new ATNNPNodeFactory(extendedToken, goal));
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
                }
#endif
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        //        public Sentence Run()
        //        {
        //#if DEBUG
        //            LogInstance.Log("Begin");
        //#endif


        //            var сlusterOfExtendedTokens = Context.GetСlusterOfExtendedTokens();

        //#if DEBUG
        //            LogInstance.Log($"сlusterOfExtendedTokens.Count = {сlusterOfExtendedTokens.Count}");
        //#endif

        //            if (сlusterOfExtendedTokens.IsEmpty())
        //            {
        //                return null;
        //            }

        //            var result = new Sentence();

        //            foreach (var extendedToken in сlusterOfExtendedTokens)
        //            {
        //#if DEBUG
        //                LogInstance.Log($"extendToken = {extendedToken}");
        //#endif

        //                var goalsList = GetGoals(extendedToken);

        //#if DEBUG
        //                LogInstance.Log($"goalsList.Count = {goalsList.Count}");
        //#endif

        //                foreach(var goal in goalsList)
        //                {
        //#if DEBUG
        //                    LogInstance.Log($"goal = {goal}");
        //#endif

        //                    switch(goal)
        //                    {
        //                        case GoalOfATNExtendToken.NP:
        //                            {
        //                                var newContext = Context.Fork();
        //                                var npNode = new ATNNPNode(extendedToken, newContext);
        //                                var npPhrase = npNode.Run();
        //#if DEBUG
        //                                LogInstance.Log($"npPhrase = {npPhrase}");
        //#endif
        //                            }
        //                            break;

        //                        default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
        //                    }
        //                }
        //            }

        //#if DEBUG
        //            LogInstance.Log("End");
        //#endif
        //            return result;
        //        }
    }
}
