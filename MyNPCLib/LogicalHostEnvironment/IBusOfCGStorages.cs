using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalHostEnvironment
{
    public interface IBusOfCGStorages
    {
        ICGStorage GetStorageWithVisibleFacts(ulong entityKey);
        ICGStorage GeneralStorageWithPublicFacts { get; }
    }
}
