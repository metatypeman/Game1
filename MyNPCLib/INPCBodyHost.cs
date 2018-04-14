﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public delegate void HumanoidStatesChangedAction(List<HumanoidStateKind> changedStates);

    public interface INPCBodyHost
    {
        HumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidBody targetState);
        StatesOfHumanoidBodyController States { get; }
        event HumanoidStatesChangedAction OnHumanoidStatesChanged;
    }
}