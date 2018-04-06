using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NotValidAbstractNPCProcess : BaseNotValidNPCProcess
    {
        public NotValidAbstractNPCProcess(INPCContext context)
            : base(context)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;
    }
}
