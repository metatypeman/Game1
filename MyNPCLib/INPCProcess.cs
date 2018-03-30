using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
