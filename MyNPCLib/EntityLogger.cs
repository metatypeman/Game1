using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class EntityLogger: IEntityLogger
    {
        private readonly object mLockObj = new object();

        private string mMarker;
        public string Marker
        {
            get
            {
                lock(mLockObj)
                {
                    return mMarker;
                }
            }

            set
            {
                lock (mLockObj)
                {
                    mMarker = value;
                }
            }
        }

        private bool mEnabled;

        public bool Enabled
        {
            get
            {
                lock (mLockObj)
                {
                    return mEnabled;
                }
            }

            set
            {
                lock (mLockObj)
                {
                    mEnabled = value;
                }
            }
        }

        [MethodForLoggingSupport]
        public void Log(string message)
        {
            var marker = string.Empty;

            lock(mLockObj)
            {
                if(!mEnabled)
                {
                    return;
                }
                marker = mMarker;
            }

            var now = DateTime.Now;
            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();

            var result = BuildLogString(marker, now, KindOfLogLevel.LOG, tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

            LogInstance.Raw(result);
        }

        [MethodForLoggingSupport]
        public void Error(string message)
        {
            var marker = string.Empty;

            lock (mLockObj)
            {
                if (!mEnabled)
                {
                    return;
                }
                marker = mMarker;
            }

            var now = DateTime.Now;
            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();

            var result = BuildLogString(marker, now, KindOfLogLevel.ERROR, tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

            LogInstance.Raw(result);
        }

        [MethodForLoggingSupport]
        public void Warning(string message)
        {
            var marker = string.Empty;

            lock (mLockObj)
            {
                if (!mEnabled)
                {
                    return;
                }
                marker = mMarker;
            }

            var now = DateTime.Now;
            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();

            var result = BuildLogString(marker, now, KindOfLogLevel.WARNING, tmpCallInfo.FullClassName, tmpCallInfo.MethodName, message);

            LogInstance.Raw(result);
        }

        private string BuildLogString(string marker, DateTime dateTime, KindOfLogLevel level, string className, string methodName, string message)
        {
            return $"{dateTime}|>>{marker}<<|{level.ToString()}|{className}|{methodName}|{message}";
        }
    }
}
