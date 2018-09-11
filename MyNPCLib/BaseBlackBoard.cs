using MyNPCLib.CGStorage;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class BaseBlackBoard
    {
        private readonly object mEntityLoggerLockObj = new object();
        private IEntityLogger mEntityLogger;

        public IEntityLogger EntityLogger
        {
            get
            {
                lock (mEntityLoggerLockObj)
                {
                    return mEntityLogger;
                }
            }

            set
            {
                lock (mEntityLoggerLockObj)
                {
                    if (mEntityLogger == value)
                    {
                        return;
                    }

                    mEntityLogger = value;
                }

                OnSetEntityLogger();
            }
        }

        protected virtual void OnSetEntityLogger()
        {
        }

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Log(message);
            }
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Error(message);
            }
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Warning(message);
            }
        }

        private INPCContext mContext;
        public INPCContext Context
        {
            get
            {
                return mContext;
            }

            set
            {
                if (mContext == value)
                {
                    return;
                }

                mContext = value;

                OnSetContext();
            }
        }

        public IEntityDictionary EntityDictionary => mContext?.EntityDictionary;

        public ContextOfCGStorage ContextOfCGStorage => mContext?.ContextOfCGStorage;
        public ICGStorage MainCGStorage => mContext?.MainCGStorage;
        public GlobalCGStorage GlobalCGStorage => mContext?.GlobalCGStorage;

        public BaseAbstractLogicalObject GetLogicalObject(string query, params QueryParam[] paramsCollection)
        {
            return mContext?.GetLogicalObject(query, paramsCollection);
        }

        public IList<VisionObject> VisibleObjects
        {
            get
            {
                return mContext?.VisibleObjects;
            }
        }

        protected virtual void OnSetContext()
        {
        }

        public virtual void Bootstrap()
        {
#if DEBUG
            Log("Begin");
#endif
        }
    }
}
