using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.TstSmartObj
{
    public interface ILogicalObject/*: IEqualityComparer<ILogicalObject>*/
    {
        object this[string propertyName] { get; }
    }
}
