using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IHandThing: IThing
    {
        bool IsReady { get; }
        bool SetToHandsOfHumanoid(IInternalHumanoid humanoid, IInternalHumanoidHostContext internalContext);
        bool SetAsAloneAndHide();
        bool ThrowOutToSurface();
    }
}
