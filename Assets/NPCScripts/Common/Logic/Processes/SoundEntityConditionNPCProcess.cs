//using MyNPCLib;
//using MyNPCLib.CGStorage;
//using MyNPCLib.LogicalSoundModeling;
//using MyNPCLib.Parser.LogicalExpression;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.NPCScripts.Common.Logic.Processes
//{
//    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
//    public class SoundEntityConditionNPCProcess : CommonBaseNPCProcess
//    {
//        public void Main(LogicalSoundInfo logicalSoundInfo)
//        {
//#if DEBUG
//            Log($"logicalSoundInfo = {logicalSoundInfo}");
//#endif

//            var localStorage = new LocalCGStorage(Context.EntityDictionary);

//            localStorage.Append(logicalSoundInfo.Storage);

//            var queryStr = "{: name(?X,?Y) :}";

//            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, Context.EntityDictionary);
//            var query = queryStorage.MainRuleInstance;
//            var keyOfActionQuestionVar = Context.EntityDictionary.GetKey("?Y");

//            var targetName = string.Empty;

//            {
//                var querySearchResultCGStorage = localStorage.Search(queryStorage);

//                var actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

//#if DEBUG
//                LogInstance.Log($"actionExpression = {actionExpression}");
//#endif

//                if (actionExpression != null)
//                {
//                    targetName = actionExpression?.FoundExpression?.AsConcept.Name;
//                }
//            }

//#if DEBUG
//            LogInstance.Log($"targetName = {targetName}");
//#endif

//            if (BlackBoard.Name == targetName)
//            {
//                BlackBoard.IsReadyForsoundCommandExecuting = true;
//            }
//            else
//            {
//                BlackBoard.IsReadyForsoundCommandExecuting = false;
//            }
//        }
//    }
//}
