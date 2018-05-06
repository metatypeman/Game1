using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public interface IHostVisionObject : IObjectToString
    {
        ulong EntityId { get; set; }
        IList<IVisionItem> VisionItems { get; set; }
    }
}
