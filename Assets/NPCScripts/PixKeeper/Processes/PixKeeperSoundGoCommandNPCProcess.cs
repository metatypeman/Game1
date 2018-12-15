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

namespace Assets.NPCScripts.PixKeeper.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    public class PixKeeperSoundGoCommandNPCProcess : PixKeeperBaseNPCProcess
    {
        public void Main(LogicalSoundInfo logicalSoundInfo)
        {
#if DEBUG
            Log($"logicalSoundInfo = {logicalSoundInfo}");
#endif

            if (!BlackBoard.IsReadyForsoundCommandExecuting)
            {
                return;
            }

            Log($"NEXT logicalSoundInfo = {logicalSoundInfo}");

            var localStorage = new LocalCGStorage(Context.EntityDictionary);

            localStorage.Append(logicalSoundInfo.Storage);

            var actionName = logicalSoundInfo.ActionName;

#if DEBUG
            Log($"actionName = {actionName}");
#endif

            var queryStr = "{: direction(go,?X) :}";

            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, Context.EntityDictionary);
            var query = queryStorage.MainRuleInstance;

#if DEBUG
            {
                var debugStr = DebugHelperForRuleInstance.ToString(query);

                Log($"debugStr (for going) = {debugStr}");
            }
#endif
            var querySearchResultCGStorage = localStorage.Search(queryStorage);

            var varNameOfDirection = "?X";

            var targetValueOfDirection = querySearchResultCGStorage.GetResultOfVarAsVariant(varNameOfDirection);

#if DEBUG
            LogInstance.Log($"targetValueOfDirection = {targetValueOfDirection}");
#endif

            var targetObject = Context.GetLogicalObject(targetValueOfDirection);
            var targetPosition = targetObject.GetValue<System.Numerics.Vector3?>("global position");

#if DEBUG
            Log($"targetPosition = {targetPosition}");
#endif

            var moveCommand = new HumanoidHStateCommand();
            moveCommand.State = HumanoidHState.Walk;
            moveCommand.TargetPosition = targetPosition;

            BlackBoard.LastCommand = moveCommand;

#if UNITY_EDITOR
            Log($"moveCommand = {moveCommand}");
#endif

            /*var childProcess = */
            ExecuteBody(moveCommand);
            //Wait(childProcess);
        }
    }
}
