using MyNPCLib;
using MyNPCLib.CGStorage;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.LogicalSoundModeling;
using MyNPCLib.Parser.LogicalExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Hipster.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    public class HipsterSoundEntityConditionNPCProcess: HipsterBaseNPCProcess
    {
        public void Main(LogicalSoundInfo logicalSoundInfo)
        {
#if DEBUG
            Log($"logicalSoundInfo = {logicalSoundInfo}");
#endif

//            var myName = string.Empty;

//            {
//                var queryStr = "{: name(?X,?Y) :}";

//                var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, Context.EntityDictionary);
//                var query = queryStorage.MainRuleInstance;

//                var querySearchResultCGStorage = Context.MainCGStorage.Search(queryStorage);

//                var keyOfActionQuestionVar = Context.EntityDictionary.GetKey("?Y");

//                var actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

//#if DEBUG
//                LogInstance.Log($"actionExpression = {actionExpression}");
//#endif

//                if (actionExpression != null)
//                {
//                    myName = actionExpression?.FoundExpression?.AsConcept.Name;
//                }
//            }

//#if DEBUG
//            LogInstance.Log($"myName = {myName}");
//#endif

            var localStorage = new LocalCGStorage(Context.EntityDictionary);

            localStorage.Append(logicalSoundInfo.Storage);

            var targetName = string.Empty;

            {
                var queryStr = "{: name(?X,?Y) :}";

                var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, Context.EntityDictionary);
                var query = queryStorage.MainRuleInstance;

                var querySearchResultCGStorage = localStorage.Search(queryStorage);

                var keyOfActionQuestionVar = Context.EntityDictionary.GetKey("?Y");

                var actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

#if DEBUG
                LogInstance.Log($"actionExpression = {actionExpression}");
#endif

                if (actionExpression != null)
                {
                    targetName = actionExpression?.FoundExpression?.AsConcept.Name;
                }
            }

#if DEBUG
            LogInstance.Log($"targetName = {targetName}");
#endif

            if (BlackBoard.Name == targetName)
            {
                BlackBoard.IsReadyForsoundCommandExecuting = true;
            }
            else
            {
                BlackBoard.IsReadyForsoundCommandExecuting = false;
            }

            //            var logicalObj = Context.GetLogicalObject(logicalSoundInfo.Storage);

            //            if(logicalObj == Context.SelfLogicalObject)
            //            {
            //#if DEBUG
            //                Log("logicalObj == Context.SelfLogicalObject !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //#endif
            //            }
        }
    }
}
