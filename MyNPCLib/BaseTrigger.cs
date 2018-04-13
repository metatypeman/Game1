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
        public BaseTrigger(PredicateOfTrigger predicate, int timeout = 1000)
        {
            mPredicate = predicate;
            mTimeOut = timeout;
        }

        private readonly PredicateOfTrigger mPredicate;
        private readonly int mTimeOut;
        private readonly object mStateLockObj = new object();
        private StateOfNPCProcess mState = StateOfNPCProcess.Created;
        private readonly object mNeedRunLockObj = new object();
        private bool mNeedRun;

        public void Start()
        {
#if DEBUG
            //LogInstance.Log($"Begin BaseTrigger Start");
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
            //LogInstance.Log($"BaseTrigger Start NEXT {mPredicate.Invoke()}");
#endif

            TryStartNRun();

#if DEBUG
            //LogInstance.Log($"End BaseTrigger Start");
#endif
        }

        public void Stop()
        {
#if DEBUG
            //LogInstance.Log($"BaseTrigger Stop");
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
            //LogInstance.Log($"BaseTrigger Stop NEXT");
#endif
        }

        private Action mOnFire;
        private readonly object mOnFireLockObj = new object();

        public event Action OnFire
        {
            add
            {
#if DEBUG
                //LogInstance.Log($"BaseTrigger OnFire add");
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
                //LogInstance.Log($"BaseTrigger OnResetCondition add");
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
                NRun();
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
            LogInstance.Log($"BaseTrigger NRun");
#endif

            while(true)
            {
#if DEBUG
                //LogInstance.Log("BaseTrigger NRun while(true) ----");
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
                //LogInstance.Log($"BaseTrigger NRun currentResult = {currentResult} mLastResult = {mLastResult}");
#endif

                if(mLastResult != currentResult)
                {
                    mLastResult = currentResult;

                    if(currentResult)
                    {
                        Task.Run(() => { mOnFire?.Invoke(); });
                    }
                    else
                    {
                        Task.Run(() => { mOnResetCondition?.Invoke(); });
                    }          
                }
            }
        }

        public void Dispose()
        {
#if DEBUG
            //LogInstance.Log($"BaseTrigger Dispose");
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
