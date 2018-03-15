using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    public class LogProxyForNLog : ILogProxy
    {
        public void Log(string message)
        {
#if DEBUG
            NLog.LogManager.GetCurrentClassLogger().Info(message);
#endif
        }
    }
}
