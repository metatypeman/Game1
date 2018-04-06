using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class MyBootNPCProcess: BaseNPCProcessWithBlackBoard<MyBlackBoard>
    {
        private void Main()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin Main");

            NLog.LogManager.GetCurrentClassLogger().Info($"Main BlackBoard.TstValue = {BlackBoard.TstValue}");

            BlackBoard.TstValue = 12;

            var command = new NPCCommand();
            command.Name = "go to far waypoint";

            var childProcess = ExecuteAsChild(command);

            Wait(childProcess);

            NLog.LogManager.GetCurrentClassLogger().Info("End Main");
        }
    }
}
