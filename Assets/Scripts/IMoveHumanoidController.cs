using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum HumanoidStateKind
    {
        HState,
        TargetPosition,
        VState,
        HandsState,
        HandsActionState,
        HeadState,
        TargetHeadPosition
    }

    public delegate void HumanoidStatesChangedAction(List<HumanoidStateKind> changedStates);

    public interface IMoveHumanoidController
    {
        HumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidController targetState);
        StatesOfHumanoidController States { get; }
        event HumanoidStatesChangedAction OnHumanoidStatesChanged;
        void Die();
        void SetAimCorrector(IAimCorrector corrector);
    }
}
