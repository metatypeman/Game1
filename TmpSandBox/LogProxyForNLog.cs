using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    public class LogProxyForNLog : ILogProxy
    {
        [MethodForLoggingSupport]
        public void Log(string message)
        {
            var now = DateTime.Now;
            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();
            var result = LogHelper.BuildLogString(now, KindOfLogLevel.LOG.ToString(), tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);
            NLog.LogManager.GetCurrentClassLogger().Info(result);
        }

        [MethodForLoggingSupport]
        public void Error(string message)
        {
            var now = DateTime.Now;

            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();
            var result = LogHelper.BuildLogString(now, KindOfLogLevel.ERROR.ToString(), tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

            NLog.LogManager.GetCurrentClassLogger().Error(result);
        }

        [MethodForLoggingSupport]
        public void Warning(string message)
        {
            var now = DateTime.Now;

            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();
            var result = LogHelper.BuildLogString(now, KindOfLogLevel.WARNING.ToString(), tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

            NLog.LogManager.GetCurrentClassLogger().Warn(result);
        }

        [MethodForLoggingSupport]
        public void Raw(string message)
        {
            NLog.LogManager.GetCurrentClassLogger().Warn(message);
        }
    }
}
