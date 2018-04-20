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
            NLog.LogManager.GetCurrentClassLogger().Info("MyBlackBoard");
        }

        public int TstValue { get; set; }

        public override void Bootstrap()
        {
            base.Bootstrap();

            NLog.LogManager.GetCurrentClassLogger().Info("Bootstrap");
        }
    }
}
