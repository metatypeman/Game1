using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalHostEnvironment
{
    public interface IHostLogicalObjectStorageForBus
    {
        ulong EntityId { get; } 
        DefaultHostCGStorage VisibleHost { get; }
        DefaultHostCGStorage PublicHost { get; }
    }
}
