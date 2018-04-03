using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum HumanoidVState
    {
        Ground,
        Jump,
        Crouch
    }

    public interface IHumanoidVStateCommand : IHumanoidBodyCommand
    {
        HumanoidVState State { get; }
    }
}
