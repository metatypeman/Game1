using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NotValidResourceNPCProcess : BaseNotValidNPCProcess
    {
        public override KindOfNPCProcess Kind => KindOfNPCProcess.Resource;
    }
}
