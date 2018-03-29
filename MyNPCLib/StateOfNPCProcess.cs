using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum StateOfNPCProcess
    {
        Created,
        Running,
        RanToCompletion,
        Canceled,
        Faulted,
        Destroyed
    }
}
