using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTests
{
    public class TestedNPCContext: BaseNPCContext
    {
        public TestedNPCContext(IEntityLogger entityLogger)
            : base(entityLogger)
        {
        }
    }
}
