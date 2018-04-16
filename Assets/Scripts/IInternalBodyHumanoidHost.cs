﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IInternalBodyHumanoidHost
    {
        InternalHumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidController targetState);
        InternalStatesOfHumanoidController States { get; }
        event InternalHumanoidStatesChangedAction OnHumanoidStatesChanged;
        void Die();
        void SetAimCorrector(IAimCorrector corrector);
    }
}