using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalHostEnvironment
{
    public interface IHostLogicalObjectStorageForBus
    {
        ulong EntityId { get; }
        DefaultHostCGStorage GeneralHost { get; }
        DefaultHostCGStorage VisibleHost { get; }
    }
}
