using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IHumanoidBodyCommand: IObjectToString
    {
        HumanoidBodyCommandKind Kind { get; }
        ulong InitiatingProcessId { get; set; }
    }
}
