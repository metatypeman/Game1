using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public delegate bool PredicateOfTrigger();

    public class BaseTrigger
    {
        public BaseTrigger(PredicateOfTrigger predicate)
        {
            mPredicate = predicate;
        }

        private readonly PredicateOfTrigger mPredicate;

        protected readonly object StateLockObj = new object();
        protected StateOfNPCProcess mState = StateOfNPCProcess.Created;

        private INPCContext mContext;
        public INPCContext Context
        {
            get
            {
                return mContext;
            }

            set
            {
                StateChecker();

                if (mContext == value)
                {
                    return;
                }

                mContext = value;

                OnSetContext();
            }
        }

        protected virtual void OnSetContext()
        {
        }

        protected void StateChecker()
        {
            lock (StateLockObj)
            {
                if (mState == StateOfNPCProcess.Destroyed)
                {
                    throw new ElementIsNotActiveException();
                }

                if (mState != StateOfNPCProcess.Created)
                {
                    throw new ElementIsModifiedAfterActivationException();
                }
            }
        }


    }
}
