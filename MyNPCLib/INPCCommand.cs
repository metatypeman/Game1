using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCCommand: IObjectToString
    {
        string Name { get; }
        ulong InitiatingProcessId { get; }
        KindOfLinkingToInitiator KindOfLinkingToInitiator { get; }
        float Priority { get; }
        IDictionary<string, object> Params { get; }
    }
}
