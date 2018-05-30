using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface ILogProxy
    {
        void Log(string message);
        void Error(string message);
        void Warning(string message);
        void Raw(string message);
    }

    public static class LogInstance
    {
        public static void SetLogProxy(ILogProxy logProxy)
        {
            lock(mLockObj)
            {
                mILogProxy = logProxy;
            }
        }

        [MethodForLoggingSupport]
        public static void Log(string message)
        {
            lock (mLockObj)
            {
                mILogProxy?.Log(message);
            }
        }

        [MethodForLoggingSupport]
        public static void Error(string message)
        {
            lock (mLockObj)
            {
                mILogProxy?.Error(message);
            }
        }

        [MethodForLoggingSupport]
        public static void Warning(string message)
        {
            lock (mLockObj)
            {
                mILogProxy?.Warning(message);
            }
        }

        [MethodForLoggingSupport]
        public static void Raw(string message)
        {
            lock (mLockObj)
            {
                mILogProxy?.Raw(message);
            }
        }

        private static object mLockObj = new object();
        private static ILogProxy mILogProxy;
    }
}
