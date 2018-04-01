using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCContext: IDisposable
    {
        StateOfNPCContext State { get; }

        INPCResourcesManager Body { get; }
        INPCResourcesManager DefaultHand { get; }
        INPCResourcesManager LeftHand { get; }
        INPCResourcesManager RightHand { get; }

        bool AddTypeOfProcess<T>();
        bool AddTypeOfProcess(Type type);

        void Bootstrap<T>();
        void Bootstrap(Type type);
        void Bootstrap();

        INPCProcess Send(INPCCommand command);

        void RegProcess(INPCProcess process, ulong parentProcessId);
        void UnRegProcess(INPCProcess process);
    }
}
