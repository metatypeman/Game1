using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum OldHumanoidVState
    {
        Ground,
        Jump,
        Crouch
    }

    public interface IOldHumanoidVStateCommand : IOldMoveHumanoidCommand
    {
        OldHumanoidVState State { get; }
    }
}
