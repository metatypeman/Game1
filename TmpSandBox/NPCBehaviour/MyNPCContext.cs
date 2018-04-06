using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    public class MyNPCContext: BaseNPCContextWithBlackBoard<MyBlackBoard>
    {
        public MyNPCContext(IHumanoidBodyController humanoidBodyController)
            : base(null, null, humanoidBodyController)
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
