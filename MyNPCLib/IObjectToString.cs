using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IObjectToString
    {
        string ToString(uint n);
        string PropertiesToSting(uint n);
    }
}
