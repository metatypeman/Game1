using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public delegate void NPCProcessStateChanged(StateOfNPCProcess state);

    public interface INPCProcess : IDisposable
    {
        StateOfNPCProcess State { get; }
        event NPCProcessStateChanged OnStateChanged;
        event Action OnRunningChanged;
        event Action OnRanToCompletionChanged;
        event Action OnCanceledChanged;
        event Action OnFaultedChanged;
        event Action OnDestroyedChanged;
        ulong Id { get; }
        KindOfNPCProcess Kind { get; }
        Task Task { get; }

        float LocalPriority { get; set; }
        float GlobalPriority { get; }
    }
}
