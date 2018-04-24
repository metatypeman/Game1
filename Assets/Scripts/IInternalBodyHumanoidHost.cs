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
        event InternalHumanoidStatesChangedAction OnHumanoidStatesChanged;
        void Die();
        event Action OnDie;
        void SetAimCorrector(IAimCorrector corrector);
        void SetInternalHumanoidHostContext(IInternalHumanoidHostContext intenalHostContext);
    }
}
