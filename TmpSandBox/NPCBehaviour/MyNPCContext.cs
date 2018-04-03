using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    public class MyNPCContext: BaseNPCContext
    {
        public MyNPCContext()
        {
            AddTypeOfProcess<MyBootNPCProcess>();
            AddTypeOfProcess<MyNPSGoToFarWayPoint>();
        }

        public override void Bootstrap()
        {
            Bootstrap<MyBootNPCProcess>();
        }
    }
}
