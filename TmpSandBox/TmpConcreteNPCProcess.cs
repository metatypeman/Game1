using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    public class TmpConcreteNPCProcess: BaseNPCProcess
    {
        protected override void FillProcessInfo(NPCProcessInfo processInfo)
        {
#if DEBUG
            NLog.LogManager.GetCurrentClassLogger().Info($"FillProcessInfo processInfo = {processInfo}");
#endif
        }
    }
}
