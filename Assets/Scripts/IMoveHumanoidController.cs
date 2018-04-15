using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum InternalHumanoidStateKind
    {
        HState,
        TargetPosition,
        VState,
        HandsState,
        HandsActionState,
        HeadState,
        TargetHeadPosition,
        ThingsCommand
    }

    public delegate void InternalHumanoidStatesChangedAction(List<InternalHumanoidStateKind> changedStates);

    public interface IMoveHumanoidController
    {
        InternalHumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidController targetState);
        InternalStatesOfHumanoidController States { get; }
        event InternalHumanoidStatesChangedAction OnHumanoidStatesChanged;
        void Die();
        void SetAimCorrector(IAimCorrector corrector);
    }
}
