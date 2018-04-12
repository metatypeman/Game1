using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewStandaloneInstance)]
    [NPCProcessName("go to far waypoint")]
    public class MyNPSGoToFarWayPoint : BaseNPCProcessWithBlackBoard<MyBlackBoard>
    {
        private void Main()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin Main");
            NLog.LogManager.GetCurrentClassLogger().Info($"Main BlackBoard.TstValue = {BlackBoard.TstValue}");

            var bodyCommand = new HumanoidHStateCommand();
            bodyCommand.State = HumanoidHState.Run;

            var process = ExecuteBody(bodyCommand);

            Wait(process);

            var handCommand = new NPCCommand();
            handCommand.Name = "fire";

            process = ExecuteDefaultHand(handCommand);

            Wait(process);

            NLog.LogManager.GetCurrentClassLogger().Info("End Main");
        }
    }
}
