using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public delegate void HumanoidStatesChangedAction(IList<HumanoidStateKind> changedStates);

    public interface INPCBodyHost
    {
        void Execute(TargetStateOfHumanoidBody targetState);
        IStatesOfHumanoidBodyHost States { get; }
        event HumanoidStatesChangedAction OnHumanoidStatesChanged;
        event Action OnDie;
        void CallInMainUI(Action function);
        TResult CallInMainUI<TResult>(Func<TResult> function);
        bool IsReady { get; }
        event Action OnReady;
    }
}
