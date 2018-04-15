using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum OldHumanoidStateKind
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

    public delegate void OldHumanoidStatesChangedAction(List<OldHumanoidStateKind> changedStates);

    public interface IMoveHumanoidController
    {
        OldHumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidController targetState);
        OldStatesOfHumanoidController States { get; }
        event OldHumanoidStatesChangedAction OnHumanoidStatesChanged;
        void Die();
        void SetAimCorrector(IAimCorrector corrector);
    }
}
