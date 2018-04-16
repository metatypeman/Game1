﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNPCLib;

namespace Assets.Scripts
{
    public enum InternalStateOfHumanoidTaskOfExecuting
    {
        Created,
        Executed,
        Filed,
        Canceled
    }

    public class InternalHumanoidTaskOfExecuting : IObjectToString
    {
        private object mLockObject = new object();
        private InternalStateOfHumanoidTaskOfExecuting mState = InternalStateOfHumanoidTaskOfExecuting.Created;

        public InternalStateOfHumanoidTaskOfExecuting State
        {
            get
            {
                lock(mLockObject)
                {
                    return mState;
                }
            }

            set
            {
                lock (mLockObject)
                {
                    mState = value;
                }
            }
        } 
        public InternalTargetStateOfHumanoidController ProcessedState { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            var nextN = n + 4;
            sb.AppendLine($"{spaces}{nameof(State)} = {State}");
            if (ProcessedState == null)
            {
                sb.AppendLine($"{spaces}{nameof(ProcessedState)} = {ProcessedState}");
            }
            else
            {
                sb.Append(ProcessedState.ToString(nextN));
            }
            return sb.ToString();
        }
    }
}
