using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public delegate bool PredicateOfTrigger();

    public class BaseTrigger : IChildComponentOfNPCProcess, ITrigger
    {
        public BaseTrigger(IEntityLogger entityLogger, PredicateOfTrigger predicate, int timeout = 1000)
        {
            mEntityLogger = entityLogger;
            mPredicate = predicate;
            mTimeOut = timeout;
        }

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        private IEntityLogger mEntityLogger;
        private readonly PredicateOfTrigger mPredicate;
        private readonly int mTimeOut;
        private readonly object mStateLockObj = new object();
        private StateOfNPCProcess mState = StateOfNPCProcess.Created;
        private readonly object mNeedRunLockObj = new object();
        private bool mNeedRun;

        public void Start()
        {
#if DEBUG
            //Log($"Begin");
#endif
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }

                if(mState == StateOfNPCProcess.Running)
                {
                    return;
                }

                mState = StateOfNPCProcess.Running;
            }

#if DEBUG
            //Log($"NEXT");
            //try
            //{
            //    Log($"NEXT {mPredicate.Invoke()}");
            //}catch(Exception e)
            //{
            //    Error($"e = {e}");
            //}    
#endif

            TryStartNRun();

#if DEBUG
            //Log($"End");
#endif
        }

        public void Stop()
        {
#if DEBUG
            //Log($"Begin");
#endif
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }

                if (mState != StateOfNPCProcess.Running)
                {
                    return;
                }

                mState = StateOfNPCProcess.RanToCompletion;
            }

            lock (mNeedRunLockObj)
            {
                mNeedRun = false;
            }
#if DEBUG
            //Log($"End");
#endif
        }

        private Action mOnFire;
        private readonly object mOnFireLockObj = new object();

        public event Action OnFire
        {
            add
            {
#if DEBUG
                //Log($"Begin");
#endif

                lock (mStateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        return;
                    }
                }

                lock (mOnFireLockObj)
                {
                    mOnFire += value;
                }

                TryStartNRun();
            }

            remove
            {
                lock (mOnFireLockObj)
                {
                    mOnFire -= value;
                }

                TryStopNRun();
            }
        }

        private Action mOnResetCondition;
        private readonly object mOnResetConditionLockObj = new object();
        public event Action OnResetCondition
        {
            add
            {
#if DEBUG
                //Log($"Begin");
#endif

                lock (mStateLockObj)
                {
                    if (mState == StateOfNPCProcess.Destroyed)
                    {
                        return;
                    }
                }

                lock(mOnResetConditionLockObj)
                {
                    mOnResetCondition += value;
                }

                TryStartNRun();
            }

            remove
            {
                lock (mOnResetConditionLockObj)
                {
                    mOnResetCondition -= value;
                }

                TryStopNRun();
            }
        }

        private void TryStartNRun()
        {
            lock (mNeedRunLockObj)
            {
                if(mNeedRun)
                {
                    return;
                }

                lock(mOnFireLockObj)
                {
                    lock(mOnResetConditionLockObj)
                    {
                        if (mOnFire == null && mOnResetCondition == null)
                        {
                            return;
                        }
                    }
                }

                lock(mStateLockObj)
                {
                    if(mState != StateOfNPCProcess.Running)
                    {
                        return;
                    }
                }

                mNeedRun = true;
            }

            Task.Run(() => {
                try
                {
                    NRun();
                }catch(Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        private void TryStopNRun()
        {
            lock (mNeedRunLockObj)
            {
                if(!mNeedRun)
                {
                    return;
                }

                lock (mOnFireLockObj)
                {
                    lock (mOnResetConditionLockObj)
                    {
                        if (mOnFire == null && mOnResetCondition == null)
                        {
                            mNeedRun = false;
                            return;
                        }
                    }
                }

                lock (mStateLockObj)
                {
                    if (mState != StateOfNPCProcess.Running)
                    {
                        mNeedRun = false;
                        return;
                    }
                }               
            }
        }

        private bool mLastResult;

        private void NRun()
        {
#if DEBUG
            //Log($"Begin");
#endif

            while (true)
            {
#if DEBUG
                //Log("while(true) ----");
#endif
                Thread.Sleep(mTimeOut);

                lock (mNeedRunLockObj)
                {
                    if (!mNeedRun)
                    {
                        break;
                    }
                }

                var currentResult = mPredicate();

#if DEBUG
                //Log($"currentResult = {currentResult} mLastResult = {mLastResult}");
#endif

                if(mLastResult != currentResult)
                {
                    mLastResult = currentResult;

                    if(currentResult)
                    {
                        Task.Run(() => {
                            try
                            {
                                mOnFire?.Invoke();
                            }
                            catch (Exception e)
                            {
#if DEBUG
                                Error($"e = {e}");
#endif
                            }
                        });
                    }
                    else
                    {
                        Task.Run(() => {
                            try
                            {
                                mOnResetCondition?.Invoke();
                            }
                            catch (Exception e)
                            {
#if DEBUG
                                Error($"e = {e}");
#endif
                            }
                        });
                    }          
                }
            }
        }

public void Dispose()
        {
#if DEBUG
            //Log($"Begin");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    return;
                }

                mState = StateOfNPCProcess.Destroyed;
            }

            lock (mNeedRunLockObj)
            {
                mNeedRun = false;
            }
        }
    }
}
