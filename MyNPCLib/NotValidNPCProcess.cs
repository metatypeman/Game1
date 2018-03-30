using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NotValidAbstractNPCProcess : INPCProcess
    {
        public StateOfNPCProcess State => StateOfNPCProcess.Faulted;

        public ulong Id => 0;

        public KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;

        public void Dispose()
        {
        }
    }
}
