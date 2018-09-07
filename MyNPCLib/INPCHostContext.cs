using MyNPCLib.CGStorage;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using MyNPCLib.LogicalSoundModeling;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib
{
    public interface INPCHostContext
    {
        INPCBodyHost BodyHost { get; }
        INPCHandHost RightHandHost { get; }
        INPCHandHost LeftHandHost { get; }
        ICGStorage SelfHostStorage { get; }
        IBusOfCGStorages BusOfCGStorages { get; }
        ulong SelfEntityId { get; }
        bool IsReady { get; }
        event Action OnReady;
        IList<IHostVisionObject> VisibleObjects { get; }
        Vector3 GlobalPosition { get; }
        event OnLogicalSoundAction OnLogicalSound;
    }
}
