using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseNotValidNPCProcess : BaseCommonNPCProcess
    {
        public BaseNotValidNPCProcess(IEntityLogger entityLogger, INPCContext context)
            : base(entityLogger)
        {
            Context = context;
        }
        
        public override StateOfNPCProcess State
        {
            get
            {
                return StateOfNPCProcess.Faulted;
            }

            set
            {
            }
        } 

        public override ulong Id
        {
            get
            {
                return 0ul;
            }

            set
            {
            }
        }

        public override void Dispose()
        {
        }
    }
}
