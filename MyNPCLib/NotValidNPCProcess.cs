using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NotValidAbstractNPCProcess : INPCProcess
    {
        public StateOfNPCProcess State => StateOfNPCProcess.Faulted;
        public event NPCProcessStateChanged OnStateChanged;
        public event Action OnRunningChanged;
        public event Action OnRanToCompletionChanged;
        public event Action OnCanceledChanged;
        public event Action OnFaultedChanged;
        public event Action OnDestroyedChanged;

        public ulong Id => 0;
        public KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;
        public Task Task => null;

        public void Dispose()
        {
        }
    }
}
