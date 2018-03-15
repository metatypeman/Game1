using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface ILogProxy
    {
        void Log(string message);
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

        public static void Log(string message)
        {
            lock (mLockObj)
            {
                mILogProxy?.Log(message);
            }
        }

        private static object mLockObj = new object();
        private static ILogProxy mILogProxy;
    }
}
