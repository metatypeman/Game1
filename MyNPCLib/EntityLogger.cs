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

        public bool mEnabled;

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

            throw new NotImplementedException();
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

            throw new NotImplementedException();
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

            throw new NotImplementedException();
        }

        private string BuildLogString(string marker, DateTime dateTime, KindOfLogLevel level, string className, string methodName, string message)
        {

        }
    }
}
