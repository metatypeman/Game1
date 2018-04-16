﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNPCLib;

namespace Assets.Scripts
{
    public enum InternalMoveHumanoidCommandKind
    {
        HState,
        VState,
        HandsState,
        HandsActionState,
        HeadState,
        Things
    }

    public interface IInternalMoveHumanoidCommand : IObjectToString
    {
        InternalMoveHumanoidCommandKind Kind { get; }
    }
}