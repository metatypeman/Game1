using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    [NPCProcessStartupMode(NPCProcessStartupMode.NewStandaloneInstance)]
    [NPCProcessName("go to far waypoint")]
    public class MyNPSGoToFarWayPoint : BaseNPCProcess
    {
        private void Main()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin Main");

            var bodyCommand = new HumanoidHStateCommand();
            bodyCommand.State = HumanoidHState.Run;

            var process = Context.Body.Send(bodyCommand.ToNPCCommand());

            NLog.LogManager.GetCurrentClassLogger().Info("End Main");
        }
    }
}
