using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NotValidAbstractNPCProcess : BaseNotValidNPCProcess
    {
        public NotValidAbstractNPCProcess(IEntityLogger entityLogger, INPCContext context)
            : base(entityLogger, context)
        {
        }

        public override KindOfNPCProcess Kind => KindOfNPCProcess.Abstract;
    }
}
