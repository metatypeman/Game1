using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    public class MyBlackBoard: BaseBlackBoard
    {
        public MyBlackBoard()
        {
            LogInstance.Log("Begin");
        }

        public int TstValue { get; set; }

        public override void Bootstrap()
        {
            base.Bootstrap();

            Log("Begin");
        }
    }
}
