using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class InvocableInMainThreadObj : IInvocableInMainThreadObj
    {
        public InvocableInMainThreadObj(Action function, IInvokingInMainThread invokingInMainThread)
        {
            mFunction = function;
            mInvokingInMainThread = invokingInMainThread;
        }

        private Action mFunction;
        private IInvokingInMainThread mInvokingInMainThread;
        private bool mHasResult;
        private readonly object mLockObj = new object();

        public void Run()
        {
            mInvokingInMainThread.SetInvocableObj(this);

            while (true)
            {
                lock (mLockObj)
                {
                    if (mHasResult)
                    {
                        break;
                    }
                }

                Thread.Sleep(10);
            }
        }

        public void Invoke()
        {
            mFunction.Invoke();

            lock (mLockObj)
            {
                mHasResult = true;
            }
        }
    }

    public class InvocableInMainThreadObj<TResult> : IInvocableInMainThreadObj
    {
        public InvocableInMainThreadObj(Func<TResult> function, IInvokingInMainThread invokingInMainThread)
        {
            mFunction = function;
            mInvokingInMainThread = invokingInMainThread;
        }

        private Func<TResult> mFunction;
        private IInvokingInMainThread mInvokingInMainThread;
        private bool mHasResult;
        private TResult mResult;
        private readonly object mLockObj = new object();

        public TResult Run()
        {
            mInvokingInMainThread.SetInvocableObj(this);

            while (true)
            {
                lock (mLockObj)
                {
                    if (mHasResult)
                    {
                        break;
                    }
                }

                Thread.Sleep(10);
            }

            return mResult;
        }

        public void Invoke()
        {
            mResult = mFunction.Invoke();

            lock (mLockObj)
            {
                mHasResult = true;
            }
        }
    }
}
