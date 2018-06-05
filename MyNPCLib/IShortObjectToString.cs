using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IShortObjectToString
    {
        string ToShortString();
        string ToShortString(uint n);
        string PropertiesToShortSting(uint n);
    }
}
