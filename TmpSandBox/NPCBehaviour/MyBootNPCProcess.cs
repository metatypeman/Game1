using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class MyBootNPCProcess: BaseNPCProcess
    {
        private void Main()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Main");
        }
    }
}
