using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface ITrigger: IDisposable
    {
        event Action OnFire;
        event Action OnResetCondition;
        void Start();
        void Stop();
    }
}
