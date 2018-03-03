using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum StateOfHumanoidTaskOfExecuting
    {
        Created,
        Executed,
        Filed,
        Canceled
    }

    public class HumanoidTaskOfExecuting : IObjectToString
    {
        private object mLockObject = new object();
        private StateOfHumanoidTaskOfExecuting mState = StateOfHumanoidTaskOfExecuting.Created;

        public StateOfHumanoidTaskOfExecuting State
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
        public TargetStateOfHumanoidController ProcessedState { get; set; }

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(HumanoidTaskOfExecuting)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(HumanoidTaskOfExecuting)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
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
