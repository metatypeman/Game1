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

        void AddTypeOfProcess<T>();
        void AddTypeOfProcess(Type type);

        void Bootstrap<T>();
        void Bootstrap(Type type);
        void Bootstrap();

        INPCProcess Send(INPCCommand command);
    }
}
