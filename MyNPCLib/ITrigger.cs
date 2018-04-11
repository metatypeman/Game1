using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface ITrigger: IDisposable
    {
        event Action OnFire;
        void Start();
        void Stop();
    }
}
