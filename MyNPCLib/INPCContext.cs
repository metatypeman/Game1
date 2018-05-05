using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCContext: INPCCancellationTokensContext, IDisposable
    {
        StateOfNPCContext State { get; }

        INPCBodyResourcesManager Body { get; }
        INPCResourcesManager DefaultHand { get; }   
        INPCResourcesManager RightHand { get; }
        INPCResourcesManager LeftHand { get; }

        bool AddTypeOfProcess<T>();
        bool AddTypeOfProcess(Type type);

        void Bootstrap<T>();
        void Bootstrap(Type type);
        void Bootstrap();

        INPCProcess Send(INPCCommand command);

        void RegProcess(INPCProcess process, ulong parentProcessId);
        void UnRegProcess(INPCProcess process);
        INPCProcess GetParentProcess(ulong childProcessId);

        NPCResourcesResolutionKind ApproveNPCResourceProcessExecute(BaseNPCResourcesResolution existingsNPCResourcesResulution);

        object NoTypedBlackBoard { get; }

        T GetInstance<T>() where T : class;

        void CallInMainUI(Action function);
        TResult CallInMainUI<TResult>(Func<TResult> function);

        BaseAbstractLogicalObject SelfLogicalObject { get; }
        BaseAbstractLogicalObject GetLogicalObject(string query);
    }
}
