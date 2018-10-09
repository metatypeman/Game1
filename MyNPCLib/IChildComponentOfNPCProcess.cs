using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IChildComponentOfNPCProcess: IDisposable
    {
        void Start();
        void Stop();
    }
}
