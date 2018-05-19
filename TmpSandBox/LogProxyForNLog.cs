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
            NLog.LogManager.GetCurrentClassLogger().Info(message);
        }

        public void Error(string message)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(message);
        }

        public void Warning(string message)
        {
            NLog.LogManager.GetCurrentClassLogger().Warn(message);
        }
    }
}
