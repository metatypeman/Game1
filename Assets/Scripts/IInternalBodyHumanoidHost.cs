using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IInternalBodyHumanoidHost: IInvokingInMainThread
    {
        void Execute(InternalTargetStateOfHumanoidController targetState);
        InternalStatesOfHumanoidController States { get; }
        event HumanoidStatesChangedAction OnHumanoidStatesChanged;
        void Die();
        event Action OnDie;
        void SetAimCorrector(IAimCorrector corrector);
        void SetInternalHumanoidHostContext(IInternalHumanoidHostContext intenalHostContext);
        void CallInMainUI(Action function);
        TResult CallInMainUI<TResult>(Func<TResult> function);
        ILogicalStorage HostLogicalStorage { get; }
        IPassiveLogicalObject SelfLogicalObject { get; }
    }
}
