using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCCommand: IObjectToString
    {
        string Name { get; }
        int InitiatingProcessId { get; }
        KindOfLinkingToInitiator KindOfLinkingToInitiator { get; }
        Dictionary<string, object> Params { get; }
    }
}
