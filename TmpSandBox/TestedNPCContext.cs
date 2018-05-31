using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox
{
    public class TestedBlackBoard
    {
    }

    public class TestedNPCContext : BaseNPCContext
    {
        public TestedNPCContext(IEntityLogger entityLogger)
            : base(entityLogger)
        {
        }
    }
}
