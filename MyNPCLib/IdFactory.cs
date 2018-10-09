using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class IdFactory: IIdFactory
    {
        public ulong GetNewId()
        {
            lock(mLockObj)
            {
                mCurrIndex++;
                return mCurrIndex;
            }
        }

        private object mLockObj = new object();
        private ulong mCurrIndex;
    }
}
