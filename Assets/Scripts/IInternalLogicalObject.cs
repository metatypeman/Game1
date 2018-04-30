using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IInternalLogicalObject
    {
        ulong EntityId { get; }
        object this[ulong propertyKey] { get; set; }
    }
}
