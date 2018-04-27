using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.TstSmartObj
{
    public interface ILogicalContext
    {
        ILogicalObject Get(string query);
    }
}
