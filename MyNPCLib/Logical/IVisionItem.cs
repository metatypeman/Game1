using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface IVisionItem: IObjectToString
    {
        Vector3 LocalDirection { get; set; }
        Vector3 Point { get; set; }
        float Distance { get; set; }
    }
}
