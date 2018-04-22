using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public delegate void NPCProcessConcreteStateChanged(INPCProcess sender);
    public delegate void NPCProcessStateChanged(INPCProcess sender, StateOfNPCProcess state);

    public interface INPCProcess : IDisposable
    {
        StateOfNPCProcess State { get; }
        event NPCProcessStateChanged OnStateChanged;
        event NPCProcessConcreteStateChanged OnRunningChanged;
        event NPCProcessConcreteStateChanged OnRanToCompletionChanged;
        event NPCProcessConcreteStateChanged OnCanceledChanged;
        event NPCProcessConcreteStateChanged OnFaultedChanged;
        event NPCProcessConcreteStateChanged OnDestroyedChanged;
        ulong Id { get; }
        KindOfNPCProcess Kind { get; }
        Task Task { get; }

        float LocalPriority { get; set; }
        float GlobalPriority { get; }
    }
}
