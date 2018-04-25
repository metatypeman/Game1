using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCBodyResourcesManager : IDisposable
    {
        INPCProcess Send(IHumanoidBodyCommand command);
        void Bootstrap();
        void UnRegProcess(ulong processId);
        void CallInMainUI(Action function);
        TResult CallInMainUI<TResult>(Func<TResult> function);
    }
}
