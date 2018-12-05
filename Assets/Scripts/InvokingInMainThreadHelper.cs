using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class InvokingInMainThreadHelper: IInvokingInMainThread
    {
        public void CallInMainUI(Action function)
        {
            var invocable = new InvocableInMainThreadObj(function, this);
            invocable.Run();
        }

        public TResult CallInMainUI<TResult>(Func<TResult> function)
        {
            var invocable = new InvocableInMainThreadObj<TResult>(function, this);
            return invocable.Run();
        }

        public void SetInvocableObj(IInvocableInMainThreadObj invokableObj)
        {
            lock (mTmpQueueLockObj)
            {
                mTmpQueue.Enqueue(invokableObj);
            }
        }

        private object mTmpQueueLockObj = new object();
        private Queue<IInvocableInMainThreadObj> mTmpQueue = new Queue<IInvocableInMainThreadObj>();

        public void Update()
        {
            List<IInvocableInMainThreadObj> invocableList = null;

            lock (mTmpQueueLockObj)
            {
                if (mTmpQueue.Count > 0)
                {
                    invocableList = mTmpQueue.ToList();
                    mTmpQueue.Clear();
                }
            }

            if (invocableList == null)
            {
                return;
            }

            foreach (var invocable in invocableList)
            {
                invocable.Invoke();
            }
        }
    }
}
