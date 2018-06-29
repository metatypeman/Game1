using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNSentenceNode: BaseATNParsingNode
    {
        public ATNSentenceNode(ContextOfATNParsing context)
            : base(context)
        {
        }

        public void Run()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif



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
