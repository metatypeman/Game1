using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCProcess : IDisposable
    {
        StateOfNPCProcess State { get; }
        ulong Id { get; }
        KindOfNPCProcess Kind { get; }
}
}
