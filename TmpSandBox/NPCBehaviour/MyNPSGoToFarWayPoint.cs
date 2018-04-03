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
            NLog.LogManager.GetCurrentClassLogger().Info("Main");
        }
    }
}
