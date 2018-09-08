using MyNPCLib;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LogProxyForDebug : ILogProxy
    {
        //public LogProxyForDebug()
        //{
            //NLogConfigurator.Config(new NLogConfiguratOptions()
            //{
            //    UseFile = true,
            //    UseConsole = true,
            //    SeparateFiles = NLogConfiguratOptionsSeparateFiles.ByDate,
            //    TargetDirectory = NLogConfiguratOptionsTargetDirectory.USERPROFILE
            //});
        //}

        [MethodForLoggingSupport]
        public void Log(string message)
        {
            var now = DateTime.Now;
            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();
            var result = LogHelper.BuildLogString(now, KindOfLogLevel.LOG.ToString(), tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

#if UNITY_EDITOR
            Debug.Log(result);
            //NLog.LogManager.GetCurrentClassLogger().Info(result);
#endif
        }

        [MethodForLoggingSupport]
        public void Error(string message)
        {
            var now = DateTime.Now;

            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();
            var result = LogHelper.BuildLogString(now, KindOfLogLevel.ERROR.ToString(), tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

#if UNITY_EDITOR
            Debug.LogError(result);
#endif
        }

        [MethodForLoggingSupport]
        public void Warning(string message)
        {
            var now = DateTime.Now;

            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();
            var result = LogHelper.BuildLogString(now, KindOfLogLevel.WARNING.ToString(), tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

#if UNITY_EDITOR
            Debug.LogWarning(result);
#endif
        }

        [MethodForLoggingSupport]
        public void Raw(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }
    }
}
